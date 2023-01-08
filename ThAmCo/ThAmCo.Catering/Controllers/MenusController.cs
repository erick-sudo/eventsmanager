using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.Models;

namespace ThAmCo.Catering.Controllers
{
    public class MenusController : Controller
    {
        private readonly CateringDbContext _context;

        public MenusController(CateringDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreatedAsync();

        }

        // GET: Menus
        [HttpGet]
        [Route("/availablemenus")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Menus.ToListAsync());
        }

        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> ViewFoodItems(int? id)
        {
            if (id == null || _context.MenuFoodItems == null)
            {
                return NotFound();
            }

            var @menufooditems = await _context.MenuFoodItems.Where<MenuFoodItem>(f => f.MenuId == id).ToListAsync();
            var foodIds = menufooditems.Select(f =>
            {
                return f.FoodItemId;
            });

            var fooditems = _context.FoodItems.Where<FoodItem>(f => foodIds.Contains(f.FoodItemId));

            return View(@fooditems);
        }


        // Get the details of a menu using its Id
        // GET: Menus/Details/5
        [HttpGet] 
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        //Accesing the menu creation page
        // GET: Menus/Create
        [HttpGet]
        [Route("/[controller]/[action]")]
        public IActionResult Create()
        {
            return View();
        }

        // Posting new Menu to the server
        // POST: Menus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("MenuId,MenuName")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Menus/Edit/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("MenuId,MenuName")] Menu menu)
        {
            if (id != menu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MenuId))
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
            return View(menu);
        }

        // GET: Menus/Delete/5
        [HttpGet]
        [Route("/[controller]/[action]/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menus == null)
            {
                return Problem("Entity set 'CateringDbContext.Menus'  is null.");
            }
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
          return _context.Menus.Any(e => e.MenuId == id);
        }
    }
}
