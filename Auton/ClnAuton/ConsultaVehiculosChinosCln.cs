using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
   {
        /// <summary>
        /// Clase de negocio para gestionar vehículos de procedencia china
        /// </summary>
        public class VehiculosChinosCln
        {
            /// <summary>
            /// Lista todos los vehículos de procedencia china
            /// </summary>
            /// <returns>Lista de vehículos chinos</returns>
            public static List<paVehiculosChinos_Result> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(string.Empty).ToList();
            }
        }

        /// <summary>
        /// Busca vehículos chinos por un parámetro
        /// </summary>
        /// <param name="parametro">Texto a buscar en marca, modelo, generación o motor</param>
        /// <returns>Lista filtrada de vehículos chinos</returns>
        public static List<paVehiculosChinos_Result> buscar(string parametro)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(parametro ?? string.Empty).ToList();
            }
        }

        /// <summary>
        /// Obtiene el total de vehículos chinos activos
        /// </summary>
        /// <returns>Número total de vehículos chinos</returns>
        public static int obtenerTotal()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(string.Empty).Count();
            }
        }

        /// <summary>
        /// Obtiene estadísticas de vehículos chinos
        /// </summary>
        /// <returns>Objeto con estadísticas</returns>
        public static EstadisticasChinos obtenerEstadisticas()
        {
            using (var context = new AutonEntities())
            {
                var vehiculos = context.paVehiculosChinos(string.Empty).ToList();

                if (!vehiculos.Any())
                {
                    return new EstadisticasChinos();
                }

                return new EstadisticasChinos
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

        /// <summary>
        /// Lista vehículos chinos por marca específica
        /// </summary>
        /// <param name="marca">Nombre de la marca</param>
        /// <returns>Lista de vehículos de esa marca</returns>
        public static List<paVehiculosChinos_Result> listarPorMarca(string marca)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(marca).ToList();
            }
        }

        /// <summary>
        /// Lista vehículos chinos por nivel de confiabilidad
        /// </summary>
        /// <param name="confiabilidad">Nivel de confiabilidad (Excelente, Bueno, Regular, Malo)</param>
        /// <returns>Lista de vehículos con ese nivel de confiabilidad</returns>
        public static List<paVehiculosChinos_Result> listarPorConfiabilidad(string confiabilidad)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(string.Empty)
                    .Where(v => v.confiabilidad == confiabilidad)
                    .ToList();
            }
        }

        /// <summary>
        /// Lista vehículos chinos con turbo
        /// </summary>
        /// <returns>Lista de vehículos turbo</returns>
        public static List<paVehiculosChinos_Result> listarConTurbo()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(string.Empty)
                    .Where(v => v.tieneTurbo == true)
                    .ToList();
            }
        }

        /// <summary>
        /// Lista vehículos chinos por país de fabricación
        /// </summary>
        /// <param name="pais">País de fabricación</param>
        /// <returns>Lista de vehículos fabricados en ese país</returns>
        public static List<paVehiculosChinos_Result> listarPorPaisFabricacion(string pais)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(string.Empty)
                    .Where(v => v.paisFabricacion != null &&
                                v.paisFabricacion.Equals(pais, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        /// <summary>
        /// Lista vehículos chinos por rango de años
        /// </summary>
        /// <param name="anioInicio">Año de inicio</param>
        /// <param name="anioFin">Año de fin</param>
        /// <returns>Lista de vehículos en ese rango</returns>
        public static List<paVehiculosChinos_Result> listarPorRangoAnios(int anioInicio, int anioFin)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(string.Empty)
                    .Where(v => v.anioFabricacion >= anioInicio && v.anioFabricacion <= anioFin)
                    .ToList();
            }
        }

        /// <summary>
        /// Obtiene el top N de vehículos chinos mejor puntuados
        /// </summary>
        /// <param name="cantidad">Cantidad de vehículos a retornar</param>
        /// <returns>Lista de los mejores vehículos</returns>
        public static List<paVehiculosChinos_Result> obtenerTopMejores(int cantidad = 10)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculosChinos(string.Empty)
                    .OrderByDescending(v => v.puntuacion)
                    .ThenBy(v => v.marca)
                    .Take(cantidad)
                    .ToList();
            }
        }
    }

    /// <summary>
    /// Clase para almacenar estadísticas de vehículos chinos
    /// </summary>
    public class EstadisticasChinos
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

        public EstadisticasChinos()
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
