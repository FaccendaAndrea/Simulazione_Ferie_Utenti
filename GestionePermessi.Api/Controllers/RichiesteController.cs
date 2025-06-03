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
    public async Task<ActionResult<IEnumerable<RichiestaPermessoDTO>>> GetRichieste()
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
            .Select(r => new RichiestaPermessoDTO
            {
                RichiestaID = r.RichiestaID,
                DataRichiesta = r.DataRichiesta,
                DataInizio = r.DataInizio,
                DataFine = r.DataFine,
                Motivazione = r.Motivazione,
                Stato = r.Stato,
                CategoriaDescrizione = r.Categoria.Descrizione,
                UtenteNomeCompleto = $"{r.Utente.Nome} {r.Utente.Cognome}",
                DataValutazione = r.DataValutazione,
                UtenteValutazioneNomeCompleto = r.UtenteValutazione != null ? $"{r.UtenteValutazione.Nome} {r.UtenteValutazione.Cognome}" : null
            })
            .ToListAsync();

        return Ok(richieste);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RichiestaPermessoDTO>> GetRichiesta(int id)
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
            .Where(r => r.RichiestaID == id && r.UtenteID == userId)
            .Select(r => new RichiestaPermessoDTO
            {
                RichiestaID = r.RichiestaID,
                DataRichiesta = r.DataRichiesta,
                DataInizio = r.DataInizio,
                DataFine = r.DataFine,
                Motivazione = r.Motivazione,
                Stato = r.Stato,
                CategoriaDescrizione = r.Categoria.Descrizione,
                UtenteNomeCompleto = $"{r.Utente.Nome} {r.Utente.Cognome}",
                DataValutazione = r.DataValutazione,
                UtenteValutazioneNomeCompleto = r.UtenteValutazione != null ? $"{r.UtenteValutazione.Nome} {r.UtenteValutazione.Cognome}" : null
            })
            .FirstOrDefaultAsync();

        if (richiesta == null)
        {
            return NotFound();
        }

        return richiesta;
    }

    [HttpPost]
    public async Task<ActionResult<RichiestaPermessoDTO>> CreateRichiesta(RichiestaPermessoCreateDTO createDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        var categoria = await _context.CategoriePermessi.FindAsync(createDto.CategoriaID);
        if (categoria == null)
        {
            return NotFound("Categoria non trovata");
        }

        // Validazione date
        if (createDto.DataInizio > createDto.DataFine)
        {
            return BadRequest("La data di inizio deve essere precedente alla data di fine");
        }

        if (createDto.DataInizio.Date < DateTime.Now.Date)
        {
            return BadRequest("La data di inizio non può essere nel passato");
        }

        var richiesta = new RichiestaPermesso
        {
            UtenteID = userId,
            DataRichiesta = DateTime.Now,
            DataInizio = createDto.DataInizio,
            DataFine = createDto.DataFine,
            Motivazione = createDto.Motivazione,
            CategoriaID = createDto.CategoriaID,
            Stato = "In attesa"
        };

        _context.RichiestePermessi.Add(richiesta);
        await _context.SaveChangesAsync();

        // Carica la richiesta appena creata con tutte le relazioni
        var richiestaCreata = await _context.RichiestePermessi
            .Include(r => r.Categoria)
            .Include(r => r.Utente)
            .Where(r => r.RichiestaID == richiesta.RichiestaID)
            .Select(r => new RichiestaPermessoDTO
            {
                RichiestaID = r.RichiestaID,
                DataRichiesta = r.DataRichiesta,
                DataInizio = r.DataInizio,
                DataFine = r.DataFine,
                Motivazione = r.Motivazione,
                Stato = r.Stato,
                CategoriaDescrizione = r.Categoria.Descrizione,
                UtenteNomeCompleto = $"{r.Utente.Nome} {r.Utente.Cognome}",
                DataValutazione = r.DataValutazione,
                UtenteValutazioneNomeCompleto = null
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetRichiesta), new { id = richiesta.RichiestaID }, richiestaCreata);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RichiestaPermessoDTO>> UpdateRichiesta(int id, RichiestaPermessoUpdateDTO updateDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var utente = await _context.Utenti.FindAsync(userId);
        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        var existingRichiesta = await _context.RichiestePermessi
            .Include(r => r.Categoria)
            .Include(r => r.Utente)
            .FirstOrDefaultAsync(r => r.RichiestaID == id && r.UtenteID == userId);

        if (existingRichiesta == null)
        {
            return NotFound("Richiesta non trovata");
        }

        if (existingRichiesta.Stato != "In attesa")
        {
            return BadRequest("Non è possibile modificare una richiesta già valutata");
        }

        // Validazione date
        if (updateDto.DataInizio > updateDto.DataFine)
        {
            return BadRequest("La data di inizio deve essere precedente alla data di fine");
        }

        if (updateDto.DataInizio.Date < DateTime.Now.Date)
        {
            return BadRequest("La data di inizio non può essere nel passato");
        }

        // Verifica che la categoria esista
        var categoria = await _context.CategoriePermessi.FindAsync(updateDto.CategoriaID);
        if (categoria == null)
        {
            return NotFound("Categoria non trovata");
        }

        // Aggiorna solo i campi modificabili
        existingRichiesta.DataInizio = updateDto.DataInizio;
        existingRichiesta.DataFine = updateDto.DataFine;
        existingRichiesta.Motivazione = updateDto.Motivazione;
        existingRichiesta.CategoriaID = updateDto.CategoriaID;

        try
        {
            await _context.SaveChangesAsync();

            // Carica la richiesta aggiornata con tutte le relazioni
            var richiestaAggiornata = await _context.RichiestePermessi
                .Include(r => r.Categoria)
                .Include(r => r.Utente)
                .Include(r => r.UtenteValutazione)
                .Where(r => r.RichiestaID == id)
                .Select(r => new RichiestaPermessoDTO
                {
                    RichiestaID = r.RichiestaID,
                    DataRichiesta = r.DataRichiesta,
                    DataInizio = r.DataInizio,
                    DataFine = r.DataFine,
                    Motivazione = r.Motivazione,
                    Stato = r.Stato,
                    CategoriaDescrizione = r.Categoria!.Descrizione,
                    UtenteNomeCompleto = $"{r.Utente!.Nome} {r.Utente.Cognome}",
                    DataValutazione = r.DataValutazione,
                    UtenteValutazioneNomeCompleto = r.UtenteValutazione != null ? $"{r.UtenteValutazione.Nome} {r.UtenteValutazione.Cognome}" : null
                })
                .FirstAsync();

            return Ok(richiestaAggiornata);
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