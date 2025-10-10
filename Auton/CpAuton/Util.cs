using CadAuton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CpAuton
{
    public class Util
    {
        // Usuario actual del sistema (usamos el resultado del PA que tiene más info)
        public static paUsuarioValidar_Result usuario { get; set; }

        /// <summary>
        /// Encripta una cadena usando MD5
        /// </summary>
        /// <param name="texto">Texto a encriptar</param>
        /// <returns>Texto encriptado en Base64</returns>
        public static string Encrypt(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            // Convertir el texto a Base64 directamente
            byte[] data = Encoding.UTF8.GetBytes(texto);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Verifica si hay un usuario logueado
        /// </summary>
        /// <returns>True si hay sesión activa</returns>
        public static bool HaySesionActiva()
        {
            return usuario != null;
        }

        /// <summary>
        /// Verifica si el usuario actual es administrador
        /// </summary>
        /// <returns>True si es administrador</returns>
        public static bool EsAdministrador()
        {
            return usuario != null && usuario.esAdministrador;
        }

        /// <summary>
        /// Cierra la sesión actual
        /// </summary>
        public static void CerrarSesion()
        {
            usuario = null;
        }

        /// <summary>
        /// Obtiene el nombre del usuario actual
        /// </summary>
        /// <returns>Nombre del usuario o "No logueado"</returns>
        public static string ObtenerNombreUsuario()
        {
            return usuario != null ? usuario.nombreCompleto : "No logueado";
        }

        /// <summary>
        /// Obtiene el usuario del login
        /// </summary>
        /// <returns>Usuario de login</returns>
        public static string ObtenerUsuarioLogin()
        {
            return usuario != null ? usuario.usuario : "";
        }
    }
}
