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
                Nome = "Mario",
                Cognome = "Rossi",
                Email = "mario.rossi@azienda.com",
                Password = hashedPassword,
                Ruolo = "Responsabile"
            },
            new Utente
            {
                UtenteID = 2,
                Nome = "Giuseppe",
                Cognome = "Verdi",
                Email = "giuseppe.verdi@azienda.com",
                Password = hashedPassword,
                Ruolo = "Dipendente"
            }
        );
    }
} 