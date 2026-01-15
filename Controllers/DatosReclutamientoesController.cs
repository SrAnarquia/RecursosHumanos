using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecursosHumanos.Models;
using RecursosHumanos.Models.ViewModels.Reclutamiento;

namespace RecursosHumanos.Controllers
{
    public class DatosReclutamientoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        #region builder

        public DatosReclutamientoesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion


        #region Index
        // GET: DatosReclutamientoes
        public async Task<IActionResult> Index(ReclutamientoIndexVM filtro,int pagina=1)
        {
            int pageSize = 10;

            var query = _context.DatosReclutamientos
                .Include(d => d.IdEmpresaNavigation)
                .Include(d => d.IdBaseNavigation)
                .Include(d => d.IdFuenteNavigation)
                .Include(d => d.IdEstatusNavigation)
                .Include(d => d.IdPosicionNavigation)
                .Include(d => d.IdReclutadorNavigation)
                .Include(d => d.IdSexoNavigation)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(filtro.Nombre))
                query = query.Where(x => x.NombreCompleto.Contains(filtro.Nombre));

            if (!string.IsNullOrEmpty(filtro.Telefono))
                query = query.Where(x => x.Telefono.Contains(filtro.Telefono));

            if (filtro.IdReclutador.HasValue)
                query = query.Where(x => x.IdReclutador == filtro.IdReclutador);

            if (filtro.FechaDesde.HasValue)
                query = query.Where(x => x.FechaContacto >= filtro.FechaDesde);

            if (filtro.FechaHasta.HasValue)
                query = query.Where(x => x.FechaContacto <= filtro.FechaHasta);

            // Ordenar
            query = query.OrderByDescending(x => x.FechaContacto);

            int totalRegistros = await query.CountAsync();

            var datos = await query
                .Skip((pagina - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Cargar combos con primer elemento en blanco
            var empresas = _context.Empresas
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Empresa1 })
                .ToList();
            empresas.Insert(0, new SelectListItem { Value = "", Text = "" });

