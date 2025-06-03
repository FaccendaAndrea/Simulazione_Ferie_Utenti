using System.ComponentModel.DataAnnotations;

namespace GestionePermessi.Api.Models;

public class CategoriaPermesso
{
    [Key]
    public int CategoriaID { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Descrizione { get; set; }
    
    // Navigation property
    public ICollection<RichiestaPermesso> Richieste { get; set; } = new List<RichiestaPermesso>();
} 