using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInSales.Core.Migrations
{
    public partial class colocandoosnormalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "29265ac0-2c90-4248-9600-b9bb44051a70", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "d2732e63-3e4f-41e2-bf0e-5e59f1d62a4f", "GERENTE" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "9751afb2-8162-4604-9bf7-167bba11368d", "USUARIO" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "98a537d1-31c9-44ec-bc4f-f5de292e6e7f", "AQAAAAEAACcQAAAAELsXSVgjaKH3d49m8jKQFQ9gPVSHJgEItlhEnjAaKD5qARFOKhfrM6chP+2oX+fwUg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "d10ececc-2c65-48de-a007-83d01064171b", null });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "37d82308-9ff3-46a9-bf56-ac959e745cb2", null });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "cfd5b309-b56b-4f63-bcd9-48801dc6a48a", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6c01a31b-baac-4002-89e9-94cecdae28f0", "AQAAAAEAACcQAAAAEPoQ4SvLojvVVpT0ojtDER3veYuBVuOoB93JBx72aFEgamNB3zrTvDGU13ImscPmHQ==" });
        }
    }
}
