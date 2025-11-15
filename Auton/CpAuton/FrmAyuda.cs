using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpAuton
{
    public partial class FrmAyuda : Form
    {
        public FrmAyuda()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "... ::: 📚 Manual de Usuario - Sistema Auton :::...";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Panel izquierdo - Índice
            Panel pnlIndice = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(250, 700),
                BackColor = Color.FromArgb(52, 58, 64),
                Dock = DockStyle.Left
            };

            Label lblTitulo = new Label
            {
                Text = "📖 ÍNDICE DE AYUDA",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 20),
                Size = new Size(230, 30)
            };
            pnlIndice.Controls.Add(lblTitulo);

            int yPos = 70;
            AgregarBotonIndice(pnlIndice, "🏠 Inicio", () => MostrarContenido("inicio"), ref yPos);
            AgregarBotonIndice(pnlIndice, "🔐 Iniciar Sesión", () => MostrarContenido("login"), ref yPos);
            AgregarBotonIndice(pnlIndice, "🚗 Consultar Vehículos", () => MostrarContenido("consulta"), ref yPos);
            AgregarBotonIndice(pnlIndice, "➕ Registrar Vehículo", () => MostrarContenido("registro"), ref yPos);
            AgregarBotonIndice(pnlIndice, "✏️ Editar Vehículo", () => MostrarContenido("edicion"), ref yPos);
            AgregarBotonIndice(pnlIndice, "🔍 Búsqueda Avanzada", () => MostrarContenido("busqueda"), ref yPos);
            AgregarBotonIndice(pnlIndice, "📊 Estadísticas", () => MostrarContenido("estadisticas"), ref yPos);
            AgregarBotonIndice(pnlIndice, "👥 Gestión Usuarios", () => MostrarContenido("usuarios"), ref yPos);
            AgregarBotonIndice(pnlIndice, "❓ Preguntas Frecuentes", () => MostrarContenido("faq"), ref yPos);

            // Panel derecho - Contenido
            Panel pnlContenido = new Panel
            {
                Location = new Point(250, 0),
                Size = new Size(750, 700),
                BackColor = Color.White,
                AutoScroll = true,
                Padding = new Padding(30),
                Name = "pnlContenido"
            };

            this.Controls.Add(pnlIndice);
            this.Controls.Add(pnlContenido);

            MostrarContenido("inicio");
        }

        private void AgregarBotonIndice(Panel panel, string texto, Action onClick, ref int yPos)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = new Point(10, yPos),
                Size = new Size(230, 40),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 76, 82);
            btn.Click += (s, e) => onClick();
            panel.Controls.Add(btn);
            yPos += 45;
        }

        private void MostrarContenido(string seccion)
        {
            var pnlContenido = this.Controls.Find("pnlContenido", true)[0] as Panel;
            pnlContenido.Controls.Clear();

            int yPos = 20;

            switch (seccion)
            {
                case "inicio":
                    AgregarTitulo(pnlContenido, "🏠 Bienvenido al Sistema Auton", ref yPos);
                    AgregarTexto(pnlContenido,
                        "El Sistema Auton es una aplicación integral para la gestión y evaluación de vehículos " +
                        "automotores según su procedencia y características técnicas.", ref yPos);

                    AgregarSubtitulo(pnlContenido, "Módulos Principales:", ref yPos);
                    AgregarLista(pnlContenido, new[] {
                    "Consulta de vehículos por procedencia",
                    "Registro y edición de vehículos",
                    "Búsqueda avanzada con múltiples filtros",
                    "Estadísticas y análisis de confiabilidad",
                    "Gestión de usuarios y empleados"
                }, ref yPos);
                    break;

                case "login":
                    AgregarTitulo(pnlContenido, "🔐 Iniciar Sesión", ref yPos);
                    AgregarTexto(pnlContenido,
                        "Para acceder a las funciones administrativas del sistema, debe iniciar sesión con sus credenciales.",
                        ref yPos);

                    AgregarSubtitulo(pnlContenido, "Pasos:", ref yPos);
                    AgregarLista(pnlContenido, new[] {
                    "1. Haga clic en 'Sistema' → 'Iniciar Sesión'",
                    "2. Ingrese su nombre de usuario",
                    "3. Ingrese su contraseña",
                    "4. Haga clic en 'Ingresar'"
                }, ref yPos);

                    AgregarAdvertencia(pnlContenido,
                        "⚠️ Usuarios por defecto:\n" +
                        "   • admin / admin123 (Administrador)\n" +
                        "   • maria.fernandez / maria123 (Usuario normal)",
                        ref yPos);
                    // Mostrar la advertencia solo si el usuario está autenticado
                    // (Util.usuario es null cuando no hay sesión iniciada)
                    // Si no está autenticado, no mostramos los credenciales por defecto.
                    if (Util.usuario == null)
                    {
                        // Remover la advertencia añadida anteriormente
                        // Buscamos el último control de tipo Panel que contenga el texto de advertencia y lo removemos
                        var last = pnlContenido.Controls.Cast<Control>().OfType<Panel>().LastOrDefault(p => p.Controls.OfType<Label>().Any(l => l.Text != null && l.Text.Contains("Usuarios por defecto")));
                        if (last != null) pnlContenido.Controls.Remove(last);
                    }
                     break;

                case "consulta":
                    AgregarTitulo(pnlContenido, "🚗 Consultar Vehículos", ref yPos);
                    AgregarTexto(pnlContenido,
                        "El módulo de consulta permite visualizar todos los vehículos registrados en el sistema " +
                        "con información detallada de cada uno.", ref yPos);

                    AgregarSubtitulo(pnlContenido, "Funciones disponibles:", ref yPos);
                    AgregarLista(pnlContenido, new[] {
                    "Búsqueda rápida por marca, modelo o año",
                    "Filtrado por procedencia (Japonés, Americano, etc.)",
                    "Visualización de detalles completos",
                    "Exportación a CSV",
                    "Edición (solo usuarios autenticados)"
                }, ref yPos);

                    AgregarInfo(pnlContenido,
                        "💡 Tip: Haga doble clic sobre un vehículo para ver todos sus detalles.", ref yPos);
                    break;

                case "registro":
                    AgregarTitulo(pnlContenido, "➕ Registrar Nuevo Vehículo", ref yPos);
                    AgregarTexto(pnlContenido,
                        "Para registrar un vehículo, debe estar autenticado en el sistema.", ref yPos);

                    AgregarSubtitulo(pnlContenido, "Información requerida:", ref yPos);
                    AgregarLista(pnlContenido, new[] {
                    "✓ Procedencia de la marca",
                    "✓ Marca y modelo del vehículo",
                    "✓ Número de chasis (17 caracteres)",
                    "✓ Características técnicas (motor, transmisión, etc.)",
                    "✓ Información de seguridad (airbags, estrellas)",
                    "✓ Evaluación de confiabilidad"
                }, ref yPos);

                    AgregarAdvertencia(pnlContenido,
                        "⚠️ El número de chasis debe ser único y tener exactamente 17 caracteres.", ref yPos);
                    break;


                case "edicion":
                    AgregarTitulo(pnlContenido, "✏️ Editar Vehículo", ref yPos);

                    AgregarAdvertencia(pnlContenido,
                        "⚠️ IMPORTANTE: Este es el manual de ayuda.\n" +
                        "Para editar vehículos reales, debe ir al módulo de Consulta desde el menú principal.",
                        ref yPos);

                    AgregarTexto(pnlContenido,
                        "La edición de vehículos permite modificar todas las características técnicas, " +
                        "evaluación y observaciones de un vehículo ya registrado.", ref yPos);

                    AgregarSubtitulo(pnlContenido, "Campos editables:", ref yPos);
                    AgregarLista(pnlContenido, new[] {
        "✓ Generación del modelo",
        "✓ País de fabricación",
        "✓ Año de fabricación",
        "✓ Tipo de transmisión",
        "✓ Turbo (sí/no)",
        "✓ Motor",
        "✓ Tipo de combustible",
        "✓ Número de airbags",
        "✓ Calificación de seguridad",
        "✓ Confiabilidad",
        "✓ Puntuación (1.0 - 10.0)",
        "✓ Observaciones",
        "✓ Número de chasis",
        "✓ Marca del vehículo (En Gestión >> Marcas/Modelos)",
        "✓ Modelo del vehícul",
        "✓ Tipo de vehículo"
    }, ref yPos);

                  

                    AgregarSubtitulo(pnlContenido, "Pasos para editar:", ref yPos);

                    Panel pnlPasos = new Panel
                    {
                        Location = new Point(30, yPos),
                        Size = new Size(690, 280),
                        BackColor = Color.FromArgb(248, 249, 250),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    int yPasos = 10;
                    AgregarPasoNumerado(pnlPasos, "1", "Vaya al menú Vehículos → Consultar", ref yPasos);
                    AgregarPasoNumerado(pnlPasos, "2", "Busque el vehículo usando el campo de búsqueda", ref yPasos);
                    AgregarPasoNumerado(pnlPasos, "3", "Haga doble clic sobre el vehículo", ref yPasos);
                    AgregarPasoNumerado(pnlPasos, "4", "Se abrirá la ventana de detalles en modo LECTURA", ref yPasos);
                    AgregarPasoNumerado(pnlPasos, "5", "Presione el botón '✏️ Editar'", ref yPasos);
                    AgregarPasoNumerado(pnlPasos, "6", "Modifique los campos que necesite", ref yPasos);
                    AgregarPasoNumerado(pnlPasos, "7", "Presione '💾 Guardar' para confirmar los cambios", ref yPasos);

                    pnlContenido.Controls.Add(pnlPasos);
                    yPos += 290;

                    AgregarInfo(pnlContenido,
                        "💡 Tip: Si cambia la confiabilidad, el sistema guardará un historial automáticamente.",
                        ref yPos);
                    break;

                case "busqueda":
                    AgregarTitulo(pnlContenido, "🔍 Búsqueda Avanzada", ref yPos);
                    AgregarTexto(pnlContenido,
                        "La búsqueda avanzada permite filtrar vehículos con múltiples criterios simultáneos.", ref yPos);

                    AgregarSubtitulo(pnlContenido, "Filtros disponibles:", ref yPos);
                    AgregarLista(pnlContenido, new[] {
                    "📍 Procedencia",
                    "⭐ Confiabilidad",
                    "⚙️ Tipo de transmisión",
                    "⛽ Tipo de combustible",
                    "📅 Rango de años",
                    "📊 Puntuación mínima",
                    "🚀 Con/Sin turbo"
                }, ref yPos);
                    break;

                case "estadisticas":
                    AgregarTitulo(pnlContenido, "📊 Estadísticas del Sistema", ref yPos);
                    AgregarTexto(pnlContenido,
                        "El módulo de estadísticas muestra análisis detallados de la base de datos de vehículos.", ref yPos);

                    AgregarSubtitulo(pnlContenido, "Información mostrada:", ref yPos);
                    AgregarLista(pnlContenido, new[] {
                    "Total de vehículos registrados",
                    "Distribución por confiabilidad",
                    "Distribución por procedencia",
                    "Top 5 marcas más registradas",
                    "Promedios de puntuación y airbags",
                    "Vehículos con/sin turbo",
                    "Rango de años"
                }, ref yPos);
                    break;

                case "usuarios":
                    AgregarTitulo(pnlContenido, "👥 Gestión de Usuarios", ref yPos);
                    AgregarTexto(pnlContenido,
                        "Solo los administradores pueden acceder a este módulo para gestionar usuarios del sistema.", ref yPos);

                    AgregarSubtitulo(pnlContenido, "Funciones:", ref yPos);
                    AgregarLista(pnlContenido, new[] {
                    "Crear nuevos usuarios",
                    "Asignar permisos de administrador",
                    "Resetear contraseñas",
                    "Eliminar usuarios",
                    "Gestionar empleados"
                }, ref yPos);

                    AgregarAdvertencia(pnlContenido,
                        "⚠️ Solo usuarios con permisos de administrador pueden acceder a esta función.", ref yPos);
                    break;

                case "faq":
                    AgregarTitulo(pnlContenido, "❓ Preguntas Frecuentes", ref yPos);

                    AgregarPregunta(pnlContenido, "¿Cómo puedo editar un vehículo?",
                        "Debe iniciar sesión primero, luego en el módulo de consulta, haga doble clic " +
                        "sobre el vehículo y presione el botón 'Editar'.", ref yPos);

                    AgregarPregunta(pnlContenido, "¿Por qué no puedo registrar vehículos?",
                        "Para registrar vehículos debe estar autenticado, vaya a Sistema → Iniciar Sesión.", ref yPos);

                    AgregarPregunta(pnlContenido, "¿Cómo exporto los datos?",
                        "En cualquier listado de vehículos, encontrará un botón 'Exportar CSV' que genera " +
                        "un archivo con todos los datos mostrados.", ref yPos);

                    AgregarPregunta(pnlContenido, "¿Qué significa la confiabilidad?",
                        "Es una evaluación de la durabilidad y fiabilidad del vehículo basada en su desempeño " +
                        "histórico, puede ser: Excelente, Bueno, Regular o Malo.", ref yPos);
                    break;
            }
        }

        private void AgregarTitulo(Panel panel, string texto, ref int yPos)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(30, yPos),
                Size = new Size(690, 40),
                AutoSize = false
            };
            panel.Controls.Add(lbl);
            yPos += 50;
        }

        private void AgregarSubtitulo(Panel panel, string texto, ref int yPos)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(30, yPos),
                Size = new Size(690, 30),
                AutoSize = false
            };
            panel.Controls.Add(lbl);
            yPos += 40;
        }

        private void AgregarTexto(Panel panel, string texto, ref int yPos)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(73, 80, 87),
                Location = new Point(30, yPos),
                Size = new Size(690, 0),
                AutoSize = true,
                MaximumSize = new Size(690, 0)
            };
            panel.Controls.Add(lbl);
            yPos += lbl.Height + 20;
        }

        private void AgregarLista(Panel panel, string[] items, ref int yPos)
        {
            foreach (var item in items)
            {
                Label lbl = new Label
                {
                    Text = item,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(73, 80, 87),
                    Location = new Point(50, yPos),
                    Size = new Size(670, 25)
                };
                panel.Controls.Add(lbl);
                yPos += 30;
            }
            yPos += 10;
        }

        private void AgregarAdvertencia(Panel panel, string texto, ref int yPos)
        {
            Panel pnlAdv = new Panel
            {
                Location = new Point(30, yPos),
                Size = new Size(690, 80),
                BackColor = Color.FromArgb(255, 243, 205),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(102, 60, 0),
                Location = new Point(10, 10),
                Size = new Size(670, 60),
                AutoSize = false
            };
            pnlAdv.Controls.Add(lbl);
            panel.Controls.Add(pnlAdv);
            yPos += 90;
        }

        private void AgregarInfo(Panel panel, string texto, ref int yPos)
        {
            Panel pnlInfo = new Panel
            {
                Location = new Point(30, yPos),
                Size = new Size(690, 50),
                BackColor = Color.FromArgb(209, 236, 241),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(12, 84, 96),
                Location = new Point(10, 10),
                Size = new Size(670, 30),
                AutoSize = false
            };
            pnlInfo.Controls.Add(lbl);
            panel.Controls.Add(pnlInfo);
            yPos += 60;
        }

        private void AgregarPasoNumerado(Panel panel, string numero, string texto, ref int yPos)
        {
            // Círculo con número
            Label lblNumero = new Label
            {
                Text = numero,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(51, 122, 183),
                Location = new Point(10, yPos),
                Size = new Size(30, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Texto del paso
            Label lblTexto = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(50, yPos + 5),
                Size = new Size(620, 25)
            };

            panel.Controls.Add(lblNumero);
            panel.Controls.Add(lblTexto);
            yPos += 35;
        }

        private void AgregarPregunta(Panel panel, string pregunta, string respuesta, ref int yPos)
        {
            Label lblPregunta = new Label
            {
                Text = "❔ " + pregunta,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(30, yPos),
                Size = new Size(690, 0),
                AutoSize = true,
                MaximumSize = new Size(690, 0)
            };
            panel.Controls.Add(lblPregunta);
            yPos += lblPregunta.Height + 10;

            Label lblRespuesta = new Label
            {
                Text = respuesta,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(73, 80, 87),
                Location = new Point(50, yPos),
                Size = new Size(670, 0),
                AutoSize = true,
                MaximumSize = new Size(670, 0)
            };
            panel.Controls.Add(lblRespuesta);
            yPos += lblRespuesta.Height + 25;
        }

        private void FrmAyuda_Load(object sender, EventArgs e)
        {
            string ruta = Application.StartupPath + @"\Audio\JVKE - golden hour (instrumental).mp3";
            AudioManager.Reproducir(ruta, true, 25 - 10);
        }

        private void FrmVehiculosChinos_FormClosed(object sender, FormClosedEventArgs e)
        {
            // reanudar menú
            string rutaMenu = Application.StartupPath + @"\Audio\Crystal Waters - Gypsy Woman (She's Homeless) (Official instrumental).mp3";
            AudioManager.Reproducir(rutaMenu, true, 60);
        }
    }
}
