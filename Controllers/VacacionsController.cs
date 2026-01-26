using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecursosHumanos.Models;
using RecursosHumanos.Models.ViewModels.Vacaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RecursosHumanos.Models.Vacacion;

namespace RecursosHumanos.Controllers
{
    public class VacacionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacacionsController(ApplicationDbContext context)
        {
            _context = context;
        }


        #region Index
        // GET: Vacacions
        public async Task<IActionResult> Index(VacacionIndexVM filtro, int pagina = 1)
        {
            int pageSize = 10;

            var query = _context.Vacacions
                .Include(v => v.IdAprobadoNavigation)
                .Include(v => v.IdRazonNavigation)
                .AsQueryable();

            // ===== FILTROS =====
            if (!string.IsNullOrEmpty(filtro.Nombre))
                query = query.Where(v => v.Nombre.Contains(filtro.Nombre));

            if (!string.IsNullOrEmpty(filtro.Departamento))
                query = query.Where(v => v.Departamento.Contains(filtro.Departamento));

            if (filtro.FechaDesde.HasValue)
                query = query.Where(v => v.FechaInicio >= filtro.FechaDesde);

            if (filtro.FechaHasta.HasValue)
                query = query.Where(v => v.FechaFinalizacion <= filtro.FechaHasta);

            int totalRegistros = await query.CountAsync();

            var datos = await query
                .OrderByDescending(v => v.FechaCreacion)
                .Skip((pagina - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VacacionIndexVM
            {
                Datos = datos,
                Nombre = filtro.Nombre,
                Departamento = filtro.Departamento,
                FechaDesde = filtro.FechaDesde,
                FechaHasta = filtro.FechaHasta,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize)
            };

            return View(model);
        }
        #endregion



        #region Aprobaciones
        public async Task<IActionResult> Aprobacion(VacacionIndexVM filtro, int pagina = 1)
        {
            int pageSize = 10;

            var query = _context.Vacacions
                .Include(v => v.IdAprobadoNavigation)
                .Include(v => v.IdRazonNavigation)
                .AsQueryable();

            // ===== FILTROS =====
            if (!string.IsNullOrEmpty(filtro.Nombre))
                query = query.Where(v => v.Nombre.Contains(filtro.Nombre));

            if (!string.IsNullOrEmpty(filtro.Departamento))
                query = query.Where(v => v.Departamento.Contains(filtro.Departamento));

            if (filtro.FechaDesde.HasValue)
                query = query.Where(v => v.FechaInicio >= filtro.FechaDesde);

            if (filtro.FechaHasta.HasValue)
                query = query.Where(v => v.FechaFinalizacion <= filtro.FechaHasta);

            int totalRegistros = await query.CountAsync();

            var datos = await query
                .OrderByDescending(v => v.FechaCreacion)
                .Skip((pagina - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VacacionIndexVM
            {
                Datos = datos,
                Nombre = filtro.Nombre,
                Departamento = filtro.Departamento,
                FechaDesde = filtro.FechaDesde,
                FechaHasta = filtro.FechaHasta,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize)



            };

            return View(model);
        }

        #endregion



        // GET: Vacacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacion = await _context.Vacacions
                .Include(v => v.IdAprobadoNavigation)
                .Include(v => v.IdRazonNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacacion == null)
            {
                return NotFound();
            }

            return View(vacacion);
        }

        // GET: Vacacions/Create
        public IActionResult Create()
        {
            var model = new VacacionCreateVM
            {
                Nuevo = new Vacacion(),
                Razones = Enum.GetValues(typeof(RazonVacaciones))
                    .Cast<RazonVacaciones>()
                    .Select(r => new SelectListItem
                    {
                        Value = ((int)r).ToString(),
                        Text = r.ToString().Replace("AsuntosPersonales", "Asuntos personales")
                    })
                    .ToList()
            };

            return PartialView("_VacacionesCrearPartial", model);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacacionCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine($"{error.Key}: {e.ErrorMessage}");
                    }
                }

                return PartialView("_VacacionesCrearPartial", model);
            }

            model.Nuevo.FechaCreacion = DateTime.Now;
            _context.Vacacions.Add(model.Nuevo);
            await _context.SaveChangesAsync();

            return Ok(); // ✅ devuelvo solo 200 OK
         }



        // GET: Vacacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacion = await _context.Vacacions.FindAsync(id);
            if (vacacion == null)
            {
                return NotFound();
            }
            ViewData["IdAprobado"] = new SelectList(_context.Aprobacions, "Id", "Id", vacacion.IdAprobado);
            ViewData["IdRazon"] = new SelectList(_context.Razones, "Id", "Id", vacacion.IdRazon);
            return View(vacacion);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Departamento,Detalles,FechaInicio,FechaFinalizacion,IdRazon,IdAprobado,FechaCreacion")] Vacacion vacacion)
        {
            if (id != vacacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacacionExists(vacacion.Id))
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
            ViewData["IdAprobado"] = new SelectList(_context.Aprobacions, "Id", "Id", vacacion.IdAprobado);
            ViewData["IdRazon"] = new SelectList(_context.Razones, "Id", "Id", vacacion.IdRazon);
            return View(vacacion);
        }

        // GET: Vacacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacion = await _context.Vacacions
                .Include(v => v.IdAprobadoNavigation)
                .Include(v => v.IdRazonNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacacion == null)
            {
                return NotFound();
            }

            return View(vacacion);
        }

        // POST: Vacacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacacion = await _context.Vacacions.FindAsync(id);
            if (vacacion != null)
            {
                _context.Vacacions.Remove(vacacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacacionExists(int id)
        {
            return _context.Vacacions.Any(e => e.Id == id);
        }
    }
}
