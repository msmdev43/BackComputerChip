using computerChip.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace computerChip.Services
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryInMinutes;

        public JwtService(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            _secretKey = jwtSettings["SecretKey"]!;
            _issuer = jwtSettings["Issuer"]!;
            _audience = jwtSettings["Audience"]!;
            _expiryInMinutes = int.Parse(jwtSettings["ExpiryInMinutes"]!);
        }

        public string GenerateToken(Usuarios usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
                new Claim(ClaimTypes.Email, usuario.email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(_expiryInMinutes),

                Issuer = _issuer,

                Audience = _audience,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_secretKey);

            return tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = _issuer,

                    ValidateAudience = true,
                    ValidAudience = _audience,

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                },
                out _);
        }
    }
}