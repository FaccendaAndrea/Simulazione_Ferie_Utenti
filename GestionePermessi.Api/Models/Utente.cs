using System.ComponentModel.DataAnnotations;

namespace GestionePermessi.Api.Models;

public class Utente
{
    [Key]
    public int UtenteID { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Nome { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Cognome { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; }
    
    [Required]
    public required string Ruolo { get; set; } // "Dipendente" o "Responsabile"
    
    // Navigation properties
    public ICollection<RichiestaPermesso> RichiesteEffettuate { get; set; } = new List<RichiestaPermesso>();
    public ICollection<RichiestaPermesso> RichiesteValutate { get; set; } = new List<RichiestaPermesso>();
} 