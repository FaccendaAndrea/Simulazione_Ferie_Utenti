using System.ComponentModel.DataAnnotations;

namespace GestionePermessi.Api.DTOs;

public class RichiestaPermessoCreateDTO
{
    [Required(ErrorMessage = "La data di inizio è obbligatoria")]
    public DateTime DataInizio { get; set; }

    [Required(ErrorMessage = "La data di fine è obbligatoria")]
    public DateTime DataFine { get; set; }

    [Required(ErrorMessage = "La motivazione è obbligatoria")]
    [MinLength(5, ErrorMessage = "La motivazione deve essere di almeno 5 caratteri")]
    public string Motivazione { get; set; } = null!;

    [Required(ErrorMessage = "La categoria è obbligatoria")]
    public int CategoriaID { get; set; }
}

public class RichiestaPermessoDTO
{
    public int RichiestaID { get; set; }
    public DateTime DataRichiesta { get; set; }
    public DateTime DataInizio { get; set; }
    public DateTime DataFine { get; set; }
    public string Motivazione { get; set; } = null!;
    public string Stato { get; set; } = null!;
    public string CategoriaDescrizione { get; set; } = null!;
    public string UtenteNomeCompleto { get; set; } = null!;
    public DateTime? DataValutazione { get; set; }
    public string? UtenteValutazioneNomeCompleto { get; set; }
}

public class ValutazionePermessoDTO
{
    [Required(ErrorMessage = "Lo stato è obbligatorio")]
    [RegularExpression("^(Approvata|Rifiutata)$", ErrorMessage = "Lo stato deve essere 'Approvata' o 'Rifiutata'")]
    public string Stato { get; set; } = null!;
} 