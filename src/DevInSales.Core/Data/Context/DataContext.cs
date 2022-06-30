using System.Reflection;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Data.Context
{
    public class DataContext : IdentityDbContext<User,ApplicationRole,int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            /*
                Ao fazermos o mapeamento em arquivos separados, acabamos por dar um override no 
                OnModelCreating do identity o que é um tanto irritante do identity que tornar 
                necessario mapearmos todos as propriedades do identity novamente. 
                Com o trecho de codigo abaixo nos livra de fazer isso, já que no fim acabamos 
                invocando o metodo pai do identity. 
            */  
            base.OnModelCreating(modelBuilder);

           
        }

        // public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses{ get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }

    }
}
        
  
