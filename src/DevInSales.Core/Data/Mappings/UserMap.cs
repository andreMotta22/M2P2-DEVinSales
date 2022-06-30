using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace DevInSales.Core.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            builder.Property(u => u.Name)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(u => u.BirthDate)
                .HasColumnType("date")
                .IsRequired();

            // builder.HasData(
            //     new User(1, "Allie.Spencer@manuel.us", "661845", "Allie Spencer", new DateTime(1980, 10, 11)),
            //     new User(2, "Earnest@kari.biz", "800631", "Lemuel Witting", new DateTime(1980, 10, 11)),
            //     new User(3, "Adella_Shanahan@kenneth.biz", "661342", "Kari Olson I", new DateTime(1980, 10, 11)),
            //     new User(4, "Americo.Strosin@kale.tv", "661964", "Marion Nolan DDS", new DateTime(1980, 10, 11))
            // );
            var user1 = new User { 
                    Id = 1,
                    Email = "Allie.Spencer@manuel.us",  
                    UserName = "Allie Spencer",
                    NormalizedUserName = "ALLIESPENCER",
                    BirthDate = new DateTime(1980, 10, 11),
                    LockoutEnabled = false,
                    EmailConfirmed = true,
                    NormalizedEmail = "ALLIE.SPENCER@MANUEL.US",
                    Name = "Allie"
            };
            user1.PasswordHash =  passwordHasher.HashPassword(user1,"desenhos1");
            builder.HasData(user1);

            
        }
    }

}
