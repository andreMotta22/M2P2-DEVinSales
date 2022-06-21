using System.Text;
using DevInSales.Core.Configuration;
using DevInSales.Core.Interfaces;
using DevInSales.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DevInSales.Api.Ioc.JwtInjection
{
    public static class JwtInjection
    {
        public static IServiceCollection AddIocJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService,TokenService>();    

            var jwtOption = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOption["SecurityKey"]));
            
            services.Configure<JwtOptions>( jwt => {
                jwt.Issuer = jwtOption["Issuer"];
                jwt.Audience = jwtOption["Audience"];
                jwt.Expire = Convert.ToInt32(jwtOption["Expiration"]);
                jwt.SigningCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // estamos validando o emissor, se o emissor for diferente do qual está setado no appSettings o token não vai passar
                ValidateIssuer = false,
                ValidIssuer = jwtOption["Issuer"],

                // vamos validar a audiencia tambem'
                ValidateAudience = false,
                ValidAudience = jwtOption["Audience"],

                // vamos validar a assinatura
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                // vamos validar o tempo de vida do token
                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
            }; 

            // Estamos falando que o asp net vai trabalhar com autenticação, mas com a autenticação do jwtBearer
            services.AddAuthentication(options => 
            {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    // adicionamos o suporte ao jwt
            })
            .AddJwtBearer(options => 
                {
                    // Estamos passando os requisitos para a validação dos paremetros
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            return services;
        }   
    }
}    