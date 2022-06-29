using System;
using System.Collections.Generic;
using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DevInSales.Testes.Controllers
{
    public class DeliveryControllerTeste
    {
        
        private readonly Mock<IProductService> _service;
        private readonly ProductController _controler; 
        public DeliveryControllerTeste()
        {
            _service = new Mock<IProductService>();
            _controler = new ProductController(_service.Object);
        }

        [Fact]
        public void ObterProdutoPorId_IdMaiorQueZero_RetornaOk() 
        {
            var produto = new Product(1,"Xbox",2000M);
            
            _service.Setup(p => p.ObterProductPorId(1)).Returns(produto);

            var expected = _controler.ObterProdutoPorId(1);

            Assert.IsAssignableFrom<OkObjectResult>(expected.Result);
        
        }

        
        [Theory]
        [InlineData(0)]
        [InlineData(null)]
        [InlineData(-1)]
        public void ObterProdutoPorId_IdInexistente_RetornaNotFound(int id) 
        {
            // var produto = new Product(1,"Xbox",2000M);
            
            _service.Setup(p => p.ObterProductPorId(id)).Returns(value: null);

            var expected = _controler.ObterProdutoPorId(id);

            Assert.IsAssignableFrom<NotFoundResult>(expected.Result);
        
        }

        [Theory]
        [InlineData(1)]
        public void AtualizarProduto_ModeloVazio_RetornaNotFound(int id) 
        {
            AddProduct model = null;
    
            var expected = _controler.AtualizarProduto(model,id);

            Assert.IsAssignableFrom<NotFoundResult>(expected);
            _service.Verify(x => x.Atualizar(),Times.Never);
        }
        

        [Theory]
        [InlineData(1)]
        public void AtualizarProduto_ModeloComNomeString_RetornaBadRequest(int id) 
        {
            AddProduct model = new AddProduct("string",300M); 

            var expected = _controler.AtualizarProduto(model,id);
            
            Assert.IsAssignableFrom<BadRequestObjectResult>(expected);

            _service.Verify(x => x.Atualizar(),Times.Never);
            _service.Verify(x => x.ProdutoExiste("string"),Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public void AtualizarProduto_ProdutoJaExistente_RetornaBadRequest(int id) 
        {
            AddProduct model = new AddProduct("teste",300M); 
            _service.Setup(x => x.ProdutoExiste("teste")).Returns(true);

            var expected = _controler.AtualizarProduto(model,id);
            
            Assert.IsAssignableFrom<BadRequestObjectResult>(expected);

            _service.Verify(x => x.Atualizar(),Times.Never);
            _service.Verify(x => x.ProdutoExiste("teste"),Times.Once);
        }

        [Fact]
        public void AtualizarProduto_ProdutoAtualizado_RetornaNoContent() 
        {
            AddProduct model = new AddProduct("teste",300M); 

            _service.Setup(x => x.ObterProductPorId(1)).Returns(new Product(1,"xbox",200M));
            _service.Setup(x => x.ProdutoExiste("teste")).Returns(false);

            var expected = _controler.AtualizarProduto(model,1);
            
            Assert.IsAssignableFrom<NoContentResult>(expected);

            _service.Verify(x => x.Atualizar(),Times.Once);
            _service.Verify(x => x.ProdutoExiste("teste"),Times.Once);
            _service.Verify(x => x.ObterProductPorId(1),Times.Once);    
        }
        
        [Fact]
        public void GetAll_PrecoMaxMenorQuePrecoMin_RetonaBadRequest()
        {
            var retorno = _controler.GetAll("teste",20,10);
            Assert.IsAssignableFrom<BadRequestObjectResult>(retorno.Result);

            _service.Verify(s => s.ObterProdutos("teste",20,10),Times.Never);
        }
        [Fact]
        public void GetAll_ListaProdutoVazia_RetonaNoContent()
        {
            _service.Setup(s => s.ObterProdutos("teste",10,20)).Returns(new List<Product>());

            var retorno = _controler.GetAll("teste",10,20);
            
            Assert.IsAssignableFrom<NoContentResult>(retorno.Result);


            _service.Verify(s => s.ObterProdutos("teste",10,20),Times.Once);
        }
    }
}