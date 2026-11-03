using Microsoft.AspNetCore.Identity;

namespace EmployeeJoiningAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }

        public bool VerifyPassword(
            string hashedPassword,
            string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                null!,
                hashedPassword,
                password);

            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}