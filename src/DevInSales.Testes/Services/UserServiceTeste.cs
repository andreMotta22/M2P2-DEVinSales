using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevInSales.Testes.Services
{
    public class UserServiceTeste
    {
        private DataContext _context;
        private Mock<IUserManager<User>> _manager;
        private Mock<ISignInManager> _sign;
        private Mock<IRoleManager> _role;
        private Mock<ITokenService> _token;
        private UserService _service;

        public UserServiceTeste()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            _manager = new Mock<IUserManager<User>>();
            _sign = new Mock<ISignInManager>();
            _role = new Mock<IRoleManager>();
            _token = new Mock<ITokenService>();
            _service = new UserService(_manager.Object, _sign.Object, _role.Object, _token.Object, _context);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async void ObterLivroPorId_IdMenorOuIgual_A_Zero_RetornarNull(int id)
        {
            Seeds();
     
            var retorno = await _service.ObterPorId(id);
            
            Assert.Null(retorno);

        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void ObterLivroPorId_IdMaiorQueZero_RetornarUser(int id)
        {
            Seeds();
            var user = await _service.ObterPorId(id);
            var retorno = await _service.ObterPorId(id);

            Assert.Equal(user, retorno);

        }

        [Theory]
        [InlineData("Allie",null,null)]
        public void ObterUsers_SeNomeNaoForNuloOuVazio_RetornaUsers(string? nome,string? dataMin, string? dataMax )
        {
            Seeds();
            var user = _context.Users.Where(p => p.Name.Contains("Allie")).FirstOrDefault();
            var lista = _service.ObterUsers(nome,dataMin,dataMax);

            Assert.Contains(user, lista);
        }
        
        [Theory]
        [InlineData("", null, null)]
        [InlineData(null, null, null)]
        public void ObterUsers_SeNomeForNuloOuVazio_RetornaUsers(string? nome, string? dataMin, string? dataMax)
        {
            Seeds();
            var listaCompleta = _context.Users.ToList(); 
            var lista = _service.ObterUsers(nome, dataMin, dataMax);

            Assert.Equal(lista.Count, listaCompleta.Count);
        }

        [Theory]
        [InlineData(null, "11/10/1980", null)]
        public void ObterUsers_SeDataMinForDiferenteDeNuloOuVazio_RetornaUsers(string? nome, string? dataMin, string? dataMax)
        {
            Seeds();
            var expected = _context.Users.Where(user => user.BirthDate >= Convert.ToDateTime(dataMin)).FirstOrDefault();
            
            var listaEsperada = _context.Users.Where(user => user.BirthDate >= Convert.ToDateTime(dataMin)).ToList();

            var lista = _service.ObterUsers(nome,dataMin, dataMax);

            Assert.Contains<User>(expected,lista);
            Assert.Equal(lista.Count, 2);
            Assert.Equal(listaEsperada.Count, lista.Count);
        }
        
        [Theory]
        [InlineData(null, null, "16/04/1998")]
        public void ObterUsers_SeDataMaxForDiferenteDeNuloOuVazio_RetornaUsers(string? nome, string? dataMin, string? dataMax)
        {
            Seeds();
            var expected = _context.Users.Where(user => user.BirthDate <= Convert.ToDateTime(dataMax)).FirstOrDefault();

            var listaEsperada = _context.Users.Where(user => user.BirthDate <= Convert.ToDateTime(dataMax)).ToList();

            var lista = _service.ObterUsers(nome, dataMin, dataMax);

            Assert.Contains<User>(expected, lista);
            Assert.Equal(lista.Count, 2);
            Assert.Equal(listaEsperada.Count, lista.Count);
        }
        
        [Theory]
        [InlineData(null, "11/10/1980", "16/04/1998")]
        public void ObterUsers_SeDatasForemDiferenteDeNuloOuVazio_RetornaUsers(string? nome, string? dataMin, string? dataMax)
        {
            Seeds();
            var expected = _context.Users.Where(user => user.BirthDate >= Convert.ToDateTime(dataMin) &&
                                                        user.BirthDate <= Convert.ToDateTime(dataMax)).ToList();

            var lista = _service.ObterUsers(nome, dataMin, dataMax);

            Assert.Equal<User>(expected, lista);
            Assert.Equal(lista.Count, expected.Count);
            
        }

        [Theory]
        [InlineData("relaie", "11/10/1980", "16/04/1998")]
        public void ObterUsers_SeTodasAsInformacoesForemDiferenteDeNuloOuVazio_RetornaUsers(string? nome, string? dataMin, string? dataMax)
        {
            Seeds();
            var expectedList = _context.Users.
                Where(user => (user.BirthDate >= Convert.ToDateTime(dataMin)) && (user.BirthDate <= Convert.ToDateTime(dataMax)) 
                && user.Name.ToUpper().Contains(nome.ToUpper())).ToList();

            var lista = _service.ObterUsers(nome, dataMin, dataMax);

            Assert.Equal<User>(expectedList, lista);
            Assert.Equal(lista.Count, expectedList.Count);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public void RemoverUser_IdInvalido_RetornaException(int id) {
            Seeds();

            var expectedMessage = Assert.Throws<ArgumentException>(() => _service.RemoverUser(id));
            Assert.Equal("Impossivel pesquisar por um id desse tipo",expectedMessage.Message);
        }
  
        
        public void Seeds() {
            _context.Users.AddRange(new List<User>() {
                new User { 
                    Id = 1,
                    Email = "Allie.Spencer@manuel.us",  
                    UserName = "Allie Spencer",
                    NormalizedUserName = "ALLIESPENCER",
                    BirthDate = new DateTime(1980, 10, 11),
                    LockoutEnabled = false,
                    EmailConfirmed = true,
                    NormalizedEmail = "ALLIE.SPENCER@MANUEL.US",
                    Name = "Allie",
                    PasswordHash = "desenhos1"
                },
                new User { 
                    Id = 2,
                    Email = "relaie22@gmail.com",  
                    UserName = "Andre Relaie",
                    NormalizedUserName = "andrerelaie".ToUpper(),
                    BirthDate = new DateTime(1998, 04, 16),
                    LockoutEnabled = false,
                    EmailConfirmed = true,
                    NormalizedEmail = "relaie22@gmail.com".ToUpper(),
                    Name = "RELAIE",
                    PasswordHash = "desenhos2"
                }
            });
            _context.SaveChanges();
        }
    }
}
