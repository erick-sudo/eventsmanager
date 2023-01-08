using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class GuestsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Guests
        [HttpGet]
        [Route("/Guests")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Guests.ToListAsync());
        }

        // GET: Guests/Details/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .FirstOrDefaultAsync(m => m.GuestId == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // GET: Guests/Create
        [HttpGet]
        [Route("/[controller]/[action]")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("GuestId,GuestName,PhoneNumber,Email")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: Guests/Edit/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // GET: Guests/BookGuest/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> BookGuest(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            var availableEvents = await _context.Events.ToListAsync();

            dynamic guestbooking = new ExpandoObject();
            guestbooking.Guest = guest;
            guestbooking.AvailableEvents = availableEvents;

            ViewBag.Data = guestbooking;

            return View();
        }

        [HttpPost]
        [Route("/Guests/BookGuest")]
        public async Task<IActionResult> BookGuest([Bind("GuestId, EventId")] GuestBooking guestbooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guestbooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("GuestId,GuestName,PhoneNumber,Email")] Guest guest)
        {
            if (id != guest.GuestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(guest.GuestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .FirstOrDefaultAsync(m => m.GuestId == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Guests == null)
            {
                return Problem("Entity set 'EventsDbContext.Guests'  is null.");
            }
            var guest = await _context.Guests.FindAsync(id);
            if (guest != null)
            {
                _context.Guests.Remove(guest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuestExists(int id)
        {
          return _context.Guests.Any(e => e.GuestId == id);
        }
    }
}
