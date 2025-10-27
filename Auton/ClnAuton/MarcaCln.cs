using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class MarcaCln
    {
        public static int insertar(Marca marca)
        {
            using (var context = new AutonEntities())
            {
                context.Marca.Add(marca);
                context.SaveChanges();
                return marca.id;
            }
        }

        public static int actualizar(Marca marca)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Marca.Find(marca.id);

                if (existente == null)
                {
                    return 0; // ← CRÍTICO: Manejar cuando no existe
                }

                existente.idProcedencia = marca.idProcedencia;
                existente.nombre = marca.nombre;
                existente.descripcion = marca.descripcion;
                existente.usuarioRegistro = marca.usuarioRegistro;

                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var marca = context.Marca.Find(id);
                marca.estado = -1;
                marca.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static Marca obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Marca.Find(id);
            }
        }

        public static List<Marca> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Marca
                    .Where(x => x.estado != -1)
                    .OrderBy(x => x.nombre)
                    .ToList();
            }
        }

        public static List<paMarcaPorProcedencia_Result> listarPorProcedencia(int idProcedencia)
        {
            using (var context = new AutonEntities())
            {
                return context.paMarcaPorProcedencia(idProcedencia).ToList();
            }
        }
    }
}
