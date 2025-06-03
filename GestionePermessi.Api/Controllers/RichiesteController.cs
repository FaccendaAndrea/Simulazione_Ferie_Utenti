using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionePermessi.Api.Data;
using GestionePermessi.Api.DTOs;
using GestionePermessi.Api.Models;
using System.Security.Claims;

namespace GestionePermessi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RichiesteController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RichiesteController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RichiestaPermesso>>> GetRichieste()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        var richieste = await _context.RichiestePermessi
            .Include(r => r.Categoria)
            .Include(r => r.Utente)
            .Include(r => r.UtenteValutazione)
            .Where(r => r.UtenteID == userId)
            .OrderByDescending(r => r.DataRichiesta)
            .ToListAsync();

        return Ok(richieste);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RichiestaPermesso>> GetRichiesta(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        var richiesta = await _context.RichiestePermessi
            .Include(r => r.Categoria)
            .Include(r => r.Utente)
            .Include(r => r.UtenteValutazione)
            .FirstOrDefaultAsync(r => r.RichiestaID == id && r.UtenteID == userId);

        if (richiesta == null)
        {
            return NotFound();
        }

        return richiesta;
    }

    [HttpPost]
    public async Task<ActionResult<RichiestaPermesso>> CreateRichiesta(RichiestaPermesso richiesta)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        var categoria = await _context.CategoriePermessi.FindAsync(richiesta.CategoriaID);
        if (categoria == null)
        {
            return NotFound("Categoria non trovata");
        }

        richiesta.UtenteID = userId;
        richiesta.DataRichiesta = DateTime.Now;
        richiesta.Stato = "In attesa";
        richiesta.Utente = utente;
        richiesta.Categoria = categoria;

        _context.RichiestePermessi.Add(richiesta);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRichiesta), new { id = richiesta.RichiestaID }, richiesta);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRichiesta(int id, RichiestaPermesso richiesta)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        if (id != richiesta.RichiestaID)
        {
            return BadRequest();
        }

        var existingRichiesta = await _context.RichiestePermessi
            .Include(r => r.Categoria)
            .Include(r => r.Utente)
            .FirstOrDefaultAsync(r => r.RichiestaID == id && r.UtenteID == userId);

        if (existingRichiesta == null)
        {
            return NotFound();
        }

        if (existingRichiesta.Stato != "In attesa")
        {
            return BadRequest("Non è possibile modificare una richiesta già valutata");
        }

        existingRichiesta.DataInizio = richiesta.DataInizio;
        existingRichiesta.DataFine = richiesta.DataFine;
        existingRichiesta.Motivazione = richiesta.Motivazione;
        existingRichiesta.CategoriaID = richiesta.CategoriaID;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RichiestaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRichiesta(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        var richiesta = await _context.RichiestePermessi
            .FirstOrDefaultAsync(r => r.RichiestaID == id && r.UtenteID == userId);

        if (richiesta == null)
        {
            return NotFound();
        }

        if (richiesta.Stato != "In attesa")
        {
            return BadRequest("Non è possibile eliminare una richiesta già valutata");
        }

        _context.RichiestePermessi.Remove(richiesta);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("da-approvare")]
    [Authorize(Roles = "Responsabile")]
    public async Task<ActionResult<IEnumerable<RichiestaPermesso>>> GetRichiesteDaApprovare()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        var richieste = await _context.RichiestePermessi
            .Include(r => r.Categoria)
            .Include(r => r.Utente)
            .Where(r => r.Stato == "In attesa")
            .OrderByDescending(r => r.DataRichiesta)
            .ToListAsync();

        return Ok(richieste);
    }

    [HttpPut("{id}/valuta")]
    [Authorize(Roles = "Responsabile")]
    public async Task<IActionResult> ValutaRichiesta(int id, [FromBody] ValutazionePermesso valutazione)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        var richiesta = await _context.RichiestePermessi
            .Include(r => r.Categoria)
            .Include(r => r.Utente)
            .FirstOrDefaultAsync(r => r.RichiestaID == id);

        if (richiesta == null)
        {
            return NotFound();
        }

        if (richiesta.Stato != "In attesa")
        {
            return BadRequest("La richiesta è già stata valutata");
        }

        richiesta.Stato = valutazione.Stato;
        richiesta.DataValutazione = DateTime.Now;
        richiesta.UtenteValutazioneID = userId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RichiestaExists(int id)
    {
        return _context.RichiestePermessi.Any(e => e.RichiestaID == id);
    }
} 