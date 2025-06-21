using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pig_Management_WebApp.Data;
using Pig_Management_WebApp.Models;

namespace Pig_Management_WebApp.Controllers
{
    public class Races_PorcsController : Controller
    {
        private readonly Pig_Management_WebAppContext _context;

        public Races_PorcsController(Pig_Management_WebAppContext context)
        {
            _context = context;
        }

        // GET: Races_Porcs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Races_Porcs.ToListAsync());
        }

        // GET: Races_Porcs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var races_Porcs = await _context.Races_Porcs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (races_Porcs == null)
            {
                return NotFound();
            }

            return View(races_Porcs);
        }

        // GET: Races_Porcs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Races_Porcs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Races_Porcs races_Porcs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(races_Porcs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(races_Porcs);
        }

        // GET: Races_Porcs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var races_Porcs = await _context.Races_Porcs.FindAsync(id);
            if (races_Porcs == null)
            {
                return NotFound();
            }
            return View(races_Porcs);
        }

        // POST: Races_Porcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Races_Porcs races_Porcs)
        {
            if (id != races_Porcs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(races_Porcs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Races_PorcsExists(races_Porcs.Id))
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
            return View(races_Porcs);
        }

        // GET: Races_Porcs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var races_Porcs = await _context.Races_Porcs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (races_Porcs == null)
            {
                return NotFound();
            }

            return View(races_Porcs);
        }

        // POST: Races_Porcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var races_Porcs = await _context.Races_Porcs.FindAsync(id);
            if (races_Porcs != null)
            {
                _context.Races_Porcs.Remove(races_Porcs);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Races_PorcsExists(int id)
        {
            return _context.Races_Porcs.Any(e => e.Id == id);
        }
    }
}
