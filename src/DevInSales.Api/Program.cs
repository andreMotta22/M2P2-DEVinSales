using System.Text;
using DevInSales.Api.Ioc.IdentityInjection;
using DevInSales.Api.Ioc.JwtInjection;
using DevInSales.Api.Ioc.SwaggerInjection;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using DevInSales.Core.Services;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerInjection(builder.Configuration);
builder.Services.AddIocIdentity(builder.Configuration);
builder.Services.AddIocJwt(builder.Configuration);


builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<ISaleProductService, SaleProductService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IUserManager<User>,UserManager>();
builder.Services.AddScoped<IRoleManager,Role>();
builder.Services.AddScoped<ISignInManager, Sign>();

// var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Jwt:SecurityKey").Value));


// builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "DevInSales API",
//         Version = "v1",
//         Description = "Projeto 2 do módulo 2 do curso DevInHouse da turma WPP",
//         Contact = new OpenApiContact
//         {
//             Name = "Turma WPP",
//             Url = new Uri("https://github.com/DEVin-Way2-Pixeon-Paradigma/M2P2-DEVinSales")
//         }
//     });
//     var xmlFile = "DevInSales.API.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

//     c.IncludeXmlComments(xmlPath);

//     c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
//     {
//         Description = @"JWT Authorization header using the bearer scheme.
//                         Enter 'bearer' [space] and your token in the text input below.
//                         Exemple: 'Bearer 123432sadasdas'",
//         Name = "Authorization",
//         In = ParameterLocation.Header, //onde o paramtro vai ser colocado ? no header
//         Type = SecuritySchemeType.ApiKey,
//         Scheme = "Bearer"                
//     });
//     c.AddSecurityRequirement(new OpenApiSecurityRequirement() 
//     {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                },
//                Scheme = "oauth2",
//                Name = "Bearer",
//                In = ParameterLocation.Header,
//             },
//                new List<string>()
//         }
//     });

// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication().UseAuthorization();
app.MapControllers();

app.Run();
