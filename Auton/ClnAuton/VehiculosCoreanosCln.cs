using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    // Clase de negocio para gestionar vehículos de procedencia coreana
    public class VehiculosCoreanosCln
    {
        // Lista todos los vehículos de procedencia coreana
        public static List<paVehiculosCoreanos_Result> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(string.Empty).ToList();
            }
        }

        // Busca vehículos coreanos por un parámetro
        public static List<paVehiculosCoreanos_Result> buscar(string parametro)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(parametro ?? string.Empty).ToList();
            }
        }

        // Obtiene el total de vehículos coreanos activos
        public static int obtenerTotal()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(string.Empty).Count();
            }
        }

        // Obtiene estadísticas de vehículos coreanos
        public static EstadisticasCoreanos obtenerEstadisticas()
        {
            using (var context = new AutonEntities())
            {
                var vehiculos = context.paVehiculosCoreanos(string.Empty).ToList();

                if (!vehiculos.Any())
                {
                    return new EstadisticasCoreanos();
                }

                return new EstadisticasCoreanos
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

        // Lista vehículos coreanos por marca específica
        public static List<paVehiculosCoreanos_Result> listarPorMarca(string marca)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(marca).ToList();
            }
        }

        // Lista vehículos coreanos por nivel de confiabilidad
        public static List<paVehiculosCoreanos_Result> listarPorConfiabilidad(string confiabilidad)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(string.Empty)
                    .Where(v => v.confiabilidad == confiabilidad)
                    .ToList();
            }
        }

        // Lista vehículos coreanos con turbo
        public static List<paVehiculosCoreanos_Result> listarConTurbo()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(string.Empty)
                    .Where(v => v.tieneTurbo == true)
                    .ToList();
            }
        }

        // Lista vehículos coreanos por país de fabricación
        public static List<paVehiculosCoreanos_Result> listarPorPaisFabricacion(string pais)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(string.Empty)
                    .Where(v => v.paisFabricacion != null &&
                                v.paisFabricacion.Equals(pais, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        // Lista vehículos coreanos por rango de años
        public static List<paVehiculosCoreanos_Result> listarPorRangoAnios(int anioInicio, int anioFin)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(string.Empty)
                    .Where(v => v.anioFabricacion >= anioInicio && v.anioFabricacion <= anioFin)
                    .ToList();
            }
        }

        // Obtiene el top N de vehículos coreanos mejor puntuados
        public static List<paVehiculosCoreanos_Result> obtenerTopMejores(int cantidad = 10)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosCoreanos(string.Empty)
                    .OrderByDescending(v => v.puntuacion)
                    .ThenBy(v => v.marca)
                    .Take(cantidad)
                    .ToList();
            }
        }
    }

    // Clase para almacenar estadísticas de vehículos coreanos
    public class EstadisticasCoreanos
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

        public EstadisticasCoreanos()
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
