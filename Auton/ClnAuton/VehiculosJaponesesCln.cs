using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class VehiculosJaponesesCln
    {
        // Inserta un nuevo vehículo
        public static int insertar(Vehiculo vehiculo)
        {
            using (var context = new AutonEntities())
            {
                context.Vehiculo.Add(vehiculo);
                context.SaveChanges();
                return vehiculo.id;
            }
        }

        // Actualiza un vehículo existente (SIN KILOMETRAJE)
        public static int actualizar(Vehiculo vehiculo)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Vehiculo.Find(vehiculo.id);
                if (existente != null)
                {
                    // Características específicas
                    existente.generacion = vehiculo.generacion;
                    existente.paisFabricacion = vehiculo.paisFabricacion;
                    existente.transmision = vehiculo.transmision;
                    existente.tieneTurbo = vehiculo.tieneTurbo;
                    existente.motor = vehiculo.motor;
                    existente.combustible = vehiculo.combustible;

                    // Seguridad
                    existente.nroAirbags = vehiculo.nroAirbags;
                    existente.seguridadChoques = vehiculo.seguridadChoques;
                    existente.organizacionSeguridad = vehiculo.organizacionSeguridad;

                    // Evaluación
                    existente.anioFabricacion = vehiculo.anioFabricacion;
                    existente.confiabilidad = vehiculo.confiabilidad;
                    existente.puntuacion = vehiculo.puntuacion;
                    existente.observaciones = vehiculo.observaciones;

                    existente.usuarioRegistro = vehiculo.usuarioRegistro;
                    return context.SaveChanges();
                }
                return 0;
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

        // Lista vehículos por rango de años
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
