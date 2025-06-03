namespace GestionePermessi.Api.DTOs;

public class RichiestaPermessoCreateDTO
{
    public DateTime DataInizio { get; set; }
    public DateTime DataFine { get; set; }
    public required string Motivazione { get; set; }
    public int CategoriaID { get; set; }
}

public class RichiestaPermessoDTO
{
    public int RichiestaID { get; set; }
    public DateTime DataRichiesta { get; set; }
    public DateTime DataInizio { get; set; }
    public DateTime DataFine { get; set; }
    public required string Motivazione { get; set; }
    public required string Stato { get; set; }
    public required string CategoriaDescrizione { get; set; }
    public required string UtenteNomeCompleto { get; set; }
    public DateTime? DataValutazione { get; set; }
    public string? UtenteValutazioneNomeCompleto { get; set; }
}

public class ValutazionePermessoDTO
{
    public required string Stato { get; set; } // "Approvata" o "Rifiutata"
} 