            var bases = _context.Bases
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Base1 })
                .ToList();
            bases.Insert(0, new SelectListItem { Value = "", Text = "" });

            var fuentes = _context.Fuentes
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Fuente1 })
                .ToList();
            fuentes.Insert(0, new SelectListItem { Value = "", Text = "" });

            var estatus = _context.Estatuses
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Estatus1 })
                .ToList();
            estatus.Insert(0, new SelectListItem { Value = "", Text = "" });

            var posiciones = _context.Posicions
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Posicion1 })
                .ToList();
            posiciones.Insert(0, new SelectListItem { Value = "", Text = "" });

            var sexos = _context.Sexos
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Sexo1 })
                .ToList();
            sexos.Insert(0, new SelectListItem { Value = "", Text = "" });

            var reclutadores = _context.Reclutadors
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.NombreCompleto })
                .ToList();
            reclutadores.Insert(0, new SelectListItem { Value = "", Text = "" });

            var model = new ReclutamientoIndexVM
            {
                Datos = datos,
                Nuevo = new DatosReclutamiento(),
                Empresas = empresas,
                Bases = bases,
                Fuentes = fuentes,
                Estatus = estatus,
                Posiciones = posiciones,
                Sexos = sexos,
                Reclutadores = reclutadores,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize)
            };

            return View(model);
        }

        #endregion


        #region Detalles
        // GET: DatosReclutamientoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datosReclutamiento = await _context.DatosReclutamientos
                .Include(d => d.IdBaseNavigation)
                .Include(d => d.IdEmpresaNavigation)
                .Include(d => d.IdEstatusNavigation)
                .Include(d => d.IdFuenteNavigation)
                .Include(d => d.IdPosicionNavigation)
                .Include(d => d.IdReclutadorNavigation)
                .Include(d => d.IdSexoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datosReclutamiento == null)
            {
                return NotFound();
            }

            return View(datosReclutamiento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "Nuevo")] DatosReclutamiento nuevo)
        {
            if (!ModelState.IsValid)
            {
                // Recargar los combos si es que vas a regresar a la vista
                ReclutamientoIndexVM vm = new ReclutamientoIndexVM
                {
                    Nuevo = nuevo,
                    MostrarModal=true,
                    Empresas = _context.Empresas.Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Empresa1
                    }).ToList(),

                 
                    Bases = _context.Bases.Select(b => new SelectListItem
                    {
                        Value = b.Id.ToString(),
                        Text = b.Base1
                    }).ToList(),
                    Fuentes = _context.Fuentes.Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Fuente1
                    }).ToList(),
                    Estatus = _context.Estatuses.Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Estatus1
                    }).ToList(),
                    Posiciones = _context.Posicions.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Posicion1
                    }).ToList(),
                    Sexos = _context.Sexos.Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Sexo1
                    }).ToList(),
                    Reclutadores = _context.Reclutadors.Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.NombreCompleto
                    }).ToList()
                };

                return View("Index", vm);
            }

            // Fecha de contacto
            nuevo.FechaContacto = DateTime.Now;

            // Guardar en DB
            _context.DatosReclutamientos.Add(nuevo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Details (Overlay)
        [HttpGet]
        public async Task<IActionResult> DetailsPartial(int id)
        {

            var datos = await _context.DatosReclutamientos
                .Include(d => d.IdBaseNavigation)
                .Include(d => d.IdEmpresaNavigation)
                .Include(d => d.IdEstatusNavigation)
                .Include(d => d.IdFuenteNavigation)
                .Include(d => d.IdPosicionNavigation)
                .Include(d => d.IdReclutadorNavigation)
                .Include(d => d.IdSexoNavigation)
                .FirstOrDefaultAsync(x => x.Id ==id);

            if (datos == null)
                return NotFound();


            var vm = new ReclutamientoDetailsVM
            {
                Datos = datos,
                MostrarModal = true
            };

            return PartialView("_ReclutamientoDetailsPartial",vm);

        }


        #endregion


        #region Edit
        // GET: DatosReclutamientoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var datos = await _context.DatosReclutamientos.FindAsync(id);
            if (datos == null)
                return NotFound();

            var vm = new ReclutamientoIndexVM
            {
                Nuevo = datos,
                Empresas = _context.Empresas.Select(e =>
                    new SelectListItem { Value = e.Id.ToString(), Text = e.Empresa1 }),

                Bases = _context.Bases.Select(b =>
                    new SelectListItem { Value = b.Id.ToString(), Text = b.Base1 }),

                Fuentes = _context.Fuentes.Select(f =>
                    new SelectListItem { Value = f.Id.ToString(), Text = f.Fuente1 }),

                Estatus = _context.Estatuses.Select(e =>
                    new SelectListItem { Value = e.Id.ToString(), Text = e.Estatus1 }),

                Posiciones = _context.Posicions.Select(p =>
                    new SelectListItem { Value = p.Id.ToString(), Text = p.Posicion1 }),

                Sexos = _context.Sexos.Select(s =>
                    new SelectListItem { Value = s.Id.ToString(), Text = s.Sexo1 }),

                Reclutadores = _context.Reclutadors.Select(r =>
                    new SelectListItem { Value = r.Id.ToString(), Text = r.NombreCompleto })
            };




            return PartialView("_ReclutamientoEditPartial", vm);
        }


        #endregion


        #region EditPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = "Nuevo")] DatosReclutamiento model)
        {
            if (!ModelState.IsValid)
            {
                var vm = new ReclutamientoIndexVM
                {
                    Nuevo = model,
                    MostrarModal = true,
                    Empresas = _context.Empresas.Select(e =>
                        new SelectListItem { Value = e.Id.ToString(), Text = e.Empresa1 }),
                    Bases = _context.Bases.Select(b =>
                        new SelectListItem { Value = b.Id.ToString(), Text = b.Base1 }),
                    Fuentes = _context.Fuentes.Select(f =>
                        new SelectListItem { Value = f.Id.ToString(), Text = f.Fuente1 }),
                    Estatus = _context.Estatuses.Select(e =>
                        new SelectListItem { Value = e.Id.ToString(), Text = e.Estatus1 }),
                    Posiciones = _context.Posicions.Select(p =>
                        new SelectListItem { Value = p.Id.ToString(), Text = p.Posicion1 }),
                    Sexos = _context.Sexos.Select(s =>
                        new SelectListItem { Value = s.Id.ToString(), Text = s.Sexo1 }),
                    Reclutadores = _context.Reclutadors.Select(r =>
                        new SelectListItem { Value = r.Id.ToString(), Text = r.NombreCompleto })
                };

                return View("Index", vm);
            }

            var entity = await _context.DatosReclutamientos.FindAsync(model.Id);
            if (entity == null) return NotFound();

            entity.NombreCompleto = model.NombreCompleto;
            entity.Telefono = model.Telefono;
            entity.Comentarios = model.Comentarios;
            entity.IdEmpresa = model.IdEmpresa;
            entity.IdBase = model.IdBase;
            entity.IdFuente = model.IdFuente;
            entity.IdEstatus = model.IdEstatus;
            entity.IdPosicion = model.IdPosicion;
            entity.IdSexo = model.IdSexo;
            entity.IdReclutador = model.IdReclutador;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region DeleteConfirm
        // POST: DatosReclutamientoes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datosReclutamiento = await _context.DatosReclutamientos.FindAsync(id);
            if (datosReclutamiento != null)
            {
                _context.DatosReclutamientos.Remove(datosReclutamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatosReclutamientoExists(int id)
        {
            return _context.DatosReclutamientos.Any(e => e.Id == id);
        }

        #endregion

        #region Delete (Overlay)

        // GET: DatosReclutamientoes/DeletePartial/5
        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id)
        {
            var datos = await _context.DatosReclutamientos
            .FirstOrDefaultAsync(x => x.Id == id);

            if (datos == null)
                return NotFound();

            var vm = new ReclutamientoDeleteVM
            {
                Datos = datos,
                MostrarModal = true
            };

            return PartialView("_ReclutamientoDeletePartial", vm);
        }

        #endregion





    }
}
