using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class ConsultaVehiculosCln
    {
        /// <summary>
        /// Inserta un nuevo vehículo
        /// </summary>
        /// <param name="vehiculo">Objeto Vehiculo a insertar</param>
        /// <returns>ID del nuevo vehículo</returns>
        public static int insertar(Vehiculo vehiculo)
        {
            using (var context = new AutonEntities())
            {
                context.Vehiculo.Add(vehiculo);
                context.SaveChanges();
                return vehiculo.id;
            }
        }

        /// <summary>
        /// Actualiza un vehículo existente
        /// </summary>
        /// <param name="vehiculo">Objeto Vehiculo con los datos actualizados</param>
        /// <returns>Número de registros afectados</returns>
        public static int actualizar(Vehiculo vehiculo)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Vehiculo.Find(vehiculo.id);
                if (existente != null)
                {
                    existente.idModeloVehiculo = vehiculo.idModeloVehiculo;
                    existente.anioFabricacion = vehiculo.anioFabricacion;
                    existente.confiabilidad = vehiculo.confiabilidad;
                    existente.puntuacion = vehiculo.puntuacion;
                    existente.observaciones = vehiculo.observaciones;
                    existente.kilometraje = vehiculo.kilometraje;
                    existente.propietario = vehiculo.propietario;
                    existente.usuarioRegistro = vehiculo.usuarioRegistro;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        /// <summary>
        /// Elimina lógicamente un vehículo (estado = -1)
        /// </summary>
        /// <param name="id">ID del vehículo a eliminar</param>
        /// <param name="usuario">Usuario que realiza la eliminación</param>
        /// <returns>Número de registros afectados</returns>
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

        /// <summary>
        /// Obtiene un vehículo por su ID
        /// </summary>
        /// <param name="id">ID del vehículo</param>
        /// <returns>Objeto Vehiculo</returns>
        public static Vehiculo obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Find(id);
            }
        }

        /// <summary>
        /// Lista todos los vehículos activos
        /// </summary>
        /// <returns>Lista de vehículos</returns>
        public static List<Vehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Where(x => x.estado != -1).ToList();
            }
        }

        /// <summary>
        /// Lista vehículos usando el procedimiento almacenado con parámetro de búsqueda
        /// </summary>
        /// <param name="parametro">Parámetro de búsqueda (marca, modelo, etc.)</param>
        /// <returns>Lista de resultados del procedimiento almacenado</returns>
        public static List<paVehiculoListar_Result> listarPa(string parametro)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculoListar(parametro ?? string.Empty).ToList();
            }
        }

        /// <summary>
        /// Lista todos los vehículos usando procedimiento almacenado (sin filtro)
        /// </summary>
        /// <returns>Lista de vehículos completa</returns>
        public static List<paVehiculoListar_Result> listarTodosPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculoListar(string.Empty).ToList();
            }
        }

        /// <summary>
        /// Obtiene el total de vehículos activos
        /// </summary>
        /// <returns>Número total de vehículos</returns>
        public static int obtenerTotalVehiculos()
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Count(x => x.estado != -1);
            }
        }

        /// <summary>
        /// Lista vehículos por confiabilidad
        /// </summary>
        /// <param name="confiabilidad">Nivel de confiabilidad (Excelente, Bueno, Regular, Malo)</param>
        /// <returns>Lista de vehículos filtrados por confiabilidad</returns>
        public static List<Vehiculo> listarPorConfiabilidad(string confiabilidad)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo
                    .Where(x => x.estado != -1 && x.confiabilidad == confiabilidad)
                    .ToList();
            }
        }

        /// <summary>
        /// Lista vehículos por rango de años
        /// </summary>
        /// <param name="anioInicio">Año de inicio</param>
        /// <param name="anioFin">Año de fin</param>
        /// <returns>Lista de vehículos en el rango de años</returns>
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
    }

}
