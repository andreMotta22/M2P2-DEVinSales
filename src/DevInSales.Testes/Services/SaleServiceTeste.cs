﻿using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevInSales.Testes.Services
{
    public class SaleServiceTeste
    {
        private readonly DataContext _context;
        private readonly SaleService _servico;
        public SaleServiceTeste()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            _servico = new SaleService(_context);
        }

        [Theory]
        [InlineData(0,1)]
        [InlineData(1,0)]
        [InlineData(0, 0)]
        public void CreateSaleByUserId_IdMaiorQueZero_RetornaException(int idBuyer, int idSeller)
        {
            var expected = Assert.Throws<ArgumentNullException>(() => _servico.CreateSaleByUserId(new Sale(idBuyer, idSeller, DateTime.Now)));
            Assert.Equal("Id não pode ser nulo nem zero.", expected.ParamName);
        }
        
        [Theory]
        [InlineData(5, 1)]
        public void CreateSaleByUserId_BuyerNaoExiste_RetornaException(int idBuyer, int idSeller)
        {
            Seeds();
            var expected = Assert.Throws<ArgumentException>(() => _servico.CreateSaleByUserId(new Sale(idBuyer, idSeller, DateTime.Now)));
            Assert.Equal("BuyerId não encontrado.", expected.Message);
        }

        [Theory]
        [InlineData(1,5)]
        public void CreateSaleByUserId_SellerNaoExiste_RetornaException(int idBuyer, int idSeller)
        {
            Seeds();
            var expected = Assert.Throws<ArgumentException>(() => _servico.CreateSaleByUserId(new Sale(idBuyer, idSeller, DateTime.Now)));
            Assert.Equal("SellerId não encontrado.", expected.Message);
        }

        [Theory]
        [InlineData(1, 2)]
        public void CreateSaleByUserId_InsereSeller_RetornaId(int idBuyer, int idSeller)
        {
            Seeds();
            var sale = new Sale(idBuyer, idSeller, DateTime.Now);

            var expectedId = 2;
            var resultId = _servico.CreateSaleByUserId(sale);

            Assert.Equal(expectedId, resultId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetSaleById_SellerNaoExiste_RetornaNull(int id) 
        {
            var sale = _servico.GetSaleById(id);
            Assert.Null(sale);
        }

        [Theory]
        [InlineData(1)]
        public void GetSaleById_SellerExiste_RetornaSellerResponse(int id)
        {
            Seeds();
            var sale1 =  _context.Sales.FirstOrDefault(u => u.Id == id);
            var sale2 = _servico.GetSaleById(id);

            Assert.Equal(sale1.Id, sale2.SaleId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]
        public void GetSaleProductsBySaleId_IdForIgualNull_RetornaListaVazia(int id ) 
        {
           Seeds();
           var saleProduct =  _context.SaleProducts.Where(p => p.SaleId == id).Include(p => p.Products)
                                 .Select(p => new SaleProductResponse(p.Products.Name, p.Amount, p.UnitPrice, p.Amount * p.UnitPrice))
                                 .ToList();
            var lista = _servico.GetSaleProductsBySaleId(id); 
           Assert.Equal(lista,saleProduct) ; 
        }

        [Theory]
        [InlineData(1)]
        public void GetSaleProductsBySaleId_IdMaioZero_RetornaLista(int id)
        {
            Seeds();
            var saleProduct = _context.SaleProducts.Where(p => p.SaleId == id).Include(p => p.Products)
                                  .Select(p => new SaleProductResponse(p.Products.Name, p.Amount, p.UnitPrice, p.Amount * p.UnitPrice))
                                  .ToList();

            var lista = _servico.GetSaleProductsBySaleId(id);
            Assert.Equal(lista.Count, saleProduct.Count);
        }

        [Theory]
        [InlineData(2)]
        public void GetSaleBySellerId_IdMaiorQueZero_RetornaLista(int id)
        {
            Seeds();
            var lista =  _context.Sales.Where(x => x.SellerId == id).ToList();
            
            var expected = _servico.GetSaleBySellerId(id);

            Assert.Equal(lista.Count, expected.Count);
            Assert.Contains<Sale>(lista.FirstOrDefault(), expected);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(null)]
        public void GetSaleBySellerId_IdInexistente_RetornaNull(int id)
        {
            Seeds();

            var expected = _servico.GetSaleBySellerId(id);
            Assert.Equal(0,expected.Count);
            Assert.Empty(expected);  
        }


        public void Seeds() 
        {
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
            _context.Sales.AddRange(new List<Sale>()
                {
                    new Sale(1,2,DateTime.Now)
                });
            _context.SaleProducts.Add(new SaleProduct(1, 1, 5, 40));
            _context.SaveChanges();

        }
    }
    
}
