using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionePermessi.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG");

            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$IQxWnl3W9K0RuNB8xXjfU.RQgVUGvp0p336FU/Zr5gXRu2kWyGrNi");

            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$IQxWnl3W9K0RuNB8xXjfU.RQgVUGvp0p336FU/Zr5gXRu2kWyGrNi");
        }
    }
}
