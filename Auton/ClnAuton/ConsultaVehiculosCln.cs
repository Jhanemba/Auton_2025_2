using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ClnAuton
{
    public class ConsultaVehiculosCln
    {
        // Inserta un nuevo vehículo CON fotografía
        public static int insertar(Vehiculo vehiculo)
        {
            using (var context = new AutonEntities())
            {
                context.Vehiculo.Add(vehiculo);
                context.SaveChanges();
                return vehiculo.id;
            }
        }

        // Actualiza un vehículo existente - permite indicar si se debe actualizar la fotografía
        public static int actualizar(Vehiculo vehiculo, bool actualizarFoto = true)
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    var existente = context.Vehiculo.Find(vehiculo.id);

                    if (existente == null)
                    {
                        return 0;
                    }

                    // Actualizar TODOS los campos
                    existente.nroChasis = vehiculo.nroChasis;
                    existente.idModeloVehiculo = vehiculo.idModeloVehiculo;
                    existente.anioFabricacion = vehiculo.anioFabricacion;
                    existente.generacion = vehiculo.generacion;
                    existente.paisFabricacion = vehiculo.paisFabricacion;
                    existente.transmision = vehiculo.transmision;
                    existente.nroVelocidades = vehiculo.nroVelocidades;
                    existente.nroPuertas = vehiculo.nroPuertas;      // ⭐ ASEGURAR QUE SE ACTUALIZA
                    existente.nroAsientos = vehiculo.nroAsientos;
                    existente.tieneTurbo = vehiculo.tieneTurbo;
                    existente.motor = vehiculo.motor;
                    existente.combustible = vehiculo.combustible;
                    existente.nroAirbags = vehiculo.nroAirbags;
                    existente.seguridadChoques = vehiculo.seguridadChoques;
                    existente.organizacionSeguridad = vehiculo.organizacionSeguridad;
                    existente.confiabilidad = vehiculo.confiabilidad;
                    existente.puntuacion = vehiculo.puntuacion;
                    existente.kilometraje = vehiculo.kilometraje;
                    existente.observaciones = vehiculo.observaciones;
                    existente.usuarioRegistro = vehiculo.usuarioRegistro;

                    // FOTOGRAFÍA: actualizar solo si se indicó
                    if (actualizarFoto)
                    {
                        existente.fotografia = vehiculo.fotografia;
                    }

                    context.Entry(existente).State = EntityState.Modified;
                    context.SaveChanges();

                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        // ⭐ NUEVO: Obtiene la fotografía de un vehículo
        public static byte[] obtenerFotografia(int id)
        {
            using (var context = new AutonEntities())
            {
                var vehiculo = context.Vehiculo
                    .Where(v => v.id == id && v.estado != -1)
                    .Select(v => v.fotografia)
                    .FirstOrDefault();

                return vehiculo;
            }
        }

        // Elimina lógicamente un vehículo (estado = -1)
        public static int eliminar(int id, string usuario)
        {
            using (var context = new AutonEntities())
            {
                var vehiculo = context.Vehiculo.Find(id);
                if (vehiculo != null)
                {
                    vehiculo.estado = -1;
                    vehiculo.usuarioRegistro = usuario;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        // Obtiene un vehículo por su ID
        public static Vehiculo obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Find(id);
            }
        }

        // Lista todos los vehículos activos
        public static List<Vehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Where(x => x.estado != -1).ToList();
            }
        }

        // Lista vehículos usando el procedimiento almacenado con parámetro de búsqueda
        public static List<paVehiculoListar_Result> listarPa(string parametro)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculoListar(parametro ?? string.Empty).ToList();
            }
        }

        // Lista todos los vehículos usando procedimiento almacenado (sin filtro)
        public static List<paVehiculoListar_Result> listarTodosPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculoListar(string.Empty).ToList();
            }
        }

        // Obtiene el total de vehículos activos
        public static int obtenerTotalVehiculos()
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Count(x => x.estado != -1);
            }
        }

        // Lista vehículos por confiabilidad
        public static List<Vehiculo> listarPorConfiabilidad(string confiabilidad)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo
                    .Where(x => x.estado != -1 && x.confiabilidad == confiabilidad)
                    .ToList();
            }
        }

        /// Lista vehículos por rango de años
        public static List<Vehiculo> listarPorRangoAnios(int anioInicio, int anioFin)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo
                    .Where(x => x.estado != -1 &&
                           x.anioFabricacion >= anioInicio &&
                           x.anioFabricacion <= anioFin)
                    .ToList();
            }
        }
        // Lista vehículos por procedencia - VERSIÓN SIMPLIFICADA
        public static List<dynamic> listarPorProcedencia(string procedencia, string parametro)
        {
            using (var context = new AutonEntities())
            {
                parametro = parametro ?? string.Empty;

                // Obtener todos los resultados sin importar el tipo
                IEnumerable<dynamic> resultadosBrutos = null;

                switch (procedencia.ToLower())
                {
                    case "japonés":
                    case "japones":
                        resultadosBrutos = context.paVehiculosJaponeses(parametro).ToList();
                        break;

                    case "americano":
                        resultadosBrutos = context.paVehiculosAmericanos(parametro).ToList();
                        break;

                    case "chino":
                        resultadosBrutos = context.paVehiculosChinos(parametro).ToList();
                        break;

                    case "alemán":
                    case "aleman":
                        resultadosBrutos = context.paVehiculosAlemanes(parametro).ToList();
                        break;

                    case "europeo":
                        resultadosBrutos = context.paVehiculosEuropeos(parametro).ToList();
                        break;

                    case "coreano":
                        resultadosBrutos = context.paVehiculosCoreanos(parametro).ToList();
                        break;

                    default:
                        resultadosBrutos = context.paVehiculoListar(parametro).ToList();
                        break;
                }

                // Convertir a lista dinámica
                return resultadosBrutos.Cast<dynamic>().ToList();
            }
        }

        // Obtiene estadísticas generales de todos los vehículos
        public static EstadisticasGenerales obtenerEstadisticasGenerales()
        {
            using (var context = new AutonEntities())
            {
                var vehiculos = context.paVehiculoListar(string.Empty).ToList();

                if (!vehiculos.Any())
                {
                    return new EstadisticasGenerales();
                }

                return new EstadisticasGenerales
                {
                    TotalVehiculos = vehiculos.Count,

                    // Por Confiabilidad
                    Excelentes = vehiculos.Count(v => v.confiabilidad == "Excelente"),
                    Buenos = vehiculos.Count(v => v.confiabilidad == "Bueno"),
                    Regulares = vehiculos.Count(v => v.confiabilidad == "Regular"),
                    Malos = vehiculos.Count(v => v.confiabilidad == "Malo"),

                    // Por Procedencia
                    Japoneses = vehiculos.Count(v => v.procedencia == "Japonés"),
                    Americanos = vehiculos.Count(v => v.procedencia == "Americano"),
                    Chinos = vehiculos.Count(v => v.procedencia == "Chino"),
                    Alemanes = vehiculos.Count(v => v.procedencia == "Alemán"),
                    Europeos = vehiculos.Count(v => v.procedencia == "Europeo"),
                    Coreanos = vehiculos.Count(v => v.procedencia == "Coreano"),

                    // Promedios
                    PromedioPuntuacion = vehiculos.Average(v => (double)v.puntuacion),
                    PromedioAirbags = vehiculos.Average(v => v.nroAirbags),

                    // Otros
                    ConTurbo = vehiculos.Count(v => v.tieneTurbo),
                    SinTurbo = vehiculos.Count(v => !v.tieneTurbo),

                    // Top Marcas
                    TopMarcas = vehiculos.GroupBy(v => v.marca)
                        .Select(g => new { Marca = g.Key, Cantidad = g.Count() })
                        .OrderByDescending(x => x.Cantidad)
                        .Take(5)
                        .ToDictionary(x => x.Marca, x => x.Cantidad),

                    // Año más reciente y más antiguo
                    AnioMasReciente = vehiculos.Max(v => v.anioFabricacion),
                    AnioMasAntiguo = vehiculos.Min(v => v.anioFabricacion)
                };
            }
        }

        // Clase para almacenar estadísticas generales
        public class EstadisticasGenerales
        {
            public int TotalVehiculos { get; set; }

            // Por Confiabilidad
            public int Excelentes { get; set; }
            public int Buenos { get; set; }
            public int Regulares { get; set; }
            public int Malos { get; set; }

            // Por Procedencia
            public int Japoneses { get; set; }
            public int Americanos { get; set; }
            public int Chinos { get; set; }
            public int Alemanes { get; set; }
            public int Europeos { get; set; }
            public int Coreanos { get; set; }

            // Promedios
            public double PromedioPuntuacion { get; set; }
            public double PromedioAirbags { get; set; }

            // Otros
            public int ConTurbo { get; set; }
            public int SinTurbo { get; set; }

            // Top Marcas
            public Dictionary<string, int> TopMarcas { get; set; }

            // Rangos
            public int AnioMasReciente { get; set; }
            public int AnioMasAntiguo { get; set; }

            public EstadisticasGenerales()
            {
                TopMarcas = new Dictionary<string, int>();
            }

            public double PorcentajeExcelentes => TotalVehiculos > 0 ? (Excelentes * 100.0 / TotalVehiculos) : 0;
            public double PorcentajeBuenos => TotalVehiculos > 0 ? (Buenos * 100.0 / TotalVehiculos) : 0;
            public double PorcentajeRegulares => TotalVehiculos > 0 ? (Regulares * 100.0 / TotalVehiculos) : 0;
            public double PorcentajeMalos => TotalVehiculos > 0 ? (Malos * 100.0 / TotalVehiculos) : 0;
        }
    }

}
