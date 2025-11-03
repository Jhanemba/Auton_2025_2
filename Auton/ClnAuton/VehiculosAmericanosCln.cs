using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    // Clase de negocio para gestionar vehículos de procedencia americana
    public class VehiculosAmericanosCln
    {
        // Lista todos los vehículos de procedencia americana
        public static List<paVehiculosAmericanos_Result> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(string.Empty).ToList();
            }
        }

        // Busca vehículos americanos por un parámetro
        public static List<paVehiculosAmericanos_Result> buscar(string parametro)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(parametro ?? string.Empty).ToList();
            }
        }

        // Obtiene el total de vehículos americanos activos
        public static int obtenerTotal()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(string.Empty).Count();
            }
        }

        // Obtiene estadísticas de vehículos americanos
        public static EstadisticasAmericanos obtenerEstadisticas()
        {
            using (var context = new AutonEntities())
            {
                var vehiculos = context.paVehiculosAmericanos(string.Empty).ToList();

                if (!vehiculos.Any())
                {
                    return new EstadisticasAmericanos();
                }

                return new EstadisticasAmericanos
                {
                    TotalVehiculos = vehiculos.Count,
                    Excelentes = vehiculos.Count(v => v.confiabilidad == "Excelente"),
                    Buenos = vehiculos.Count(v => v.confiabilidad == "Bueno"),
                    Regulares = vehiculos.Count(v => v.confiabilidad == "Regular"),
                    Malos = vehiculos.Count(v => v.confiabilidad == "Malo"),
                    PromedioPuntuacion = vehiculos.Average(v => (double)v.puntuacion),
                    Marcas = vehiculos.Select(v => v.marca).Distinct().OrderBy(m => m).ToList(),
                    ConTurbo = vehiculos.Count(v => v.tieneTurbo),
                    SinTurbo = vehiculos.Count(v => !v.tieneTurbo)
                };
            }
        }

        // Lista vehículos americanos por marca específica
        public static List<paVehiculosAmericanos_Result> listarPorMarca(string marca)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(marca).ToList();
            }
        }

        // Lista vehículos americanos por nivel de confiabilidad
        public static List<paVehiculosAmericanos_Result> listarPorConfiabilidad(string confiabilidad)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(string.Empty)
                    .Where(v => v.confiabilidad == confiabilidad)
                    .ToList();
            }
        }

        // Lista vehículos americanos con turbo
        public static List<paVehiculosAmericanos_Result> listarConTurbo()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(string.Empty)
                    .Where(v => v.tieneTurbo == true)
                    .ToList();
            }
        }

        // Lista vehículos americanos por país de fabricación
        public static List<paVehiculosAmericanos_Result> listarPorPaisFabricacion(string pais)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(string.Empty)
                    .Where(v => v.paisFabricacion != null &&
                                v.paisFabricacion.Equals(pais, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        // Lista vehículos americanos por rango de años
        public static List<paVehiculosAmericanos_Result> listarPorRangoAnios(int anioInicio, int anioFin)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(string.Empty)
                    .Where(v => v.anioFabricacion >= anioInicio && v.anioFabricacion <= anioFin)
                    .ToList();
            }
        }

        // Obtiene el top N de vehículos americanos mejor puntuados
        public static List<paVehiculosAmericanos_Result> obtenerTopMejores(int cantidad = 10)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosAmericanos(string.Empty)
                    .OrderByDescending(v => v.puntuacion)
                    .ThenBy(v => v.marca)
                    .Take(cantidad)
                    .ToList();
            }
        }
    }

    // Clase para almacenar estadísticas de vehículos americanos
    public class EstadisticasAmericanos
    {
        public int TotalVehiculos { get; set; }
        public int Excelentes { get; set; }
        public int Buenos { get; set; }
        public int Regulares { get; set; }
        public int Malos { get; set; }
        public double PromedioPuntuacion { get; set; }
        public List<string> Marcas { get; set; }
        public int ConTurbo { get; set; }
        public int SinTurbo { get; set; }

        public EstadisticasAmericanos()
        {
            Marcas = new List<string>();
        }

        public override string ToString()
        {
            return $"Total: {TotalVehiculos} | " +
                   $"Excelentes: {Excelentes} | " +
                   $"Buenos: {Buenos} | " +
                   $"Regulares: {Regulares} | " +
                   $"Malos: {Malos} | " +
                   $"Promedio: {PromedioPuntuacion:N2}";
        }

        public string ObtenerMarcasTexto()
        {
            return Marcas != null && Marcas.Any() ? string.Join(", ", Marcas) : "Sin marcas";
        }
    }
}
