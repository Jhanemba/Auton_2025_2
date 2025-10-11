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
    public partial class FrmEditarVehiculo : Form
    {
        private int idVehiculo;
        private Vehiculo vehiculoActual;

        public FrmEditarVehiculo(int idVehiculo)
        {
            InitializeComponent();
            this.idVehiculo = idVehiculo;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Editar Vehículo";
            this.Size = new Size(550, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Panel pnlPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            int yPos = 10;
            int lineHeight = 30;
            int lblWidth = 120;
            int ctrlWidth = 300;

            // Título
            Label lblTitulo = new Label
            {
                Text = "EDITAR VEHÍCULO",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(20, yPos),
                Size = new Size(500, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 40;

            // Confiabilidad
            Label lblConfiabilidad = new Label
            {
                Text = "Confiabilidad:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            cboConfiabilidad = new ComboBox
            {
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
            pnlPrincipal.Controls.Add(lblConfiabilidad);
            pnlPrincipal.Controls.Add(cboConfiabilidad);
            yPos += lineHeight;

            // Puntuación
            Label lblPuntuacion = new Label
            {
                Text = "Puntuación:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtPuntuacion = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23)
            };
            pnlPrincipal.Controls.Add(lblPuntuacion);
            pnlPrincipal.Controls.Add(txtPuntuacion);
            yPos += lineHeight;

            // Kilometraje
            Label lblKilometraje = new Label
            {
                Text = "Kilometraje:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtKilometraje = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23)
            };
            pnlPrincipal.Controls.Add(lblKilometraje);
            pnlPrincipal.Controls.Add(txtKilometraje);
            yPos += lineHeight;

            // Propietario
            Label lblPropietario = new Label
            {
                Text = "Propietario:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtPropietario = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23)
            };
            pnlPrincipal.Controls.Add(lblPropietario);
            pnlPrincipal.Controls.Add(txtPropietario);
            yPos += lineHeight;

            // Observaciones
            Label lblObservaciones = new Label
            {
                Text = "Observaciones:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtObservaciones = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            pnlPrincipal.Controls.Add(lblObservaciones);
            pnlPrincipal.Controls.Add(txtObservaciones);
            yPos += 90;

            // Botones
            Button btnGuardar = new Button
            {
                Text = "Guardar",
                Size = new Size(100, 35),
                Location = new Point(150, yPos),
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
                Location = new Point(260, yPos),
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

        private void FrmEditarVehiculo_Load(object sender, EventArgs e)
        {
            CargarDatosVehiculo();
        }

        private void CargarDatosVehiculo()
        {
            try
            {
                vehiculoActual = VehiculoCln.obtenerUno(idVehiculo);

                if (vehiculoActual != null)
                {
                    // Cargar los datos actuales del vehículo
                    cboConfiabilidad.SelectedItem = vehiculoActual.confiabilidad;
                    txtPuntuacion.Text = vehiculoActual.puntuacion.ToString();
                    txtKilometraje.Text = vehiculoActual.kilometraje?.ToString() ?? "";
                    txtPropietario.Text = vehiculoActual.propietario ?? "";
                    txtObservaciones.Text = vehiculoActual.observaciones ?? "";
                }
                else
                {
                    MessageBox.Show("No se pudo cargar el vehículo", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private bool Validar()
        {
            if (cboConfiabilidad.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un nivel de confiabilidad", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboConfiabilidad.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPuntuacion.Text, out decimal puntuacion) || puntuacion < 1.0m || puntuacion > 10.0m)
            {
                MessageBox.Show("La puntuación debe estar entre 1.0 y 10.0", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPuntuacion.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(txtKilometraje.Text))
            {
                if (!int.TryParse(txtKilometraje.Text, out int km) || km < 0)
                {
                    MessageBox.Show("El kilometraje debe ser un número positivo", "Validación",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKilometraje.Focus();
                    return false;
                }
            }

            return true;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    // Actualizar solo los campos editables
                    vehiculoActual.confiabilidad = cboConfiabilidad.SelectedItem.ToString();
                    vehiculoActual.puntuacion = decimal.Parse(txtPuntuacion.Text);
                    vehiculoActual.kilometraje = string.IsNullOrEmpty(txtKilometraje.Text) ? (int?)null : int.Parse(txtKilometraje.Text);
                    vehiculoActual.propietario = string.IsNullOrEmpty(txtPropietario.Text) ? null : txtPropietario.Text;
                    vehiculoActual.observaciones = string.IsNullOrEmpty(txtObservaciones.Text) ? null : txtObservaciones.Text;
                    vehiculoActual.usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : Environment.UserName;

                    int resultado = VehiculoCln.actualizar(vehiculoActual);

                    if (resultado > 0)
                    {
                        MessageBox.Show("Vehículo actualizado exitosamente", "Éxito",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar los cambios: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private ComboBox cboConfiabilidad;
        private TextBox txtPuntuacion;
        private TextBox txtKilometraje;
        private TextBox txtPropietario;
        private TextBox txtObservaciones;

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    this.Name = "FrmEditarVehiculo";
        //    this.Load += new System.EventHandler(this.FrmEditarVehiculo_Load);
        //    this.ResumeLayout(false);
        //}
    }
}
