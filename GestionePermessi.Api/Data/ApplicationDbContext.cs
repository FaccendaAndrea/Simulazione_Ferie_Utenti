using Microsoft.EntityFrameworkCore;
using GestionePermessi.Api.Models;
using BCrypt.Net;

namespace GestionePermessi.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Utente> Utenti { get; set; }
    public DbSet<CategoriaPermesso> CategoriePermessi { get; set; }
    public DbSet<RichiestaPermesso> RichiestePermessi { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<RichiestaPermesso>()
            .HasOne(r => r.Utente)
            .WithMany(u => u.RichiesteEffettuate)
            .HasForeignKey(r => r.UtenteID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RichiestaPermesso>()
            .HasOne(r => r.UtenteValutazione)
            .WithMany(u => u.RichiesteValutate)
            .HasForeignKey(r => r.UtenteValutazioneID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RichiestaPermesso>()
            .HasOne(r => r.Categoria)
            .WithMany(c => c.Richieste)
            .HasForeignKey(r => r.CategoriaID)
            .OnDelete(DeleteBehavior.Restrict);

        // Add seed data
        modelBuilder.Entity<CategoriaPermesso>().HasData(
            new CategoriaPermesso { CategoriaID = 1, Descrizione = "Ferie" },
            new CategoriaPermesso { CategoriaID = 2, Descrizione = "Permesso medico" },
            new CategoriaPermesso { CategoriaID = 3, Descrizione = "Permesso personale" }
        );

        // Add test users
        // Hash pre-calcolato di "Password123!" usando BCrypt con work factor 11
        var hashedPassword = "$2a$11$K8H6sBpYaZBKJYrv0eQh2evNDx6wPfZbgTH6zUT4RRRA9.OKA.rKG";
        modelBuilder.Entity<Utente>().HasData(
            new Utente
            {
                UtenteID = 1,
                Nome = "Andrea",
                Cognome = "Faccenda",
                Email = "csgopro@azienda.com",
                Password = hashedPassword,
                Ruolo = "Responsabile"
            },
            new Utente
            {
                UtenteID = 2,
                Nome = "Giorgio",
                Cognome = "Gialli",
                Email = "giorgiogialli@azienda.com",
                Password = hashedPassword,
                Ruolo = "Dipendente"
            },
            new Utente
            {
                UtenteID = 3,
                Nome = "Mario",
                Cognome = "Rossi",
                Email = "mario.rossi@azienda.com",
                Password = hashedPassword,
                Ruolo = "Responsabile"
            },
            new Utente
            {
                UtenteID = 4,
                Nome = "Giuseppe",
                Cognome = "Verdi",
                Email = "giuseppe.verdi@azienda.com",
                Password = hashedPassword,
                Ruolo = "Dipendente"
            }
        );

        // Seed richieste permesso (copertura casi: in attesa, approvata, rifiutata, varie categorie)
        modelBuilder.Entity<RichiestaPermesso>().HasData(
            // In attesa (Giorgio Gialli)
            new RichiestaPermesso
            {
                RichiestaID = 1,
                DataRichiesta = new DateTime(2025, 6, 1),
                DataInizio = new DateTime(2025, 6, 10),
                DataFine = new DateTime(2025, 6, 12),
                Motivazione = "Ferie estive",
                Stato = "In attesa",
                CategoriaID = 1,
                UtenteID = 2
            },
            // Approvata (Giorgio Gialli, valutata da Andrea Faccenda)
            new RichiestaPermesso
            {
                RichiestaID = 2,
                DataRichiesta = new DateTime(2025, 7, 1),
                DataInizio = new DateTime(2025, 7, 10),
                DataFine = new DateTime(2025, 7, 12),
                Motivazione = "Visita medica",
                Stato = "Approvata",
                CategoriaID = 2,
                UtenteID = 2,
                DataValutazione = new DateTime(2025, 7, 5),
                UtenteValutazioneID = 1
            },
            // Rifiutata (Giorgio Gialli, valutata da Andrea Faccenda)
            new RichiestaPermesso
            {
                RichiestaID = 3,
                DataRichiesta = new DateTime(2025, 8, 1),
                DataInizio = new DateTime(2025, 8, 10),
                DataFine = new DateTime(2025, 8, 11),
                Motivazione = "Permesso personale non urgente",
                Stato = "Rifiutata",
                CategoriaID = 3,
                UtenteID = 2,
                DataValutazione = new DateTime(2025, 8, 12),
                UtenteValutazioneID = 1
            },
            // In attesa (Giuseppe Verdi)
            new RichiestaPermesso
            {
                RichiestaID = 4,
                DataRichiesta = new DateTime(2025, 9, 2),
                DataInizio = new DateTime(2025, 9, 15),
                DataFine = new DateTime(2025, 9, 16),
                Motivazione = "Ferie brevi",
                Stato = "In attesa",
                CategoriaID = 1,
                UtenteID = 4
            },
            // Approvata (Giuseppe Verdi, valutata da Mario Rossi)
            new RichiestaPermesso
            {
                RichiestaID = 5,
                DataRichiesta = new DateTime(2025, 10, 1),
                DataInizio = new DateTime(2025, 10, 10),
                DataFine = new DateTime(2025, 10, 12),
                Motivazione = "Permesso personale urgente",
                Stato = "Approvata",
                CategoriaID = 3,
                UtenteID = 4,
                DataValutazione = new DateTime(2025, 10, 13),
                UtenteValutazioneID = 3
            },
            // Rifiutata (Giuseppe Verdi, valutata da Mario Rossi)
            new RichiestaPermesso
            {
                RichiestaID = 6,
                DataRichiesta = new DateTime(2025, 11, 1),
                DataInizio = new DateTime(2025, 11, 10),
                DataFine = new DateTime(2025, 11, 11),
                Motivazione = "Permesso medico non documentato",
                Stato = "Rifiutata",
                CategoriaID = 2,
                UtenteID = 4,
                DataValutazione = new DateTime(2025, 11, 12),
                UtenteValutazioneID = 3
            }
        );
    }
} 