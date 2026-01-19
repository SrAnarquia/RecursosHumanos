using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecursosHumanos.Models;
using RecursosHumanos.Models.ViewModels.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecursosHumanos.Controllers
{
    public class CatalogoCursosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogoCursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        // GET: CatalogoCursos
        public async Task<IActionResult> Index(CatalogoCursosIndexVM filtro, int pagina = 1)
        {
            int pageSize = 10;

            var query = _context.CatalogoCursos
                .Include(c => c.IdDepartamentoNavigation)
                .Include(c => c.IdNivelNavigation)
                .Include(c => c.IdTipoCursoNavigation)
                .AsQueryable();

            // ===== FILTROS =====
            if (!string.IsNullOrEmpty(filtro.Nombre))
                query = query.Where(x => x.Nombre.Contains(filtro.Nombre));

            if (filtro.IdDepartamento.HasValue)
                query = query.Where(x => x.IdDepartamento == filtro.IdDepartamento);

            if (filtro.IdNivel.HasValue)
                query = query.Where(x => x.IdNivel == filtro.IdNivel);

            if (filtro.IdTipoCurso.HasValue)
                query = query.Where(x => x.IdTipoCurso == filtro.IdTipoCurso);

            if (filtro.FechaDesde.HasValue)
                query = query.Where(x => x.FechaInicio >= filtro.FechaDesde);

            if (filtro.FechaHasta.HasValue)
                query = query.Where(x => x.FechaFinalizacion <= filtro.FechaHasta);

            // ===== ORDEN =====
            query = query.OrderByDescending(x => x.FechaCreacion);

            int totalRegistros = await query.CountAsync();

            var datos = await query
                .Skip((pagina - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ===== COMBOS =====
            var departamentos = _context.Departamentos
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nombre })
                .ToList();
            departamentos.Insert(0, new SelectListItem { Value = "", Text = "" });

            var niveles = _context.CatalogoNivels
                .Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Nombre})
                .ToList();
            niveles.Insert(0, new SelectListItem { Value = "", Text = "" });

            var tiposCurso = _context.TipoCursos
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Nombre })
                .ToList();
            tiposCurso.Insert(0, new SelectListItem { Value = "", Text = "" });

            var model = new CatalogoCursosIndexVM
            {
                Datos = datos,
                Departamentos = departamentos,
                Niveles = niveles,
                TiposCurso = tiposCurso,

                Nuevo = new CatalogoCursoCreateVM
                {
                    Departamentos = departamentos,
                    Niveles = niveles,
                    TiposCurso = tiposCurso
                },

                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize)
            };

            ModelState.Clear();

            return View(model);
        }
        #endregion

        /*
        #region Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string data)
        {
            
        }
        #endregion
        */

    }
}
