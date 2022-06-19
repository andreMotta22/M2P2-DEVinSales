using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInSales.Core.Migrations
{
    public partial class colocandoumasenha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "5cc2bafe-1888-47bc-b320-29b6bf89e526", "ALLIESPENCER", "AQAAAAEAACcQAAAAELmI4uL262xrQqEnju74+38S79w7jMhem1IsvO3GBfwQkWM0FtajZoCcuzM1oteZvw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "5d7d6a91-0f96-4552-aadb-605171c5e261", null, null });
        }
    }
}
