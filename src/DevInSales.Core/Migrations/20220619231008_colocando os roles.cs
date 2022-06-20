using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInSales.Core.Migrations
{
    public partial class colocandoosroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "d10ececc-2c65-48de-a007-83d01064171b", "Admin", null },
                    { 2, "37d82308-9ff3-46a9-bf56-ac959e745cb2", "Gerente", null },
                    { 3, "cfd5b309-b56b-4f63-bcd9-48801dc6a48a", "Usuario", null }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6c01a31b-baac-4002-89e9-94cecdae28f0", "AQAAAAEAACcQAAAAEPoQ4SvLojvVVpT0ojtDER3veYuBVuOoB93JBx72aFEgamNB3zrTvDGU13ImscPmHQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5cc2bafe-1888-47bc-b320-29b6bf89e526", "AQAAAAEAACcQAAAAELmI4uL262xrQqEnju74+38S79w7jMhem1IsvO3GBfwQkWM0FtajZoCcuzM1oteZvw==" });
        }
    }
}
