using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecursosHumanos.Models;
using RecursosHumanos.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecursosHumanos.Controllers
{
    public class EstadisticasReclutamientoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadisticasReclutamientoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int? idEmpresa,
            int? idReclutador)
        {
            var query = _context.DatosReclutamientos
                .Include(x => x.IdEstatusNavigation)
                .Include(x => x.IdFuenteNavigation)
                .AsQueryable();

            if (fechaDesde.HasValue)
                query = query.Where(x => x.FechaContacto >= fechaDesde);

            if (fechaHasta.HasValue)
                query = query.Where(x => x.FechaContacto <= fechaHasta);

            if (idEmpresa.HasValue)
                query = query.Where(x => x.IdEmpresa == idEmpresa);

            if (idReclutador.HasValue)
                query = query.Where(x => x.IdReclutador == idReclutador);

            var datos = await query.ToListAsync();

            var vm = new ReclutamientoEstadisticasVM
            {
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                IdEmpresa = idEmpresa,
                IdReclutador = idReclutador,

                TotalRegistros = datos.Count,
                TotalContactados = datos.Count(x => x.FechaContacto != null),
                TotalContratados = datos.Count(x => x.IdEstatusNavigation.Estatus1 == "Contratado"),
                TotalNoContratados = datos.Count(x => x.IdEstatusNavigation.Estatus1 != "Contratado"),

                Meses = datos
                    .GroupBy(x => x.FechaContacto.ToString("yyyy-MM"))
                    .OrderBy(x => x.Key)
                    .Select(x => x.Key)
                    .ToList(),

                ContactadosPorMes = datos
                    .GroupBy(x => x.FechaContacto.ToString("yyyy-MM"))
                    .OrderBy(x => x.Key)
                    .Select(x => x.Count())
                    .ToList(),

                ContratadosPorMes = datos
                    .Where(x => x.IdEstatusNavigation.Estatus1 == "Contratado")
                    .GroupBy(x => x.FechaContacto.ToString("yyyy-MM"))
                    .OrderBy(x => x.Key)
                    .Select(x => x.Count())
                    .ToList(),

                FuentesContratacion = datos
                    .Where(x => x.IdEstatusNavigation.Estatus1 == "Contratado")
                    .GroupBy(x => x.IdFuenteNavigation.Fuente1)
                    .ToDictionary(x => x.Key, x => x.Count()),

                RazonesNoContratacion = datos
                    .Where(x => x.IdEstatusNavigation.Estatus1 != "Contratado")
                    .GroupBy(x => x.Comentarios)
                    .Where(x => !string.IsNullOrEmpty(x.Key))
                    .ToDictionary(x => x.Key, x => x.Count())
            };

            return View(vm);
        }
    }

}
