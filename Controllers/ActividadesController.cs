using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Suri.DTO;
using Suri.Models;

namespace Suri.Controllers
{


    public class ActividadesController : Controller
    {
        private readonly SuriDbContext _context;
        private readonly UserManager<MyUsers> userManager;
        public ActividadesController(SuriDbContext context, UserManager<MyUsers> userManager)
        {
            this.userManager = userManager;
            _context = context;
        }

        // GET: Actividades
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActividadesAsignadas()
        {
            var suriDbContext = _context.Actividades
                .Include(a => a.Localidad)
                .Include(a => a.MyUser)
                .Include(a => a.Prioridad)
                .Where(x => x.Estado == false);

            return View(await suriDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActividadesRealizadas()
        {
            var suriDbContext = _context.Actividades
                .Include(a => a.Localidad)
                .Include(a => a.MyUser)
                .Include(a => a.Prioridad)
                .Where(x => x.Estado == true);
            return View(await suriDbContext.ToListAsync());
        }

        [Authorize(Roles = "Tecnico")]
        public async Task<IActionResult> ActividadesAsignadasTecnico()
        {
            string UserId = userManager.GetUserId(User);
            var suriDbContext = _context.Actividades
                .Include(a => a.Localidad)
                .Include(a => a.MyUser)
                .Include(a => a.Prioridad)
                .Where(x => x.Estado == false && x.MyUserId == UserId);
            return View(await suriDbContext.ToListAsync());
        }

        [Authorize(Roles = "Tecnico")]
        public async Task<IActionResult> ActividadesRealizadasTecnico()
        {
            string UserId = userManager.GetUserId(User);
            var suriDbContext = _context.Actividades
                .Include(a => a.Localidad)
                .Include(a => a.MyUser)
                .Include(a => a.Prioridad)
                .Where(x => x.Estado == true && x.MyUserId == UserId);

            return View(await suriDbContext.ToListAsync());
        }

        [Authorize(Roles = "Tecnico")]
        public async Task<IActionResult> RealizarActividad(int id)
        {
            string UserId = userManager.GetUserId(User);
            var actividad = await _context.Actividades
             .Include(a => a.Localidad)
             .Include(a => a.MyUser)
             .Include(a => a.Prioridad)
             .Where(x => x.Estado == false && x.MyUserId == UserId)
             .FirstOrDefaultAsync(m => m.Id == id);

            if (actividad == null)
            {
                return NotFound();
            }
            return View(actividad);
        }

        [HttpPost]
        [Authorize(Roles = "Tecnico")]
        public async Task<IActionResult> TerminarActividad(Actividades model)
        {

            string UserId = userManager.GetUserId(User);
            var actividad = await _context.Actividades.FindAsync(model.Id);

            if (actividad == null)
            {
                return NotFound();
            }
            if (actividad.MyUserId != UserId)
            {
                return NotFound();
            }


            actividad.Asistente = model.Asistente;
            actividad.FechaRealizacion = model.FechaRealizacion;
            actividad.Nota = model.Nota;
            actividad.accion = model.accion;
            actividad.Estado = true;
            if (model.Asistente == null || model.FechaRealizacion == null || model.accion == null)
            {
                return RedirectToAction("RealizarActividad", new { id = actividad.Id });

            }
            try
            {
                _context.Update(actividad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ActividadesAsignadasTecnico));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActividadesExists(actividad.Id))
                {
                    return View(actividad);

                }
                else
                {
                    throw;
                }
            }

        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividades = await _context.Actividades
                .Include(a => a.Localidad)
                .Include(a => a.MyUser)
                .Include(a => a.Prioridad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actividades == null)
            {
                return NotFound();
            }

            return View(actividades);
        }

        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            ViewData["LocalidadId"] = new SelectList(_context.Set<Localidades>(), "Id", "Nombre");
            ViewData["MyUserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["PrioridadId"] = new SelectList(_context.Prioridades.OrderByDescending(x => x.Id), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Actividades actividades)
        {

            if (ModelState.IsValid)
            {
                _context.Add(actividades);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ActividadesAsignadas));
            }
            ViewData["LocalidadId"] = new SelectList(_context.Set<Localidades>(), "Id", "Nombre", actividades.LocalidadId);
            ViewData["MyUserId"] = new SelectList(_context.Users, "Id", "UserName", actividades.MyUserId);
            ViewData["PrioridadId"] = new SelectList(_context.Prioridades.OrderByDescending(x => x.Id), "Id", "Name");

            return View(actividades);
        }

        [Authorize(Roles = "Admin")]
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
            ViewData["MyUserId"] = new SelectList(_context.Users, "Id", "UserName", actividades.MyUserId);
            ViewData["PrioridadId"] = new SelectList(_context.Prioridades.OrderByDescending(x => x.Id), "Id", "Name");
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();

            return View(actividades);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string returnUrl, Actividades actividades)
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
                return Redirect(returnUrl);
            }
            ViewData["LocalidadId"] = new SelectList(_context.Set<Localidades>(), "Id", "Nombre", actividades.LocalidadId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", actividades.MyUserId);
            ViewData["PrioridadId"] = new SelectList(_context.Prioridades.OrderByDescending(x => x.Id), "Id", "Name");

            return View(actividades);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividades = await _context.Actividades
                .Include(a => a.Localidad)
                .Include(a => a.MyUser)
                .Include(a => a.Prioridad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actividades == null)
            {
                return NotFound();
            }
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();

            return View(actividades);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl)
        {
            var actividades = await _context.Actividades.FindAsync(id);
            _context.Actividades.Remove(actividades);
            await _context.SaveChangesAsync();
            return Redirect(returnUrl);
        }

        private bool ActividadesExists(int id)
        {
            return _context.Actividades.Any(e => e.Id == id);
        }
    }
}
