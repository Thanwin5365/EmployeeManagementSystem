
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string? neonConnectionString =
    configuration.GetConnectionString("NeonConnection");

if (string.IsNullOrWhiteSpace(neonConnectionString))
{
    throw new InvalidOperationException(
        "NeonConnection was not found in appsettings.json.");
}

string sqlServerConnectionString =
    "Server=localhost;" +
    "Database=EmployeeDB;" +
    "Integrated Security=True;" +
    "TrustServerCertificate=True;";

// Parent tables must be migrated before dependent tables.
string[] tables =
{
    "Managers",
    "Level2Managers",
    "HRManagers",
    "Employees",
    "ManagerConnections",
    "EmployeeAttendance",
    "EmployeePunches",
    "EmployeeVacations",
    "JoiningReports",
    "JoiningReportApprovalHistory"
};

// Identity columns whose sequences must be synchronized afterward.
(string Table, string Column)[] identityColumns =
{
    ("EmployeeAttendance", "AttendanceId"),
    ("EmployeePunches", "PunchId"),
    ("EmployeeVacations", "VacationId"),
    ("Employees", "EmployeeId"),
    ("HRManagers", "HrManagerId"),
    ("JoiningReportApprovalHistory", "ApprovalHistoryId"),
    ("JoiningReports", "Id"),
    ("ManagerConnections", "Id")
};

static string QuoteIdentifier(string identifier)
{
    return "\"" + identifier.Replace("\"", "\"\"") + "\"";
}

try
{
    await using var sqlConnection =
        new SqlConnection(sqlServerConnectionString);

    await using var neonConnection =
        new NpgsqlConnection(neonConnectionString);

    Console.WriteLine("Connecting to SQL Server...");
    await sqlConnection.OpenAsync();
    Console.WriteLine("SQL Server connected.");

    Console.WriteLine("Connecting to Neon...");
    await neonConnection.OpenAsync();
    Console.WriteLine("Neon connected.");

    // Safety check: refuse to migrate if any target table has data.
    Console.WriteLine("\nChecking Neon tables...");

    foreach (string table in tables)
    {
        string countSql =
            $"SELECT COUNT(*) FROM {QuoteIdentifier(table)};";

        await using var countCommand =
            new NpgsqlCommand(countSql, neonConnection);

        long count = Convert.ToInt64(
            await countCommand.ExecuteScalarAsync());

        Console.WriteLine($"{table}: {count}");

        if (count != 0)
        {
            throw new InvalidOperationException(
                $"Migration stopped: Neon table {table} is not empty.");
        }
    }

    Console.WriteLine("\nAll destination tables are empty.");
    Console.WriteLine("Starting migration...\n");

    await using var transaction =
        await neonConnection.BeginTransactionAsync();

    try
    {
        int grandTotal = 0;

        foreach (string table in tables)
        {
            Console.WriteLine($"Migrating {table}...");

            string selectSql =
                $"SELECT * FROM [dbo].[{table}];";

            await using var selectCommand =
                new SqlCommand(selectSql, sqlConnection);

            await using var reader =
                await selectCommand.ExecuteReaderAsync();

            int columnCount = reader.FieldCount;

            string[] columnNames = Enumerable
                .Range(0, columnCount)
                .Select(reader.GetName)
                .ToArray();

            string quotedColumns = string.Join(
                ", ",
                columnNames.Select(QuoteIdentifier));

            string parameters = string.Join(
                ", ",
                Enumerable.Range(0, columnCount)
                    .Select(i => $"@p{i}"));

            string insertSql =
                $"INSERT INTO {QuoteIdentifier(table)} " +
                $"({quotedColumns}) VALUES ({parameters});";

            int rowCount = 0;

            while (await reader.ReadAsync())
            {
                object[] values = new object[columnCount];

                reader.GetValues(values);

                await using var insertCommand =
                    new NpgsqlCommand(
                        insertSql,
                        neonConnection,
                        transaction);

                for (int i = 0; i < columnCount; i++)
                {
                    insertCommand.Parameters.AddWithValue(
                        $"p{i}",
                        values[i] ?? DBNull.Value);
                }

                await insertCommand.ExecuteNonQueryAsync();

                rowCount++;
            }

            grandTotal += rowCount;

            Console.WriteLine(
                $"{table}: {rowCount} rows migrated.\n");
        }

        // Synchronize PostgreSQL identity sequences.
        Console.WriteLine("Synchronizing identity sequences...");

        foreach (var identity in identityColumns)
        {
            string tableName = QuoteIdentifier(identity.Table);
            string columnName = QuoteIdentifier(identity.Column);

            string sequenceSql = $"""
                SELECT setval(
                    pg_get_serial_sequence(
                        '{tableName.Replace("'", "''")}',
                        '{identity.Column.Replace("'", "''")}'
                    ),
                    COALESCE(MAX({columnName}), 1),
                    COUNT(*) > 0
                )
                FROM {tableName};
                """;

            await using var sequenceCommand =
                new NpgsqlCommand(
                    sequenceSql,
                    neonConnection,
                    transaction);

            await sequenceCommand.ExecuteScalarAsync();
        }

        await transaction.CommitAsync();

        Console.WriteLine("\n==================================");
        Console.WriteLine("MIGRATION COMPLETED SUCCESSFULLY!");
        Console.WriteLine($"Total rows migrated: {grandTotal}");
        Console.WriteLine("==================================");
    }
    catch
    {
        await transaction.RollbackAsync();

        Console.WriteLine(
            "\nMigration failed. Neon transaction rolled back.");

        throw;
    }
}
catch (Exception ex)
{
    Console.WriteLine("\nMIGRATION ERROR:");
    Console.WriteLine(ex.Message);
}
