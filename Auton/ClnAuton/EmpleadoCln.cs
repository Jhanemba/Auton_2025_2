using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class EmpleadoCln
    {
        public static int insertar(Empleado empleado)
        {
            using (var context = new AutonEntities())
            {
                context.Empleado.Add(empleado);
                context.SaveChanges();
                return empleado.id;
            }
        }

        public static int actualizar(Empleado empleado)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Empleado.Find(empleado.id);
                if (existente == null) return 0;

                existente.idCargo = empleado.idCargo;
                existente.cedulaIdentidad = empleado.cedulaIdentidad;
                existente.nombres = empleado.nombres;
                existente.primerApellido = empleado.primerApellido;
                existente.segundoApellido = empleado.segundoApellido;
                existente.direccion = empleado.direccion;
                existente.celular = empleado.celular;
                existente.usuarioRegistro = empleado.usuarioRegistro;

                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new AutonEntities())
            {
                var empleado = context.Empleado.Find(id);
                if (empleado == null) return 0;

                empleado.estado = -1;
                empleado.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static Empleado obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Empleado.Find(id);
            }
        }

        public static List<Empleado> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Empleado
                    .Where(e => e.estado != -1)
                    .OrderBy(e => e.nombres)
                    .ToList();
            }
        }
        // Método usando procedimiento almacenado
        public static List<paEmpleadoListar_Result> listarConUsuario(string parametro = "")
        {
            using (var context = new AutonEntities())
            {
                return context.paEmpleadoListar(parametro ?? string.Empty).ToList();
            }
        }
    }
}
