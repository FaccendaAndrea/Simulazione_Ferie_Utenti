using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GestionePermessi.Api.Data;
using GestionePermessi.Api.DTOs;
using GestionePermessi.Api.Models;

namespace GestionePermessi.Api.Controllers;

/// <summary>
/// Controller per la gestione dell'autenticazione degli utenti
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    /// <summary>
    /// Registra un nuovo utente nel sistema
    /// </summary>
    /// <param name="registerDto">Dati di registrazione dell'utente</param>
    /// <returns>Token JWT e informazioni dell'utente registrato</returns>
    /// <response code="200">Registrazione completata con successo</response>
    /// <response code="400">Email già registrata nel sistema</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDTO>> Register(RegisterDTO registerDto)
    {
        // Validazione email
        if (!IsValidEmail(registerDto.Email))
        {
            return BadRequest("L'email non è in un formato valido");
        }

        // Validazione password
        if (registerDto.Password.Length < 8)
        {
            return BadRequest("La password deve essere di almeno 8 caratteri");
        }

        // Validazione ruolo
        if (registerDto.Ruolo != "Dipendente" && registerDto.Ruolo != "Responsabile")
        {
            return BadRequest("Il ruolo deve essere 'Dipendente' o 'Responsabile'");
        }

        if (await _context.Utenti.AnyAsync(u => u.Email == registerDto.Email))
        {
            return BadRequest("Email già registrata");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        var utente = new Utente
        {
            Nome = registerDto.Nome,
            Cognome = registerDto.Cognome,
            Email = registerDto.Email,
            Password = hashedPassword,
            Ruolo = registerDto.Ruolo
        };

        _context.Utenti.Add(utente);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(utente);
        return new LoginResponseDTO
        {
            Token = token,
            Nome = utente.Nome,
            Cognome = utente.Cognome,
            Email = utente.Email,
            Ruolo = utente.Ruolo
        };
    }

    /// <summary>
    /// Effettua il login di un utente esistente
    /// </summary>
    /// <param name="loginDto">Credenziali di accesso</param>
    /// <returns>Token JWT e informazioni dell'utente</returns>
    /// <response code="200">Login effettuato con successo</response>
    /// <response code="401">Credenziali non valide</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDTO>> Login(LoginDTO loginDto)
    {
        // Validazione email
        if (!IsValidEmail(loginDto.Email))
        {
            return BadRequest("L'email non è in un formato valido");
        }

        // Validazione password
        if (loginDto.Password.Length < 8)
        {
            return BadRequest("La password deve essere di almeno 8 caratteri");
        }

        var utente = await _context.Utenti.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (utente == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, utente.Password))
        {
            return Unauthorized("Credenziali non valide");
        }

        var token = GenerateJwtToken(utente);
        return new LoginResponseDTO
        {
            Token = token,
            Nome = utente.Nome,
            Cognome = utente.Cognome,
            Email = utente.Email,
            Ruolo = utente.Ruolo
        };
    }

    private string GenerateJwtToken(Utente utente)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, utente.UtenteID.ToString()),
            new Claim(ClaimTypes.Email, utente.Email),
            new Claim(ClaimTypes.Role, utente.Ruolo)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email && email.Contains("@") && email.Contains(".");
        }
        catch
        {
            return false;
        }
    }
} 