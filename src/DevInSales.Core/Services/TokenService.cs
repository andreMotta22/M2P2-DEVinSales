using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DevInSales.Core.Configuration;
using DevInSales.Core.DTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DevInSales.Core.Services
{
    public class TokenService:ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions; 

        public TokenService(UserManager<User> userManager,
                            IOptions<JwtOptions> jtw)
        {
            _jwtOptions = jtw.Value;
            _userManager = userManager;
        }

        public async Task<UserLoginResponse> GetToken(string email) {
            
            // vai pegar todos os dados do usuario pelo email
            var userLogin = await _userManager.FindByEmailAsync(email);
            // Vai pegar todas as claims 
            var tokenClaims = await GetClaims(userLogin);
            
            var dataExpiracao = DateTime.Now.AddSeconds(_jwtOptions.Expire);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,     // issuer: quem é o emissor
                audience: _jwtOptions.Audience, // audience: pra quem está sendo emitido
                claims: tokenClaims,
                notBefore: DateTime.Now,        // só pode ser usado depois de tal data 
                expires: dataExpiracao,         // data de expiração    
                signingCredentials: _jwtOptions.SigningCredentials // SecretKey 
                
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            return new UserLoginResponse{
                Sucess =  true,
                Token = token,
            };    
        }
        public async Task<IList<Claim>> GetClaims(User user) {
            
            var claims = new List<Claim>();
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(user.Id)));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // é o id do token
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));            
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString())); 

            foreach(var role in roles)
                claims.Add(new Claim("role",role));

            return claims;
        }

    }
}