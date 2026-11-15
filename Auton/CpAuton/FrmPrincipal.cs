using CadAuton;
using ClnAuton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpAuton
{
    public partial class FrmPrincipal : Form
    {
        private List<Vehiculo> vehiculosCarrusel = new List<Vehiculo>();
        private List<paVehiculoListar_Result> vehiculosCarruselResumen = new List<paVehiculoListar_Result>();
        private int indiceCarruselActual = 0;
        private Timer timerCarrusel;
        private Timer timerFade;
        private float opacidadActual = 1.0f;
        private const float velocidadFade = 0.05f;
        private bool estoyEnFade = false;
        private Image imagenAnterior;
        private ToolTip toolTipCarrusel = new ToolTip();


        public FrmPrincipal()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ActualizarInterfaz();
        }

        private void btnCaTodoVehiculos_Click(object sender, EventArgs e)
        {
            new FrmConsultaVehiculos().ShowDialog();
            RecargarCarrusel();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "...::: Sistema Auton - Confiabilidad Vehicular :::...";
            this.WindowState = FormWindowState.Maximized;

            // Crear MenuStrip
            MenuStrip menuPrincipal = new MenuStrip();
            menuPrincipal.Name = "menuPrincipal";

            // Menú Vehículos
            ToolStripMenuItem menuVehiculos = new ToolStripMenuItem("Vehículos");
            ToolStripMenuItem menuConsultar = new ToolStripMenuItem("Consultar", null, MenuConsultar_Click);
            ToolStripMenuItem menuNuevo = new ToolStripMenuItem("Nuevo", null, MenuNuevo_Click);
            ToolStripMenuItem menuEditar = new ToolStripMenuItem("Editar", null, MenuEditar_Click);
            menuNuevo.Name = "menuNuevo";
            menuEditar.Name = "menuEditar";

            menuVehiculos.DropDownItems.AddRange(new ToolStripItem[] {
                menuConsultar, new ToolStripSeparator(), menuNuevo, menuEditar
            });

            // Menú Reportes
            ToolStripMenuItem menuReportes = new ToolStripMenuItem("Reportes");
            ToolStripMenuItem menuEstadisticas = new ToolStripMenuItem("Estadísticas", null, MenuEstadisticas_Click);

            menuReportes.DropDownItems.Add(menuEstadisticas);

            // Menú Administración
            ToolStripMenuItem menuAdmin = new ToolStripMenuItem("Administración");
            menuAdmin.Name = "menuAdmin";
            ToolStripMenuItem menuUsuarios = new ToolStripMenuItem("Usuarios", null, MenuUsuarios_Click);
            ToolStripMenuItem menuEmpleados = new ToolStripMenuItem("Empleados", null, MenuEmpleados_Click);

            menuAdmin.DropDownItems.AddRange(new ToolStripItem[] {
                menuUsuarios, menuEmpleados
            });

            // Menú Sistema
            ToolStripMenuItem menuSistema = new ToolStripMenuItem("Sistema");
            ToolStripMenuItem menuLogin = new ToolStripMenuItem("Iniciar Sesión", null, MenuLogin_Click);
            menuLogin.Name = "menuLogin";
            ToolStripMenuItem menuCerrarSesion = new ToolStripMenuItem("Cerrar Sesión", null, MenuCerrarSesion_Click);
            menuCerrarSesion.Name = "menuCerrarSesion";
            ToolStripMenuItem menuSalir = new ToolStripMenuItem("Salir", null, MenuSalir_Click);

            menuSistema.DropDownItems.AddRange(new ToolStripItem[] {
                menuLogin, menuCerrarSesion, new ToolStripSeparator(), menuSalir
            });

            menuPrincipal.Items.AddRange(new ToolStripItem[] {
                menuVehiculos, menuReportes, menuAdmin, menuSistema
            });

            // StatusStrip
            StatusStrip statusStrip = new StatusStrip();
            statusStrip.Name = "statusStrip";

            ToolStripStatusLabel lblUsuario = new ToolStripStatusLabel
            {
                Name = "lblUsuario",
                Text = "Usuario: Invitado (Solo consulta)",
                BorderSides = ToolStripStatusLabelBorderSides.Right
            };

            ToolStripStatusLabel lblFecha = new ToolStripStatusLabel
            {
                Name = "lblFecha",
                Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                Spring = true,
                TextAlign = ContentAlignment.MiddleRight
            };

            statusStrip.Items.AddRange(new ToolStripItem[] { lblUsuario, lblFecha });

            // Timer para actualizar fecha
            Timer timer = new Timer { Interval = 60000 };
            timer.Tick += (s, e) => {
                var ss = this.Controls.Find("statusStrip", false);
                if (ss.Length > 0 && ss[0] is StatusStrip strip)
                {
                    ((ToolStripStatusLabel)strip.Items["lblFecha"]).Text =
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                }
            };
            timer.Start();

            this.Controls.AddRange(new Control[] { statusStrip, menuPrincipal });
            this.MainMenuStrip = menuPrincipal;
        }

        private bool ValidarAutenticacion()
        {
            if (Util.usuario != null)
                return true;

            FrmAutenticacion frmAuth = new FrmAutenticacion();
            frmAuth.StartPosition = FormStartPosition.CenterParent;

            if (frmAuth.ShowDialog(this) == DialogResult.OK)
            {
                ActualizarInterfaz();
                return true;
            }

            return false;
        }

        private void ActualizarInterfaz()
        {
            var menuStrip = this.Controls.Find("menuPrincipal", false);
            if (menuStrip.Length > 0 && menuStrip[0] is MenuStrip ms)
            {
                // Actualizar visibilidad de menús según autenticación
                var menuAdmin = ms.Items.Find("menuAdmin", true);
                if (menuAdmin.Length > 0)
                    menuAdmin[0].Visible = Util.usuario != null && Util.EsAdministrador();

                var menuLogin = ms.Items.Find("menuLogin", true);
                if (menuLogin.Length > 0)
                    menuLogin[0].Visible = Util.usuario == null;

                var menuCerrarSesion = ms.Items.Find("menuCerrarSesion", true);
                if (menuCerrarSesion.Length > 0)
                    menuCerrarSesion[0].Visible = Util.usuario != null;
            }

            // Actualizar StatusBar
            var statusStrip = this.Controls.Find("statusStrip", false);
            if (statusStrip.Length > 0 && statusStrip[0] is StatusStrip ss)
            {
                var lblUsuario = (ToolStripStatusLabel)ss.Items["lblUsuario"];
                if (Util.usuario != null)
                {
                    lblUsuario.Text = $"Usuario: {Util.usuario.nombreCompleto} - {Util.usuario.cargo}";
                }
                else
                {
                    lblUsuario.Text = "Usuario: Invitado (Solo consulta)";
                }
            }
            // Reiniciar carrusel cuando cambia la autenticación
            try
            {
                if (timerCarrusel != null)
                {
                    timerCarrusel.Stop();
                    timerCarrusel.Dispose();
                    timerCarrusel = null;
                }
                InicializarCarrusel();
            }
            catch { }
        }

        // Eventos de menú
        private void MenuConsultar_Click(object sender, EventArgs e)
        {
            new FrmConsultaVehiculos().ShowDialog();
            RecargarCarrusel();
        }

        private void MenuNuevo_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                using (var frmRegistro = new FrmRegistroVehiculo())
                {
                    frmRegistro.ShowDialog(this);
                    RecargarCarrusel();
                }
            }
        }

        private void MenuJaponeses_Click(object sender, EventArgs e)
        {
            new FrmVehiculosJaponeses().ShowDialog();
        }

        private void MenuAmericanos_Click(object sender, EventArgs e)
        {
            new FrmVehiculosAmericanos().ShowDialog();
        }

        private void MenuChinos_Click(object sender, EventArgs e)
        {
            new FrmVehiculosChinos().ShowDialog();
        }

        private void MenuAlemanes_Click(object sender, EventArgs e)
        {
            new FrmVehiculosAlemanes().ShowDialog();
        }

        private void MenuEuropeos_Click(object sender, EventArgs e)
        {
            new FrmVehiculosEuropeos().ShowDialog();
        }

        private void MenuCoreanos_Click(object sender, EventArgs e)
        {
            new FrmVehiculosCoreanos().ShowDialog();
        }

        private void MenuBusquedaAvanzada_Click(object sender, EventArgs e)
        {
            new FrmBusquedaAvanzada().ShowDialog();
        }

        private void btnEstadisticas_Click(object sender, EventArgs e)
        {
            using (var frmStats = new FrmEstadisticas())
            {
                frmStats.ShowDialog(this);
            }
        }

        private void MenuEditar_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                using (var frmEditar = new FrmConsultaVehiculos())
                {
                    frmEditar.ShowDialog(this);
                    RecargarCarrusel();
                }
            }
        }

        private void MenuEstadisticas_Click(object sender, EventArgs e)
        {
            using (var frmStats = new FrmEstadisticas())
            {
                frmStats.ShowDialog(this);
            }
        }

        private void MenuGestionarVehiculos_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                using (var frmGestion = new FrmGestionarVehiculos())
                {
                    frmGestion.ShowDialog(this);
                    // No necesitamos recargar nada aquí porque no tiene lista
                }
            }
        }

        private void MenuGestionarMarcasModelos_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                using (var frmGestionMM = new FrmGestionarMarcasModelos())
                {
                    frmGestionMM.ShowDialog(this);
                    // No necesitamos recargar nada aquí porque no tiene lista
                }
            }
        }

        private void menuAdministrarUsuarios_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                var frmUsuarios = new FrmGestionarUsuarios();
                frmUsuarios.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("⚠️ Solo los administradores pueden gestionar usuarios.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void menuAdministrarEmpleados_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                var frmEmpleados = new FrmGestionarEmpleados();
                frmEmpleados.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("⚠️ Solo los administradores pueden gestionar usuarios.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            using (var frmAcerca = new FrmAcercaDe())
            {
                frmAcerca.ShowDialog(this);
            }
        }

        private void MenuUsuarios_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                if (Util.EsAdministrador())
                {
                    using (var frmUsu = new FrmGestionarUsuarios())
                    {
                        frmUsu.ShowDialog(this);
                    }
                }
                else
                {
                    MessageBox.Show("No tiene permisos de administrador",
                                  "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void MenuEmpleados_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                if (Util.EsAdministrador())
                {
                    using (var frmEmple = new FrmGestionarEmpleados())
                    {
                        frmEmple.ShowDialog(this);
                    }
                }
                else
                {
                    MessageBox.Show("No tiene permisos de administrador",
                                  "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void MenuLogin_Click(object sender, EventArgs e)
        {
            ValidarAutenticacion();
        }

        private void MenuCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Desea cerrar la sesión actual?",
                                                    "Cerrar Sesión",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                Util.CerrarSesion();
                ActualizarInterfaz();
                MessageBox.Show("Sesión cerrada. Ahora está en modo Invitado (solo consulta)",
                              "Sesión Cerrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MenuSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Está seguro que desea salir del sistema?",
                                                    "Salir",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
            if (resultado == DialogResult.No)
            {
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        private void btnCaManualUsuario_Click(object sender, EventArgs e)
        {
            using (var frmAyuda = new FrmAyuda())
            {
                frmAyuda.ShowDialog(this);
            }
        }

        private void IniciarEfectoFadeIn()
        {
            opacidadActual = 0.0f;

            timerFade = new Timer();
            timerFade.Interval = 30;
            timerFade.Tick += (s, e) =>
            {
                opacidadActual += velocidadFade;

                if (opacidadActual >= 1.0f)
                {
                    opacidadActual = 1.0f;
                    timerFade.Stop();
                    timerFade.Dispose();
                    timerFade = null;
                    estoyEnFade = false;

                    // Desconectar el paint handler
                    pictureBox1.Paint -= PictureBox1_Paint;
                }

                pictureBox1.Invalidate();
            };

            timerFade.Start();
        }

        // Método para cargar solo la imagen sin mostrarla
        private void CargarImagenActual()
        {
            try
            {
                if (vehiculosCarrusel.Count == 0) return;
                if (indiceCarruselActual < 0 || indiceCarruselActual >= vehiculosCarrusel.Count) return;

                var vehiculo = vehiculosCarrusel[indiceCarruselActual];
                var vehiculoResumen = vehiculosCarruselResumen.Count > indiceCarruselActual
                    ? vehiculosCarruselResumen[indiceCarruselActual]
                    : null;

                if (vehiculo.fotografia == null || vehiculo.fotografia.Length == 0) return;

                using (var ms = new System.IO.MemoryStream(vehiculo.fotografia))
                {
                    var nuevaImagen = Image.FromStream(ms);
                    pictureBox1.Image = (Image)nuevaImagen.Clone();
                }

                // Actualizar tooltip
                if (vehiculoResumen != null)
                {
                    string infoVehiculo = $"🏆 {indiceCarruselActual + 1}/{vehiculosCarrusel.Count}\n" +
                                         $"Marca: {vehiculoResumen.marca}\n" +
                                         $"Modelo: {vehiculoResumen.modelo}\n" +
                                         $"Confiabilidad: {vehiculo.confiabilidad}\n" +
                                         $"Puntuación: {vehiculo.puntuacion}/10";
                    toolTipCarrusel.SetToolTip(pictureBox1, infoVehiculo);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cargando imagen: {ex.Message}");
            }
        }

        private void IniciarEfectoFade()
        {
            if (estoyEnFade) return;

            estoyEnFade = true;
            opacidadActual = 1.0f;

            // ⭐ GUARDAR LA IMAGEN ACTUAL ANTES DE CAMBIAR
            imagenAnterior = pictureBox1.Image != null ? (Image)pictureBox1.Image.Clone() : null;

            // ⭐ CAMBIAR AL SIGUIENTE VEHÍCULO
            indiceCarruselActual = (indiceCarruselActual + 1) % vehiculosCarrusel.Count;

            // ⭐ CARGAR LA NUEVA IMAGEN (pero sin mostrarla aún)
            CargarImagenActual();

            // Crear timer para el fade-out
            timerFade = new Timer();
            timerFade.Interval = 30;
            timerFade.Tick += (s, e) =>
            {
                opacidadActual -= velocidadFade;

                if (opacidadActual <= 0)
                {
                    opacidadActual = 0;
                    timerFade.Stop();
                    timerFade.Dispose();
                    timerFade = null;

                    // Limpiar imagen anterior
                    if (imagenAnterior != null)
                    {
                        imagenAnterior.Dispose();
                        imagenAnterior = null;
                    }

                    // Iniciar fade-in
                    IniciarEfectoFadeIn();
                }

                pictureBox1.Invalidate();
            };

            // ⭐ CONECTAR EL PAINT HANDLER
            pictureBox1.Paint -= PictureBox1_Paint;
            pictureBox1.Paint += PictureBox1_Paint;

            timerFade.Start();
        }

        // ⭐ NUEVO: Paint handler para dibujar el fade
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!estoyEnFade) return;

            // Dibujar imagen anterior con opacidad decreciente (fade-out)
            if (imagenAnterior != null && opacidadActual > 0)
            {
                Bitmap bmpFadeOut = AplicarOpacidad(imagenAnterior, opacidadActual);
                e.Graphics.DrawImage(bmpFadeOut, 0, 0, pictureBox1.Width, pictureBox1.Height);
                bmpFadeOut.Dispose();
            }

            // Dibujar imagen nueva con opacidad creciente (fade-in)
            float opacidadNueva = 1.0f - opacidadActual;
            if (pictureBox1.Image != null && opacidadNueva > 0)
            {
                Bitmap bmpFadeIn = AplicarOpacidad(pictureBox1.Image, opacidadNueva);
                e.Graphics.DrawImage(bmpFadeIn, 0, 0, pictureBox1.Width, pictureBox1.Height);
                bmpFadeIn.Dispose();
            }
        }

        private Bitmap AplicarOpacidad(Image imagen, float opacidad)
        {
            if (imagen == null) return null;

            Bitmap bmp = new Bitmap(imagen.Width, imagen.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, opacidad, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                ImageAttributes ia = new ImageAttributes();
                ia.SetColorMatrix(cm);

                g.DrawImage(imagen, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, imagen.Width, imagen.Height, GraphicsUnit.Pixel, ia);
            }
            return bmp;
        }

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    if (timerCarrusel != null)
        //    {
        //        timerCarrusel.Stop();
        //        timerCarrusel.Dispose();
        //    }

        //    DialogResult resultado = MessageBox.Show("¿Está seguro que desea salir del sistema?",
        //                                            "Salir",
        //                                            MessageBoxButtons.YesNo,
        //                                            MessageBoxIcon.Question);
        //    if (resultado == DialogResult.No)
        //    {
        //        e.Cancel = true;
        //    }
        //    base.OnFormClosing(e);
        //}

        private void ActualizarImagenCarrusel()
        {
            try
            {
                if (vehiculosCarrusel.Count == 0) return;

                var vehiculo = vehiculosCarrusel[indiceCarruselActual];
                if (vehiculo.fotografia == null || vehiculo.fotografia.Length == 0) return;

                // Convertir bytes a imagen
                using (var ms = new System.IO.MemoryStream(vehiculo.fotografia))
                {
                    var nuevaImagen = Image.FromStream(ms);

                    // Efecto de fade: opacidad gradual
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                    }

                    pictureBox1.Image = (Image)nuevaImagen.Clone();

                    string infoVehiculo = $"🏆 {vehiculosCarrusel.IndexOf(vehiculo) + 1}/3\n" +
                                         //$"Marca: {vehiculoResumen.marca}\n" +
                                         //$"Modelo: {vehiculoResumen.modelo}\n" +
                                         $"Confiabilidad: {vehiculo.confiabilidad}\n" +
                                         $"Puntuación: {vehiculo.puntuacion}/10";
                    toolTipCarrusel.SetToolTip(pictureBox1, infoVehiculo);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error actualizando carrusel: {ex.Message}");
            }
        }

        private void AvanzarCarrusel()
        {
            if (vehiculosCarrusel.Count == 0 || estoyEnFade) return;

            // ⭐ INICIAR EL EFECTO FADE EN VEZ DE CAMBIAR DIRECTAMENTE
            IniciarEfectoFade();
        }

        private void InicializarCarrusel()
        {
            try
            {
                // DESPUÉS (descarta sin foto primero, luego toma 3):
                var todosValidos = ConsultaVehiculosCln.listarTodosPa()
                    .Where(v => (v.confiabilidad == "Bueno" || v.confiabilidad == "Excelente"))
                    .OrderByDescending(v => v.puntuacion)
                    .ToList();

                // Convertir a objetos Vehiculo para obtener fotografías
                vehiculosCarrusel.Clear();
                vehiculosCarruselResumen.Clear(); // GUARDAR TAMBIÉN EL RESUMEN

                foreach (var vehiculoResumen in todosValidos)
                {
                    if (vehiculosCarrusel.Count >= 3) break; // Ya tenemos 3, parar

                    var vehiculoCompleto = ConsultaVehiculosCln.obtenerUno(vehiculoResumen.id);
                    if (vehiculoCompleto != null && vehiculoCompleto.fotografia != null && vehiculoCompleto.fotografia.Length > 0)
                    {
                        vehiculosCarrusel.Add(vehiculoCompleto);
                        vehiculosCarruselResumen.Add(vehiculoResumen);
                    }
                }

                // Solo activar carrusel si hay 3 o más vehículos con foto
                if (vehiculosCarrusel.Count >= 3)
                {
                    indiceCarruselActual = 0;
                    CargarImagenActual();

                    // Crear y configurar timer para cambiar imagen cada 5 segundos
                    timerCarrusel = new Timer();
                    timerCarrusel.Interval = 5000; // 5 segundos
                    timerCarrusel.Tick += (s, e) => AvanzarCarrusel();
                    timerCarrusel.Start();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error inicializando carrusel: {ex.Message}");
            }
        }

        /// Recarga el carrusel deteniéndolo y reinicializándolo.
        public void RecargarCarrusel()
        {
            try
            {
                if (timerCarrusel != null)
                {
                    timerCarrusel.Stop();
                    timerCarrusel.Dispose();
                    timerCarrusel = null;
                }

                if (timerFade != null)
                {
                    timerFade.Stop();
                    timerFade.Dispose();
                    timerFade = null;
                }

                estoyEnFade = false;
                indiceCarruselActual = 0;

                if (imagenAnterior != null)
                {
                    imagenAnterior.Dispose();
                    imagenAnterior = null;
                }

                InicializarCarrusel();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error recargando carrusel: {ex.Message}");
            }
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // Inicializa pasando el control wmpPlayer que colocaste en el diseñador
            AudioManager.Inicializar(this.wmpPlayer);

            // Reproducir música del menú
            string ruta = Application.StartupPath + @"\Audio\Crystal Waters - Gypsy Woman (She's Homeless) (Official instrumental).mp3";
            AudioManager.Reproducir(ruta, true, 60);

            //InicializarCarrusel();
        }
    }
}
