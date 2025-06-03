namespace GestionePermessi.Api.DTOs;

public class StatisticheDTO
{
    public required string UtenteNomeCompleto { get; set; }
    public int Anno { get; set; }
    public int Mese { get; set; }
    public int GiorniTotali { get; set; }
    public required string CategoriaDescrizione { get; set; }
}

public class StatisticheFilterDTO
{
    public int? Anno { get; set; }
    public int? Mese { get; set; }
    public int? CategoriaID { get; set; }
    public int? UtenteID { get; set; }
} 