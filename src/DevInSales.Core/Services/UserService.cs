using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DevInSales.Core.Data.Context;
using DevInSales.Core.DTOs;
using DevInSales.Core.Interfaces;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DevInSales.Core.Entities
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _userSign;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ITokenService _token;

        public UserService(UserManager<User> userManager,
                           SignInManager<User> sign,
                           RoleManager<ApplicationRole> roleManager,
                           ITokenService token,
                           DataContext context)
        {
            _userManager = userManager;
            _userSign = sign;
            _roleManager = roleManager;
            _token = token;
            _context = context;
        }

        public async Task<UserCadastroResponse> CriarUser(UserRequest user)
        {
            var newUser = new User {
                UserName = user.UserName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Name = user.Name,
                EmailConfirmed = true
            };
            var role = user.Role.ToUpper();
            
            var result =  await _userManager.CreateAsync(newUser, user.Password);

            if(result.Succeeded) {
                if(await _roleManager.RoleExistsAsync(char.ToUpper(role[0]) + role.Substring(1)))
                    await _userManager.AddToRoleAsync(newUser,char.ToUpper(role[0]) + role.Substring(1));
                else 
                    await _userManager.AddToRoleAsync(newUser,"Usuario");
            
                await _userManager.SetLockoutEnabledAsync(newUser,false);
            }
            var response = new UserCadastroResponse(result.Succeeded);

            if(!result.Succeeded && result.Errors.Count() > 0 ) {
                response.AdicionarErros(result.Errors.Select(erro => erro.Description));
            }
            return response;
        }

        public async Task<UserLoginResponse> LogarUser(UserLoginRequest user){
            
            User? usuario = await _userManager.FindByEmailAsync(user.Email);

            var result = await _userSign.PasswordSignInAsync(usuario?.UserName ?? "",user.Password,false,false);
            
            if(result.Succeeded) {
                return await _token.GetToken(user.Email);
            }
            else {
                var login = new UserLoginResponse();
                if(result.IsLockedOut)
                    login.Erro = "Esta conta está bloqueada";
                else if(result.IsNotAllowed)
                    login.Erro = "Esta conta não pode fazer login atualmente";
                else if(result.RequiresTwoFactor)
                    login.Erro = "Requer a autenticação de 2 fatores";
                else
                    login.Erro = "Usuario ou Senha estão incorretos";
                return login;    
            }
        }

        public async Task<User?> ObterPorId(int id)
        {
           return await _context.Users.FindAsync(id);
        }
        public List<User> ObterUsers(string? name, string? DataMin, string? DataMax)
        {
            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.ToUpper().Contains(name.ToUpper()));
            if (!string.IsNullOrEmpty(DataMin))
                query = query.Where(p => p.BirthDate >= DateTime.Parse(DataMin));
            if (!string.IsNullOrEmpty(DataMax))
                query = query.Where(p => p.BirthDate <= DateTime.Parse(DataMax));

            return query.ToList();
        }
        
        public void RemoverUser(int id)
        {
            if (id >= 0)
            {
                var user = _context.Users.FirstOrDefault(user => user.Id == id);
                if (user != null)
                    _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
        
    }
}