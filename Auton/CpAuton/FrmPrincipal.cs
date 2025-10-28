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
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ActualizarInterfaz();
        }

        private void btnCaTodoVehiculos_Click(object sender, EventArgs e)
        {
            new FrmConsultaVehiculos().ShowDialog();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Sistema Auton - Confiabilidad Vehicular";
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
        }

        // Eventos de menú
        private void MenuConsultar_Click(object sender, EventArgs e)
        {
            new FrmConsultaVehiculos().ShowDialog();
        }

        private void MenuNuevo_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                using (var frmRegistro = new FrmRegistroVehiculo())
                {
                    frmRegistro.ShowDialog(this);
                    // No necesitamos recargar nada aquí porque no tiene lista
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
                new FrmConsultaVehiculos().ShowDialog();
                //MessageBox.Show("Formulario de Editar Vehículo (Por implementar)",
                //              "En desarrollo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MenuEstadisticas_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reporte de Estadísticas (Por implementar)",
                          "En desarrollo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void MenuUsuarios_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                if (Util.EsAdministrador())
                {
                    MessageBox.Show("Gestión de Usuarios (Por implementar)",
                                  "En desarrollo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Gestión de Empleados (Por implementar)",
                                  "En desarrollo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
