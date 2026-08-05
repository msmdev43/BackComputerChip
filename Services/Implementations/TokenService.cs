using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using computerChip.Models;
using computerChip.Repositories.Interfaces;
using computerChip.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace computerChip.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public TokenService(
            ITokenRepository tokenRepository,
            IUsuarioRepository usuarioRepository,
            IConfiguration configuration)
        {
            _tokenRepository = tokenRepository;
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        // ============================================
        // OPERACIONES DE LECTURA
        // ============================================

        public async Task<SantanderToken?> GetByUsuarioAsync(int usuarioId)
        {
            return await _tokenRepository.GetByUsuarioAsync(usuarioId);
        }

        public async Task<SantanderToken?> GetValidTokenAsync(int usuarioId)
        {
            return await _tokenRepository.GetValidTokenAsync(usuarioId);
        }

        public async Task<bool> TokenExistsAsync(int usuarioId)
        {
            return await _tokenRepository.TokenExistsAsync(usuarioId);
        }

        public async Task<bool> IsTokenExpiredAsync(int usuarioId)
        {
            return await _tokenRepository.IsTokenExpiredAsync(usuarioId);
        }

        public async Task<SantanderToken?> GetByAccessTokenAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return null;

            return await _tokenRepository.GetByAccessTokenAsync(accessToken);
        }

        public async Task<SantanderToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            return await _tokenRepository.GetByRefreshTokenAsync(refreshToken);
        }

        // ============================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================

        public async Task<bool> SaveTokenAsync(int usuarioId, string accessToken, string refreshToken, int expiresIn)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("El access token no puede estar vacío", nameof(accessToken));

            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("El refresh token no puede estar vacío", nameof(refreshToken));

            if (expiresIn <= 0)
                throw new ArgumentException("El tiempo de expiración debe ser mayor a 0", nameof(expiresIn));

            // Verificar que el usuario existe
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null || usuario.deletedAt != null)
                throw new InvalidOperationException("El usuario no existe o está desactivado");

            return await _tokenRepository.UpdateTokenAsync(usuarioId, accessToken, refreshToken, expiresIn);
        }

        public async Task<bool> RefreshTokenAsync(int usuarioId, string newAccessToken, string newRefreshToken, int expiresIn)
        {
            if (string.IsNullOrWhiteSpace(newAccessToken))
                throw new ArgumentException("El nuevo access token no puede estar vacío", nameof(newAccessToken));

            if (string.IsNullOrWhiteSpace(newRefreshToken))
                throw new ArgumentException("El nuevo refresh token no puede estar vacío", nameof(newRefreshToken));

            if (expiresIn <= 0)
                throw new ArgumentException("El tiempo de expiración debe ser mayor a 0", nameof(expiresIn));

            // Verificar que el token exista antes de actualizar
            if (!await _tokenRepository.TokenExistsAsync(usuarioId))
                return false;

            return await _tokenRepository.UpdateTokenAsync(usuarioId, newAccessToken, newRefreshToken, expiresIn);
        }

        public async Task<bool> RevokeTokenAsync(int usuarioId)
        {
            var token = await _tokenRepository.GetByUsuarioAsync(usuarioId);
            if (token == null)
                return false;

            return await _tokenRepository.RevokeTokenAsync(usuarioId);
        }

        public async Task<int> CleanExpiredTokensAsync()
        {
            try
            {
                // Obtener todos los tokens
                var allTokens = await _tokenRepository.GetAllAsync();
                var count = 0;

                foreach (var token in allTokens)
                {
                    var timeElapsed = DateTime.Now - token.createdAt;
                    if (timeElapsed.TotalSeconds >= token.expiresIn)
                    {
                        _tokenRepository.Remove(token);
                        count++;
                    }
                }

                await _tokenRepository.SaveChangesAsync();
                return count;
            }
            catch
            {
                return 0;
            }
        }

        // ============================================
        // OPERACIONES AUXILIARES - JWT
        // ============================================

        public async Task<string> GenerateAccessTokenAsync(int usuarioId)
        {
            // Obtener el usuario
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null || usuario.deletedAt != null)
                throw new InvalidOperationException("El usuario no existe o está desactivado");

            // Configuración JWT
            var secretKey = _configuration["JWT:SecretKey"] ?? "tu-super-secret-key-minimo-32-caracteres";
            var issuer = _configuration["JWT:Issuer"] ?? "ComputerChip";
            var audience = _configuration["JWT:Audience"] ?? "ComputerChipUsers";
            var expireMinutes = int.Parse(_configuration["JWT:ExpireMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new Claim(ClaimTypes.Email, usuario.email ?? string.Empty),
                new Claim(ClaimTypes.Name, usuario.nombreCompleto ?? string.Empty),
                new Claim("usuarioId", usuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            // Crear token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            return await Task.FromResult(GenerateRefreshToken());
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<bool> ValidateAccessTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var secretKey = _configuration["JWT:SecretKey"] ?? "tu-super-secret-key-minimo-32-caracteres";
                var issuer = _configuration["JWT:Issuer"] ?? "ComputerChip";
                var audience = _configuration["JWT:Audience"] ?? "ComputerChipUsers";

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out _);
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int?> GetUsuarioIdFromTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var secretKey = _configuration["JWT:SecretKey"] ?? "tu-super-secret-key-minimo-32-caracteres";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var usuarioIdClaim = principal.FindFirst("usuarioId") ?? principal.FindFirst(ClaimTypes.NameIdentifier);

                if (usuarioIdClaim != null && int.TryParse(usuarioIdClaim.Value, out var usuarioId))
                    return await Task.FromResult(usuarioId);

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DateTime?> GetTokenExpirationAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
                if (expClaim != null && long.TryParse(expClaim.Value, out var expSeconds))
                {
                    var expiration = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                    return await Task.FromResult(expiration);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> RefreshJwtTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            try
            {
                // Buscar el token en la base de datos
                var santanderToken = await _tokenRepository.GetByRefreshTokenAsync(refreshToken);
                if (santanderToken == null)
                    return null;

                // Verificar que el refresh token no haya expirado
                var timeElapsed = DateTime.Now - santanderToken.createdAt;
                if (timeElapsed.TotalSeconds >= santanderToken.expiresIn)
                    return null;

                // Generar nuevo access token
                var newAccessToken = await GenerateAccessTokenAsync(santanderToken.usuarioId);

                // Actualizar el token en la base de datos (opcional, si quieres mantener el refresh token)
                // await _tokenRepository.UpdateTokenAsync(santanderToken.usuarioId, newAccessToken, refreshToken, santanderToken.expiresIn);

                return newAccessToken;
            }
            catch
            {
                return null;
            }
        }
    }
}