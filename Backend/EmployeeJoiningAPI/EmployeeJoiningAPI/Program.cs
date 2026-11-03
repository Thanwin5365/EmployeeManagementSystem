
using EmployeeJoiningAPI.Data;
using EmployeeJoiningAPI.Services;
using EmployeeJoiningAPI.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// REPOSITORIES
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();

// SERVICES
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IManagerAuthService, ManagerAuthService>();
builder.Services.AddScoped<IJoiningReportService, JoiningReportService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

// CONTROLLERS
builder.Services.AddControllers();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IJoiningReportRepository, JoiningReportRepository>();
builder.Services.AddScoped<IJoiningReportService, JoiningReportService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// ATTENDANCE
builder.Services.AddScoped<AttendanceRepository>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

var app = builder.Build();


// MIDDLEWARE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
