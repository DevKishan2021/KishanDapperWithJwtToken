using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace DapperWithJwtBL
{
    public class Account
    {
        [Key]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("profileImage")]
        public string? ProfileImage { get; set; }

        [JsonProperty("firstname")]
        public string? Firstname { get; set; }

        [JsonProperty("lastname")]
        public string? Lastname { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
        public bool? IsEmailVerified { get; set; }

        [JsonProperty("mobile")]
        public string? Mobile { get; set; }
        public bool? IsMobileVerified { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("occupation")]
        public string? Occupation { get; set; }

        [JsonProperty("agreeTerms")]
        public bool? AgreeTerms { get; set; }

        [JsonProperty("coins")]
        public int Coins { get; set; }

        public string? Password
        {
            get => _password;
            set => _password = HashPassword(value);
        }

        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("password")]
        private string? _password;

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string enteredPassword)
        {
            if (string.IsNullOrEmpty(enteredPassword) || string.IsNullOrEmpty(_password))
                return false;

            string hashedEnteredPassword = HashPassword(enteredPassword);
            return _password == hashedEnteredPassword;
        }
    }

    public class UserAdminView
    {

    }
    public class UserIntersts
    {
        [Key]
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Interests { get; set; }
    }
    public class CreateAccountDto
    {
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool AgreeTerms { get; set; }
    }
    public class SignupDTO
    {
        public required string Username { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public required string Email { get; set; }
        public required string Mobile { get; set; }
        public required string Password { get; set; }
        public required string Occupation { get; set; }
        public required string IdentifierType { get; set; }
    }
    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class JWTClaimDTO
    {
        public required string Email { get; set; }
    }
    public class UserActivity
    {
        [Key]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Type { get; set; }
        public string? Details { get; set; }
        public string? SessionToken { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Location { get; set; }
        public string? EventSource { get; set; }
        public DateTime ActivityTime { get; set; }

    }
    public class FogrgotPasswordDTO
    {
        public required string Token { get; set; }
        public required string password { get; set; }
    }
    public class GoogleUser
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public bool EmailVerified { get; set; }
        public string Picture { get; set; }
    }
    public class AuthCientResponse
    {
        public Account User { get; set; }
        public string Type { get; set; }
    }
}
