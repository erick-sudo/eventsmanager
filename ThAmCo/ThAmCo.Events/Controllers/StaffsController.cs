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
    public class StaffsController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Staffs
        [HttpGet]
        [Route("/Staffs")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Staffs.ToListAsync());
        }

        // GET: Staffs/Details/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        [HttpGet]
        [Route("/[controller]/[action]")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,StaffName,PhoneNumber,Email")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }


        //Unassign staff from a specific event
        [HttpGet]
        [Route("/[controller]/[action]")]
        public async Task<IActionResult> UnassignStaff([Bind("StaffId, EventId")] Staffing staffing)
        {
            if (ModelState.IsValid)
            {
                _context.Remove(staffing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Guests/BookGuest/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> AssignStaff(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var availableEvents = await _context.Events.ToListAsync();

            dynamic staffbooking = new ExpandoObject();
            staffbooking.Staff = staff;
            staffbooking.AvailableEvents = availableEvents;

            ViewBag.Data = staffbooking;

            return View();
        }

        [HttpPost]
        [Route("/Staffs/AssignStaff")]
        public async Task<IActionResult> AssignStaff([Bind("StaffId, EventId")] Staffing staffing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staffing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Staffs/Edit/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,StaffName,PhoneNumber,Email")] Staff staff)
        {
            if (id != staff.StaffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffId))
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
            return View(staff);
        }

        // GET: Staffs/Delete/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Staffs == null)
            {
                return Problem("Entity set 'EventsDbContext.Staffs'  is null.");
            }
            var staff = await _context.Staffs.FindAsync(id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
            }

            var staffings = await _context.Staffing.Where(st => st.StaffId == id).ToArrayAsync<Staffing>();
            foreach(var stf in staffings)
            {
                _context.Staffing.Remove(stf);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
          return _context.Staffs.Any(e => e.StaffId == id);
        }
    }
}
