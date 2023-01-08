using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ThAmCo.Catering.Controllers
{
    [ApiController]
    [EnableCors("corsapp")]
    public class FoodBookingsController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public FoodBookingsController(CateringDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreatedAsync();
        }

        [HttpGet]
        [Route("thamcorp/api/foodbookings")]
        public async Task<IActionResult> GetFoodBookings()
        {
            var dto = await _context.FoodBookings
                                    .Select(e => new
                                    {
                                        e.FoodBookingId,
                                        e.ClientReferenceId,
                                        e.NumberOfGuests,
                                        e.MenuId
                                    }).ToListAsync();
            return Ok(dto);
        }

        [HttpGet]
        [Route("thamcorp/api/menus")]
        public async Task<IActionResult> GetMenus()
        {
            var dto = await _context.Menus
                                    .Select(e => new
                                    {
                                        e.MenuId,
                                        e.MenuName,
                                        e.MenuItems
                                    }).ToListAsync();
            return Ok(dto);
        }

        [HttpGet]
        [Route("thamcorp/api/fooditems")]
        public async Task<IActionResult> GetFoodItems()
        {
            var dto = await _context.FoodItems
                                    .Select(e => new
                                    {
                                        e.FoodItemId,
                                        e.FoodItemName,
                                        e.Description,
                                        e.UnitPrice
                                    }).ToListAsync();
            return Ok(dto);
        }
    }
}
