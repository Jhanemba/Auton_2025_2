using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class UsuarioCln
    {
        public static int insertar(Usuario usuario)
        {
            using (var context = new AutonEntities())
            {
                context.Usuario.Add(usuario);
                context.SaveChanges();
                return usuario.id;
            }
        }

        public static int actualizar(Usuario usuario)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Usuario.Find(usuario.id);
                existente.idEmpleado = usuario.idEmpleado;
                existente.usuario1 = usuario.usuario1;
                existente.clave = usuario.clave;
                existente.esAdministrador = usuario.esAdministrador;
                existente.usuarioRegistro = usuario.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var usuario = context.Usuario.Find(id);
                usuario.estado = -1;
                usuario.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static Usuario obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Usuario.Find(id);
            }
        }

        public static Usuario obtenerUnoPorEmpleado(int idEmpleado)
        {
            using (var context = new AutonEntities())
            {
                return context.Usuario.Where(x => x.idEmpleado == idEmpleado).FirstOrDefault();
            }
        }

        public static List<Usuario> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Usuario
                    .Where(x => x.estado != -1)
                    .OrderBy(x => x.usuario1)
                    .ToList();
            }
        }

        public static Usuario validar(string usuario, string clave)
        {
            using (var context = new AutonEntities())
            {
                return context.Usuario
                    .Where(u => u.usuario1 == usuario && u.clave == clave && u.estado == 1)
                    .FirstOrDefault();
            }
        }

        public static paUsuarioValidar_Result validarPa(string usuario, string clave)
        {
            using (var context = new AutonEntities())
            {
                return context.paUsuarioValidar(usuario, clave).FirstOrDefault();
            }
        }
    }
}
