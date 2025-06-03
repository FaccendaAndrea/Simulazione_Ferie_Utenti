using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionePermessi.Api.Data;
using GestionePermessi.Api.DTOs;

namespace GestionePermessi.Api.Controllers;

[Authorize(Roles = "Responsabile")]
[ApiController]
[Route("api/[controller]")]
public class StatisticheController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StatisticheController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatisticheDTO>>> GetStatistiche([FromQuery] StatisticheFilterDTO filter)
    {
        try
        {
            var query = _context.RichiestePermessi
                .Include(r => r.Utente)
                .Include(r => r.Categoria)
                .Where(r => r.Stato == "Approvata");

            if (filter.Anno.HasValue)
            {
                query = query.Where(r => r.DataInizio.Year == filter.Anno.Value);
            }

            if (filter.Mese.HasValue)
            {
                query = query.Where(r => r.DataInizio.Month == filter.Mese.Value);
            }

            if (filter.CategoriaID.HasValue)
            {
                query = query.Where(r => r.CategoriaID == filter.CategoriaID.Value);
            }

            if (filter.UtenteID.HasValue)
            {
                query = query.Where(r => r.UtenteID == filter.UtenteID.Value);
            }

            var statistiche = query
                .AsEnumerable()
                .GroupBy(r => new 
                { 
                    r.UtenteID, 
                    r.Utente.Nome, 
                    r.Utente.Cognome,
                    Anno = r.DataInizio.Year,
                    Mese = r.DataInizio.Month,
                    r.CategoriaID,
                    CategoriaDescrizione = r.Categoria.Descrizione
                })
                .Select(g => new StatisticheDTO
                {
                    UtenteNomeCompleto = $"{g.Key.Nome} {g.Key.Cognome}",
                    Anno = g.Key.Anno,
                    Mese = g.Key.Mese,
                    GiorniTotali = g.Sum(r => (r.DataFine - r.DataInizio).Days + 1),
                    CategoriaDescrizione = g.Key.CategoriaDescrizione
                })
                .OrderBy(s => s.UtenteNomeCompleto)
                .ThenBy(s => s.Anno)
                .ThenBy(s => s.Mese)
                .ToList();

            return Ok(statistiche);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore interno: {ex.Message}");
        }
    }
}