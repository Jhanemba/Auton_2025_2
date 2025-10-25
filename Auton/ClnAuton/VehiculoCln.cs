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
        /// Actualiza un vehículo - VERSIÓN COMPLETA CORREGIDA ✅
        /// </summary>
        public static int actualizar(Vehiculo vehiculo)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Vehiculo.Find(vehiculo.id);
                if (existente != null)
                {
                    // CAMPOS BÁSICOS
                    existente.idModeloVehiculo = vehiculo.idModeloVehiculo;
                    existente.anioFabricacion = vehiculo.anioFabricacion;

                    // ⭐ CARACTERÍSTICAS ESPECÍFICAS (LOS QUE FALTABAN)
                    existente.generacion = vehiculo.generacion;
                    existente.paisFabricacion = vehiculo.paisFabricacion;  // ← ESTE ERA TU PROBLEMA
                    existente.transmision = vehiculo.transmision;
                    existente.tieneTurbo = vehiculo.tieneTurbo;
                    existente.motor = vehiculo.motor;
                    existente.combustible = vehiculo.combustible;

                    // ⭐ SEGURIDAD
                    existente.nroAirbags = vehiculo.nroAirbags;
                    existente.seguridadChoques = vehiculo.seguridadChoques;
                    existente.organizacionSeguridad = vehiculo.organizacionSeguridad;

                    // ⭐ EVALUACIÓN
                    existente.confiabilidad = vehiculo.confiabilidad;
                    existente.puntuacion = vehiculo.puntuacion;
                    existente.kilometraje = vehiculo.kilometraje;
                    existente.observaciones = vehiculo.observaciones;

                    // METADATA
                    existente.usuarioRegistro = vehiculo.usuarioRegistro;

                    return context.SaveChanges();
                }
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
