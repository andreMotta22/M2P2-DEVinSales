using System;
using Microsoft.OpenApi.Models;

namespace DevInSales.Api.Ioc.SwaggerInjection
{
    public static class SwaggerInjection
    {
        public static IServiceCollection AddSwaggerInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DevInSales API",
                    Version = "v1",
                    Description = "Projeto 2 do módulo 2 do curso DevInHouse da turma WPP",
                    Contact = new OpenApiContact
                    {
                        Name = "Turma WPP",
                        Url = new Uri("https://github.com/DEVin-Way2-Pixeon-Paradigma/M2P2-DEVinSales")
                    }
                });
                var xmlFile = "DevInSales.API.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the bearer scheme.
                                    Enter 'bearer' [space] and your token in the text input below.
                                    Exemple: 'Bearer 123432sadasdas'",
                    Name = "Authorization",
                    In = ParameterLocation.Header, //onde o paramtro vai ser colocado ? no header
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"                
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() 
                {
                    {
                        new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

             });
            return services;
        }
    }
}