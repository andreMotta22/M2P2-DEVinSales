using Microsoft.AspNetCore.Identity;

namespace DevInSales.Core.Interfaces
{
    public interface ISignInManager
    {
        public Task<SignInResult> PasswordSignInAsync(string userName,string password, bool isPersistente, 
        bool lockOnFailure);
        
    }
}