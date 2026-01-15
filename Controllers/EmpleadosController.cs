using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using RecursosHumanos.Models.ViewModels.Empleados;

public class EmpleadosController : Controller
{
    private readonly IConfiguration _configuration;

    public EmpleadosController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

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
            FiltroEstado = filtro.FiltroEstado
        };

        return View(vm);
    }
}
