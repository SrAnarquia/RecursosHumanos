using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RecursosHumanos.Models;
using RecursosHumanos.Models.ViewModels.Empleados;
using System.Data;

public class EmpleadosController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public EmpleadosController(IConfiguration configuration,ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    #region Index
    // ===================== INDEX =====================
    public IActionResult Index(PersonalListadoVM filtro, int pagina = 1)
    {
        int pageSize = 10;
        List<PersonalListadoVM> lista = new();

        using (SqlConnection cn = new SqlConnection(
            _configuration.GetConnectionString("AlertasConnection")))
        {
            using SqlCommand cmd = new SqlCommand("Empleados_datos", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new PersonalListadoVM
                {
                    IdPersonal = Convert.ToInt32(dr["id_personal"]),
                    FotoPersonal = dr["foto_personal"] as byte[],
                    Nombre = dr["nombre"].ToString(),
                    Departamento = dr["descripcion"].ToString(),
                    TipoEmpleado = dr["tipo_empleado"].ToString(),
                    Telefono = dr["telefono"].ToString(),
                    Email = dr["email"].ToString(),
                    Estado = dr["estado"].ToString()
                });
            }
        }

        // ===================== FILTROS =====================
        if (!string.IsNullOrEmpty(filtro.FiltroNombre))
            lista = lista.Where(x => x.Nombre.Contains(filtro.FiltroNombre)).ToList();

        if (!string.IsNullOrEmpty(filtro.FiltroDepartamento))
            lista = lista.Where(x => x.Departamento == filtro.FiltroDepartamento).ToList();

        if (!string.IsNullOrEmpty(filtro.FiltroTipoEmpleado))
            lista = lista.Where(x => x.TipoEmpleado == filtro.FiltroTipoEmpleado).ToList();

        if (!string.IsNullOrEmpty(filtro.FiltroEstado))
            lista = lista.Where(x => x.Estado == filtro.FiltroEstado).ToList();


        var departamentos = lista
            .Select(x => x.Departamento)
            .Distinct()
            .OrderBy(x => x)
            .Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            })
            .ToList();

        var tiposEmpleado = lista
            .Select(x => x.TipoEmpleado)
            .Distinct()
            .OrderBy(x => x)
            .Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            })
            .ToList();

        var estados = lista
            .Select(x => x.Estado)
            .Distinct()
            .OrderBy(x => x)
            .Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            })
            .ToList();





        // ===================== PAGINACIÓN =====================
        int totalRegistros = lista.Count;
        var datosPaginados = lista
            .Skip((pagina - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var vm = new PersonalListadoVM
        {
            Datos = datosPaginados,
            PaginaActual = pagina,
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize),

            FiltroNombre = filtro.FiltroNombre,
            FiltroDepartamento = filtro.FiltroDepartamento,
            FiltroTipoEmpleado = filtro.FiltroTipoEmpleado,
            FiltroEstado = filtro.FiltroEstado,


            Departamentos = departamentos,
            TiposEmpleado = tiposEmpleado,
            Estados = estados
        };

        return View(vm);
    }
    #endregion


    #region Portafolio
    public IActionResult Portafolio(int id)
    {
        var vm = new PersonalPortafolioVM();
        vm.Cursos = new List<CursoPersonaVM>();

        // ===================== PERSONA (Alarmas / LRdb) =====================
        using (SqlConnection cn = new SqlConnection(
            _configuration.GetConnectionString("AlertasConnection")))
        {
            using SqlCommand cmd = new SqlCommand("Empleado_DatosPorId", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdPersonal", id);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                vm.IdPersonal = id;
                vm.FotoPersonal = dr["foto_personal"] as byte[];
                vm.Nombre = dr["nombre"].ToString();
                vm.Departamento = dr["Departamento"].ToString();
                vm.TipoEmpleado = dr["tipo_empleado"].ToString();
                vm.Curp = dr["curp"].ToString();
                vm.Telefono = dr["telefono"].ToString();
                vm.Email = dr["email"].ToString();
                vm.Estado = dr["estado"].ToString();
            }
        }

        // ===================== CURSOS (DefaultConnection) =====================
        using (SqlConnection cn = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection")))
        {
            using SqlCommand cmd = new SqlCommand("Entrenamiento_PersonaCursos", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdPersonal", id);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                vm.Cursos.Add(new CursoPersonaVM
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    NombreCurso = dr["NombreCurso"].ToString(),
                    Descripcion = dr["Descripcion"].ToString(),
                    FechaInicio = dr["FechaInicio"] as DateTime?,
                    FechaFinalizacion = dr["FechaFinalizacion"] as DateTime?,
                    Estatus = dr["Estatus"].ToString(),
                    Diploma = dr["Diploma"].ToString()
                });
            }
        }

        // ===================== CREATE MODAL (LINQ / EF) =====================
        var estatus = _context.EstatusCursos
            .Select(x => new SelectListItem
            {
                Text = x.Estatus,
                Value = x.Id.ToString()
            })
            .ToList();


        vm.NuevoCurso = new CursoPersonaCreateVM
        {
            IdPersona = id
        };

        vm.NuevoCurso = new CursoPersonaCreateVM
        {
            IdPersona = id,
            Estatus = _context.EstatusCursos
         .Select(x => new SelectListItem
         {
             Text = x.Estatus,
             Value = x.Id.ToString()
         })
         .ToList()
        };


        return View(vm);
    }
    #endregion


    
    #region CrearEmpleadoCurso GET (OVERLAY)
    [HttpGet]
    public IActionResult CreateCurso(int idPersona)
    {
        var estatus = _context.EstatusCursos
            .Select(x => new SelectListItem
            {
                Text = x.Estatus,
                Value = x.Id.ToString()
            })
            .ToList();

        var vm = new CursoPersonaCreateVM
        {
            IdPersona = idPersona,
            Estatus = estatus
        };

        return PartialView("_CreateCurso", vm);
    }



    #endregion

    #region CrearEmpleadoCurso POST
    [HttpPost]
    [ValidateAntiForgeryToken]
   
    public IActionResult CreateCurso(CursoPersonaCreateVM model)
    {
        // Traemos estatus para el dropdown siempre
        model.Estatus = _context.EstatusCursos
            .Select(x => new SelectListItem
            {
                Text = x.Estatus,
                Value = x.Id.ToString()
            }).ToList();

        if (!ModelState.IsValid)
        {
            // Si hay errores, devolvemos el partial view
            return PartialView("_CreateCurso", model);
        }

        _context.CursosPersonas.Add(new CursosPersona
        {
            IdPersona = model.IdPersona,
            NombreCurso = model.NombreCurso,
            Descripcion = model.Descripcion,
            FechaInicio = model.FechaInicio,
            FechaFinalizacion = model.FechaFinalizacion,
            IdEstatus = model.IdEstatus.Value,
            FechaCreacion = DateTime.Now
        });

        _context.SaveChanges();

        // Si todo sale bien, devolvemos un success simple
        return Json(new { success = true });
    }



    #endregion



}
