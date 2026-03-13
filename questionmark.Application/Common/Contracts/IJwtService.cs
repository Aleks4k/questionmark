using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Common.Contracts
{
    public interface IJwtService
    {
        string GenerateAccessToken(List<Claim> claims);
        string GenerateRefreshToken(List<Claim> claims);
        int getRefreshTokenTTL();
        Task<bool> ValidateRefreshToken(string token);
        Task<string> GetEncryptedUserIdFromRefreshToken(string token);
        Task<string> GetEncryptedUserIdFromAccessToken(string token);
    }
}
