using CadAuton;
using ClnAuton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpAuton
{
    public partial class FrmNuevoEmpleado : Form
    {
        public int NuevoIdEmpleado { get; private set; }

        private ComboBox cboCargo;
        private TextBox txtCI;
        private TextBox txtNombres;
        private TextBox txtPrimerApellido;
        private TextBox txtSegundoApellido;
        private TextBox txtDireccion;
        private TextBox txtCelular;

        // P/Invoke para establecer "cue banner" (placeholder) en TextBox (compatible con .NET Framework 4.8)
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public FrmNuevoEmpleado()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "...::: Auton - Nuevo Empleado :::...";
            this.Size = new Size(550, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 10F);

            Panel pnlPrincipal = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(534, 450),
                Padding = new Padding(20),
                BackColor = Color.White
            };

            int yPos = 15;
            int lineHeight = 55;

            // Título
            Label lblTitulo = new Label
            {
                Text = "👤 REGISTRO RÁPIDO DE EMPLEADO",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(20, yPos),
                Size = new Size(494, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 45;

            // Cargo
            AgregarLabel(pnlPrincipal, "Cargo:", 20, yPos, 150);
            cboCargo = new ComboBox
            {
                Location = new Point(180, yPos),
                Size = new Size(320, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            pnlPrincipal.Controls.Add(cboCargo);
            yPos += lineHeight;

            // CI
            AgregarLabel(pnlPrincipal, "CI:", 20, yPos, 150);
            txtCI = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(200, 28),
                Font = new Font("Segoe UI", 10),
                MaxLength = 12
            };
            pnlPrincipal.Controls.Add(txtCI);

            Label lblEjemploCI = new Label
            {
                Text = "Ej: 12345678",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(385, yPos + 5),
                AutoSize = true
            };
            pnlPrincipal.Controls.Add(lblEjemploCI);
            yPos += lineHeight;

            // Nombres
            AgregarLabel(pnlPrincipal, "Nombres:", 20, yPos, 150);
            txtNombres = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(320, 28),
                Font = new Font("Segoe UI", 10),
                MaxLength = 50
            };
            pnlPrincipal.Controls.Add(txtNombres);
            yPos += lineHeight;

            // Primer Apellido
            AgregarLabel(pnlPrincipal, "Primer Apellido:", 20, yPos, 150);
            txtPrimerApellido = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(320, 28),
                Font = new Font("Segoe UI", 10),
                MaxLength = 30
            };
            pnlPrincipal.Controls.Add(txtPrimerApellido);
            yPos += lineHeight;

            // Segundo Apellido
            AgregarLabel(pnlPrincipal, "Segundo Apellido:", 20, yPos, 150);
            txtSegundoApellido = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(320, 28),
                Font = new Font("Segoe UI", 10),
                MaxLength = 30
            };
            pnlPrincipal.Controls.Add(txtSegundoApellido);
            // Establecer placeholder compatible con .NET Framework 4.8
            SetPlaceholder(txtSegundoApellido, "(Opcional)");
            yPos += lineHeight;

            // Dirección
            AgregarLabel(pnlPrincipal, "Dirección:", 20, yPos, 150);
            txtDireccion = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(320, 28),
                Font = new Font("Segoe UI", 10),
                MaxLength = 250
            };
            pnlPrincipal.Controls.Add(txtDireccion);
            yPos += lineHeight;

            // Celular
            AgregarLabel(pnlPrincipal, "Celular:", 20, yPos, 150);
            txtCelular = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(200, 28),
                Font = new Font("Segoe UI", 10),
                MaxLength = 15
            };
            txtCelular.KeyPress += TxtCelular_KeyPress;
            pnlPrincipal.Controls.Add(txtCelular);

            Label lblEjemploCel = new Label
            {
                Text = "Ej: 71234567",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(385, yPos + 5),
                AutoSize = true
            };
            pnlPrincipal.Controls.Add(lblEjemploCel);

            // Panel de botones
            Panel pnlBotones = new Panel
            {
                Location = new Point(0, 450),
                Size = new Size(534, 70),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnGuardar = new Button
            {
                Text = "💾 Guardar Empleado",
                Size = new Size(180, 45),
                Location = new Point(100, 12),
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            Button btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Size = new Size(180, 45),
                Location = new Point(300, 12),
                BackColor = Color.FromArgb(217, 83, 79),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            pnlBotones.Controls.Add(btnGuardar);
            pnlBotones.Controls.Add(btnCancelar);

            this.Controls.Add(pnlBotones);
            this.Controls.Add(pnlPrincipal);
        }

        private void AgregarLabel(Panel panel, string texto, int x, int y, int width)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(x, y + 3),
                Size = new Size(width, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(33, 37, 41)
            };
            panel.Controls.Add(lbl);
        }

        private void FrmNuevoEmpleado_Load(object sender, EventArgs e)
        {
            CargarCargos();
            txtNombres.Focus();
        }

        private void CargarCargos()
        {
            try
            {
                // Método 1: Usando CargoCln
                var cargos = CargoCln.listar();

                if (cargos == null)
                {
                    MessageBox.Show("⚠️ CargoCln.listar() devolvió NULL",
                        "Debug", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cargos.Count == 0)
                {
                    MessageBox.Show("⚠️ No hay cargos en la base de datos.\n\n" +
                                  "Por favor, inserte cargos primero:\n" +
                                  "INSERT INTO Cargo (descripcion) VALUES ('Administrador'), ('Técnico');",
                        "Sin Cargos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Limpiar ComboBox primero
                cboCargo.DataSource = null;
                cboCargo.Items.Clear();

                // Asignar datos
                cboCargo.DisplayMember = "descripcion";
                cboCargo.ValueMember = "id";
                cboCargo.DataSource = cargos;
                cboCargo.SelectedIndex = -1;

                MessageBox.Show($"✅ Cargos cargados exitosamente: {cargos.Count}",
                    "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al cargar cargos:\n\n" +
                              $"Mensaje: {ex.Message}\n\n" +
                              $"Inner: {ex.InnerException?.Message}\n\n" +
                              $"Stack: {ex.StackTrace}",
                    "Error Detallado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtCelular_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool Validar()
        {
            if (cboCargo.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar un cargo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCargo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCI.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar el CI", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCI.Focus();
                return false;
            }

            if (txtCI.Text.Length < 6)
            {
                MessageBox.Show("⚠️ El CI debe tener al menos 6 dígitos", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCI.Focus();
                return false;
            }

            // Verificar CI duplicado
            try
            {
                using (var context = new AutonEntities())
                {
                    var existe = context.Empleado.Any(e =>
                        e.cedulaIdentidad == txtCI.Text.Trim() &&
                        e.estado != -1);

                    if (existe)
                    {
                        MessageBox.Show($"⚠️ Ya existe un empleado con el CI: {txtCI.Text}",
                            "CI Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCI.Focus();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar CI: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar los nombres", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombres.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrimerApellido.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar el primer apellido", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrimerApellido.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar la dirección", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDireccion.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCelular.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar el número de celular", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCelular.Focus();
                return false;
            }

            if (txtCelular.Text.Length < 8)
            {
                MessageBox.Show("⚠️ El celular debe tener al menos 8 dígitos", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCelular.Focus();
                return false;
            }

            return true;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                var nuevoEmpleado = new Empleado
                {
                    idCargo = (int)cboCargo.SelectedValue,
                    cedulaIdentidad = txtCI.Text.Trim(),
                    nombres = txtNombres.Text.Trim(),
                    primerApellido = txtPrimerApellido.Text.Trim(),
                    segundoApellido = string.IsNullOrWhiteSpace(txtSegundoApellido.Text) ?
                        null : txtSegundoApellido.Text.Trim(),
                    direccion = txtDireccion.Text.Trim(),
                    celular = long.Parse(txtCelular.Text.Trim()),
                    estado = 1,
                    usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : "sistema",
                    fechaRegistro = DateTime.Now
                };

                NuevoIdEmpleado = EmpleadoCln.insertar(nuevoEmpleado);

                if (NuevoIdEmpleado > 0)
                {
                    MessageBox.Show(
                        $"✅ Empleado registrado exitosamente\n\n" +
                        $"Nombre: {txtNombres.Text} {txtPrimerApellido.Text}\n" +
                        $"CI: {txtCI.Text}\n\n" +
                        $"Ahora puede asignarle un usuario.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ No se pudo registrar el empleado",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar:\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetPlaceholder(TextBox tb, string placeholder)
        {
            if (tb == null) return;
            // wParam = (IntPtr)1 -> mostrar placeholder cuando el control no tiene foco y está vacío
            SendMessage(tb.Handle, EM_SETCUEBANNER, (IntPtr)1, placeholder);
        }

        private void FrmNuevoEmpleado_Load_1(object sender, EventArgs e)
        {

        }
    }
}
