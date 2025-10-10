using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class CargoCln
    {
        public static int insertar(Cargo cargo)
        {
            using (var context = new AutonEntities())
            {
                context.Cargo.Add(cargo);
                context.SaveChanges();
                return cargo.id;
            }
        }

        public static int actualizar(Cargo cargo)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Cargo.Find(cargo.id);
                existente.descripcion = cargo.descripcion;
                existente.usuarioRegistro = cargo.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var cargo = context.Cargo.Find(id);
                cargo.estado = -1;
                cargo.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static Cargo obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Cargo.Find(id);
            }
        }

        public static List<Cargo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Cargo
                    .Where(x => x.estado != -1)
                    .OrderBy(x => x.descripcion)
                    .ToList();
            }
        }

        public static List<paCargoListar_Result> listarPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paCargoListar().ToList();
            }
        }
    }
}
