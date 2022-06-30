using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevInSales.Core.Data.Mappings
{
    public class RolesMapping : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new List<ApplicationRole>()
                {
                    new ApplicationRole {Id = 1, Name = "Admin", NormalizedName = "ADMIN"},
                    new ApplicationRole {Id = 2, Name = "Gerente" , NormalizedName = "GERENTE"},
                    new ApplicationRole {Id = 3, Name = "Usuario" , NormalizedName = "USUARIO"}
                }
            );
        }
    }
}