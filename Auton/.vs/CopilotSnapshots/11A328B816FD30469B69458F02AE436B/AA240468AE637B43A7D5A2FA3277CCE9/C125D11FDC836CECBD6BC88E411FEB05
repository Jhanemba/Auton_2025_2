using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class ModeloVehiculoCln
    {
        public static int insertar(ModeloVehiculo modelo)
        {
            using (var context = new AutonEntities())
            {
                context.ModeloVehiculo.Add(modelo);
                context.SaveChanges();
                return modelo.id;
            }
        }

        public static int actualizar(ModeloVehiculo modelo)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.ModeloVehiculo.Find(modelo.id);

                if (existente == null)
                {
                    return 0; // No existe el modelo
                }

                // ACTUALIZAR TODOS LOS CAMPOS
                existente.idMarca = modelo.idMarca;
                existente.idTipoVehiculo = modelo.idTipoVehiculo;
                existente.nombre = modelo.nombre;
                existente.descripcion = modelo.descripcion;
                existente.usuarioRegistro = modelo.usuarioRegistro;

                // MARCAR COMO MODIFICADO (forzar detección de cambios)
                context.Entry(existente).State = System.Data.Entity.EntityState.Modified;

                // Guardar cambios
                context.SaveChanges();

                // RETORNAR 1 SI NO HUBO EXCEPCIONES
                return 1;
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var modelo = context.ModeloVehiculo.Find(id);
                if (modelo == null)
                {
                    return 0;
                }
                modelo.estado = -1;
                modelo.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static ModeloVehiculo obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.ModeloVehiculo.Find(id);
            }
        }

        public static List<ModeloVehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.ModeloVehiculo
                    .Where(x => x.estado != -1)
                    .OrderBy(x => x.nombre)
                    .ToList();
            }
        }

        public static List<paModeloPorMarca_Result> listarPorMarca(int idMarca)
        {
            using (var context = new AutonEntities())
            {
                return context.paModeloPorMarca(idMarca).ToList();
            }
        }
    }
}
