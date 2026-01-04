using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ClnAuton
{
    public class VehiculoCln
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
                    existente.nroPuertas = vehiculo.nroPuertas;
                    existente.nroAsientos = vehiculo.nroAsientos;
                    existente.traccion = vehiculo.traccion;
                    existente.tieneTurbo = vehiculo.tieneTurbo;
                    existente.motor = vehiculo.motor;
                    existente.combustible = vehiculo.combustible;
                    existente.frenos = vehiculo.frenos;
                    existente.nroAirbags = vehiculo.nroAirbags;
                    existente.seguridadChoques = vehiculo.seguridadChoques;
                    existente.organizacionSeguridad = vehiculo.organizacionSeguridad;
                    existente.confiabilidad = vehiculo.confiabilidad;
                    existente.puntuacion = vehiculo.puntuacion;
                    existente.kilometraje = vehiculo.kilometraje;
                    existente.observaciones = vehiculo.observaciones;
                    //existente.usuarioRegistro = vehiculo.usuarioRegistro;

                    // Copiar usuarioModificacion y fechaModificacion si vienen en el objeto
                    if (!string.IsNullOrWhiteSpace(vehiculo.usuarioModificacion))
                    {
                        existente.usuarioModificacion = vehiculo.usuarioModificacion;
                        existente.fechaModificacion = vehiculo.fechaModificacion ?? DateTime.Now;
                    }

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
                    // Registrar quién hizo la eliminación en la auditoría
                    if (!string.IsNullOrWhiteSpace(usuario))
                    {
                        vehiculo.usuarioModificacion = usuario;
                        vehiculo.fechaModificacion = DateTime.Now;
                    }
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
    }
}
