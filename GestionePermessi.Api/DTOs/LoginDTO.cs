using System.ComponentModel.DataAnnotations;

namespace GestionePermessi.Api.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "L'email è obbligatoria")]
    [EmailAddress(ErrorMessage = "L'email non è in un formato valido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La password è obbligatoria")]
    [MinLength(8, ErrorMessage = "La password deve essere di almeno 8 caratteri")]
    public string Password { get; set; } = null!;
} 