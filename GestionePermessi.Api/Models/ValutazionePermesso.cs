using System.ComponentModel.DataAnnotations;

namespace GestionePermessi.Api.Models
{
    public class ValutazionePermesso
    {
        [Required]
        public string Stato { get; set; } = null!;
    }
}