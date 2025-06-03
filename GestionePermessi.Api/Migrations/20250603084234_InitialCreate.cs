using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionePermessi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriePermessi",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descrizione = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriePermessi", x => x.CategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    UtenteID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Ruolo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.UtenteID);
                });

            migrationBuilder.CreateTable(
                name: "RichiestePermessi",
                columns: table => new
                {
                    RichiestaID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataRichiesta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataInizio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFine = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Motivazione = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Stato = table.Column<string>(type: "TEXT", nullable: false),
                    CategoriaID = table.Column<int>(type: "INTEGER", nullable: false),
                    UtenteID = table.Column<int>(type: "INTEGER", nullable: false),
                    DataValutazione = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UtenteValutazioneID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RichiestePermessi", x => x.RichiestaID);
                    table.ForeignKey(
                        name: "FK_RichiestePermessi_CategoriePermessi_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "CategoriePermessi",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RichiestePermessi_Utenti_UtenteID",
                        column: x => x.UtenteID,
                        principalTable: "Utenti",
                        principalColumn: "UtenteID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RichiestePermessi_Utenti_UtenteValutazioneID",
                        column: x => x.UtenteValutazioneID,
                        principalTable: "Utenti",
                        principalColumn: "UtenteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CategoriePermessi",
                columns: new[] { "CategoriaID", "Descrizione" },
                values: new object[,]
                {
                    { 1, "Ferie" },
                    { 2, "Permesso medico" },
                    { 3, "Permesso personale" }
                });

            migrationBuilder.InsertData(
                table: "Utenti",
                columns: new[] { "UtenteID", "Cognome", "Email", "Nome", "Password", "Ruolo" },
                values: new object[,]
                {
                    { 1, "Rossi", "mario.rossi@azienda.com", "Mario", "$2a$11$IQxWnl3W9K0RuNB8xXjfU.RQgVUGvp0p336FU/Zr5gXRu2kWyGrNi", "Responsabile" },
                    { 2, "Verdi", "giuseppe.verdi@azienda.com", "Giuseppe", "$2a$11$IQxWnl3W9K0RuNB8xXjfU.RQgVUGvp0p336FU/Zr5gXRu2kWyGrNi", "Dipendente" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RichiestePermessi_CategoriaID",
                table: "RichiestePermessi",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_RichiestePermessi_UtenteID",
                table: "RichiestePermessi",
                column: "UtenteID");

            migrationBuilder.CreateIndex(
                name: "IX_RichiestePermessi_UtenteValutazioneID",
                table: "RichiestePermessi",
                column: "UtenteValutazioneID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RichiestePermessi");

            migrationBuilder.DropTable(
                name: "CategoriePermessi");

            migrationBuilder.DropTable(
                name: "Utenti");
        }
    }
}
