using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevInSales.Api.Controllers;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DevInSales.Testes.Controllers
{
    public class StateControllerTest
    {
        private readonly Mock<IStateService> _service;
        private readonly StateController _controler;
        public StateControllerTest()
        {
            _service = new Mock<IStateService>();
            _controler = new StateController(_service.Object);
        }

        [Fact]
        public void GetByStateId_IdNaoAchado_RetornaNotFound() 
        {
            _service.Setup(s => s.GetById(1)).Returns(value:null);

            var retorno = _controler.GetByStateId(1);

            Assert.IsAssignableFrom<NotFoundResult>(retorno);

        }

        [Fact]
        public void GetByStateId_IdMaiorQueZero_RetornaOk() 
        {
            var state = new ReadState(){Id = 1, Name = "Amazonia", Initials = "AM" };

            _service.Setup(s => s.GetById(1)).Returns(value: state);

            var retorno = _controler.GetByStateId(1);

            Assert.IsAssignableFrom<OkObjectResult>(retorno);
            _service.Verify(s => s.GetById(1), Times.Once);
            
        }

        [Theory]
        [InlineData("teste")]
        public void GetAll_NomeVazioOuNulo_RetornaNoContent(string? nome) {
            _service.Setup(s => s.GetAll(nome)).Returns(value: new List<ReadState>());
            
            var retorno = _controler.GetAll(nome);

            Assert.IsAssignableFrom<NoContentResult>(retorno); 
        }

        [Theory]
        [InlineData("Amazonia")]
        [InlineData("")]
        [InlineData(null)]
        public void GetAll_NomePreenchido_RetornaOk(string? nome) {
            _service.Setup(s => s.GetAll(nome)).Returns(value: new List<ReadState>(){new ReadState(){Id = 1, Name = "Amazonia", Initials = "AM" }});
            
            var retorno = _controler.GetAll(nome);

            Assert.IsAssignableFrom<OkObjectResult>(retorno); 
        }
    }
}