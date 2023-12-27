using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task2.Data;
using Task2.Models;

namespace Task2.Controllers
{
    public class LeadsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public LeadsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Leads
        public async Task<IActionResult> Index()
        {
            return View(await _context.InventoryUsers.ToListAsync());
        }

        // GET: Leads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryLeadsEntity = await _context.InventoryUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryLeadsEntity == null)
            {
                return NotFound();
            }

            return View(inventoryLeadsEntity);
        }

        // GET: Leads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Department,Email")] InventoryLeadsEntity inventoryLeadsEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryLeadsEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryLeadsEntity);
        }

        // GET: Leads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryLeadsEntity = await _context.InventoryUsers.FindAsync(id);
            if (inventoryLeadsEntity == null)
            {
                return NotFound();
            }
            return View(inventoryLeadsEntity);
        }

        // POST: Leads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Department,Email")] InventoryLeadsEntity inventoryLeadsEntity)
        {
            if (id != inventoryLeadsEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryLeadsEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryLeadsEntityExists(inventoryLeadsEntity.Id))
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
            return View(inventoryLeadsEntity);
        }

        // GET: Leads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryLeadsEntity = await _context.InventoryUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryLeadsEntity == null)
            {
                return NotFound();
            }

            return View(inventoryLeadsEntity);
        }

        // POST: Leads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryLeadsEntity = await _context.InventoryUsers.FindAsync(id);
            if (inventoryLeadsEntity != null)
            {
                _context.InventoryUsers.Remove(inventoryLeadsEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryLeadsEntityExists(int id)
        {
            return _context.InventoryUsers.Any(e => e.Id == id);
        }
    }
}
