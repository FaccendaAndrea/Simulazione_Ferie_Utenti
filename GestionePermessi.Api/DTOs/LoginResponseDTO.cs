namespace GestionePermessi.Api.DTOs;

public class LoginResponseDTO
{
    public string Token { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Cognome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Ruolo { get; set; } = null!;
} 