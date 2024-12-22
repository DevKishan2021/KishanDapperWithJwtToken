using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DapperWithJwtBL;
using DbContext_DapperWithJwt.Dapper;
using IDPContext_DapperWithJwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DbContext_DapperWithJwt
{
    public class CommonDbContext : ICommonDbcontext
    {
        private readonly IConfiguration _config;
        private readonly IDapperService _db;
        private readonly ApplicationDbContext _context;

        public CommonDbContext(IConfiguration config, IDapperService db, ApplicationDbContext context)
        {
            _config = config;
            _db = db;
            _context = context;
        }

        public string GenerateCode(string requestType)
        {
            throw new NotImplementedException();
        }

        public string GenerateJwtToken(Account user, TimeSpan duration)
        {
            var secretKey = _config["Jwt:SecretKey"] ?? throw new Exception("Seccret key not found");
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new("userId",user.Id),
                new(ClaimTypes.Email, user.Email??""),
                new(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                   issuer: issuer,
                   audience: audience,
                   claims: claims,
                   expires: DateTime.UtcNow.Add(duration),
                   signingCredentials: credentials
            );

            return tokenHandler.WriteToken(token);
        }

        public Dictionary<string, string> ReadToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToDictionary(x => x.Type, x => x.Value);
            return claims;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var secretKey = _config["Jwt:SecretKey"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = false,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return principal;
            }
            catch (Exception)
            {
                throw new Exception("Token Is Expired Or Invalid Token");
            }
        }

    }
}
