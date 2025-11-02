using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class VehiculoCln
    {
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
        /// Actualiza un vehículo existente - VERSIÓN CORREGIDA
        /// </summary>
        /// <param name="vehiculo">Objeto Vehiculo con los datos actualizados</param>
        /// <returns>1 si se actualizó correctamente, 0 si no</returns>
        public static int actualizar(Vehiculo vehiculo)
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    // Buscar el vehículo existente
                    var existente = context.Vehiculo.Find(vehiculo.id);

                    if (existente == null)
                    {
                        return 0; // No existe el vehículo
                    }

                    // ⭐ ACTUALIZAR TODOS LOS CAMPOS
                    existente.nroChasis = vehiculo.nroChasis;
                    existente.idModeloVehiculo = vehiculo.idModeloVehiculo;
                    existente.anioFabricacion = vehiculo.anioFabricacion;
                    existente.generacion = vehiculo.generacion;
                    existente.paisFabricacion = vehiculo.paisFabricacion;
                    existente.transmision = vehiculo.transmision;
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

                    // ✅ MARCAR COMO MODIFICADO (forzar detección de cambios)
                    context.Entry(existente).State = System.Data.Entity.EntityState.Modified;

                    // Guardar cambios
                    context.SaveChanges();

                    // ✅ SIEMPRE RETORNAR 1 SI LLEGAMOS AQUÍ (no hubo excepciones)
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var vehiculo = context.Vehiculo.Find(id);
                if (vehiculo != null)
                {
                    vehiculo.estado = -1;
                    vehiculo.usuarioRegistro = usuarioRegistro;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Vehiculo obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Find(id);
            }
        }

        public static List<Vehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Where(x => x.estado != -1).ToList();
            }
        }

        public static List<paVehiculoListar_Result> listarPa(string parametro)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculoListar(parametro ?? string.Empty).ToList();
            }
        }
    }
}
