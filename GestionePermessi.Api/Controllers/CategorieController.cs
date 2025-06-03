using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionePermessi.Api.Data;
using GestionePermessi.Api.Models;

namespace GestionePermessi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategorieController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategorieController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaPermesso>>> GetCategorie()
    {
        return await _context.CategoriePermessi.ToListAsync();
    }
} 