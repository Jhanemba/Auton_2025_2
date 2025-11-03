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

        // Encripta una cadena usando MD5
        public static string Encrypt(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            // Convertir el texto a Base64 directamente
            byte[] data = Encoding.UTF8.GetBytes(texto);
            return Convert.ToBase64String(data);
        }

        // Verifica si hay un usuario logueado
        public static bool HaySesionActiva()
        {
            return usuario != null;
        }

        // Verifica si el usuario actual es administrador
        public static bool EsAdministrador()
        {
            return usuario != null && usuario.esAdministrador;
        }

        // Cierra la sesión actual
        public static void CerrarSesion()
        {
            usuario = null;
        }

        // Obtiene el nombre del usuario actual
        public static string ObtenerNombreUsuario()
        {
            return usuario != null ? usuario.nombreCompleto : "No logueado";
        }

        // Obtiene el usuario del login
        public static string ObtenerUsuarioLogin()
        {
            return usuario != null ? usuario.usuario : "";
        }
    }
}
