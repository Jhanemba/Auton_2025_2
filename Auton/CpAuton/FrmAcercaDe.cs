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
    public partial class FrmAcercaDe : Form
    {
        public FrmAcercaDe()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Acerca del Sistema";
            this.Size = new Size(650, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Panel header con gradiente
            Panel pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(650, 150),
                BackColor = Color.FromArgb(41, 128, 185)
            };

            // Logo o ícono del sistema
            Label lblLogo = new Label
            {
                Text = "🚗",
                Font = new Font("Segoe UI", 48, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(280, 20),
                Size = new Size(100, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHeader.Controls.Add(lblLogo);

            Label lblNombreSistema = new Label
            {
                Text = "SISTEMA AUTON",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(150, 105),
                Size = new Size(350, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHeader.Controls.Add(lblNombreSistema);

            // Panel de información
            Panel pnlInfo = new Panel
            {
                Location = new Point(30, 170),
                Size = new Size(590, 420), // aumentar alto para mostrar más contenido
                BackColor = Color.White,
                AutoScroll = true
            };

            int yPos = 20;

            // Versión
            AgregarSeccion(pnlInfo, "📌 Información del Sistema", ref yPos);
            AgregarInfo(pnlInfo, "Versión:", "1.0.0", ref yPos);
            AgregarInfo(pnlInfo, "Fecha de Lanzamiento:", DateTime.Now.ToString("MMMM yyyy"), ref yPos);
            AgregarInfo(pnlInfo, "Estado:", "Producción", ref yPos);
            yPos += 15;

            // Descripción
            AgregarSeccion(pnlInfo, "📝 Descripción", ref yPos);
            Label lblDescripcion = new Label
            {
                Text = "Sistema integral para la gestión y administración de vehículos automotores, " +
                       "permite el registro, control y seguimiento de vehículos, empleados, usuarios y " +
                       "operaciones relacionadas con el mantenimiento y evaluación técnica vehicular.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, yPos),
                Size = new Size(550, 80),
                AutoSize = false
            };
            pnlInfo.Controls.Add(lblDescripcion);
            yPos += 95;

            // Módulos
            AgregarSeccion(pnlInfo, "🔧 Módulos Implementados", ref yPos);
            AgregarModulo(pnlInfo, "✓ Gestión de Empleados", ref yPos);
            AgregarModulo(pnlInfo, "✓ Gestión de Usuarios", ref yPos);
            AgregarModulo(pnlInfo, "✓ Gestión de Marcas y Modelos", ref yPos);
            AgregarModulo(pnlInfo, "✓ Gestión de Vehículos", ref yPos);
            AgregarModulo(pnlInfo, "✓ Sistema de Seguridad y Autenticación", ref yPos);
            yPos += 15;

            // Desarrollador
            AgregarSeccion(pnlInfo, "👨‍💻 Desarrollo", ref yPos);
            AgregarInfo(pnlInfo, "Desarrollado por:", "Kevin Car", ref yPos);
            AgregarInfo(pnlInfo, "Institución:", "Universidad Mayor, Real Y Pontificia de", ref yPos);
            AgregarInfo(pnlInfo, "", "San Francisco Xavier de Chuquisaca", ref yPos);
            AgregarInfo(pnlInfo, "", "", ref yPos);
            AgregarInfo(pnlInfo, "Ubicación:", "Sucre, Bolivia", ref yPos);
            yPos += 155;


            // Botón cerrar
            //Button btnCerrar = new Button
            //{
            //    Text = "Cerrar",
            //    Size = new Size(150, 45),
            //    Location = new Point(250, 610),
            //    BackColor = Color.FromArgb(41, 128, 185),
            //    ForeColor = Color.White,
            //    FlatStyle = FlatStyle.Flat,
            //    Font = new Font("Segoe UI", 11, FontStyle.Bold),
            //    Cursor = Cursors.Hand,
            //    DialogResult = DialogResult.OK
            //};
            //btnCerrar.FlatAppearance.BorderSize = 0;
            //btnCerrar.Click += (s, e) => this.Close();

            // Panel footer
            Panel pnlFooter = new Panel
            {
                Size = new Size(650, 45),
                BackColor = Color.FromArgb(236, 240, 241),
                Dock = DockStyle.Bottom
            };

            Label lblFooter = new Label
            {
                Text = "© 2025 Sistema Auton - Todos los derechos reservados",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(120, 8),
                Size = new Size(400, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlFooter.Controls.Add(lblFooter);

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlInfo);
            //this.Controls.Add(btnCerrar);
            this.Controls.Add(pnlFooter);
        }

        private void AgregarSeccion(Panel panel, string titulo, ref int yPos)
        {
            Label lblSeccion = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, yPos),
                Size = new Size(550, 25)
            };
            panel.Controls.Add(lblSeccion);
            yPos += 35;
        }

        private void AgregarInfo(Panel panel, string etiqueta, string valor, ref int yPos)
        {
            Label lblEtiqueta = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(40, yPos),
                Size = new Size(200, 25)
            };
            panel.Controls.Add(lblEtiqueta);

            Label lblValor = new Label
            {
                Text = valor,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(250, yPos),
                Size = new Size(320, 25)
            };
            panel.Controls.Add(lblValor);

            yPos += 30;
        }

        private void AgregarModulo(Panel panel, string modulo, ref int yPos)
        {
            Label lblModulo = new Label
            {
                Text = modulo,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(39, 174, 96),
                Location = new Point(40, yPos),
                Size = new Size(500, 25)
            };
            panel.Controls.Add(lblModulo);
            yPos += 28;
        }

        private void FrmAcercaDe_Load(object sender, EventArgs e)
        {
            string ruta = Application.StartupPath + @"\Audio\WELTiTA (Instrumental) - Bad Bunny ft. Chuwi-(Nonek).mp3";
            string ruta2 = Application.StartupPath + @"\Audio\Milo J - Solifican12 (Instrumental)-(146k).mp3";
            string ruta3 = Application.StartupPath + @"\Audio\Amor salvaje-(Nonek).mp3";
            string ruta4 = Application.StartupPath + @"\Audio\cuando el mundo acabe-(Nonek).mp3";
            string ruta5 = Application.StartupPath + @"\Audio\Como No Quererte-(Nonek)-pt2.mp3";
            string ruta6 = Application.StartupPath + @"\Audio\Amor Eterno-(Nonek)-pt2.mp3";
            string ruta7 = Application.StartupPath + @"\Audio\Niño-(Nonek).mp3";
            string ruta8 = Application.StartupPath + @"\Audio\氤氲之森-(Nonek).mp3";


            AudioManager.LoopPlaylist = false;
            AudioManager.SetPlaylist(new List<string> { ruta, ruta2, ruta3, ruta4, ruta5, ruta6, ruta7, ruta8 });
            AudioManager.PlayPlaylist(25 - 10 + 35, 45, 45, 45, 45 - 10, 45 - 10, 45, 45); // volumen


            //AudioManager.Reproducir(ruta, ruta2, true, 25 - 10 + 35);
        }

        private void FrmAcercaDe_FormClosed(object sender, FormClosedEventArgs e)
        {
            // reanudar menú
            string rutaMenu = Application.StartupPath + @"\Audio\Crystal Waters - Gypsy Woman (She's Homeless) (Official instrumental).mp3";
            AudioManager.Reproducir(rutaMenu, true, 60);
        }
    }
}
