using CadAuton;
using ClnAuton;
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
    public partial class FrmNuevoModelo : Form
    {
        private int idMarca;
        private int idTipo;
        public int NuevoIdModelo { get; private set; }

        public FrmNuevoModelo(int idMarca, int idTipo)
        {
            InitializeComponent();
            this.idMarca = idMarca;
            this.idTipo = idTipo;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Nuevo Modelo";
            this.Size = new Size(500, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Panel pnlPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            int yPos = 20;
            int lineHeight = 30;

            // Título
            Label lblTitulo = new Label
            {
                Text = "NUEVO MODELO",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(20, yPos),
                Size = new Size(380, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 40;

            // Nombre
            Label lblNombre = new Label
            {
                Text = "Nombre Modelo:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(100, 20)
            };
            txtNombre = new TextBox
            {
                Location = new Point(130, yPos),
                Size = new Size(320, 23)
            };
            pnlPrincipal.Controls.Add(lblNombre);
            pnlPrincipal.Controls.Add(txtNombre);
            yPos += lineHeight;

            // Año Inicio
            Label lblAnioInicio = new Label
            {
                Text = "Año Inicio:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(100, 20)
            };
            txtAnioInicio = new TextBox
            {
                Location = new Point(130, yPos),
                Size = new Size(320, 23)
            };
            pnlPrincipal.Controls.Add(lblAnioInicio);
            pnlPrincipal.Controls.Add(txtAnioInicio);
            yPos += lineHeight;

            // Año Fin
            Label lblAnioFin = new Label
            {
                Text = "Año Fin (Opcional):",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(100, 20)
            };
            txtAnioFin = new TextBox
            {
                Location = new Point(130, yPos),
                Size = new Size(320, 23)
            };
            pnlPrincipal.Controls.Add(lblAnioFin);
            pnlPrincipal.Controls.Add(txtAnioFin);
            yPos += lineHeight;

            // Motor
            Label lblMotor = new Label
            {
                Text = "Motor:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(100, 20)
            };
            txtMotor = new TextBox
            {
                Location = new Point(130, yPos),
                Size = new Size(320, 23)
            };
            pnlPrincipal.Controls.Add(lblMotor);
            pnlPrincipal.Controls.Add(txtMotor);
            yPos += lineHeight;

            // Combustible
            Label lblCombustible = new Label
            {
                Text = "Combustible:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(100, 20)
            };
            cboCombustible = new ComboBox
            {
                Location = new Point(130, yPos),
                Size = new Size(320, 23),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cboCombustible.Items.AddRange(new[] { "Gasolina", "Diesel", "Híbrido", "Eléctrico", "GLP", "Otro" });
            pnlPrincipal.Controls.Add(lblCombustible);
            pnlPrincipal.Controls.Add(cboCombustible);
            yPos += lineHeight;

            // Descripción
            Label lblDescripcion = new Label
            {
                Text = "Descripción:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(100, 20)
            };
            txtDescripcion = new TextBox
            {
                Location = new Point(130, yPos),
                Size = new Size(320, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            pnlPrincipal.Controls.Add(lblDescripcion);
            pnlPrincipal.Controls.Add(txtDescripcion);
            yPos += 90;

            // Botones
            Button btnGuardar = new Button
            {
                Text = "Guardar",
                Size = new Size(100, 35),
                Location = new Point(130, yPos),
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnGuardar.Click += BtnGuardar_Click;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(100, 35),
                Location = new Point(240, yPos),
                BackColor = Color.FromArgb(217, 83, 79),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnCancelar.Click += (s, e) => this.Close();

            pnlPrincipal.Controls.Add(btnGuardar);
            pnlPrincipal.Controls.Add(btnCancelar);

            this.Controls.Add(pnlPrincipal);
        }

        private bool Validar()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar el nombre del modelo", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (txtNombre.Text.Length > 100)
            {
                MessageBox.Show("El nombre no puede exceder 100 caracteres", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtAnioInicio.Text, out int anioInicio) || anioInicio < 1900 || anioInicio > DateTime.Now.Year)
            {
                MessageBox.Show($"El año inicio debe estar entre 1900 y {DateTime.Now.Year}", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnioInicio.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(txtAnioFin.Text))
            {
                if (!int.TryParse(txtAnioFin.Text, out int anioFin) || anioFin < anioInicio || anioFin > DateTime.Now.Year)
                {
                    MessageBox.Show($"El año fin debe estar entre {anioInicio} y {DateTime.Now.Year}", "Validación",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAnioFin.Focus();
                    return false;
                }
            }

            // VALIDACIÓN: Verificar si el modelo ya existe para esta marca
            try
            {
                var todosLosModelos = ModeloVehiculoCln.listar();
                var modeloExistente = todosLosModelos
                    .Where(m => m.idMarca == idMarca && m.estado != -1)
                    .FirstOrDefault(m => m.nombre.Equals(txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                if (modeloExistente != null)
                {
                    MessageBox.Show($"El modelo '{txtNombre.Text.Trim()}' ya existe para esta marca",
                                  "Modelo Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar el modelo: {ex.Message}",
                              "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    var nuevoModelo = new ModeloVehiculo
                    {
                        idMarca = idMarca,
                        idTipoVehiculo = idTipo,
                        nombre = txtNombre.Text.Trim(),
                        anioInicio = int.Parse(txtAnioInicio.Text),
                        anioFin = string.IsNullOrEmpty(txtAnioFin.Text) ? (int?)null : int.Parse(txtAnioFin.Text),
                        motor = string.IsNullOrEmpty(txtMotor.Text) ? null : txtMotor.Text.Trim(),
                        combustible = cboCombustible.SelectedItem != null ? cboCombustible.SelectedItem.ToString() : null,
                        estado = 1,
                        usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : Environment.UserName,
                        fechaRegistro = DateTime.Now
                    };

                    NuevoIdModelo = ModeloVehiculoCln.insertar(nuevoModelo);

                    if (NuevoIdModelo > 0)
                    {
                        MessageBox.Show($"Modelo registrado exitosamente",
                                      "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el modelo: {ex.Message}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private TextBox txtNombre;
        private TextBox txtAnioInicio;
        private TextBox txtAnioFin;
        private TextBox txtMotor;
        private ComboBox cboCombustible;
        private TextBox txtDescripcion;

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    this.Name = "FrmNuevoModelo";
        //    this.ResumeLayout(false);
        //}
    }
}
