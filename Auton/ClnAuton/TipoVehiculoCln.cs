using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class TipoVehiculoCln
    {
        public static int insertar(TipoVehiculo tipo)
        {
            using (var context = new AutonEntities())
            {
                context.TipoVehiculo.Add(tipo);
                context.SaveChanges();
                return tipo.id;
            }
        }

        public static int actualizar(TipoVehiculo tipo)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.TipoVehiculo.Find(tipo.id);
                existente.descripcion = tipo.descripcion;
                existente.usuarioRegistro = tipo.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var tipo = context.TipoVehiculo.Find(id);
                tipo.estado = -1;
                tipo.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static TipoVehiculo obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.TipoVehiculo.Find(id);
            }
        }

        public static List<TipoVehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.TipoVehiculo
                    .Where(x => x.estado != -1)
                    .OrderBy(x => x.descripcion)
                    .ToList();
            }
        }

        public static List<paTipoVehiculoListar_Result> listarPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paTipoVehiculoListar().ToList();
            }
        }
    }
}
