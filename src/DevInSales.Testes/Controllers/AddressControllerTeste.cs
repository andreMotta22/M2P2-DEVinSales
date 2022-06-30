using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DevInSales.Testes.Controllers
{
    public class AddressControllerTeste
    {
        private readonly Mock<IAddressService> _addressService;
        private readonly Mock<IStateService> _stateService;
        private readonly Mock<ICityService> _cityService;

        private readonly AddressController _controller;
        public AddressControllerTeste() 
        {
            _addressService = new Mock<IAddressService>();
            _cityService = new Mock<ICityService>();
            _stateService = new Mock<IStateService>();

            _controller = new AddressController(_addressService.Object, _stateService.Object, _cityService.Object);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(null)]
        public void UpdateAddress_IdInvalido_RetornaNotFound(int id ) {
            _addressService.Setup(x => x.GetById(id)).Returns(value:null);
            var retorno = (_controller.UpdateAddress(id,new UpdateAddress("rua",3,"D", "223423"))) as NotFoundResult;

            Assert.Equal(404,retorno?.StatusCode); 
        }

        [Theory]
        // [InlineData(null,2,"d","32432")]
        // [InlineData("Rua",null,"d","32432")]
        // [InlineData("Rua",2,null,"32432")]
        // [InlineData("Rua",2,"D",null)]
        [InlineData(null,null,null,null)]
        public void UpdateAddress_DadosInvalidos_RetornaBadRequest(string? rua, int? number, string? complement, string? cep ) {
            _addressService.Setup(x => x.GetById(1)).Returns(value: new Address("rua","adasd",4,"d",2));
            
            var retorno = _controller.UpdateAddress(1,new UpdateAddress(rua,number,complement, cep)) as BadRequestResult;

            Assert.Equal(400,retorno?.StatusCode); 
            _addressService.Verify(x => x.GetById(1),Times.Once);
        }

        [Theory]
        [InlineData("Rua",4,"d","32432")]
        public void UpdateAddress_DadosValidos_RetornaNoContent(string? rua, int? number, string? complement, string? cep ) {
            
            var endereco = new Address("rua","adasd",4,"d",2);
            
            _addressService.Setup(x => x.GetById(1)).Returns(value:endereco );
            _addressService.Setup(x => x.Update(endereco));
            
            var retorno = _controller.UpdateAddress(1,new UpdateAddress(rua,number,complement, cep)) as NoContentResult;

            Assert.Equal(204,retorno?.StatusCode); 
            _addressService.Verify(x => x.GetById(1),Times.Once);
            _addressService.Verify(x => x.Update(endereco));
        }
    }
}