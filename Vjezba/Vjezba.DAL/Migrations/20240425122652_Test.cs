using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vjezba.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Clients_ClientID",
                table: "Meeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting");

            migrationBuilder.RenameTable(
                name: "Meeting",
                newName: "Meetings");

            migrationBuilder.RenameIndex(
                name: "IX_Meeting_ClientID",
                table: "Meetings",
                newName: "IX_Meetings_ClientID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings",
                column: "ID");

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Zagreb" },
                    { 2, "Split" },
                    { 3, "Osijek" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ID", "Address", "CityID", "Email", "FirstName", "Gender", "LastName", "PhoneNumber" },
                values: new object[] { 1, "Ulica grada Mainza 3", 2, "ivan.horvat@gmail.com", "Ivan", "M", "Horvat", "0956789012" });

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Clients_ClientID",
                table: "Meetings",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Clients_ClientID",
                table: "Meetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings");

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "Meetings",
                newName: "Meeting");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_ClientID",
                table: "Meeting",
                newName: "IX_Meeting_ClientID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Clients_ClientID",
                table: "Meeting",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID");
        }
    }
}
