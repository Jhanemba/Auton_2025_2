using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class ProcedenciaCln
    {
        public static int insertar(Procedencia procedencia)
        {
            using (var context = new AutonEntities())
            {
                context.Procedencia.Add(procedencia);
                context.SaveChanges();
                return procedencia.id;
            }
        }

        public static int actualizar(Procedencia procedencia)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Procedencia.Find(procedencia.id);
                existente.codigo = procedencia.codigo;
                existente.descripcion = procedencia.descripcion;
                existente.usuarioRegistro = procedencia.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var procedencia = context.Procedencia.Find(id);
                procedencia.estado = -1;
                procedencia.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static Procedencia obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Procedencia.Find(id);
            }
        }

        public static List<Procedencia> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Procedencia
                    .Where(x => x.estado != -1)
                    .OrderBy(x => x.descripcion)
                    .ToList();
            }
        }

        public static List<paProcedenciaListar_Result> listarPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paProcedenciaListar().ToList();
            }
        }
    }
}
