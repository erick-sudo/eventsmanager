using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Venues.Data;

namespace ThAmCo.Venues.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("corsapp")]
    public class EventTypesController : ControllerBase
    {
        private readonly VenuesDbContext _context;

        public EventTypesController(VenuesDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreatedAsync();
        }

        // GET: api/EventTypes
        [HttpGet]
        public async Task<IActionResult> GetEventTypes()
        {
            var dto = await _context.EventTypes
                                    .Select(e => new
                                    {
                                        e.Id,
                                        e.Title
                                    }).ToListAsync();
            return Ok(dto);
        }
    }
}
