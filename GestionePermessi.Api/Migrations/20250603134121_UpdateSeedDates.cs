using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionePermessi.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RichiestePermessi",
                columns: new[] { "RichiestaID", "CategoriaID", "DataFine", "DataInizio", "DataRichiesta", "DataValutazione", "Motivazione", "Stato", "UtenteID", "UtenteValutazioneID" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ferie estive", "In attesa", 2, null },
                    { 2, 2, new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Visita medica", "Approvata", 2, 1 },
                    { 3, 3, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Permesso personale non urgente", "Rifiutata", 2, 1 }
                });

            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 1,
                columns: new[] { "Cognome", "Email", "Nome" },
                values: new object[] { "Faccenda", "csgopro@azienda.com", "Andrea" });

            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 2,
                columns: new[] { "Cognome", "Email", "Nome" },
                values: new object[] { "Gialli", "giorgiogialli@azienda.com", "Giorgio" });

            migrationBuilder.InsertData(
                table: "Utenti",
                columns: new[] { "UtenteID", "Cognome", "Email", "Nome", "Password", "Ruolo" },
                values: new object[,]
                {
                    { 3, "Rossi", "mario.rossi@azienda.com", "Mario", "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG", "Responsabile" },
                    { 4, "Verdi", "giuseppe.verdi@azienda.com", "Giuseppe", "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG", "Dipendente" }
                });

            migrationBuilder.InsertData(
                table: "RichiestePermessi",
                columns: new[] { "RichiestaID", "CategoriaID", "DataFine", "DataInizio", "DataRichiesta", "DataValutazione", "Motivazione", "Stato", "UtenteID", "UtenteValutazioneID" },
                values: new object[,]
                {
                    { 4, 1, new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ferie brevi", "In attesa", 4, null },
                    { 5, 3, new DateTime(2025, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Permesso personale urgente", "Approvata", 4, 3 },
                    { 6, 2, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Permesso medico non documentato", "Rifiutata", 4, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RichiestePermessi",
                keyColumn: "RichiestaID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RichiestePermessi",
                keyColumn: "RichiestaID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RichiestePermessi",
                keyColumn: "RichiestaID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RichiestePermessi",
                keyColumn: "RichiestaID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RichiestePermessi",
                keyColumn: "RichiestaID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RichiestePermessi",
                keyColumn: "RichiestaID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 1,
                columns: new[] { "Cognome", "Email", "Nome" },
                values: new object[] { "Rossi", "mario.rossi@azienda.com", "Mario" });

            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "UtenteID",
                keyValue: 2,
                columns: new[] { "Cognome", "Email", "Nome" },
                values: new object[] { "Verdi", "giuseppe.verdi@azienda.com", "Giuseppe" });
        }
    }
}
