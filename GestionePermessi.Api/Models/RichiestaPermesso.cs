using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionePermessi.Api.Models;

public class RichiestaPermesso
{
    [Key]
    public int RichiestaID { get; set; }
    
    [Required]
    public DateTime DataRichiesta { get; set; }
    
    [Required]
    public DateTime DataInizio { get; set; }
    
    [Required]
    public DateTime DataFine { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Motivazione { get; set; } = null!;
    
    [Required]
    public string Stato { get; set; } = null!; // "In attesa", "Approvata", "Rifiutata"
    
    [Required]
    public int CategoriaID { get; set; }
    [ForeignKey("CategoriaID")]
    public CategoriaPermesso? Categoria { get; set; }
    
    [Required]
    public int UtenteID { get; set; }
    [ForeignKey("UtenteID")]
    public Utente? Utente { get; set; }
    
    public DateTime? DataValutazione { get; set; }
    
    public int? UtenteValutazioneID { get; set; }
    [ForeignKey("UtenteValutazioneID")]
    public Utente? UtenteValutazione { get; set; }
} 