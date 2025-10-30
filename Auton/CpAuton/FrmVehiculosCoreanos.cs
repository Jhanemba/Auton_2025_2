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
    public partial class FrmVehiculosCoreanos : Form
    {
        //private Panel pnlSuperior;
        //private Label lblTitulo;
        //private TextBox txtBuscar;
        //private Label lblBuscar;
        //private DataGridView dgvVehiculos;
        //private Panel pnlInferior;
        //private Label lblTotal;
        //private Label lblEstadisticas;
        //private Label lblMarcas;
        //private Button btnExportar;
        //private Button btnCerrar;

        public FrmVehiculosCoreanos()
        {
            InitializeComponent();
        }

        private void FrmVehiculosCoreanos_Load(object sender, EventArgs e)
        {
            ConfigurarFormulario();
            ConfigurarDataGridView();
            CargarVehiculosCoreanos();
            btnNuevo.Visible = Util.usuario != null;
        }

        private void ConfigurarFormulario()
        {
            this.Text = "...::: Auton - Vehículos Coreanos :::...";
            //this.Size = new Size(1400, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            //this.BackColor = Color.White;
        }

        private void ConfigurarDataGridView()
        {
            dgvVehiculos.AutoGenerateColumns = false;
            dgvVehiculos.AllowUserToAddRows = false;
            dgvVehiculos.AllowUserToDeleteRows = false;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculos.MultiSelect = false;
            dgvVehiculos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 240, 245);
            dgvVehiculos.BackgroundColor = Color.White;
            dgvVehiculos.BorderStyle = BorderStyle.None;
            dgvVehiculos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVehiculos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(200, 16, 46); // Rojo coreano
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

        private void CargarVehiculosCoreanos()
        {
            try
            {
                var vehiculosCoreanos = VehiculosCoreanosCln.listar();
                dgvVehiculos.DataSource = vehiculosCoreanos;

                ActualizarEstadisticas();
                ColorearFilasPorConfiabilidad();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al cargar vehículos coreanos: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarEstadisticas()
        {
            try
            {
                var stats = VehiculosCoreanosCln.obtenerEstadisticas();

                if (lblTotal != null)
                {
                    lblTotal.Text = $"🇰🇷 Total de Vehículos Coreanos: {stats.TotalVehiculos}";
                    lblTotal.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    lblTotal.ForeColor = Color.FromArgb(200, 16, 46);
                }

                if (lblEstadisticas != null)
                {
                    lblEstadisticas.Text = $"📊 {stats.ToString()}";
                    lblEstadisticas.Font = new Font("Segoe UI", 9);
                    lblEstadisticas.ForeColor = Color.FromArgb(52, 58, 64);
                }

                if (lblMarcas != null)
                {
                    lblMarcas.Text = $"🏢 Marcas: {stats.ObtenerMarcasTexto()} | " +
                                   $"🚀 Con Turbo: {stats.ConTurbo} | Sin Turbo: {stats.SinTurbo}";
                    lblMarcas.Font = new Font("Segoe UI", 9);
                    lblMarcas.ForeColor = Color.FromArgb(52, 58, 64);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar estadísticas: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var resultado = VehiculosCoreanosCln.buscar(txtBuscar.Text.Trim());
                dgvVehiculos.DataSource = resultado;

                if (lblTotal != null)
                {
                    lblTotal.Text = $"🇰🇷 Total de Vehículos Coreanos: {resultado.Count}";
                }

                ColorearFilasPorConfiabilidad();
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
                var vehiculo = (paVehiculosCoreanos_Result)dgvVehiculos.CurrentRow.DataBoundItem;
                var vehiculoCompleto = VehiculoCln.obtenerUno(vehiculo.id);

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

                Label lblTitulo = new Label
                {
                    Text = $"🇰🇷 {vehiculo.marca} {vehiculo.modelo}",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(200, 16, 46),
                    Location = new Point(30, yPos),
                    Size = new Size(600, 30)
                };
                pnlPrincipal.Controls.Add(lblTitulo);
                yPos += 50;

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
                        Size = new Size(600, 80),
                        Multiline = true,
                        ReadOnly = true,
                        BackColor = Color.FromArgb(248, 249, 250),
                        BorderStyle = BorderStyle.FixedSingle,
                        Text = vehiculoCompleto.observaciones,
                        Font = new Font("Segoe UI", 9)
                    };
                    pnlPrincipal.Controls.Add(txtObs);
                }

                Button btnCerrar = new Button
                {
                    Text = "✅ Cerrar",
                    Size = new Size(150, 45),
                    Location = new Point(260, 550),
                    BackColor = Color.FromArgb(200, 16, 46),
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
                    FileName = $"Vehiculos_Coreanos_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
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
                    MessageBox.Show("✅ Vehículos coreanos exportados exitosamente.", "Exportar",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al exportar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            new FrmRegistroVehiculo().ShowDialog();
        }

        //private void InitializeComponent()
        //{
        //    this.pnlSuperior = new System.Windows.Forms.Panel();
        //    this.lblTitulo = new System.Windows.Forms.Label();
        //    this.lblBuscar = new System.Windows.Forms.Label();
        //    this.txtBuscar = new System.Windows.Forms.TextBox();
        //    this.dgvVehiculos = new System.Windows.Forms.DataGridView();
        //    this.pnlInferior = new System.Windows.Forms.Panel();
        //    this.lblTotal = new System.Windows.Forms.Label();
        //    this.lblEstadisticas = new System.Windows.Forms.Label();
        //    this.lblMarcas = new System.Windows.Forms.Label();
        //    this.btnExportar = new System.Windows.Forms.Button();
        //    this.btnCerrar = new System.Windows.Forms.Button();
        //    this.pnlSuperior.SuspendLayout();
        //    ((System.ComponentModel.ISupportInitialize)(this.dgvVehiculos)).BeginInit();
        //    this.pnlInferior.SuspendLayout();
        //    this.SuspendLayout();

        //    // pnlSuperior
        //    this.pnlSuperior.BackColor = System.Drawing.Color.FromArgb(200, 16, 46);
        //    this.pnlSuperior.Controls.Add(this.lblTitulo);
        //    this.pnlSuperior.Controls.Add(this.txtBuscar);
        //    this.pnlSuperior.Controls.Add(this.lblBuscar);
        //    this.pnlSuperior.Dock = System.Windows.Forms.DockStyle.Top;
        //    this.pnlSuperior.Location = new System.Drawing.Point(0, 0);
        //    this.pnlSuperior.Name = "pnlSuperior";
        //    this.pnlSuperior.Size = new System.Drawing.Size(1384, 100);
        //    this.pnlSuperior.TabIndex = 0;

        //    // lblTitulo
        //    this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
        //    this.lblTitulo.ForeColor = System.Drawing.Color.White;
        //    this.lblTitulo.Location = new System.Drawing.Point(20, 15);
        //    this.lblTitulo.Name = "lblTitulo";
        //    this.lblTitulo.Size = new System.Drawing.Size(500, 40);
        //    this.lblTitulo.TabIndex = 0;
        //    this.lblTitulo.Text = "🇰🇷 VEHÍCULOS COREANOS";

        //    // lblBuscar
        //    this.lblBuscar.AutoSize = true;
        //    this.lblBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        //    this.lblBuscar.ForeColor = System.Drawing.Color.White;
        //    this.lblBuscar.Location = new System.Drawing.Point(800, 28);
        //    this.lblBuscar.Name = "lblBuscar";
        //    this.lblBuscar.Size = new System.Drawing.Size(95, 23);
        //    this.lblBuscar.TabIndex = 1;
        //    this.lblBuscar.Text = "🔍 Buscar:";

        //    // txtBuscar
        //    this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 11F);
        //    this.txtBuscar.Location = new System.Drawing.Point(900, 25);
        //    this.txtBuscar.Name = "txtBuscar";
        //    this.txtBuscar.Size = new System.Drawing.Size(450, 32);
        //    this.txtBuscar.TabIndex = 2;
        //    this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);

        //    // dgvVehiculos
        //    this.dgvVehiculos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //    this.dgvVehiculos.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.dgvVehiculos.Location = new System.Drawing.Point(0, 100);
        //    this.dgvVehiculos.Name = "dgvVehiculos";
        //    this.dgvVehiculos.RowHeadersWidth = 51;
        //    this.dgvVehiculos.Size = new System.Drawing.Size(1384, 451);
        //    this.dgvVehiculos.TabIndex = 1;
        //    this.dgvVehiculos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVehiculos_CellDoubleClick);

        //    // pnlInferior
        //    this.pnlInferior.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
        //    this.pnlInferior.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        //    this.pnlInferior.Controls.Add(this.lblMarcas);
        //    this.pnlInferior.Controls.Add(this.lblEstadisticas);
        //    this.pnlInferior.Controls.Add(this.lblTotal);
        //    this.pnlInferior.Controls.Add(this.btnCerrar);
        //    this.pnlInferior.Controls.Add(this.btnExportar);
        //    this.pnlInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
        //    this.pnlInferior.Location = new System.Drawing.Point(0, 551);
        //    this.pnlInferior.Name = "pnlInferior";
        //    this.pnlInferior.Size = new System.Drawing.Size(1384, 110);
        //    this.pnlInferior.TabIndex = 2;

        //    // lblTotal
        //    this.lblTotal.AutoSize = true;
        //    this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        //    this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(200, 16, 46);
        //    this.lblTotal.Location = new System.Drawing.Point(20, 15);
        //    this.lblTotal.Name = "lblTotal";
        //    this.lblTotal.Size = new System.Drawing.Size(280, 25);
        //    this.lblTotal.TabIndex = 0;
        //    this.lblTotal.Text = "🇰🇷 Total: 0";

        //    // lblEstadisticas
        //    this.lblEstadisticas.AutoSize = true;
        //    this.lblEstadisticas.Font = new System.Drawing.Font("Segoe UI", 9F);
        //    this.lblEstadisticas.Location = new System.Drawing.Point(20, 45);
        //    this.lblEstadisticas.Name = "lblEstadisticas";
        //    this.lblEstadisticas.Size = new System.Drawing.Size(200, 20);
        //    this.lblEstadisticas.TabIndex = 1;
        //    this.lblEstadisticas.Text = "📊 Estadísticas";

        //    // lblMarcas
        //    this.lblMarcas.AutoSize = true;
        //    this.lblMarcas.Font = new System.Drawing.Font("Segoe UI", 9F);
        //    this.lblMarcas.Location = new System.Drawing.Point(20, 70);
        //    this.lblMarcas.Name = "lblMarcas";
        //    this.lblMarcas.Size = new System.Drawing.Size(150, 20);
        //    this.lblMarcas.TabIndex = 2;
        //    this.lblMarcas.Text = "🏢 Marcas";

        //    // btnExportar
        //    this.btnExportar.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
        //    this.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        //    this.btnExportar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        //    this.btnExportar.ForeColor = System.Drawing.Color.White;
        //    this.btnExportar.Location = new System.Drawing.Point(1050, 30);
        //    this.btnExportar.Name = "btnExportar";
        //    this.btnExportar.Size = new System.Drawing.Size(150, 45);
        //    this.btnExportar.TabIndex = 3;
        //    this.btnExportar.Text = "📥 Exportar CSV";
        //    this.btnExportar.UseVisualStyleBackColor = false;
        //    this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);

        //    // btnCerrar
        //    this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
        //    this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        //    this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        //    this.btnCerrar.ForeColor = System.Drawing.Color.White;
        //    this.btnCerrar.Location = new System.Drawing.Point(1220, 30);
        //    this.btnCerrar.Name = "btnCerrar";
        //    this.btnCerrar.Size = new System.Drawing.Size(140, 45);
        //    this.btnCerrar.TabIndex = 4;
        //    this.btnCerrar.Text = "❌ Cerrar";
        //    this.btnCerrar.UseVisualStyleBackColor = false;
        //    this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);

        //    // FrmVehiculosCoreanos
        //    this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.ClientSize = new System.Drawing.Size(1384, 661);
        //    this.Controls.Add(this.dgvVehiculos);
        //    this.Controls.Add(this.pnlInferior);
        //    this.Controls.Add(this.pnlSuperior);
        //    this.Name = "FrmVehiculosCoreanos";
        //    this.Text = "Vehículos Coreanos";
        //    this.Load += new System.EventHandler(this.FrmVehiculosCoreanos_Load);
        //    this.pnlSuperior.ResumeLayout(false);
        //    this.pnlSuperior.PerformLayout();
        //    ((System.ComponentModel.ISupportInitialize)(this.dgvVehiculos)).EndInit();
        //    this.pnlInferior.ResumeLayout(false);
    }
}
