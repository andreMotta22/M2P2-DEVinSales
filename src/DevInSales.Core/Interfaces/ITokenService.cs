using System.Security.Claims;
using DevInSales.Core.DTOs;
using DevInSales.Core.Entities;

namespace DevInSales.Core.Interfaces
{
    public interface ITokenService
    {
        public Task<UserLoginResponse> GetToken(string email);
        public Task<IList<Claim>> GetClaims(User user);
    }
}