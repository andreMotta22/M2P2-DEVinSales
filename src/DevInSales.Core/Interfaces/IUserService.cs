using System.Security.Claims;
using DevInSales.Core.DTOs;
using DevInSales.Core.Entities;

namespace DevInSales.EFCoreApi.Core.Interfaces
{
    public interface IUserService
    {
        public List<User> ObterUsers(string? name, string? DateMin, string? DateMax);
        public Task<User?> ObterPorId(int id);
        public Task<UserCadastroResponse> CriarUser(UserRequest user);
        public Task<UserLoginResponse> LogarUser(UserLoginRequest user);

        // public void RemoverUser(int id);
    }
}