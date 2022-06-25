using DevInSales.Core.Data.Context;
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
            Seeds();
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
            //Seeds();
            var expected = Assert.Throws<ArgumentException>(() => _servico.CreateSaleByUserId(new Sale(idBuyer, idSeller, DateTime.Now)));
            Assert.Equal("BuyerId não encontrado.", expected.Message);
        }

        [Theory]
        [InlineData(1,5)]
        public void CreateSaleByUserId_SellerNaoExiste_RetornaException(int idBuyer, int idSeller)
        {
            //Seeds();
            var expected = Assert.Throws<ArgumentException>(() => _servico.CreateSaleByUserId(new Sale(idBuyer, idSeller, DateTime.Now)));
            Assert.Equal("SellerId não encontrado.", expected.Message);
        }

        [Theory]
        [InlineData(1, 2)]
        public void CreateSaleByUserId_InsereSeller_RetornaId(int idBuyer, int idSeller)
        {
            //Seeds();
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
            //Seeds();
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
           //Seeds();
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
            //Seeds();
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
            //Seeds();
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
            //Seeds();

            var expected = _servico.GetSaleBySellerId(id);
            Assert.Equal(0,expected.Count);
            Assert.Empty(expected);  
        }

        [Theory]
        [InlineData(1)]
        public void GetSaleByBuyerId_IdMaiorQueZero_RetornaLista(int id)
        {
            //Seeds();
            var lista = _context.Sales.Where(p => p.BuyerId == id).ToList();

            var expected = _servico.GetSaleByBuyerId(id);

            Assert.Equal(lista.Count, expected.Count);
            Assert.Contains<Sale>(lista.FirstOrDefault(), expected);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(null)]
        public void GetSaleByBuyerId_IdInexistente_RetornaNull(int id)
        {
            //Seeds();

            var expected = _servico.GetSaleByBuyerId(id);
            Assert.Equal(0, expected.Count);
            Assert.Empty(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        public void UpdateUnitPrice_SaleNãoAchado_RetornaException(int saleId) 
        {
            //Seeds();
            var resultado = Assert.Throws<Exception>(() => _servico.UpdateUnitPrice(saleId,1,5.00M));
            Assert.Equal("A sale não existe", resultado.Message);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        public void UpdateUnitPrice_SaleProductNãoAchado_RetornaException(int productId)
        {
            //Seeds();
            var resultado = Assert.Throws<Exception>(() => _servico.UpdateUnitPrice(1, productId, 5.00M));
            Assert.Equal("O Item procurado não existe", resultado.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        public void UpdateUnitPrice_PrecoMenorQueZero_RetornaException(decimal price)
        {
            //Seeds();
            var resultado = Assert.Throws<ArgumentException>(() => _servico.UpdateUnitPrice(1, 1, price));
            Assert.Equal("O preco do item não pode ser menor que zero", resultado.Message);
        }

        [Theory]
        [InlineData(0,1,20)]
        public void UpdateAmount_VendaNãoExiste_RetornaException(int saleId, int productId, int amount) 
        {
            var resultado = Assert.Throws<ArgumentException>(() => _servico.UpdateAmount(saleId, productId, amount));
            Assert.Equal("Não existe venda com esse Id. (Parameter 'saleId')", resultado.Message);
        }

        [Theory]
        [InlineData(1, 0, 20)]
        public void UpdateAmount_ProdutoVendidoNãoExiste_RetornaException(int saleId, int productId, int amount)
        {
            var resultado = Assert.Throws<ArgumentException>(() => _servico.UpdateAmount(saleId, productId, amount));
            Assert.Equal("Não existe este produto nesta venda. (Parameter 'productId')", resultado.Message);
        }

        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(1, 1, -1)]
        public void UpdateAmount_ValorMenorQueZero_RetornaException(int saleId, int productId, int amount)
        {
            var resultado = Assert.Throws<ArgumentException>(() => _servico.UpdateAmount(saleId, productId, amount));
            Assert.Equal("Quantidade não pode ser menor ou igual a zero. (Parameter 'amount')", resultado.Message);
        }

        [Theory]
        [InlineData(3,-1)]
        [InlineData(3, 0)]
        [InlineData(3, null)]
        public void CreateDeliveryForASale_SaleNull_RetornaException(int adress, int sale)
        {
           var result = Assert.Throws<ArgumentException>(() => _servico.CreateDeliveryForASale(new Delivery(adress, sale, DateTime.Now)));
            Assert.Equal("Não existe venda com esse Id. (Parameter 'saleId')", result.Message);
        }

        [Theory]
        [InlineData(-1,1)]
        [InlineData(0,1)]
        [InlineData(null,1)]
        public void CreateDeliveryForASale_AdressNull_RetornaException(int adress, int sale)
        {
            var result = Assert.Throws<ArgumentException>(() => _servico.CreateDeliveryForASale(new Delivery(adress, sale, DateTime.Now)));
            Assert.Equal("Não existe endereço com esse Id. (Parameter 'AddressId')", result.Message);

        }

        // acho que foi o melhor teste unitario que fiz até agr
        [Theory]
        [InlineData(1, 1)]
        public void CreateDeliveryForASale_AdressExiste_RetornaException(int adress, int sale)
        {
            var delivery = new Delivery(adress, sale, DateTime.Now);
            var id =_servico.CreateDeliveryForASale(delivery);
            Assert.Equal(delivery.Id, id);
            Assert.True(_context.Deliveries.Count() >= 1);
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
            _context.Addresses.Add(new Address("rua do figo","43244234",2,"ewe",1));
            _context.SaveChanges();

        }
    }
    
}
