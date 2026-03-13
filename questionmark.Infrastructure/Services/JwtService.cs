using questionmark.Application.Common.Contracts;
using questionmark.Infrastructure.Settings;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        public JwtService(JwtSettings jwtSettings) {
            _jwtSettings = jwtSettings;
        }
        public async Task<string> GetEncryptedUserIdFromRefreshToken(string token)
        {
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenKey)),
                ClockSkew = TimeSpan.Zero
            };
            var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(token, validationParameters);
            if (!tokenValidationResult.IsValid)
            {
                return string.Empty;
            }
            else
            {
                if (tokenValidationResult.Claims.TryGetValue(ClaimTypes.NameIdentifier, out var userIdObj))
                {
                    return userIdObj.ToString()!;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public async Task<string> GetEncryptedUserIdFromAccessToken(string token)
        {
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenKey)),
                ClockSkew = TimeSpan.Zero
            };
            var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(token, validationParameters);
            if (!tokenValidationResult.IsValid)
            {
                return string.Empty;
            }
            else
            {
                if (tokenValidationResult.Claims.TryGetValue(ClaimTypes.NameIdentifier, out var userIdObj))
                {
                    return userIdObj.ToString()!;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public async Task<bool> ValidateRefreshToken(string token)
        {
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenKey)),
                ClockSkew = TimeSpan.Zero
            };
            var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(token, validationParameters);
            return tokenValidationResult.IsValid;
        }
        public string GenerateAccessToken(List<Claim> claims)
        {
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMilliseconds(_jwtSettings.AccessTokenTTL),
                SigningCredentials = creds
            };
            jsonWebTokenHandler.SetDefaultTimesOnTokenCreation = false;
            var token = jsonWebTokenHandler.CreateToken(tokenDescriptor);
            return token;
        }
        public string GenerateRefreshToken(List<Claim> claims)
        {
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenTTL),
                SigningCredentials = creds
            };
            jsonWebTokenHandler.SetDefaultTimesOnTokenCreation = false;
            var token = jsonWebTokenHandler.CreateToken(tokenDescriptor);
            return token;
        }
        public int getRefreshTokenTTL()
        {
            return _jwtSettings.RefreshTokenTTL;
        }
    }
}
