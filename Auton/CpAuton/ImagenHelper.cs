using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpAuton
{
    /// <summary>
    /// Clase auxiliar para procesamiento de imágenes
    /// Optimiza, redimensiona y convierte imágenes para almacenamiento en BD
    /// </summary>
    public static class ImagenHelper
    {
        // Tamaño máximo para almacenamiento (ancho máximo)
        private const int ANCHO_MAXIMO = 1200;
        private const int ALTO_MAXIMO = 900;
        private const long CALIDAD_JPEG = 85L; // Calidad de compresión JPEG (0-100)

        /// <summary>
        /// Convierte una imagen a un array de bytes optimizado para BD
        /// </summary>
        public static byte[] ConvertirImagenABytes(Image imagen)
        {
            if (imagen == null) return null;

            try
            {
                // Redimensionar si es necesario
                Image imagenOptimizada = RedimensionarImagen(imagen, ANCHO_MAXIMO, ALTO_MAXIMO);

                using (MemoryStream ms = new MemoryStream())
                {
                    // Configurar encoder JPEG con calidad específica
                    ImageCodecInfo jpegCodec = ObtenerEncoderInfo("image/jpeg");
                    System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
                    encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(
                        System.Drawing.Imaging.Encoder.Quality, CALIDAD_JPEG);

                    imagenOptimizada.Save(ms, jpegCodec, encoderParams);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir imagen: {ex.Message}");
            }
        }

        /// <summary>
        /// Convierte un array de bytes a una imagen
        /// </summary>
        public static Image ConvertirBytesAImagen(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir bytes a imagen: {ex.Message}");
            }
        }

        /// <summary>
        /// Redimensiona una imagen manteniendo la proporción
        /// </summary>
        private static Image RedimensionarImagen(Image imagen, int anchoMax, int altoMax)
        {
            int anchoOriginal = imagen.Width;
            int altoOriginal = imagen.Height;

            // Si la imagen ya es pequeña, no redimensionar
            if (anchoOriginal <= anchoMax && altoOriginal <= altoMax)
                return new Bitmap(imagen);

            // Calcular nuevas dimensiones manteniendo proporción
            double ratioX = (double)anchoMax / anchoOriginal;
            double ratioY = (double)altoMax / altoOriginal;
            double ratio = Math.Min(ratioX, ratioY);

            int nuevoAncho = (int)(anchoOriginal * ratio);
            int nuevoAlto = (int)(altoOriginal * ratio);

            // Crear bitmap con alta calidad
            Bitmap bitmapRedimensionado = new Bitmap(nuevoAncho, nuevoAlto);
            using (Graphics g = Graphics.FromImage(bitmapRedimensionado))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                g.DrawImage(imagen, 0, 0, nuevoAncho, nuevoAlto);
            }

            return bitmapRedimensionado;
        }

        /// <summary>
        /// Obtiene el encoder para un tipo MIME específico
        /// </summary>
        private static ImageCodecInfo ObtenerEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo encoder in encoders)
            {
                if (encoder.MimeType == mimeType)
                    return encoder;
            }
            return null;
        }

        /// <summary>
        /// Carga una imagen desde un archivo
        /// </summary>
        public static Image CargarImagenDesdeArchivo(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException("El archivo de imagen no existe");

            try
            {
                // Cargar en memoria para evitar bloqueo del archivo
                using (FileStream fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read))
                {
                    return Image.FromStream(fs);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar imagen: {ex.Message}");
            }
        }

        /// <summary>
        /// Valida si un archivo es una imagen válida
        /// </summary>
        public static bool EsImagenValida(string rutaArchivo)
        {
            try
            {
                string extension = Path.GetExtension(rutaArchivo).ToLower();
                string[] extensionesValidas = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

                if (Array.IndexOf(extensionesValidas, extension) == -1)
                    return false;

                // Intentar cargar la imagen
                using (Image img = Image.FromFile(rutaArchivo))
                {
                    return img != null;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene el tamaño en KB de una imagen
        /// </summary>
        public static double ObtenerTamanoKB(byte[] bytes)
        {
            if (bytes == null) return 0;
            return Math.Round(bytes.Length / 1024.0, 2);
        }

        /// <summary>
        /// Crea una imagen por defecto (sin foto disponible)
        /// </summary>
        public static Image CrearImagenPorDefecto(int ancho, int alto)
        {
            Bitmap bmp = new Bitmap(ancho, alto);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Fondo gris claro
                g.Clear(Color.FromArgb(240, 240, 240));

                // Borde
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 2))
                {
                    g.DrawRectangle(pen, 1, 1, ancho - 2, alto - 2);
                }

                // Texto "Sin foto"
                string texto = "📷\nSin foto disponible";
                using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(texto, font, Brushes.Gray, new RectangleF(0, 0, ancho, alto), sf);
                }
            }
            return bmp;
        }

        /// <summary>
        /// Ajusta una imagen para un PictureBox manteniendo proporción
        /// </summary>
        public static void AjustarImagenPictureBox(PictureBox pb, Image imagen)
        {
            if (imagen == null)
            {
                pb.Image = CrearImagenPorDefecto(pb.Width, pb.Height);
                pb.SizeMode = PictureBoxSizeMode.CenterImage;
                return;
            }

            pb.Image = imagen;
            pb.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
