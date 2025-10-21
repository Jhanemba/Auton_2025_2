using CadAuton;
using ClnAuton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpAuton
{
    public partial class FrmConsultaVehiculos : Form
    {

        private Label lblEstadoUsuario;
        private Button btnSesion;

        public FrmConsultaVehiculos()
        {
            InitializeComponent();
        }

        private void FrmConsultaVehiculos_Load(object sender, EventArgs e)
        {
            AgregarBotonesSesion();
            ConfigurarDataGridView();
            CargarVehiculos("");
        }

        private void AgregarBotonesSesion()
        {
            Panel pnlSesion = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnSesion = new Button
            {
                Text = Util.usuario != null ? "Cerrar Sesión" : "Iniciar Sesión",
                Size = new Size(120, 35),
                Location = new Point(10, 7),
                BackColor = Util.usuario != null ? Color.FromArgb(217, 83, 79) : Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnSesion.Click += BtnSesion_Click;

            lblEstadoUsuario = new Label
            {
                Text = Util.usuario != null ? $"Usuario: {Util.usuario.nombreCompleto}" : "Usuario: No autenticado",
                Size = new Size(400, 35),
                Location = new Point(140, 7),
                Font = new Font("Arial", 9),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlSesion.Controls.Add(btnSesion);
            pnlSesion.Controls.Add(lblEstadoUsuario);

            this.Controls.Add(pnlSesion);
        }

        private void BtnSesion_Click(object sender, EventArgs e)
        {
            if (Util.usuario != null)
            {
                DialogResult resultado = MessageBox.Show("¿Desea cerrar la sesión?",
                                                        "Cerrar Sesión",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    Util.CerrarSesion();
                    ActualizarInterfazSesion();
                }
            }
            else
            {
                FrmAutenticacion frmAuth = new FrmAutenticacion();
                if (frmAuth.ShowDialog(this) == DialogResult.OK)
                {
                    ActualizarInterfazSesion();
                }
            }
        }

        private void ActualizarInterfazSesion()
        {
            if (btnSesion != null)
            {
                btnSesion.Text = Util.usuario != null ? "Cerrar Sesión" : "Iniciar Sesión";
                btnSesion.BackColor = Util.usuario != null ? Color.FromArgb(217, 83, 79) : Color.FromArgb(51, 122, 183);
            }

            if (lblEstadoUsuario != null)
            {
                lblEstadoUsuario.Text = Util.usuario != null ? $"Usuario: {Util.usuario.nombreCompleto}" : "Usuario: No autenticado";
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvVehiculos.AutoGenerateColumns = false;
            dgvVehiculos.AllowUserToAddRows = false;
            dgvVehiculos.AllowUserToDeleteRows = false;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculos.MultiSelect = false;
            dgvVehiculos.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

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
                HeaderText = "Marca",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelo",
                DataPropertyName = "modelo",
                HeaderText = "Modelo",
                Width = 120
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colGeneracion",
                DataPropertyName = "generacion",
                HeaderText = "Generación",
                Width = 120
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPais",
                DataPropertyName = "paisFabricacion",
                HeaderText = "País",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAnio",
                DataPropertyName = "anioFabricacion",
                HeaderText = "Año",
                Width = 60
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTransmision",
                DataPropertyName = "transmision",
                HeaderText = "Transmisión",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colTurbo",
                DataPropertyName = "tieneTurbo",
                HeaderText = "Turbo",
                Width = 60
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMotor",
                DataPropertyName = "motor",
                HeaderText = "Motor",
                Width = 80
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colConfiabilidad",
                DataPropertyName = "confiabilidad",
                HeaderText = "Confiabilidad",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPuntuacion",
                DataPropertyName = "puntuacion",
                HeaderText = "Puntuación",
                Width = 80,
                DefaultCellStyle = { Format = "N1" }
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCombustible",
                DataPropertyName = "combustible",
                HeaderText = "Combustible",
                Width = 80
            });
        }

        private void CargarVehiculos(string parametro)
        {
            try
            {
                var vehiculos = ConsultaVehiculosCln.listarPa(parametro ?? "");
                dgvVehiculos.DataSource = vehiculos;

                if (lblTotal != null)
                {
                    lblTotal.Text = $"Total de vehículos: {vehiculos.Count}";
                }

                ColorearFilasPorConfiabilidad();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los vehículos: {ex.Message}",
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
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            break;
                        case "Bueno":
                            row.DefaultCellStyle.BackColor = Color.LightBlue;
                            break;
                        case "Regular":
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            break;
                        case "Malo":
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                            break;
                    }
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarVehiculos(txtBuscar.Text.Trim());
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
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

                Form frmDetalles = new Form
                {
                    Text = "Detalles del Vehículo",
                    Size = new Size(700, 780),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                Panel pnlPrincipal = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(20)
                };

                int yPos = 20;
                int lineHeight = 35;

                // Etiqueta de estado edición
                Label lblEstado = new Label
                {
                    Text = "MODO LECTURA",
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    ForeColor = Color.Red,
                    Location = new Point(20, yPos),
                    Size = new Size(620, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlPrincipal.Controls.Add(lblEstado);
                yPos += 35;

                // ===== IDENTIFICACIÓN =====
                AgregarTitulo(pnlPrincipal, "📋 IDENTIFICACIÓN", yPos);
                yPos += 30;

                var txtMarca = AgregarCampo(pnlPrincipal, "Marca:", vehiculo.marca, yPos, true);
                yPos += lineHeight;

                var txtModelo = AgregarCampo(pnlPrincipal, "Modelo:", vehiculo.modelo, yPos, true);
                yPos += lineHeight;

                var txtTipo = AgregarCampo(pnlPrincipal, "Tipo:", vehiculo.tipoVehiculo, yPos, true);
                yPos += lineHeight;

                var nudAnio = AgregarNumerico(pnlPrincipal, "Año Fabricación:", vehiculoCompleto.anioFabricacion, yPos, 1900, DateTime.Now.Year, true);
                yPos += lineHeight;

                // ===== CARACTERÍSTICAS ESPECÍFICAS =====
                yPos += 10;
                AgregarTitulo(pnlPrincipal, "⚙️ CARACTERÍSTICAS ESPECÍFICAS", yPos);
                yPos += 30;

                var txtGeneracion = AgregarCampo(pnlPrincipal, "Generación:", vehiculoCompleto.generacion, yPos, false);
                yPos += lineHeight;

                var txtPais = AgregarCampo(pnlPrincipal, "País Fabricación:", vehiculoCompleto.paisFabricacion, yPos, false);
                yPos += lineHeight;

                var txtTransmision = AgregarCampo(pnlPrincipal, "Transmisión:", vehiculoCompleto.transmision, yPos, false);
                yPos += lineHeight;

                var chkTurbo = AgregarCheckBox(pnlPrincipal, "Tiene Turbo:", vehiculoCompleto.tieneTurbo, yPos, false);
                yPos += lineHeight;

                var txtMotor = AgregarCampo(pnlPrincipal, "Motor:", vehiculoCompleto.motor, yPos, false);
                yPos += lineHeight;

                var txtCombustible = AgregarCampo(pnlPrincipal, "Combustible:", vehiculoCompleto.combustible, yPos, false);
                yPos += lineHeight;

                // ===== SEGURIDAD =====
                yPos += 10;
                AgregarTitulo(pnlPrincipal, "🛡️ SEGURIDAD", yPos);
                yPos += 30;

                var nudAirbags = AgregarNumerico(pnlPrincipal, "Nro. Airbags:", vehiculoCompleto.nroAirbags, yPos, 0, 12, false);
                yPos += lineHeight;

                var txtOrganizacion = AgregarCampo(pnlPrincipal, "Organización:", vehiculoCompleto.organizacionSeguridad, yPos, false);
                yPos += lineHeight;

                var nudEstrellas = AgregarNumerico(pnlPrincipal, "Estrellas:", vehiculoCompleto.seguridadChoques, yPos, 0, 5, false);
                yPos += lineHeight;

                // ===== EVALUACIÓN =====
                yPos += 10;
                AgregarTitulo(pnlPrincipal, "⭐ EVALUACIÓN", yPos);
                yPos += 30;

                ComboBox cboConfiabilidad = new ComboBox
                {
                    Location = new Point(170, yPos),
                    Size = new Size(250, 23),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Enabled = false
                };
                cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
                cboConfiabilidad.SelectedItem = vehiculoCompleto.confiabilidad;
                AgregarLabel(pnlPrincipal, "Confiabilidad:", 20, yPos);
                pnlPrincipal.Controls.Add(cboConfiabilidad);
                yPos += lineHeight;

                var nudPuntuacion = AgregarNumericoDecimal(pnlPrincipal, "Puntuación:", vehiculoCompleto.puntuacion, yPos, 1.0m, 10.0m, false);
                yPos += lineHeight;

                // Observaciones
                yPos += 10;
                AgregarLabel(pnlPrincipal, "Observaciones:", 20, yPos);
                TextBox txtObservaciones = new TextBox
                {
                    Location = new Point(170, yPos),
                    Size = new Size(480, 300),
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke,
                    Text = vehiculoCompleto.observaciones ?? ""
                };
                pnlPrincipal.Controls.Add(txtObservaciones);
                yPos += 90;

                // Panel inferior con botones
                Panel pnlBotones = new Panel
                {
                    Height = 100,
                    Dock = DockStyle.Bottom,
                    Padding = new Padding(580, 100, 580, 100),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Botones Editar y Eliminar (solo si hay usuario autenticado)
                if (Util.usuario != null)
                {
                    Button btnEditar = new Button
                    {
                        Text = "Editar",
                        Size = new Size(110, 40),
                        Location = new Point(150, 10),
                        BackColor = Color.FromArgb(51, 122, 183),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Arial", 9, FontStyle.Bold)
                    };
                    btnEditar.Click += (s, e) =>
                    {
                        if (btnEditar.Text == "Editar")
                        {
                            // Activar modo edición
                            txtGeneracion.ReadOnly = false;
                            txtPais.ReadOnly = false;
                            txtTransmision.ReadOnly = false;
                            chkTurbo.Enabled = true;
                            txtMotor.ReadOnly = false;
                            txtCombustible.ReadOnly = false;
                            nudAirbags.Enabled = true;
                            txtOrganizacion.ReadOnly = false;
                            nudEstrellas.Enabled = true;
                            nudAnio.Enabled = true;
                            cboConfiabilidad.Enabled = true;
                            nudPuntuacion.Enabled = true;
                            txtObservaciones.ReadOnly = false;

                            // Cambiar colores
                            txtGeneracion.BackColor = Color.White;
                            txtPais.BackColor = Color.White;
                            txtTransmision.BackColor = Color.White;
                            txtMotor.BackColor = Color.White;
                            txtCombustible.BackColor = Color.White;
                            txtOrganizacion.BackColor = Color.White;
                            txtObservaciones.BackColor = Color.White;

                            lblEstado.Text = "MODO EDICIÓN";
                            lblEstado.ForeColor = Color.Green;
                            btnEditar.Text = "Guardar";
                            btnEditar.BackColor = Color.FromArgb(40, 167, 69);
                        }
                        else
                        {
                            // VALIDACIONES
                            if (string.IsNullOrWhiteSpace(txtGeneracion.Text))
                            {
                                MessageBox.Show("La generación es obligatoria", "Validación",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            if (string.IsNullOrWhiteSpace(txtPais.Text))
                            {
                                MessageBox.Show("El país de fabricación es obligatorio", "Validación",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            if (cboConfiabilidad.SelectedIndex == -1)
                            {
                                MessageBox.Show("Debe seleccionar un nivel de confiabilidad", "Validación",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // ACTUALIZAR VEHÍCULO
                            try
                            {
                                vehiculoCompleto.generacion = txtGeneracion.Text.Trim();
                                vehiculoCompleto.paisFabricacion = txtPais.Text.Trim();
                                vehiculoCompleto.transmision = txtTransmision.Text.Trim();
                                vehiculoCompleto.tieneTurbo = chkTurbo.Checked;
                                vehiculoCompleto.motor = txtMotor.Text.Trim();
                                vehiculoCompleto.combustible = txtCombustible.Text.Trim();
                                vehiculoCompleto.nroAirbags = (int)nudAirbags.Value;
                                vehiculoCompleto.organizacionSeguridad = txtOrganizacion.Text.Trim();
                                vehiculoCompleto.seguridadChoques = (int)nudEstrellas.Value;
                                vehiculoCompleto.anioFabricacion = (int)nudAnio.Value;
                                vehiculoCompleto.confiabilidad = cboConfiabilidad.SelectedItem.ToString();
                                vehiculoCompleto.puntuacion = nudPuntuacion.Value;
                                vehiculoCompleto.observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();
                                vehiculoCompleto.usuarioRegistro = Util.usuario.usuario;

                                int resultado = VehiculoCln.actualizar(vehiculoCompleto);

                                if (resultado > 0)
                                {
                                    // Desactivar modo edición
                                    txtGeneracion.ReadOnly = true;
                                    txtPais.ReadOnly = true;
                                    txtTransmision.ReadOnly = true;
                                    chkTurbo.Enabled = false;
                                    txtMotor.ReadOnly = true;
                                    txtCombustible.ReadOnly = true;
                                    nudAirbags.Enabled = false;
                                    txtOrganizacion.ReadOnly = true;
                                    nudEstrellas.Enabled = false;
                                    nudAnio.Enabled = false;
                                    cboConfiabilidad.Enabled = false;
                                    nudPuntuacion.Enabled = false;
                                    txtObservaciones.ReadOnly = true;

                                    txtGeneracion.BackColor = Color.WhiteSmoke;
                                    txtPais.BackColor = Color.WhiteSmoke;
                                    txtTransmision.BackColor = Color.WhiteSmoke;
                                    txtMotor.BackColor = Color.WhiteSmoke;
                                    txtCombustible.BackColor = Color.WhiteSmoke;
                                    txtOrganizacion.BackColor = Color.WhiteSmoke;
                                    txtObservaciones.BackColor = Color.WhiteSmoke;

                                    lblEstado.Text = "MODO LECTURA";
                                    lblEstado.ForeColor = Color.Red;
                                    btnEditar.Text = "Editar";
                                    btnEditar.BackColor = Color.FromArgb(51, 122, 183);

                                    MessageBox.Show("✅ Vehículo actualizado exitosamente", "Éxito",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    frmDetalles.Close();
                                    CargarVehiculos("");
                                }
                                else
                                {
                                    MessageBox.Show("❌ No se pudo actualizar el vehículo", "Error",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"❌ Error al guardar: {ex.Message}", "Error",
                                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    Button btnEliminar = new Button
                    {
                        Text = "Eliminar",
                        Size = new Size(110, 40),
                        Location = new Point(270, 10),
                        BackColor = Color.FromArgb(217, 83, 79),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Arial", 9, FontStyle.Bold)
                    };
                    btnEliminar.Click += (s, e) => EliminarVehiculo(vehiculo, frmDetalles);

                    pnlBotones.Controls.Add(btnEditar);
                    pnlBotones.Controls.Add(btnEliminar);
                }

                Button btnCerrar = new Button
                {
                    Text = "Cerrar",
                    Size = new Size(110, 40),
                    Location = new Point(450, 10),
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    DialogResult = DialogResult.OK
                };

                pnlBotones.Controls.Add(btnCerrar);
                frmDetalles.Controls.Add(pnlBotones);
                frmDetalles.Controls.Add(pnlPrincipal);
                frmDetalles.AcceptButton = btnCerrar;

                frmDetalles.ShowDialog(this);
            }
        }

        private void AgregarTitulo(Panel panel, string texto, int yPos)
        {
            Panel pnlTitulo = new Panel
            {
                Location = new Point(20, yPos),
                Size = new Size(620, 25),
                BackColor = Color.FromArgb(233, 236, 239)
            };

            Label lblTitulo = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(10, 4),
                AutoSize = true
            };
            pnlTitulo.Controls.Add(lblTitulo);
            panel.Controls.Add(pnlTitulo);
        }

        private void AgregarLabel(Panel panel, string texto, int x, int yPos)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(x, yPos + 3),
                Size = new Size(140, 20),
                ForeColor = Color.DarkBlue
            };
            panel.Controls.Add(lbl);
        }

        private TextBox AgregarCampo(Panel panel, string label, string valor, int yPos, bool soloLectura)
        {
            AgregarLabel(panel, label, 20, yPos);
            TextBox txt = new TextBox
            {
                Location = new Point(170, yPos),
                Size = new Size(480, 23),
                ReadOnly = soloLectura,
                BackColor = Color.WhiteSmoke,
                Text = valor ?? ""
            };
            panel.Controls.Add(txt);
            return txt;
        }

        private NumericUpDown AgregarNumerico(Panel panel, string label, int valor, int yPos, int min, int max, bool soloLectura)
        {
            AgregarLabel(panel, label, 20, yPos);
            NumericUpDown nud = new NumericUpDown
            {
                Location = new Point(170, yPos),
                Size = new Size(120, 23),
                Minimum = min,
                Maximum = max,
                Value = valor,
                Enabled = !soloLectura,
                TextAlign = HorizontalAlignment.Center
            };
            panel.Controls.Add(nud);
            return nud;
        }

        private NumericUpDown AgregarNumericoDecimal(Panel panel, string label, decimal valor, int yPos, decimal min, decimal max, bool soloLectura)
        {
            AgregarLabel(panel, label, 20, yPos);
            NumericUpDown nud = new NumericUpDown
            {
                Location = new Point(170, yPos),
                Size = new Size(120, 23),
                Minimum = min,
                Maximum = max,
                DecimalPlaces = 1,
                Increment = 0.5m,
                Value = valor,
                Enabled = !soloLectura,
                TextAlign = HorizontalAlignment.Center
            };
            panel.Controls.Add(nud);
            return nud;
        }

        private CheckBox AgregarCheckBox(Panel panel, string label, bool valor, int yPos, bool soloLectura)
        {
            AgregarLabel(panel, label, 20, yPos);
            CheckBox chk = new CheckBox
            {
                Location = new Point(170, yPos + 3),
                Checked = valor,
                Enabled = !soloLectura,
                AutoSize = true
            };
            panel.Controls.Add(chk);
            return chk;
        }

        private void EliminarVehiculo(paVehiculoListar_Result vehiculo, Form frmDetalles)
        {
            DialogResult resultado = MessageBox.Show($"¿Está seguro que desea eliminar el vehículo '{vehiculo.modelo} {vehiculo.generacion}'?",
                                                    "Eliminar Vehículo",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                try
                {
                    int resultado_op = VehiculoCln.eliminar(vehiculo.id, Util.usuario.usuario);
                    if (resultado_op > 0)
                    {
                        frmDetalles.Close();
                        CargarVehiculos("");
                        MessageBox.Show("✅ Vehículo eliminado exitosamente", "Éxito",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al eliminar el vehículo: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            ExportarACSV();
        }

        private void ExportarACSV()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = $"Vehiculos_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
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
                    MessageBox.Show("✅ Datos exportados exitosamente.", "Exportar",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al exportar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void MenuNuevo_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                using (var frmRegistro = new FrmRegistroVehiculo())
                {
                    if (frmRegistro.ShowDialog(this) == DialogResult.OK)
                    {
                        CargarVehiculos("");
                    }
                }
            }
        }
    }
}
