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

    public EmpleadosController(IConfiguration configuration, ApplicationDbContext context)
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
    public IActionResult Portafolio(int id, string? curso, DateTime? fechaDesde, DateTime? fechaHasta, int pagina = 1)
    {
        int pageSize = 10; // puedes ajustar el tamaño de página

        var vm = new PersonalPortafolioVM
        {
            FiltroCurso = curso,
            FechaDesde = fechaDesde,
            FechaHasta = fechaHasta,
            Cursos = new List<CursoPersonaVM>()
        };

        // ===================== PERSONA =====================
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

        // ===================== CURSOS =====================
        var cursosTemp = new List<CursoPersonaVM>();
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
                cursosTemp.Add(new CursoPersonaVM
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

        // ===================== FILTROS =====================
        if (!string.IsNullOrEmpty(curso))
        {
            cursosTemp = cursosTemp
                .Where(c => c.NombreCurso.Contains(curso, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (fechaDesde.HasValue)
            cursosTemp = cursosTemp
                .Where(c => c.FechaInicio >= fechaDesde.Value)
                .ToList();

        if (fechaHasta.HasValue)
            cursosTemp = cursosTemp
                .Where(c => c.FechaFinalizacion <= fechaHasta.Value)
                .ToList();

        // ===================== PAGINACIÓN =====================
        int totalRegistros = cursosTemp.Count;
        vm.TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize);
        vm.PaginaActual = pagina;

        vm.Cursos = cursosTemp
            .Skip((pagina - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // ===================== NUEVO CURSO (OVERLAY) =====================
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

    #region Incidentes

    #endregion

    #region Creates
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
        // Traemos siempre el dropdown de estatus
        model.Estatus = _context.EstatusCursos
            .Select(x => new SelectListItem
            {
                Text = x.Estatus,
                Value = x.Id.ToString()
            })
            .ToList();

        // Validación de modelo
        if (!ModelState.IsValid)
        {
            return PartialView("_CreateCurso", model);
        }

        string diplomaPath = null;

        // ===================== GUARDAR ARCHIVO =====================
        if (model.DiplomaFile != null && model.DiplomaFile.Length > 0)
        {
            // Carpeta dentro de wwwroot
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FileUploaded");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Nombre único para evitar colisiones
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.DiplomaFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.DiplomaFile.CopyTo(fileStream);
            }

            // Guardamos ruta relativa para la base de datos
            diplomaPath = "/FileUploaded/" + uniqueFileName;
        }

        // ===================== GUARDAR EN DB =====================
        _context.CursosPersonas.Add(new CursosPersona
        {
            IdPersona = model.IdPersona,
            NombreCurso = model.NombreCurso,
            Descripcion = model.Descripcion,
            FechaInicio = model.FechaInicio,
            FechaFinalizacion = model.FechaFinalizacion,
            IdEstatus = model.IdEstatus.Value,
            Diploma = diplomaPath, // <-- ruta guardada
            FechaCreacion = DateTime.Now
        });

        _context.SaveChanges();

        // Retornar éxito al overlay
        return Json(new { success = true });
    }


    #endregion

    #endregion


    #region Edits


    #region Edit GET 
    [HttpGet]
    public IActionResult EditCurso(int id)
    {
        var curso = _context.CursosPersonas.FirstOrDefault(x => x.Id == id);
        if (curso == null) return NotFound();

        var vm = new CursoPersonaEditVM
        {
            Id = curso.Id,
            IdPersona = curso.IdPersona.Value,
            NombreCurso = curso.NombreCurso,
            Descripcion = curso.Descripcion,
            FechaInicio = curso.FechaInicio,
            FechaFinalizacion = curso.FechaFinalizacion,
            IdEstatus = curso.IdEstatus,
            Diploma = curso.Diploma,
            Estatus = _context.EstatusCursos
                .Select(x => new SelectListItem
                {
                    Text = x.Estatus,
                    Value = x.Id.ToString()
                }).ToList()
        };

        return PartialView("_EditCurso", vm);
    }

    #endregion


    #region Edit POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditCurso(CursoPersonaEditVM model)
    {
        model.Estatus = _context.EstatusCursos
            .Select(x => new SelectListItem
            {
                Text = x.Estatus,
                Value = x.Id.ToString()
            }).ToList();

        if (!ModelState.IsValid)
            return PartialView("_EditCurso", model);

        var curso = _context.CursosPersonas.FirstOrDefault(x => x.Id == model.Id);
        if (curso == null) return NotFound();

        // Actualizar archivo si viene uno nuevo
        if (model.DiplomaFile != null && model.DiplomaFile.Length > 0)
        {
            string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FileUploaded");
            Directory.CreateDirectory(uploads);

            string fileName = Guid.NewGuid() + Path.GetExtension(model.DiplomaFile.FileName);
            string path = Path.Combine(uploads, fileName);

            using var fs = new FileStream(path, FileMode.Create);
            model.DiplomaFile.CopyTo(fs);

            curso.Diploma = "/FileUploaded/" + fileName;
        }

        curso.NombreCurso = model.NombreCurso;
        curso.Descripcion = model.Descripcion;
        curso.FechaInicio = model.FechaInicio;
        curso.FechaFinalizacion = model.FechaFinalizacion;
        curso.IdEstatus = model.IdEstatus.Value;

        _context.SaveChanges();

        return Json(new { success = true });
    }



    #endregion

    #endregion


    #region Deletes

    #region Delete GET

    [HttpGet]
    public IActionResult DeleteCurso(int id)
    {
        var curso = (from c in _context.CursosPersonas
                     join e in _context.EstatusCursos
                         on c.IdEstatus equals e.Id
                     where c.Id == id
                     select new CursoPersonaDeleteVM
                     {
                         Id = c.Id,
                         IdPersona = c.IdPersona.Value,
                         NombreCurso = c.NombreCurso,
                         Descripcion = c.Descripcion,
                         FechaInicio = c.FechaInicio,
                         FechaFinalizacion = c.FechaFinalizacion,
                         Estatus = e.Estatus
                     }).FirstOrDefault();

        if (curso == null)
            return NotFound();

        return PartialView("_DeleteCurso", curso);
    }


    #endregion



    #region Delete POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteCurso(CursoPersonaDeleteVM model)
    {
        var curso = _context.CursosPersonas.FirstOrDefault(x => x.Id == model.Id);
        if (curso == null)
            return NotFound();

        // 🔥 eliminar archivo si existe
        if (!string.IsNullOrEmpty(curso.Diploma))
        {
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                curso.Diploma.TrimStart('/')
            );

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        _context.CursosPersonas.Remove(curso);
        _context.SaveChanges();

        return Json(new { success = true });
    }


    #endregion

    #endregion


    #region Details

    #region Details

    [HttpGet]
    public IActionResult DetailsCurso(int id)
    {
        var curso = (from c in _context.CursosPersonas
                     join e in _context.EstatusCursos
                         on c.IdEstatus equals e.Id
                     where c.Id == id
                     select new CursoPersonaDetailsVM
                     {
                         Id = c.Id,
                         NombreCurso = c.NombreCurso,
                         Descripcion = c.Descripcion,
                         FechaInicio = c.FechaInicio,
                         FechaFinalizacion = c.FechaFinalizacion,
                         Estatus = e.Estatus,
                         Diploma = c.Diploma
                     }).FirstOrDefault();

        if (curso == null)
            return NotFound();

        return PartialView("_DetailsCurso", curso);
    }

    #endregion

    #endregion
}
