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

        // Actualiza un vehículo existente - CON FOTOGRAFÍA
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

                    // Actualizar todos los campos
                    existente.nroChasis = vehiculo.nroChasis;
                    existente.idModeloVehiculo = vehiculo.idModeloVehiculo;
                    existente.version = vehiculo.version;
                    existente.anioFabricacion = vehiculo.anioFabricacion;
                    existente.generacion = vehiculo.generacion;
                    existente.paisFabricacion = vehiculo.paisFabricacion;
                    existente.transmision = vehiculo.transmision;
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
                    existente.usuarioRegistro = vehiculo.usuarioRegistro;

                    // ⭐ ACTUALIZAR FOTOGRAFÍA solo si se indica
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

        // Elimina lógicamente un vehículo
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

        // Obtiene un vehículo por su ID (CON fotografía)
        public static Vehiculo obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Find(id);
            }
        }

        // ⭐ NUEVO: Obtiene SOLO la fotografía de un vehículo
        // (Optimizado para no cargar todos los datos)
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

        // ⭐ NUEVO: Actualiza SOLO la fotografía de un vehículo
        public static int actualizarFotografia(int id, byte[] fotografia, string usuario)
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    var vehiculo = context.Vehiculo.Find(id);
                    if (vehiculo == null) return 0;

                    vehiculo.fotografia = fotografia;
                    vehiculo.usuarioRegistro = usuario;

                    return context.SaveChanges();
                }
            }
            catch
            {
                return 0;
            }
        }

        // ⭐ NUEVO: Elimina la fotografía de un vehículo
        public static int eliminarFotografia(int id, string usuario)
        {
            return actualizarFotografia(id, null, usuario);
        }

        // Lista todos los vehículos activos
        public static List<Vehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Where(x => x.estado != -1).ToList();
            }
        }

        // Lista vehículos usando procedimiento almacenado
        public static List<paVehiculoListar_Result> listarPa(string parametro)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculoListar(parametro ?? string.Empty).ToList();
            }
        }
    }
}
