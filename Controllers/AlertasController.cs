using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using RecursosHumanos.Models.ViewModels;

public class AlertasController : Controller
{
    private readonly IConfiguration _configuration;

    public AlertasController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        List<PersonalAlarmaVM> lista = new();

        using (SqlConnection cn = new SqlConnection(
            _configuration.GetConnectionString("AlertasConnection")))
        {
            using SqlCommand cmd = new SqlCommand("Personal_Alarmas", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new PersonalAlarmaVM
                {
                    IdPersonal = Convert.ToInt32(dr["id_personal"]),
                    FotoPersonal = dr["foto_personal"] as byte[],
                    Nombre = dr["nombre"].ToString(),
                    IdDepto = Convert.ToInt32(dr["id_depto"]),
                    FechaIngreso = Convert.ToDateTime(dr["fecha_ingreso"]),
                    FechaBaja = dr["fecha_baja"] as DateTime?,
                    FechaReingreso = dr["fecha_reingreso"] as DateTime?,
                    Estado = dr["estado"].ToString(),
                    FechaUltimaLiquidacion = dr["fecha_ultima_liquidacion"] as DateTime?,
                    DiasDesdeUltimaLiquidacion = dr["dias_desde_ultima_liquidacion"] as int?
                });
            }
        }

        return View(lista);
    }

    public IActionResult Details(int id)
    {
        PersonalAlarmaVM model = null;

        using (SqlConnection cn = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection")))
        {
            using SqlCommand cmd = new SqlCommand("Personal_Alarmas", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (Convert.ToInt32(dr["id_personal"]) == id)
                {
                    model = new PersonalAlarmaVM
                    {
                        IdPersonal = id,
                        FotoPersonal = dr["foto_personal"] as byte[],
                        Nombre = dr["nombre"].ToString(),
                        FechaIngreso = Convert.ToDateTime(dr["fecha_ingreso"]),
                        FechaUltimaLiquidacion = dr["fecha_ultima_liquidacion"] as DateTime?,
                        DiasDesdeUltimaLiquidacion = dr["dias_desde_ultima_liquidacion"] as int?
                    };
                    break;
                }
            }
        }

        return View(model);
    }
}
