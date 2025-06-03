using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionePermessi.Api.Data;

namespace GestionePermessi.Api.Controllers;

[Authorize(Roles = "Responsabile")]
[ApiController]
[Route("api/[controller]")]
public class UtentiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UtentiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetUtenti()
    {
        var utenti = await _context.Utenti
            .Select(u => new { u.UtenteID, u.Nome, u.Cognome, u.Ruolo })
            .ToListAsync();

        return Ok(utenti);
    }
} 