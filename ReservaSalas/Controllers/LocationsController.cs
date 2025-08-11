using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaSalas.Data;
using ReservaSalas.Models;
using System;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LocationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/locations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
    {
        return await _context.Locations.ToListAsync();
    }
}