using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Suri.Models;

namespace Suri.Controllers
{
    public class ActividadesController : Controller
    {
        private readonly SuriDbContext _context;

        public ActividadesController(SuriDbContext context)
        {
            _context = context;
        }

        // GET: Actividades
        public async Task<IActionResult> Index()
        {
            var suriDbContext = _context.Actividades.Include(a => a.Localidad).Include(a => a.MyUser);
            return View(await suriDbContext.ToListAsync());
        }

        // GET: Actividades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividades = await _context.Actividades
                .Include(a => a.Localidad)
                .Include(a => a.MyUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actividades == null)
            {
                return NotFound();
            }

            return View(actividades);
        }

        // GET: Actividades/Create
        public IActionResult Create()
        {
            ViewData["LocalidadId"] = new SelectList(_context.Set<Localidades>(), "Id", "Nombre");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Actividades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Actividades actividades)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(actividades);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalidadId"] = new SelectList(_context.Set<Localidades>(), "Id", "Nombre", actividades.LocalidadId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", actividades.UserId);
            return View(actividades);
        }

        // GET: Actividades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividades = await _context.Actividades.FindAsync(id);
            if (actividades == null)
            {
                return NotFound();
            }
            ViewData["LocalidadId"] = new SelectList(_context.Set<Localidades>(), "Id", "Nombre", actividades.LocalidadId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", actividades.UserId);
            return View(actividades);
        }

        // POST: Actividades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Actividad,FechaAsignacion,Nota,Asistente,FechaRealizacion,Action,UserId,LocalidadId,Prioridad,Estado")] Actividades actividades)
        {
            if (id != actividades.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actividades);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActividadesExists(actividades.Id))
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
            ViewData["LocalidadId"] = new SelectList(_context.Set<Localidades>(), "Id", "Nombre", actividades.LocalidadId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", actividades.UserId);
            return View(actividades);
        }

        // GET: Actividades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividades = await _context.Actividades
                .Include(a => a.Localidad)
                .Include(a => a.MyUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actividades == null)
            {
                return NotFound();
            }

            return View(actividades);
        }

        // POST: Actividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actividades = await _context.Actividades.FindAsync(id);
            _context.Actividades.Remove(actividades);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActividadesExists(int id)
        {
            return _context.Actividades.Any(e => e.Id == id);
        }
    }
}
