using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevInSales.Api.Controllers;
using DevInSales.Core.DTOs;
using DevInSales.Core.Entities;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DevInSales.Testes.Controllers
{
    public class UserControllerTeste
    {
        private readonly Mock<IUserService> _service; 
        private readonly UserController _controller; 
        public UserControllerTeste() 
        {
            _service = new Mock<IUserService>();
            _controller = new UserController(_service.Object);
        }

        [Fact]
        public void ObterUsers_UsuarioNaoAchado_RetornaNoContent() 
        {
            _service.Setup(s => s.ObterUsers("andre", "16/04/1998", "04/05/2010")).Returns(value:new List<User>());
            
            var retorno = _controller.ObterUsers("andre", "16/04/1998", "04/05/2010");
            
            Assert.IsAssignableFrom<NoContentResult>(retorno.Result);
        }

        [Fact]
        public void ObterUsers_UsuarioAchado_RetornaOk() 
        {
            _service.Setup(s => s.ObterUsers("andre", "16/04/1998", "04/05/2010")).Returns(value:new List<User>() {
                new User(){Id = 1, Name = "andre", UserName = "relaie", Email = "relaie22@gmail.com", BirthDate = DateTime.Now }
            });
            
            var retorno = _controller.ObterUsers("andre", "16/04/1998", "04/05/2010").Result as OkObjectResult;
            
            Assert.Equal(200,retorno?.StatusCode);
            
            // retorna o objeto, convertido para o tipo List<UserResponse quando bem-sucedido
            var items = Assert.IsType<List<UserResponse>>(retorno?.Value);
            Assert.Equal(1, items.Count);
            
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(null)]
        public async Task ObterPorId_IdInvalido_RetornaNotFound(int id)
        {
            _service.Setup(s => s.ObterPorId(id)).ReturnsAsync(value:null);
            var retorno = await (_controller.ObterUserPorId(id));
            var result = retorno.Result as NotFoundResult;

            Assert.Equal(404, result?.StatusCode);
            _service.Verify(s => s.ObterPorId(id),Times.Once);

        }

        [Fact]
        public async Task ObterPorId_IdValido_RetornaOk()
        {
            var usuario = new User(){Id = 1, Name = "andre", UserName = "relaie", Email = "relaie22@gmail.com", BirthDate = DateTime.Now };
            
            _service.Setup(s => s.ObterPorId(1)).ReturnsAsync(value:usuario);

            var retorno = await (_controller.ObterUserPorId(1));
            var result = retorno.Result as OkObjectResult;

            Assert.Equal(200, result?.StatusCode);
            
            var user = Assert.IsType<UserResponse>(result?.Value);
            
            Assert.Equal(usuario.Id,user.Id);
            
            _service.Verify(s => s.ObterPorId(1),Times.Once);


        }

        [Fact]
        public async Task CriarUser_CadastrarUser_RetornaOk() 
        {
            var usuarioNovo = new UserRequest(){Name = "teste", UserName = "teste22", Email = "teste@gmail.com", BirthDate = DateTime.Now };
            var retornoResponse =  new UserCadastroResponse(true);
            _service.Setup(x => x.CriarUser(usuarioNovo)).ReturnsAsync(retornoResponse);
            
            var retorno = await _controller.CriarUser(usuarioNovo);
            var response = retorno.Result as OkObjectResult;
            
            Assert.Equal(200, response?.StatusCode);
            _service.Verify(x => x.CriarUser(usuarioNovo),Times.Once);
        }

        [Fact]
        public async Task LogarUser_EntrarComUsuario_RetornaOk() 
        {
            var usuario = new UserLoginRequest() {Email = "relaie22@gmail.com", Password = "123456"};

            var retornoResponse =  new UserLoginResponse {Sucess = true, Token = "asdla23oeje89dujalkdnk2q",Erro = ""};
            
            _service.Setup(x => x.LogarUser(usuario)).ReturnsAsync(retornoResponse);
            var retorno = (await _controller.LogarUser(usuario)).Result as OkObjectResult;

            var resultado = Assert.IsType<UserLoginResponse>(retorno?.Value);

            Assert.True(resultado.Sucess);
            Assert.Equal(200, retorno?.StatusCode);
            _service.Verify(x => x.LogarUser(usuario),Times.Once);
        }

        [Fact]
        public async Task LogarUser_EntrarComUsuario_Unauthorized() 
        {
            var usuario = new UserLoginRequest() {Email = "relaie22@gmail.com", Password = "123456"};

            var retornoResponse =  new UserLoginResponse {Sucess = false, Token = "asdla23oeje89dujalkdnk2q",Erro = "senha e usuario incorretos"};
            
            _service.Setup(x => x.LogarUser(usuario)).ReturnsAsync(retornoResponse);

            var retorno = (await _controller.LogarUser(usuario)).Result as UnauthorizedObjectResult;

            Assert.Equal(401, retorno?.StatusCode);
            _service.Verify(x => x.LogarUser(usuario),Times.Once);
        }

        [Fact]
        public void ExcluirUser_IdValido_RetornaNoContent() 
        {
            _service.Setup(x => x.RemoverUser(1));
            var result = _controller.ExcluirUser(1) as NoContentResult;
            
            Assert.Equal(204, result.StatusCode);
            _service.Verify(x => x.RemoverUser(1),Times.Once);
        }
    }
}