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
            btnNuevo.Visible = Util.usuario != null;
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
            btnNuevo.Visible = Util.usuario != null;
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

                // ===== FORMULARIO PRINCIPAL =====
                Form frmDetalles = new Form
                {
                    Text = $"Detalles del Vehículo - {vehiculo.marca} {vehiculo.modelo}",
                    Size = new Size(1200, 800),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    Font = new Font("Segoe UI", 10F),
                    AutoScroll = true,
                    ShowInTaskbar = false
                };

                Panel pnlPrincipal = new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(1184, 700),
                    Padding = new Padding(20),
                    BackColor = Color.White,
                    AutoScroll = true
                };

                int yPos = 15;
                int lineHeight = 45;
                int labelWidth = 150;
                int ctrlWidth = 250;
                int colLeftX = 170;
                int colRightX = 650;

                // ===== TÍTULO Y ESTADO =====
                Label lblTitulo = new Label
                {
                    Text = "📋 CONSULTA/EDICIÓN DE VEHÍCULO",
                    Font = new Font("Segoe UI", 15, FontStyle.Bold),
                    ForeColor = Color.FromArgb(51, 122, 183),
                    Location = new Point(20, yPos),
                    Size = new Size(1140, 32),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlPrincipal.Controls.Add(lblTitulo);
                yPos += 40;

                Label lblEstado = new Label
                {
                    Text = "🔒 MODO LECTURA",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.Red,
                    Location = new Point(20, yPos),
                    Size = new Size(1140, 25),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(255, 230, 230),
                    Padding = new Padding(5)
                };
                pnlPrincipal.Controls.Add(lblEstado);
                yPos += 35;

                // ==================== COLUMNA IZQUIERDA ====================
                AgregarSeccion(pnlPrincipal, "📋 IDENTIFICACIÓN DEL MODELO", 20, yPos, 540);
                int yPosLeft = yPos + 38;

                // Marca (solo lectura)
                AgregarLabelForm(pnlPrincipal, "Marca:", 20, yPosLeft, labelWidth);
                TextBox txtMarca = new TextBox
                {
                    Location = new Point(colLeftX, yPosLeft),
                    Size = new Size(ctrlWidth, 28),
                    Font = new Font("Segoe UI", 10F),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke,
                    Text = vehiculo.marca
                };
                pnlPrincipal.Controls.Add(txtMarca);
                yPosLeft += lineHeight;

                // Modelo (solo lectura)
                AgregarLabelForm(pnlPrincipal, "Modelo:", 20, yPosLeft, labelWidth);
                TextBox txtModelo = new TextBox
                {
                    Location = new Point(colLeftX, yPosLeft),
                    Size = new Size(ctrlWidth, 28),
                    Font = new Font("Segoe UI", 10F),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke,
                    Text = vehiculo.modelo
                };
                pnlPrincipal.Controls.Add(txtModelo);
                yPosLeft += lineHeight;

                // Tipo (solo lectura)
                AgregarLabelForm(pnlPrincipal, "Tipo:", 20, yPosLeft, labelWidth);
                TextBox txtTipo = new TextBox
                {
                    Location = new Point(colLeftX, yPosLeft),
                    Size = new Size(ctrlWidth, 28),
                    Font = new Font("Segoe UI", 10F),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke,
                    Text = vehiculo.tipoVehiculo
                };
                pnlPrincipal.Controls.Add(txtTipo);
                yPosLeft += lineHeight;

                // Nro. de Chasis (solo lectura)
                AgregarLabelForm(pnlPrincipal, "Nro. de Chasis:", 20, yPosLeft, labelWidth);
                TextBox txtChasis = new TextBox
                {
                    Location = new Point(colLeftX, yPosLeft),
                    Size = new Size(ctrlWidth, 28),
                    Font = new Font("Segoe UI", 10F),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke,
                    Text = vehiculoCompleto.nroChasis
                };
                pnlPrincipal.Controls.Add(txtChasis);
                yPosLeft += lineHeight;

                // Año de Fabricación (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Año Fabricación:", 20, yPosLeft, labelWidth);
                NumericUpDown nudAnio = new NumericUpDown
                {
                    Location = new Point(colLeftX, yPosLeft),
                    Size = new Size(120, 28),
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 1900,
                    Maximum = DateTime.Now.Year,
                    Value = vehiculoCompleto.anioFabricacion,
                    TextAlign = HorizontalAlignment.Center,
                    Enabled = false
                };
                pnlPrincipal.Controls.Add(nudAnio);
                yPosLeft += lineHeight;

                // ==================== COLUMNA DERECHA ====================
                AgregarSeccion(pnlPrincipal, "⚙️ CARACTERÍSTICAS ESPECÍFICAS", 590, yPos, 560);
                int yPosRight = yPos + 38;

                // Generación (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Generación:", 590, yPosRight, labelWidth);
                TextBox txtGeneracion = new TextBox
                {
                    Location = new Point(colRightX + 90, yPosRight),
                    Size = new Size(ctrlWidth, 28),
                    Font = new Font("Segoe UI", 10F),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke,
                    Text = vehiculoCompleto.generacion
                };
                pnlPrincipal.Controls.Add(txtGeneracion);
                yPosRight += lineHeight;

                // País de Fabricación (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "País Fabricación:", 590, yPosRight, labelWidth);
                ComboBox cboPaisFabricacion = new ComboBox
                {
                    Location = new Point(colRightX + 90, yPosRight),
                    Size = new Size(ctrlWidth, 28),
                    DropDownStyle = ComboBoxStyle.DropDown,
                    Font = new Font("Segoe UI", 10F),
                    Enabled = false
                };
                cboPaisFabricacion.Items.AddRange(new[] { "Japón", "Alemania", "Estados Unidos", "Corea del Sur",
                    "China", "India", "Brasil", "México", "España", "Francia", "Italia", "Reino Unido" });
                cboPaisFabricacion.Text = vehiculoCompleto.paisFabricacion;
                pnlPrincipal.Controls.Add(cboPaisFabricacion);
                yPosRight += lineHeight;

                // Transmisión (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Transmisión:", 590, yPosRight, labelWidth);
                ComboBox cboTransmision = new ComboBox
                {
                    Location = new Point(colRightX + 90, yPosRight),
                    Size = new Size(ctrlWidth, 28),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 10F),
                    Enabled = false
                };
                cboTransmision.Items.AddRange(new[] { "Manual", "Automática", "CVT", "Semiautomática", "Doble Embrague" });
                cboTransmision.Text = vehiculoCompleto.transmision;
                pnlPrincipal.Controls.Add(cboTransmision);
                yPosRight += lineHeight;

                // Tiene Turbo (EDITABLE)
                CheckBox chkTieneTurbo = new CheckBox
                {
                    Text = "🚀 Tiene Turbo",
                    Location = new Point(colRightX + 90, yPosRight + 3),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(220, 53, 69),
                    Checked = vehiculoCompleto.tieneTurbo,
                    Enabled = false
                };
                pnlPrincipal.Controls.Add(chkTieneTurbo);
                yPosRight += lineHeight;

                // Motor (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Motor:", 590, yPosRight, labelWidth);
                TextBox txtMotor = new TextBox
                {
                    Location = new Point(colRightX + 90, yPosRight),
                    Size = new Size(ctrlWidth, 28),
                    Font = new Font("Segoe UI", 10F),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke,
                    Text = vehiculoCompleto.motor
                };
                pnlPrincipal.Controls.Add(txtMotor);
                yPosRight += lineHeight;

                // Combustible (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Combustible:", 590, yPosRight, labelWidth);
                ComboBox cboCombustible = new ComboBox
                {
                    Location = new Point(colRightX + 90, yPosRight),
                    Size = new Size(ctrlWidth, 28),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 10F),
                    Enabled = false
                };
                cboCombustible.Items.AddRange(new[] { "Gasolina", "Diesel", "Híbrido", "Eléctrico", "GLP", "GNC" });
                cboCombustible.Text = vehiculoCompleto.combustible;
                pnlPrincipal.Controls.Add(cboCombustible);
                yPosRight += lineHeight;

                // ==================== SEGURIDAD ====================
                yPos = Math.Max(yPosLeft, yPosRight) + 15;
                AgregarSeccion(pnlPrincipal, "🛡️ SEGURIDAD", 20, yPos, 540);
                yPosLeft = yPos + 38;

                // Airbags (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Airbags:", 20, yPosLeft, labelWidth);
                NumericUpDown nudAirbags = new NumericUpDown
                {
                    Location = new Point(colLeftX, yPosLeft),
                    Size = new Size(100, 28),
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 0,
                    Maximum = 12,
                    Value = vehiculoCompleto.nroAirbags,
                    TextAlign = HorizontalAlignment.Center,
                    Enabled = false
                };
                pnlPrincipal.Controls.Add(nudAirbags);
                yPosLeft += lineHeight;

                // Organización (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Organización:", 20, yPosLeft, labelWidth);
                ComboBox cboOrganizacion = new ComboBox
                {
                    Location = new Point(colLeftX, yPosLeft),
                    Size = new Size(ctrlWidth, 28),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 10F),
                    Enabled = false
                };
                cboOrganizacion.Items.AddRange(new[] { "Latin NCAP", "Euro NCAP", "IIHS", "NHTSA", "ANCAP", "Sin calificación" });
                cboOrganizacion.Text = vehiculoCompleto.organizacionSeguridad;
                pnlPrincipal.Controls.Add(cboOrganizacion);
                yPosLeft += lineHeight;

                // Estrellas (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Estrellas:", 20, yPosLeft, labelWidth);
                NumericUpDown nudEstrellas = new NumericUpDown
                {
                    Location = new Point(colLeftX, yPosLeft),
                    Size = new Size(100, 28),
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 0,
                    Maximum = 5,
                    Value = vehiculoCompleto.seguridadChoques,
                    TextAlign = HorizontalAlignment.Center,
                    Enabled = false
                };
                pnlPrincipal.Controls.Add(nudEstrellas);
                yPosLeft += lineHeight;

                // ==================== EVALUACIÓN ====================
                AgregarSeccion(pnlPrincipal, "⭐ EVALUACIÓN", 590, yPos, 560);
                yPosRight = yPos + 38;

                // Confiabilidad (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Confiabilidad:", 590, yPosRight, labelWidth);
                ComboBox cboConfiabilidad = new ComboBox
                {
                    Location = new Point(colRightX + 90, yPosRight),
                    Size = new Size(ctrlWidth, 28),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 10F),
                    Enabled = false
                };
                cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
                cboConfiabilidad.Text = vehiculoCompleto.confiabilidad;
                pnlPrincipal.Controls.Add(cboConfiabilidad);
                yPosRight += lineHeight;

                // Puntuación (EDITABLE)
                AgregarLabelForm(pnlPrincipal, "Puntuación:", 590, yPosRight, labelWidth);
                NumericUpDown nudPuntuacion = new NumericUpDown
                {
                    Location = new Point(colRightX + 90, yPosRight),
                    Size = new Size(120, 28),
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 1.0m,
                    Maximum = 10.0m,
                    DecimalPlaces = 1,
                    Increment = 0.5m,
                    Value = vehiculoCompleto.puntuacion,
                    TextAlign = HorizontalAlignment.Center,
                    Enabled = false
                };
                pnlPrincipal.Controls.Add(nudPuntuacion);
                yPosRight += lineHeight;

                // ==================== OBSERVACIONES ====================
                yPos = Math.Max(yPosLeft, yPosRight) + 15;
                AgregarLabelForm(pnlPrincipal, "Observaciones:", 20, yPos, labelWidth);
                TextBox txtObservaciones = new TextBox
                {
                    Location = new Point(colLeftX, yPos),
                    Size = new Size(990, 65),
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Segoe UI", 10F),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke,
                    Text = vehiculoCompleto.observaciones ?? ""
                };
                pnlPrincipal.Controls.Add(txtObservaciones);

                // ==================== PANEL DE BOTONES ====================
                Panel pnlBotones = new Panel
                {
                    Location = new Point(0, 700),
                    Size = new Size(1184, 110),
                    BackColor = Color.FromArgb(248, 249, 250),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Botón Editar/Guardar (solo si hay usuario autenticado)
                if (Util.usuario != null)
                {
                    Button btnEditar = new Button
                    {
                        Text = "✏️ Editar",
                        Size = new Size(180, 45),
                        Location = new Point(350, 12),
                        BackColor = Color.FromArgb(51, 122, 183),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        Cursor = Cursors.Hand
                    };
                    btnEditar.FlatAppearance.BorderSize = 0;

                    btnEditar.Click += (s, e) =>
                    {
                        if (btnEditar.Text == "✏️ Editar")
                        {
                            // ACTIVAR MODO EDICIÓN
                            nudAnio.Enabled = true;
                            txtGeneracion.ReadOnly = false;
                            txtGeneracion.BackColor = Color.White;
                            cboPaisFabricacion.Enabled = true;
                            cboTransmision.Enabled = true;
                            chkTieneTurbo.Enabled = true;
                            txtMotor.ReadOnly = false;
                            txtMotor.BackColor = Color.White;
                            cboCombustible.Enabled = true;
                            nudAirbags.Enabled = true;
                            cboOrganizacion.Enabled = true;
                            nudEstrellas.Enabled = true;
                            cboConfiabilidad.Enabled = true;
                            nudPuntuacion.Enabled = true;
                            txtObservaciones.ReadOnly = false;
                            txtObservaciones.BackColor = Color.White;

                            lblEstado.Text = "✏️ MODO EDICIÓN - Modifique los campos necesarios";
                            lblEstado.ForeColor = Color.Green;
                            lblEstado.BackColor = Color.FromArgb(230, 255, 230);
                            btnEditar.Text = "💾 Guardar";
                            btnEditar.BackColor = Color.FromArgb(40, 167, 69);
                        }
                        else
                        {
                            // GUARDAR CAMBIOS
                            if (!ValidarEdicion(txtGeneracion.Text, cboPaisFabricacion.SelectedIndex, cboConfiabilidad.SelectedIndex))
                                return;

                            try
                            {
                                vehiculoCompleto.anioFabricacion = (int)nudAnio.Value;
                                vehiculoCompleto.generacion = txtGeneracion.Text.Trim();
                                vehiculoCompleto.paisFabricacion = cboPaisFabricacion.Text.Trim();
                                vehiculoCompleto.transmision = cboTransmision.Text;
                                vehiculoCompleto.tieneTurbo = chkTieneTurbo.Checked;
                                vehiculoCompleto.motor = txtMotor.Text.Trim();
                                vehiculoCompleto.combustible = cboCombustible.Text;
                                vehiculoCompleto.nroAirbags = (int)nudAirbags.Value;
                                vehiculoCompleto.organizacionSeguridad = cboOrganizacion.Text;
                                vehiculoCompleto.seguridadChoques = (int)nudEstrellas.Value;
                                vehiculoCompleto.confiabilidad = cboConfiabilidad.Text;
                                vehiculoCompleto.puntuacion = nudPuntuacion.Value;
                                vehiculoCompleto.observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();
                                vehiculoCompleto.usuarioRegistro = Util.usuario.usuario;

                                int resultado = VehiculoCln.actualizar(vehiculoCompleto);

                                if (resultado > 0)
                                {
                                    // DESACTIVAR MODO EDICIÓN
                                    nudAnio.Enabled = false;
                                    txtGeneracion.ReadOnly = true;
                                    txtGeneracion.BackColor = Color.WhiteSmoke;
                                    cboPaisFabricacion.Enabled = false;
                                    cboTransmision.Enabled = false;
                                    chkTieneTurbo.Enabled = false;
                                    txtMotor.ReadOnly = true;
                                    txtMotor.BackColor = Color.WhiteSmoke;
                                    cboCombustible.Enabled = false;
                                    nudAirbags.Enabled = false;
                                    cboOrganizacion.Enabled = false;
                                    nudEstrellas.Enabled = false;
                                    cboConfiabilidad.Enabled = false;
                                    nudPuntuacion.Enabled = false;
                                    txtObservaciones.ReadOnly = true;
                                    txtObservaciones.BackColor = Color.WhiteSmoke;

                                    lblEstado.Text = "🔒 MODO LECTURA";
                                    lblEstado.ForeColor = Color.Red;
                                    lblEstado.BackColor = Color.FromArgb(255, 230, 230);
                                    btnEditar.Text = "✏️ Editar";
                                    btnEditar.BackColor = Color.FromArgb(51, 122, 183);

                                    MessageBox.Show("✅ Vehículo actualizado exitosamente",
                                                  "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    frmDetalles.Close();
                                    CargarVehiculos("");
                                }
                                else
                                {
                                    MessageBox.Show("❌ No se pudo actualizar el vehículo",
                                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"❌ Error al guardar: {ex.Message}",
                                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    Button btnEliminar = new Button
                    {
                        Text = "🗑️ Eliminar",
                        Size = new Size(180, 45),
                        Location = new Point(550, 12),
                        BackColor = Color.FromArgb(217, 83, 79),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        Cursor = Cursors.Hand
                    };
                    btnEliminar.FlatAppearance.BorderSize = 0;
                    btnEliminar.Click += (s, e) => EliminarVehiculo(vehiculo, frmDetalles);

                    pnlBotones.Controls.Add(btnEditar);
                    pnlBotones.Controls.Add(btnEliminar);
                }

                Button btnCerrar = new Button
                {
                    Text = "❌ Cerrar",
                    Size = new Size(180, 45),
                    Location = new Point(750, 12),
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    DialogResult = DialogResult.OK
                };
                btnCerrar.FlatAppearance.BorderSize = 0;

                pnlBotones.Controls.Add(btnCerrar);
                frmDetalles.Controls.Add(pnlBotones);
                frmDetalles.Controls.Add(pnlPrincipal);
                frmDetalles.AcceptButton = btnCerrar;

                frmDetalles.ShowDialog(this);
            }
        }

        private void AgregarSeccion(Panel panel, string titulo, int x, int yPos, int width)
        {
            Panel pnlSeccion = new Panel
            {
                Location = new Point(x, yPos),
                Size = new Size(width, 32),
                BackColor = Color.FromArgb(52, 58, 64)
            };

            Label lblSeccion = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 7),
                AutoSize = true
            };
            pnlSeccion.Controls.Add(lblSeccion);
            panel.Controls.Add(pnlSeccion);
        }

        private void AgregarLabelForm(Panel panel, string texto, int x, int yPos, int width)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(x, yPos + 3),
                Size = new Size(width, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(33, 37, 41)
            };
            panel.Controls.Add(lbl);
        }

        private bool ValidarEdicion(string generacion, int paisIndex, int confiabilidadIndex)
        {
            if (string.IsNullOrWhiteSpace(generacion))
            {
                MessageBox.Show("⚠️ La generación es obligatoria", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (paisIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el país de fabricación", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (confiabilidadIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el nivel de confiabilidad", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void EliminarVehiculo(paVehiculoListar_Result vehiculo, Form frmDetalles)
        {
            DialogResult resultado = MessageBox.Show(
                $"⚠️ ¿Está seguro que desea eliminar este vehículo?\n\n" +
                $"Modelo: {vehiculo.modelo}\n" +
                $"Generación: {vehiculo.generacion}\n" +
                $"País: {vehiculo.paisFabricacion}\n\n" +
                $"Esta acción no se puede deshacer.",
                "Confirmar Eliminación",
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
            ActualizarInterfazSesion();
        }
        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) btnBuscar.PerformClick();
        }
    }
}
