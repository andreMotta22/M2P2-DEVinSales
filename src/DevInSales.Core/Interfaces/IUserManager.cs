using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Interfaces
{
    public interface IUserManager<TUser> where TUser : class
    {
        public Task<TUser> FindByEmailAsync(string email);
        public Task<IdentityResult> CreateAsync(TUser user,string password);
        public Task<IdentityResult> AddToRoleAsync(TUser user,string role);
        public Task<IdentityResult> SetLockoutEnabledAsync(TUser user,bool anwser);

    }
}
