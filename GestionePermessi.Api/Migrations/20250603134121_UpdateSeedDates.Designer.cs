﻿// <auto-generated />
using System;
using GestionePermessi.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestionePermessi.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250603134121_UpdateSeedDates")]
    partial class UpdateSeedDates
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.5");

            modelBuilder.Entity("GestionePermessi.Api.Models.CategoriaPermesso", b =>
                {
                    b.Property<int>("CategoriaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descrizione")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("CategoriaID");

                    b.ToTable("CategoriePermessi");

                    b.HasData(
                        new
                        {
                            CategoriaID = 1,
                            Descrizione = "Ferie"
                        },
                        new
                        {
                            CategoriaID = 2,
                            Descrizione = "Permesso medico"
                        },
                        new
                        {
                            CategoriaID = 3,
                            Descrizione = "Permesso personale"
                        });
                });

            modelBuilder.Entity("GestionePermessi.Api.Models.RichiestaPermesso", b =>
                {
                    b.Property<int>("RichiestaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoriaID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataFine")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataInizio")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataRichiesta")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataValutazione")
                        .HasColumnType("TEXT");

                    b.Property<string>("Motivazione")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Stato")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UtenteID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UtenteValutazioneID")
                        .HasColumnType("INTEGER");

                    b.HasKey("RichiestaID");

                    b.HasIndex("CategoriaID");

                    b.HasIndex("UtenteID");

                    b.HasIndex("UtenteValutazioneID");

                    b.ToTable("RichiestePermessi");

                    b.HasData(
                        new
                        {
                            RichiestaID = 1,
                            CategoriaID = 1,
                            DataFine = new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataInizio = new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataRichiesta = new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Motivazione = "Ferie estive",
                            Stato = "In attesa",
                            UtenteID = 2
                        },
                        new
                        {
                            RichiestaID = 2,
                            CategoriaID = 2,
                            DataFine = new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataInizio = new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataRichiesta = new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataValutazione = new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Motivazione = "Visita medica",
                            Stato = "Approvata",
                            UtenteID = 2,
                            UtenteValutazioneID = 1
                        },
                        new
                        {
                            RichiestaID = 3,
                            CategoriaID = 3,
                            DataFine = new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataInizio = new DateTime(2025, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataRichiesta = new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataValutazione = new DateTime(2025, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Motivazione = "Permesso personale non urgente",
                            Stato = "Rifiutata",
                            UtenteID = 2,
                            UtenteValutazioneID = 1
                        },
                        new
                        {
                            RichiestaID = 4,
                            CategoriaID = 1,
                            DataFine = new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataInizio = new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataRichiesta = new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Motivazione = "Ferie brevi",
                            Stato = "In attesa",
                            UtenteID = 4
                        },
                        new
                        {
                            RichiestaID = 5,
                            CategoriaID = 3,
                            DataFine = new DateTime(2025, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataInizio = new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataRichiesta = new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataValutazione = new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Motivazione = "Permesso personale urgente",
                            Stato = "Approvata",
                            UtenteID = 4,
                            UtenteValutazioneID = 3
                        },
                        new
                        {
                            RichiestaID = 6,
                            CategoriaID = 2,
                            DataFine = new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataInizio = new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataRichiesta = new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DataValutazione = new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Motivazione = "Permesso medico non documentato",
                            Stato = "Rifiutata",
                            UtenteID = 4,
                            UtenteValutazioneID = 3
                        });
                });

            modelBuilder.Entity("GestionePermessi.Api.Models.Utente", b =>
                {
                    b.Property<int>("UtenteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Ruolo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UtenteID");

                    b.ToTable("Utenti");

                    b.HasData(
                        new
                        {
                            UtenteID = 1,
                            Cognome = "Faccenda",
                            Email = "csgopro@azienda.com",
                            Nome = "Andrea",
                            Password = "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG",
                            Ruolo = "Responsabile"
                        },
                        new
                        {
                            UtenteID = 2,
                            Cognome = "Gialli",
                            Email = "giorgiogialli@azienda.com",
                            Nome = "Giorgio",
                            Password = "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG",
                            Ruolo = "Dipendente"
                        },
                        new
                        {
                            UtenteID = 3,
                            Cognome = "Rossi",
                            Email = "mario.rossi@azienda.com",
                            Nome = "Mario",
                            Password = "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG",
                            Ruolo = "Responsabile"
                        },
                        new
                        {
                            UtenteID = 4,
                            Cognome = "Verdi",
                            Email = "giuseppe.verdi@azienda.com",
                            Nome = "Giuseppe",
                            Password = "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG",
                            Ruolo = "Dipendente"
                        });
                });

            modelBuilder.Entity("GestionePermessi.Api.Models.RichiestaPermesso", b =>
                {
                    b.HasOne("GestionePermessi.Api.Models.CategoriaPermesso", "Categoria")
                        .WithMany("Richieste")
                        .HasForeignKey("CategoriaID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GestionePermessi.Api.Models.Utente", "Utente")
                        .WithMany("RichiesteEffettuate")
                        .HasForeignKey("UtenteID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GestionePermessi.Api.Models.Utente", "UtenteValutazione")
                        .WithMany("RichiesteValutate")
                        .HasForeignKey("UtenteValutazioneID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Categoria");

                    b.Navigation("Utente");

                    b.Navigation("UtenteValutazione");
                });

            modelBuilder.Entity("GestionePermessi.Api.Models.CategoriaPermesso", b =>
                {
                    b.Navigation("Richieste");
                });

            modelBuilder.Entity("GestionePermessi.Api.Models.Utente", b =>
                {
                    b.Navigation("RichiesteEffettuate");

                    b.Navigation("RichiesteValutate");
                });
#pragma warning restore 612, 618
        }
    }
}
