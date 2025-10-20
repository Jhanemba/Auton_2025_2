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
                existente.idMarca = modelo.idMarca;
                existente.idTipoVehiculo = modelo.idTipoVehiculo;
                existente.nombre = modelo.nombre;
                //existente.anioInicio = modelo.anioInicio;
                //existente.anioFin = modelo.anioFin;
                //existente.motor = modelo.motor;
                //existente.combustible = modelo.combustible;
                existente.usuarioRegistro = modelo.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var modelo = context.ModeloVehiculo.Find(id);
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
