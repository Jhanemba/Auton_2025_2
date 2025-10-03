using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnAuton
{
    public class ConsultaVehiculosCln
    {
        /// <summary>
        /// Inserta un nuevo vehículo
        /// </summary>
        /// <param name="vehiculo">Objeto Vehiculo a insertar</param>
        /// <returns>ID del nuevo vehículo</returns>
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
        /// Actualiza un vehículo existente
        /// </summary>
        /// <param name="vehiculo">Objeto Vehiculo con los datos actualizados</param>
        /// <returns>Número de registros afectados</returns>
        public static int actualizar(Vehiculo vehiculo)
        {
            using (var context = new AutonEntities())
            {
                var existente = context.Vehiculo.Find(vehiculo.id);
                if (existente != null)
                {
                    existente.idModeloVehiculo = vehiculo.idModeloVehiculo;
                    existente.anioFabricacion = vehiculo.anioFabricacion;
                    existente.confiabilidad = vehiculo.confiabilidad;
                    existente.puntuacion = vehiculo.puntuacion;
                    existente.observaciones = vehiculo.observaciones;
                    existente.kilometraje = vehiculo.kilometraje;
                    existente.propietario = vehiculo.propietario;
                    existente.usuarioRegistro = vehiculo.usuarioRegistro;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        /// <summary>
        /// Elimina lógicamente un vehículo (estado = -1)
        /// </summary>
        /// <param name="id">ID del vehículo a eliminar</param>
        /// <param name="usuario">Usuario que realiza la eliminación</param>
        /// <returns>Número de registros afectados</returns>
        public static int eliminar(int id, string usuario)
        {
            using (var context = new AutonEntities())
            {
                var vehiculo = context.Vehiculo.Find(id);
                if (vehiculo != null)
                {
                    vehiculo.estado = -1;
                    vehiculo.usuarioRegistro = usuario;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        /// <summary>
        /// Obtiene un vehículo por su ID
        /// </summary>
        /// <param name="id">ID del vehículo</param>
        /// <returns>Objeto Vehiculo</returns>
        public static Vehiculo obtenerUno(int id)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Find(id);
            }
        }

        /// <summary>
        /// Lista todos los vehículos activos
        /// </summary>
        /// <returns>Lista de vehículos</returns>
        public static List<Vehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Where(x => x.estado != -1).ToList();
            }
        }

        /// <summary>
        /// Lista vehículos usando el procedimiento almacenado con parámetro de búsqueda
        /// </summary>
        /// <param name="parametro">Parámetro de búsqueda (marca, modelo, etc.)</param>
        /// <returns>Lista de resultados del procedimiento almacenado</returns>
        public static List<paVehiculoListar_Result> listarPa(string parametro)
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculoListar(parametro ?? string.Empty).ToList();
            }
        }

        /// <summary>
        /// Lista todos los vehículos usando procedimiento almacenado (sin filtro)
        /// </summary>
        /// <returns>Lista de vehículos completa</returns>
        public static List<paVehiculoListar_Result> listarTodosPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paVehiculoListar(string.Empty).ToList();
            }
        }

        /// <summary>
        /// Obtiene el total de vehículos activos
        /// </summary>
        /// <returns>Número total de vehículos</returns>
        public static int obtenerTotalVehiculos()
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo.Count(x => x.estado != -1);
            }
        }

        /// <summary>
        /// Lista vehículos por confiabilidad
        /// </summary>
        /// <param name="confiabilidad">Nivel de confiabilidad (Excelente, Bueno, Regular, Malo)</param>
        /// <returns>Lista de vehículos filtrados por confiabilidad</returns>
        public static List<Vehiculo> listarPorConfiabilidad(string confiabilidad)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo
                    .Where(x => x.estado != -1 && x.confiabilidad == confiabilidad)
                    .ToList();
            }
        }

        /// <summary>
        /// Lista vehículos por rango de años
        /// </summary>
        /// <param name="anioInicio">Año de inicio</param>
        /// <param name="anioFin">Año de fin</param>
        /// <returns>Lista de vehículos en el rango de años</returns>
        public static List<Vehiculo> listarPorRangoAnios(int anioInicio, int anioFin)
        {
            using (var context = new AutonEntities())
            {
                return context.Vehiculo
                    .Where(x => x.estado != -1 &&
                           x.anioFabricacion >= anioInicio &&
                           x.anioFabricacion <= anioFin)
                    .ToList();
            }
        }
    }

    public class ProcedenciaCln
    {
        /// <summary>
        /// Lista todas las procedencias activas usando procedimiento almacenado
        /// </summary>
        /// <returns>Lista de procedencias</returns>
        public static List<paProcedenciaListar_Result> listarPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paProcedenciaListar().ToList();
            }
        }

        /// <summary>
        /// Lista todas las procedencias activas
        /// </summary>
        /// <returns>Lista de procedencias</returns>
        public static List<Procedencia> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Procedencia.Where(x => x.estado != -1)
                                         .OrderBy(x => x.descripcion)
                                         .ToList();
            }
        }
    }

    public class MarcaCln
    {
        /// <summary>
        /// Lista marcas por procedencia usando procedimiento almacenado
        /// </summary>
        /// <param name="idProcedencia">ID de la procedencia</param>
        /// <returns>Lista de marcas</returns>
        public static List<paMarcaPorProcedencia_Result> listarPorProcedencia(int idProcedencia)
        {
            using (var context = new AutonEntities())
            {
                return context.paMarcaPorProcedencia(idProcedencia).ToList();
            }
        }

        /// <summary>
        /// Lista todas las marcas activas
        /// </summary>
        /// <returns>Lista de marcas</returns>
        public static List<Marca> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Marca.Where(x => x.estado != -1)
                                   .OrderBy(x => x.nombre)
                                   .ToList();
            }
        }
    }

    public class ModeloVehiculoCln
    {
        /// <summary>
        /// Lista modelos por marca usando procedimiento almacenado
        /// </summary>
        /// <param name="idMarca">ID de la marca</param>
        /// <returns>Lista de modelos</returns>
        public static List<paModeloPorMarca_Result> listarPorMarca(int idMarca)
        {
            using (var context = new AutonEntities())
            {
                return context.paModeloPorMarca(idMarca).ToList();
            }
        }

        /// <summary>
        /// Lista todos los modelos activos
        /// </summary>
        /// <returns>Lista de modelos</returns>
        public static List<ModeloVehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.ModeloVehiculo.Where(x => x.estado != -1)
                                           .OrderBy(x => x.nombre)
                                           .ToList();
            }
        }
    }

    public class TipoVehiculoCln
    {
        /// <summary>
        /// Lista todos los tipos de vehículo activos usando procedimiento almacenado
        /// </summary>
        /// <returns>Lista de tipos de vehículo</returns>
        public static List<paTipoVehiculoListar_Result> listarPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paTipoVehiculoListar().ToList();
            }
        }

        /// <summary>
        /// Lista todos los tipos de vehículo activos
        /// </summary>
        /// <returns>Lista de tipos de vehículo</returns>
        public static List<TipoVehiculo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.TipoVehiculo.Where(x => x.estado != -1)
                                         .OrderBy(x => x.descripcion)
                                         .ToList();
            }
        }
    }

    public class CargoCln
    {
        /// <summary>
        /// Lista todos los cargos activos usando procedimiento almacenado
        /// </summary>
        /// <returns>Lista de cargos</returns>
        public static List<paCargoListar_Result> listarPa()
        {
            using (var context = new AutonEntities())
            {
                return context.paCargoListar().ToList();
            }
        }

        /// <summary>
        /// Lista todos los cargos activos
        /// </summary>
        /// <returns>Lista de cargos</returns>
        public static List<Cargo> listar()
        {
            using (var context = new AutonEntities())
            {
                return context.Cargo.Where(x => x.estado != -1)
                                   .OrderBy(x => x.descripcion)
                                   .ToList();
            }
        }
    }

    public class UsuarioCln
    {
        /// <summary>
        /// Valida credenciales de usuario usando procedimiento almacenado
        /// </summary>
        /// <param name="usuario">Nombre de usuario</param>
        /// <param name="clave">Contraseña (ya encriptada en base64)</param>
        /// <returns>Datos del usuario validado o null</returns>
        public static paUsuarioValidar_Result validarCredenciales(string usuario, string clave)
        {
            using (var context = new AutonEntities())
            {
                var resultado = context.paUsuarioValidar(usuario, clave).FirstOrDefault();
                return resultado;
            }
        }

        /// <summary>
        /// Encripta la contraseña en Base64
        /// </summary>
        /// <param name="clave">Contraseña en texto plano</param>
        /// <returns>Contraseña encriptada</returns>
        public static string encriptarClave(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave))
                return string.Empty;

            byte[] bytes = Encoding.UTF8.GetBytes(clave);
            return Convert.ToBase64String(bytes);
        }
    }
}
