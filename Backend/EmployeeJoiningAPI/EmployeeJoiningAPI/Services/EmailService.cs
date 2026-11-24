
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EmployeeJoiningAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EmailService(
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var apiKey =
                _configuration["RESEND_API_KEY"]
                ?? throw new InvalidOperationException(
                    "Resend API key is not configured.");

            var requestData = new
            {
                from = "Alpha Data Employee Portal <onboarding@resend.dev>",
                to = new[] { toEmail },
                subject = subject,
                text = body
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.resend.com/emails");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = JsonContent.Create(requestData);

            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(30));

            try
            {
                Console.WriteLine("Resend: Sending email...");

                using var response = await _httpClient.SendAsync(
                    request,
                    cts.Token);

                var responseBody =
                    await response.Content.ReadAsStringAsync(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"Resend API Error: {responseBody}");

                    throw new HttpRequestException(
                        $"Resend email failed. Status: {(int)response.StatusCode}. Details: {responseBody}");
                }

                Console.WriteLine(
                    $"Resend: Email sent successfully. Response: {responseBody}");
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException(
                    "Resend email request timed out after 30 seconds.");
            }
        }
    }
}
