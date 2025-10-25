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
    public partial class FrmVehiculosJaponeses : Form
    {
        public FrmVehiculosJaponeses()
        {
            InitializeComponent();
        }

        private void FrmVehiculosJaponeses_Load(object sender, EventArgs e)
        {
            ConfigurarFormulario();
            ConfigurarDataGridView();
            CargarVehiculosJaponeses();
            btnNuevo.Visible = Util.usuario != null;
        }

        private void ConfigurarFormulario()
        {
            this.Text = "...::: Auton - Vehículos Japoneses :::...";
            //this.Size = new Size(1083, 593);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.PapayaWhip;

        }

        private void ConfigurarDataGridView()
        {
            dgvVehiculos.AutoGenerateColumns = false;
            dgvVehiculos.AllowUserToAddRows = false;
            dgvVehiculos.AllowUserToDeleteRows = false;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculos.MultiSelect = false;
            dgvVehiculos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgvVehiculos.BackgroundColor = Color.White;
            dgvVehiculos.BorderStyle = BorderStyle.None;
            dgvVehiculos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVehiculos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 57, 70);
            dgvVehiculos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvVehiculos.ColumnHeadersHeight = 40;
            dgvVehiculos.EnableHeadersVisualStyles = false;
            dgvVehiculos.RowTemplate.Height = 35;

            dgvVehiculos.Columns.Clear();

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                DataPropertyName = "id",
                HeaderText = "ID",
                Width = 50,
                Visible = false
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMarca",
                DataPropertyName = "marca",
                HeaderText = "🏢 Marca",
                Width = 120
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelo",
                DataPropertyName = "modelo",
                HeaderText = "🚗 Modelo",
                Width = 140
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colGeneracion",
                DataPropertyName = "generacion",
                HeaderText = "📅 Generación",
                Width = 150
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPais",
                DataPropertyName = "paisFabricacion",
                HeaderText = "🌍 País Fabricación",
                Width = 140
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAnio",
                DataPropertyName = "anioFabricacion",
                HeaderText = "📆 Año",
                Width = 80
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTransmision",
                DataPropertyName = "transmision",
                HeaderText = "⚙️ Transmisión",
                Width = 120
            });

            dgvVehiculos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colTurbo",
                DataPropertyName = "tieneTurbo",
                HeaderText = "🚀 Turbo",
                Width = 80
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMotor",
                DataPropertyName = "motor",
                HeaderText = "🔧 Motor",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCombustible",
                DataPropertyName = "combustible",
                HeaderText = "⛽ Combustible",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colConfiabilidad",
                DataPropertyName = "confiabilidad",
                HeaderText = "⭐ Confiabilidad",
                Width = 120
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPuntuacion",
                DataPropertyName = "puntuacion",
                HeaderText = "📊 Puntuación",
                Width = 100,
                DefaultCellStyle = { Format = "N1" }
            });
        }

        private void CargarVehiculosJaponeses()
        {
            try
            {
                // Obtener todos los vehículos y filtrar por procedencia japonesa
                var todosLosVehiculos = ConsultaVehiculosCln.listarPa("");

                // Filtrar solo los de procedencia "Japonés"
                var vehiculosJaponeses = todosLosVehiculos
                    .Where(v => v.procedencia != null && v.procedencia.Equals("Japonés", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(v => v.marca)
                    .ThenBy(v => v.modelo)
                    .ThenBy(v => v.anioFabricacion)
                    .ToList();

                dgvVehiculos.DataSource = vehiculosJaponeses;

                // Actualizar contador
                ActualizarEstadisticas(vehiculosJaponeses);

                // Colorear filas según confiabilidad
                ColorearFilasPorConfiabilidad();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al cargar vehículos japoneses: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarEstadisticas(List<paVehiculoListar_Result> vehiculos)
        {
            if (lblTotal != null)
            {
                lblTotal.Text = $"🇯🇵 Total de Vehículos Japoneses: {vehiculos.Count}";
                lblTotal.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblTotal.ForeColor = Color.FromArgb(230, 57, 70);
            }

            // Estadísticas adicionales
            if (vehiculos.Any())
            {
                var excelentes = vehiculos.Count(v => v.confiabilidad == "Excelente");
                var buenos = vehiculos.Count(v => v.confiabilidad == "Bueno");
                var regulares = vehiculos.Count(v => v.confiabilidad == "Regular");
                var malos = vehiculos.Count(v => v.confiabilidad == "Malo");
                var promedioPuntuacion = vehiculos.Average(v => (double)v.puntuacion);

                if (lblEstadisticas != null)
                {
                    lblEstadisticas.Text = $"📊 Estadísticas: " +
                        $"Excelentes: {excelentes} | " +
                        $"Buenos: {buenos} | " +
                        $"Regulares: {regulares} | " +
                        $"Malos: {malos} | " +
                        $"Promedio: {promedioPuntuacion:N2}";
                    lblEstadisticas.Font = new Font("Segoe UI", 9);
                    lblEstadisticas.ForeColor = Color.FromArgb(52, 58, 64);
                }

                // Mostrar marcas japonesas encontradas
                var marcas = vehiculos.Select(v => v.marca).Distinct().OrderBy(m => m).ToList();
                if (lblMarcas != null)
                {
                    lblMarcas.Text = $"🏢 Marcas: {string.Join(", ", marcas)}";
                    lblMarcas.Font = new Font("Segoe UI", 9);
                    lblMarcas.ForeColor = Color.FromArgb(52, 58, 64);
                }
            }
        }

        private void ColorearFilasPorConfiabilidad()
        {
            foreach (DataGridViewRow row in dgvVehiculos.Rows)
            {
                if (row.Cells["colConfiabilidad"].Value != null)
                {
                    string confiabilidad = row.Cells["colConfiabilidad"].Value.ToString();
                    switch (confiabilidad)
                    {
                        case "Excelente":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(198, 246, 213);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 70, 32);
                            break;
                        case "Bueno":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(207, 226, 255);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(13, 60, 97);
                            break;
                        case "Regular":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(102, 60, 0);
                            break;
                        case "Malo":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(248, 215, 218);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(88, 21, 28);
                            break;
                    }
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    CargarVehiculosJaponeses();
                }
                else
                {
                    var todosLosVehiculos = ConsultaVehiculosCln.listarPa("");
                    var filtrado = todosLosVehiculos
                        .Where(v => v.procedencia != null &&
                                    v.procedencia.Equals("Japonés", StringComparison.OrdinalIgnoreCase))
                        .Where(v =>
                            (v.marca != null && v.marca.ToLower().Contains(txtBuscar.Text.ToLower())) ||
                            (v.modelo != null && v.modelo.ToLower().Contains(txtBuscar.Text.ToLower())) ||
                            (v.generacion != null && v.generacion.ToLower().Contains(txtBuscar.Text.ToLower())) ||
                            (v.motor != null && v.motor.ToLower().Contains(txtBuscar.Text.ToLower())))
                        .OrderBy(v => v.marca)
                        .ThenBy(v => v.modelo)
                        .ToList();

                    dgvVehiculos.DataSource = filtrado;
                    ActualizarEstadisticas(filtrado);
                    ColorearFilasPorConfiabilidad();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvVehiculos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MostrarDetallesVehiculo();
            }
        }

        private void MostrarDetallesVehiculo()
        {
            if (dgvVehiculos.CurrentRow != null)
            {
                var vehiculo = (paVehiculoListar_Result)dgvVehiculos.CurrentRow.DataBoundItem;
                var vehiculoCompleto = VehiculoCln.obtenerUno(vehiculo.id);

                // Crear formulario de detalles
                Form frmDetalles = new Form
                {
                    Text = $"Detalles - {vehiculo.marca} {vehiculo.modelo}",
                    Size = new Size(700, 650),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 10F)
                };

                Panel pnlPrincipal = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(30)
                };

                int yPos = 20;
                int lineHeight = 35;

                // Título
                Label lblTitulo = new Label
                {
                    Text = $"🇯🇵 {vehiculo.marca} {vehiculo.modelo}",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(230, 57, 70),
                    Location = new Point(30, yPos),
                    Size = new Size(600, 30)
                };
                pnlPrincipal.Controls.Add(lblTitulo);
                yPos += 50;

                // Procedencia
                AgregarCampoDetalle(pnlPrincipal, "🌏 Procedencia:", vehiculo.procedencia, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "🏢 Marca:", vehiculo.marca, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "🚗 Modelo:", vehiculo.modelo, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "📅 Generación:", vehiculoCompleto.generacion, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "🌍 País Fabricación:", vehiculoCompleto.paisFabricacion, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "📆 Año:", vehiculoCompleto.anioFabricacion.ToString(), ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "⚙️ Transmisión:", vehiculoCompleto.transmision, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "🚀 Turbo:", vehiculoCompleto.tieneTurbo ? "Sí" : "No", ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "🔧 Motor:", vehiculoCompleto.motor, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "⛽ Combustible:", vehiculoCompleto.combustible, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "🛡️ Airbags:", vehiculoCompleto.nroAirbags.ToString(), ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "⭐ Seguridad:", $"{vehiculoCompleto.seguridadChoques} estrellas ({vehiculoCompleto.organizacionSeguridad})", ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "⭐ Confiabilidad:", vehiculoCompleto.confiabilidad, ref yPos);
                AgregarCampoDetalle(pnlPrincipal, "📊 Puntuación:", vehiculoCompleto.puntuacion.ToString("N1"), ref yPos);

                // Observaciones
                if (!string.IsNullOrWhiteSpace(vehiculoCompleto.observaciones))
                {
                    yPos += 10;
                    Label lblObsTitle = new Label
                    {
                        Text = "📝 Observaciones:",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Location = new Point(30, yPos),
                        Size = new Size(600, 25),
                        ForeColor = Color.FromArgb(52, 58, 64)
                    };
                    pnlPrincipal.Controls.Add(lblObsTitle);
                    yPos += 30;

                    TextBox txtObs = new TextBox
                    {
                        Location = new Point(30, yPos),
                        Size = new Size(600, 180),
                        Multiline = true,
                        ReadOnly = true,
                        BackColor = Color.FromArgb(248, 249, 250),
                        BorderStyle = BorderStyle.FixedSingle,
                        Text = vehiculoCompleto.observaciones,
                        Font = new Font("Segoe UI", 9)
                    };
                    pnlPrincipal.Controls.Add(txtObs);
                }

                // Botón cerrar
                Button btnCerrar = new Button
                {
                    Text = "✅ Cerrar",
                    Size = new Size(150, 45),
                    Location = new Point(260, 550),
                    BackColor = Color.FromArgb(230, 57, 70),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    DialogResult = DialogResult.OK
                };
                btnCerrar.FlatAppearance.BorderSize = 0;

                frmDetalles.Controls.Add(btnCerrar);
                frmDetalles.Controls.Add(pnlPrincipal);
                frmDetalles.AcceptButton = btnCerrar;

                frmDetalles.ShowDialog(this);
            }
        }

        private void AgregarCampoDetalle(Panel panel, string label, string valor, ref int yPos)
        {
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, yPos),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(52, 58, 64)
            };
            panel.Controls.Add(lblLabel);

            Label lblValor = new Label
            {
                Text = valor ?? "N/A",
                Font = new Font("Segoe UI", 10),
                Location = new Point(250, yPos),
                Size = new Size(380, 25),
                ForeColor = Color.FromArgb(73, 80, 87)
            };
            panel.Controls.Add(lblValor);

            yPos += 35;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = $"Vehiculos_Japoneses_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder csv = new StringBuilder();
                    csv.AppendLine("Marca,Modelo,Generación,País,Año,Transmisión,Turbo,Motor,Combustible,Confiabilidad,Puntuación");

                    foreach (DataGridViewRow row in dgvVehiculos.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            csv.AppendLine($"{row.Cells["colMarca"].Value}," +
                                         $"{row.Cells["colModelo"].Value}," +
                                         $"{row.Cells["colGeneracion"].Value}," +
                                         $"{row.Cells["colPais"].Value}," +
                                         $"{row.Cells["colAnio"].Value}," +
                                         $"{row.Cells["colTransmision"].Value}," +
                                         $"{row.Cells["colTurbo"].Value}," +
                                         $"{row.Cells["colMotor"].Value}," +
                                         $"{row.Cells["colCombustible"].Value}," +
                                         $"{row.Cells["colConfiabilidad"].Value}," +
                                         $"{row.Cells["colPuntuacion"].Value}");
                        }
                    }

                    System.IO.File.WriteAllText(saveFileDialog.FileName, csv.ToString(), Encoding.UTF8);
                    MessageBox.Show("✅ Vehículos japoneses exportados exitosamente.", "Exportar",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al exportar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    this.Name = "FrmVehiculosJaponeses";
        //    this.Load += new System.EventHandler(this.FrmVehiculosJaponeses_Load);
        //    this.ResumeLayout(false);
        //}
    }
}
