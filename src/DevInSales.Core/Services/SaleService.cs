using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;

using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class SaleService : ISaleService
    {
        private readonly DataContext _context;

        public SaleService(DataContext context)
        {
            _context = context;
        }
        public int CreateSaleByUserId(Sale sale)
        {
            
            if (sale.SaleDate == DateTime.MinValue)
                sale.SetSaleDateToToday();
            if (sale.BuyerId == 0 || sale.SellerId == 0)
                throw new ArgumentNullException("Id não pode ser nulo nem zero.");
            if (!_context.Users.Any(user => user.Id == sale.BuyerId))
                throw new ArgumentException("BuyerId não encontrado.");
            if (!_context.Users.Any(user => user.Id == sale.SellerId))
                throw new ArgumentException("SellerId não encontrado.");
                
            _context.Sales.Add(sale);
            _context.SaveChanges();

            return sale.Id;
        }

        public SaleResponse GetSaleById(int id)
        {
            Sale? sale = _context.Sales
                .Include(p => p.Buyer)
                .Include(p => p.Seller)
                .FirstOrDefault(p => p.Id == id);

            if (sale == null)
            {
                return null;
            }

            var listaProdutos = GetSaleProductsBySaleId(id);

            return new SaleResponse(sale.Id, sale.Seller.Name, sale.Buyer.Name, sale.SaleDate, listaProdutos);
        }

        public List<SaleProductResponse> GetSaleProductsBySaleId(int id)
        {
            return _context.SaleProducts
                .Where(p => p.SaleId == id)
                .Include(p => p.Products)
                .Select(p => new SaleProductResponse(p.Products.Name, p.Amount, p.UnitPrice, p.Amount * p.UnitPrice))
                .ToList();
        }

        public List<Sale> GetSaleBySellerId(int? userId)
        {
            return _context.Sales.Where(p => p.SellerId == userId).ToList();
        }

        public List<Sale> GetSaleByBuyerId(int? userId)
        {
            return _context.Sales.Where(p => p.BuyerId == userId).ToList();
        }
        public void UpdateUnitPrice(int saleId, int productId, decimal price)
        {
            Sale? sale = _context.Sales
                .FirstOrDefault(p => p.Id == saleId);
            if (sale == null)
                throw new Exception(); 

            SaleProduct? saleproduct = _context.SaleProducts                           
                .FirstOrDefault(p => p.ProductId == productId);

            if (saleproduct == null)
                throw new Exception();  
                  
            if(price <= 0)
                throw new ArgumentException();
                
            saleproduct.UpdateUnitPrice(price);         
            
            _context.SaveChanges();            
        }  
        
        public void UpdateAmount(int saleId, int productId, int amount)
        {

            if (!_context.Sales.Any(p => p.Id == saleId))
                throw new ArgumentException("Não existe venda com esse Id.", "saleId");

            var saleProduct = _context.SaleProducts.FirstOrDefault(p=> p.ProductId == productId);

            if (saleProduct == null)
                throw new ArgumentException("Não existe este produto nesta venda.", "productId");


            if (amount <= 0)
                throw new ArgumentException("Quantidade não pode ser menor ou igual a zero.", "amount");


            saleProduct.UpdateAmount(amount);

            _context.SaveChanges();
        }

        public int CreateDeliveryForASale(Delivery delivery)
        {
            Sale? sale = _context.Sales.FirstOrDefault(p => p.Id == delivery.SaleId);

            if (sale == null)
                throw new ArgumentException("Não existe venda com esse Id.","saleId");


            Address? address = _context.Addresses.FirstOrDefault(p => p.Id == delivery.AddressId);

            if (address == null)
                throw new ArgumentException("Não existe endereço com esse Id.", "AddressId");


            _context.Deliveries.Add(delivery);
            _context.SaveChanges();

            return delivery.Id;

        }

        public Delivery? GetDeliveryById(int deliveryId)
        {
            return _context.Deliveries.FirstOrDefault(p => p.Id == deliveryId);
        }
    }
}