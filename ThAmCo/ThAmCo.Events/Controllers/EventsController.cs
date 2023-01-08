using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{

    [Route("/[controller]/[action]")]
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: Events
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            //Return the Index View
              return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //Check if the ID is null or the Events exist
            if (id == null || _context.Events == null)
            {
                //404 not found client error
                return NotFound();
            }

            var @theevent = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@theevent == null)
            {
                return NotFound();
            }

            return View(@theevent);
        }

        //View Guests in a specific event
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> ViewGuests(int? id)
        {
            if(id == null || _context.GuestBookings == null)
            {
                return NotFound();
            }

            var @guestbkngs = await _context.GuestBookings.Where(gb => gb.EventId == id).ToListAsync();

            var @guests = new List<dynamic>();

            guestbkngs.ForEach(async booking =>
            {
                if (_context.Guests != null)
                {
                    var guest = _context.Guests.FindAsync(booking.GuestId);
                    
                    guests.Add(new {TheGuest = guest.Result, Attended = booking.Attended});
                }
            });

            if (@guests == null || @guests.Count < 1)
            {
                return NotFound();
            }

            var gst = await _context.Events.FindAsync(id);

            dynamic guestbookingsData = new ExpandoObject();
            guestbookingsData.Guests = guests;
            guestbookingsData.EventTitle = gst.Title;
            ViewBag.Data = guestbookingsData;

            return View();
        }

        //Viewing the staff allocated to a specific Event
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> ViewStaff(int? id)
        {
            if (id == null || _context.Staffing == null)
            {
                return NotFound();
            }

            var @staffbkngs = await _context.Staffing.Where(gb => gb.EventId == id).ToListAsync();

            var @staffs = new List<dynamic>();

            staffbkngs.ForEach(async assignment =>
            {
                if (_context.Staffs != null)
                {
                    var staff = _context.Staffs.FindAsync(assignment.StaffId);

                    staffs.Add(new { TheStaff = staff.Result });
                }
            });

            if (@staffs == null)
            {
                return NotFound();
            }

            var evt = await _context.Events.FindAsync(id);

            dynamic staffbookingsData = new ExpandoObject();
            staffbookingsData.Staffs = staffs;
            staffbookingsData.EventTitle = evt.Title;
            staffbookingsData.EventId = evt.EventId;

            ViewBag.Data = staffbookingsData;

            return View();
        }

        // Create a new Event
        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        //Persist data to the api
        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Title,DateOfEvent,EventType")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        //Edit an Event
        // GET: Events/Edit/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @theevent = await _context.Events.FindAsync(id);
            if (@theevent == null)
            {
                return NotFound();
            }
            return View(@theevent);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Title,DateOfEvent,EventType")] Event @theevent)
        {
            if (id != @theevent.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@theevent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@theevent.EventId))
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
            return View(@theevent);
        }
        

        // GET: Events/Delete/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @theevent = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@theevent == null)
            {
                return NotFound();
            }

            return View(@theevent);
        }


        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'EventsDbContext.Events'  is null.");
            }
            var @theevent = await _context.Events.FindAsync(id);
            if (@theevent != null)
            {
                _context.Events.Remove(@theevent);
            }
            var @staffs = await _context.Staffing.Where(st => st.EventId == id).ToListAsync<Staffing>();
            foreach(var staff  in staffs)
            {
                _context.Staffing.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Checks if an event with that Id exists
        private bool EventExists(int id)
        {
          return _context.Events.Any(e => e.EventId == id);
        }
    }
}