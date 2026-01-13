namespace RecursosHumanos.Models.ViewModels
{
    public class ReclutamientoEstadisticasVM
    {
        //Filtros
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdReclutador { get; set; }

        //KPIs
        public int TotalRegistros { get; set; }
        public int TotalContactados { get; set; }
        public int TotalContratados { get; set; }
        public int TotalNoContratados { get; set; }


        //Graficas 
        public List<string> Meses { get; set; } = new();
        public List<int> ContactadosPorMes { get; set; } = new();

        public List<int> ContratadosPorMes { get; set; } = new();

        public Dictionary<string, int> FuentesContratacion { get; set; } = new();

        public Dictionary<string, int> RazonesNoContratacion { get; set; } = new();

        public Dictionary<string, int> SexoDistribucion { get; set; } = new();
    }
}
