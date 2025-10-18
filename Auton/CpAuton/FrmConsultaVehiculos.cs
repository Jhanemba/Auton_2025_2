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

            btnSesion = new Button // ← QUITAR 'Button' para usar la variable de clase
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

            // Actualizar label
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
                Width = 150
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProcedencia",
                DataPropertyName = "procedencia",
                HeaderText = "Procedencia",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTipoVehiculo",
                DataPropertyName = "tipoVehiculo",
                HeaderText = "Tipo",
                Width = 80
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
                Name = "colKilometraje",
                DataPropertyName = "kilometraje",
                HeaderText = "Km",
                Width = 80,
                DefaultCellStyle = { Format = "N0" }
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPropietario",
                DataPropertyName = "propietario",
                HeaderText = "Propietario",
                Width = 120
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
                    Size = new Size(600, 650),
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
                int lineHeight = 30;

                // Etiqueta de estado edición
                Label lblEstado = new Label
                {
                    Text = "MODO LECTURA",
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    ForeColor = Color.Red,
                    Location = new Point(20, yPos),
                    Size = new Size(500, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlPrincipal.Controls.Add(lblEstado);
                yPos += 30;

                // Marca (Editable)
                Label lblMarca = new Label { Text = "Marca:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtMarca = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculo.marca };
                pnlPrincipal.Controls.Add(lblMarca);
                pnlPrincipal.Controls.Add(txtMarca);
                yPos += lineHeight;

                // Modelo (Editable)
                Label lblModelo = new Label { Text = "Modelo:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtModelo = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculo.modelo };
                pnlPrincipal.Controls.Add(lblModelo);
                pnlPrincipal.Controls.Add(txtModelo);
                yPos += lineHeight;

                // Procedencia (Editable)
                Label lblProcedencia = new Label { Text = "Procedencia:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtProcedencia = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculo.procedencia };
                pnlPrincipal.Controls.Add(lblProcedencia);
                pnlPrincipal.Controls.Add(txtProcedencia);
                yPos += lineHeight;

                // Tipo Vehículo (Editable)
                Label lblTipo = new Label { Text = "Tipo Vehículo:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtTipo = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculo.tipoVehiculo };
                pnlPrincipal.Controls.Add(lblTipo);
                pnlPrincipal.Controls.Add(txtTipo);
                yPos += lineHeight;

                // Año Fabricación (Editable)
                Label lblAnio = new Label { Text = "Año Fabricación:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtAnio = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculoCompleto.anioFabricacion.ToString() };
                pnlPrincipal.Controls.Add(lblAnio);
                pnlPrincipal.Controls.Add(txtAnio);
                yPos += lineHeight;

                // Confiabilidad (Editable)
                Label lblConfiabilidad = new Label { Text = "Confiabilidad:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                ComboBox cboConfiabilidad = new ComboBox { Location = new Point(150, yPos), Size = new Size(300, 23), DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
                cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
                cboConfiabilidad.SelectedItem = vehiculoCompleto.confiabilidad;
                pnlPrincipal.Controls.Add(lblConfiabilidad);
                pnlPrincipal.Controls.Add(cboConfiabilidad);
                yPos += lineHeight;

                // Puntuación (Editable)
                Label lblPuntuacion = new Label { Text = "Puntuación:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtPuntuacion = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculoCompleto.puntuacion.ToString() };
                pnlPrincipal.Controls.Add(lblPuntuacion);
                pnlPrincipal.Controls.Add(txtPuntuacion);
                yPos += lineHeight;

                // Kilometraje (Editable)
                Label lblKilometraje = new Label { Text = "Kilometraje:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtKilometraje = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculoCompleto.kilometraje?.ToString() ?? "" };
                pnlPrincipal.Controls.Add(lblKilometraje);
                pnlPrincipal.Controls.Add(txtKilometraje);
                yPos += lineHeight;

                // Propietario (Editable)
                Label lblPropietario = new Label { Text = "Propietario:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtPropietario = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculoCompleto.propietario ?? "" };
                pnlPrincipal.Controls.Add(lblPropietario);
                pnlPrincipal.Controls.Add(txtPropietario);
                yPos += lineHeight;

                // Motor (Solo lectura)
                Label lblMotor = new Label { Text = "Motor:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtMotor = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculo.motor ?? "" };
                pnlPrincipal.Controls.Add(lblMotor);
                pnlPrincipal.Controls.Add(txtMotor);
                yPos += lineHeight;

                // Combustible (Editable)
                Label lblCombustible = new Label { Text = "Combustible:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtCombustible = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 23), ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculo.combustible ?? "" };
                pnlPrincipal.Controls.Add(lblCombustible);
                pnlPrincipal.Controls.Add(txtCombustible);
                yPos += lineHeight;

                // Observaciones (Editable)
                Label lblObservaciones = new Label { Text = "Observaciones:", Font = new Font("Arial", 9, FontStyle.Bold), Location = new Point(20, yPos), Size = new Size(120, 20), ForeColor = Color.DarkBlue };
                TextBox txtObservaciones = new TextBox { Location = new Point(150, yPos), Size = new Size(300, 80), Multiline = true, ScrollBars = ScrollBars.Vertical, ReadOnly = true, BackColor = Color.WhiteSmoke, Text = vehiculoCompleto.observaciones ?? "" };
                pnlPrincipal.Controls.Add(lblObservaciones);
                pnlPrincipal.Controls.Add(txtObservaciones);
                yPos += 90;

                // Panel inferior con botones
                Panel pnlBotones = new Panel
                {
                    Height = 50,
                    Dock = DockStyle.Bottom,
                    Padding = new Padding(20, 10, 20, 10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Botones Editar y Eliminar (solo si hay usuario autenticado)
                if (Util.usuario != null)
                {
                    Button btnEditar = new Button
                    {
                        Text = "Editar",
                        Size = new Size(100, 35),
                        Location = new Point(10, 5),
                        BackColor = Color.FromArgb(51, 122, 183),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Arial", 9, FontStyle.Bold)
                    };
                    btnEditar.Click += (s, e) =>
                    {
                        if (btnEditar.Text == "Editar")
                        {
                            // Activamos modo edición
                            txtAnio.ReadOnly = false;
                            cboConfiabilidad.Enabled = true;
                            txtPuntuacion.ReadOnly = false;
                            txtKilometraje.ReadOnly = false;
                            txtPropietario.ReadOnly = false;
                            txtMotor.ReadOnly = false;           // ← HABILITADO
                            txtCombustible.ReadOnly = false;     // ← HABILITADO
                            txtObservaciones.ReadOnly = false;

                            // Cambiamos colores
                            txtAnio.BackColor = Color.White;
                            txtPuntuacion.BackColor = Color.White;
                            txtKilometraje.BackColor = Color.White;
                            txtPropietario.BackColor = Color.White;
                            txtMotor.BackColor = Color.White;           // ← HABILITADO
                            txtCombustible.BackColor = Color.White;
                            txtObservaciones.BackColor = Color.White;

                            lblEstado.Text = "MODO EDICIÓN";
                            lblEstado.ForeColor = Color.Green;
                            btnEditar.Text = "Guardar";
                            btnEditar.BackColor = Color.FromArgb(40, 167, 69);
                        }
                        else
                        {
                            // VALIDACIONES
                            if (!int.TryParse(txtAnio.Text, out int anio) || anio < 1900 || anio > DateTime.Now.Year)
                            {
                                MessageBox.Show($"El año debe estar entre 1900 y {DateTime.Now.Year}", "Validación",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            if (cboConfiabilidad.SelectedIndex == -1)
                            {
                                MessageBox.Show("Debe seleccionar un nivel de confiabilidad", "Validación",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            if (!decimal.TryParse(txtPuntuacion.Text, out decimal puntuacion) || puntuacion < 1.0m || puntuacion > 10.0m)
                            {
                                MessageBox.Show("La puntuación debe estar entre 1.0 y 10.0", "Validación",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            if (!string.IsNullOrWhiteSpace(txtKilometraje.Text) && !int.TryParse(txtKilometraje.Text, out int km))
                            {
                                MessageBox.Show("El kilometraje debe ser un número válido", "Validación",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // ACTUALIZAMOS VEHÍCULO
                            try
                            {
                                vehiculoCompleto.anioFabricacion = anio;
                                vehiculoCompleto.confiabilidad = cboConfiabilidad.SelectedItem.ToString();
                                vehiculoCompleto.puntuacion = puntuacion;
                                vehiculoCompleto.kilometraje = string.IsNullOrEmpty(txtKilometraje.Text) ? (int?)null : int.Parse(txtKilometraje.Text);
                                vehiculoCompleto.propietario = string.IsNullOrEmpty(txtPropietario.Text) ? null : txtPropietario.Text;
                                vehiculoCompleto.observaciones = string.IsNullOrEmpty(txtObservaciones.Text) ? null : txtObservaciones.Text;
                                vehiculoCompleto.usuarioRegistro = Util.usuario.usuario;

                                int resultadoModelo = 0;
                                int resultadoVehiculo = 0;

                                // ACTUALIZAMOS MODELO (Motor y Combustible) ← IMPORTANTE
                                // Obtenemos el modelo de forma independiente para evitar ObjectDisposedException
                                var modeloVehiculo = ModeloVehiculoCln.obtenerUno(vehiculoCompleto.idModeloVehiculo);
                                if (modeloVehiculo != null)
                                {
                                    modeloVehiculo.motor = string.IsNullOrEmpty(txtMotor.Text) ? null : txtMotor.Text.Trim();
                                    modeloVehiculo.combustible = string.IsNullOrEmpty(txtCombustible.Text) ? null : txtCombustible.Text.Trim();
                                    modeloVehiculo.usuarioRegistro = Util.usuario.usuario;

                                    // Actualizamos el modelo primero
                                    resultadoModelo = ModeloVehiculoCln.actualizar(modeloVehiculo);
                                }

                                // Ahora actualizamos el vehículo
                                resultadoVehiculo = ConsultaVehiculosCln.actualizar(vehiculoCompleto);

                                // Si se actualizó CUALQUIERA de los dos (modelo o vehículo), es éxito
                                if (resultadoModelo > 0 || resultadoVehiculo > 0)
                                {
                                    // Desactivamos modo edición
                                    txtAnio.ReadOnly = true;
                                    cboConfiabilidad.Enabled = false;
                                    txtPuntuacion.ReadOnly = true;
                                    txtKilometraje.ReadOnly = true;
                                    txtPropietario.ReadOnly = true;
                                    txtMotor.ReadOnly = true;
                                    txtCombustible.ReadOnly = true;
                                    txtObservaciones.ReadOnly = true;

                                    txtAnio.BackColor = Color.WhiteSmoke;
                                    txtPuntuacion.BackColor = Color.WhiteSmoke;
                                    txtKilometraje.BackColor = Color.WhiteSmoke;
                                    txtPropietario.BackColor = Color.WhiteSmoke;
                                    txtMotor.BackColor = Color.WhiteSmoke;
                                    txtCombustible.BackColor = Color.WhiteSmoke;
                                    txtObservaciones.BackColor = Color.WhiteSmoke;

                                    lblEstado.Text = "MODO LECTURA";
                                    lblEstado.ForeColor = Color.Red;
                                    btnEditar.Text = "Editar";
                                    btnEditar.BackColor = Color.FromArgb(51, 122, 183);
                                    MessageBox.Show("Vehículo actualizado exitosamente", "Éxito",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    frmDetalles.Close();
                                    CargarVehiculos("");
                                }
                                else
                                {
                                    MessageBox.Show("No se actualizó ningún registro", "Advertencia",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al guardar: {ex.Message}\n\nStack: {ex.StackTrace}",
                                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                            //if (resultado > 0)
                            //{
                            //    // Desactivar modo edición
                            //    txtAnio.ReadOnly = true;
                            //    cboConfiabilidad.Enabled = false;
                            //    txtPuntuacion.ReadOnly = true;
                            //    txtKilometraje.ReadOnly = true;
                            //    txtPropietario.ReadOnly = true;
                            //    txtMotor.ReadOnly = true;           // ← DESHABILITADO
                            //    txtCombustible.ReadOnly = true;     // ← DESHABILITADO
                            //    txtObservaciones.ReadOnly = true;

                            //    txtAnio.BackColor = Color.WhiteSmoke;
                            //    txtPuntuacion.BackColor = Color.WhiteSmoke;
                            //    txtKilometraje.BackColor = Color.WhiteSmoke;
                            //    txtPropietario.BackColor = Color.WhiteSmoke;
                            //    txtMotor.BackColor = Color.WhiteSmoke;        // ← DESHABILITADO
                            //    txtCombustible.BackColor = Color.WhiteSmoke;
                            //    txtObservaciones.BackColor = Color.WhiteSmoke;

                            //    lblEstado.Text = "MODO LECTURA";
                            //    lblEstado.ForeColor = Color.Red;
                            //    btnEditar.Text = "Editar";
                            //    btnEditar.BackColor = Color.FromArgb(51, 122, 183);
                            //    MessageBox.Show("Vehículo actualizado exitosamente", "Éxito",
                            //                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //    frmDetalles.Close();
                            //    CargarVehiculos("");
                            //}
                        }
                    };

                    Button btnEliminar = new Button
                    {
                        Text = "Eliminar",
                        Size = new Size(100, 35),
                        Location = new Point(120, 5),
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
                    Size = new Size(100, 35),
                    Location = new Point(430, 5),
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

        private void AgregarEtiqueta(Panel panel, string titulo, string valor, ref int yPos, bool esObservacion, object control)
        {
            Label lblTitulo = new Label
            {
                Text = titulo + ":",
                Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(120, 20),
                ForeColor = Color.DarkBlue
            };

            Label lblValor = new Label
            {
                Text = string.IsNullOrEmpty(valor) ? "N/A" : valor,
                Location = new Point(150, yPos),
                Size = new Size(300, 20),
                Font = new Font("Microsoft Sans Serif", 9)
            };
            panel.Controls.Add(lblTitulo);
            panel.Controls.Add(lblValor);
            yPos += 25;
        }

        private void EditarVehiculo(paVehiculoListar_Result vehiculoListado, Form frmDetalles)
        {
            try
            {
                // Obtener datos completos del vehículo
                var vehiculo = VehiculoCln.obtenerUno(vehiculoListado.id);

                // Buscar controles editables en el formulario
                ComboBox cboConfiabilidad = null;
                TextBox txtPuntuacion = null;
                TextBox txtKilometraje = null;
                TextBox txtPropietario = null;
                TextBox txtObservaciones = null;

                foreach (Control ctrl in frmDetalles.Controls[0].Controls) // Panel principal
                {
                    if (ctrl is ComboBox cbo && cbo.Items.Count == 4 && cbo.Items.Contains("Excelente"))
                        cboConfiabilidad = cbo;
                    if (ctrl is TextBox txt)
                    {
                        // Identificar por posición relativa
                        if (txt.Size.Height == 23) // campos de una línea
                        {
                            if (txtPuntuacion == null && txt.Text.Contains("."))
                                txtPuntuacion = txt;
                            else if (txtKilometraje == null && !txt.Text.Contains("."))
                                txtKilometraje = txt;
                            else if (txtPropietario == null && txtKilometraje != null)
                                txtPropietario = txt;
                        }
                        else if (txt.Multiline) // campo de observaciones
                            txtObservaciones = txt;
                    }
                }

                // Si ya están en modo edición, guardar cambios
                if (cboConfiabilidad != null && !cboConfiabilidad.Enabled)
                {
                    // Validar
                    if (cboConfiabilidad.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debe seleccionar un nivel de confiabilidad", "Validación",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!decimal.TryParse(txtPuntuacion.Text, out decimal puntuacion) || puntuacion < 1.0m || puntuacion > 10.0m)
                    {
                        MessageBox.Show("La puntuación debe estar entre 1.0 y 10.0", "Validación",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Guardar cambios
                    vehiculo.confiabilidad = cboConfiabilidad.SelectedItem.ToString();
                    vehiculo.puntuacion = decimal.Parse(txtPuntuacion.Text);
                    vehiculo.kilometraje = string.IsNullOrEmpty(txtKilometraje.Text) ? (int?)null : int.Parse(txtKilometraje.Text);
                    vehiculo.propietario = string.IsNullOrEmpty(txtPropietario.Text) ? null : txtPropietario.Text;
                    vehiculo.observaciones = string.IsNullOrEmpty(txtObservaciones.Text) ? null : txtObservaciones.Text;
                    vehiculo.usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : Environment.UserName;

                    int resultado = VehiculoCln.actualizar(vehiculo);

                    if (resultado > 0)
                    {
                        // Habilitar campos nuevamente
                        cboConfiabilidad.Enabled = false;
                        txtPuntuacion.ReadOnly = true;
                        txtKilometraje.ReadOnly = true;
                        txtPropietario.ReadOnly = true;
                        txtObservaciones.ReadOnly = true;

                        // Cambiar botón
                        foreach (Control ctrl in frmDetalles.Controls)
                        {
                            if (ctrl is Panel pnl && pnl.Dock == DockStyle.Bottom)
                            {
                                foreach (Button btn in pnl.Controls)
                                {
                                    if (btn.Text == "Editar")
                                    {
                                        btn.Text = "Editar";
                                        btn.BackColor = Color.FromArgb(51, 122, 183);
                                    }
                                }
                            }
                        }

                        MessageBox.Show("Vehículo actualizado exitosamente", "Éxito",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmDetalles.Close();
                        CargarVehiculos("");
                    }
                }
                else
                {
                    // Activar modo edición
                    cboConfiabilidad.Enabled = true;
                    txtPuntuacion.ReadOnly = false;
                    txtKilometraje.ReadOnly = false;
                    txtPropietario.ReadOnly = false;
                    txtObservaciones.ReadOnly = false;
                    txtPuntuacion.BackColor = Color.White;
                    txtKilometraje.BackColor = Color.White;
                    txtPropietario.BackColor = Color.White;
                    txtObservaciones.BackColor = Color.White;

                    MessageBox.Show("Formulario en modo edición. Realiza los cambios y haz clic en Editar nuevamente para guardar.",
                                  "Modo Edición", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarVehiculo(paVehiculoListar_Result vehiculo, Form frmDetalles)
        {
            DialogResult resultado = MessageBox.Show($"¿Está seguro que desea eliminar el vehículo '{vehiculo.modelo}'?",
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
                        MessageBox.Show("Vehículo eliminado exitosamente", "Éxito",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el vehículo: {ex.Message}", "Error",
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
                    csv.AppendLine("Marca,Modelo,Procedencia,Tipo,Año,Confiabilidad,Puntuación,Kilometraje,Propietario,Motor,Combustible");

                    foreach (DataGridViewRow row in dgvVehiculos.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            csv.AppendLine($"{row.Cells["colMarca"].Value}," +
                                         $"{row.Cells["colModelo"].Value}," +
                                         $"{row.Cells["colProcedencia"].Value}," +
                                         $"{row.Cells["colTipoVehiculo"].Value}," +
                                         $"{row.Cells["colAnio"].Value}," +
                                         $"{row.Cells["colConfiabilidad"].Value}," +
                                         $"{row.Cells["colPuntuacion"].Value}," +
                                         $"{row.Cells["colKilometraje"].Value}," +
                                         $"{row.Cells["colPropietario"].Value}," +
                                         $"{row.Cells["colMotor"].Value}," +
                                         $"{row.Cells["colCombustible"].Value}");
                        }
                    }

                    System.IO.File.WriteAllText(saveFileDialog.FileName, csv.ToString(), Encoding.UTF8);
                    MessageBox.Show("Datos exportados exitosamente.", "Exportar",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
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

        private void MenuNuevo_Click(object sender, EventArgs e)
        {
            if (ValidarAutenticacion())
            {
                using (var frmRegistro = new FrmRegistroVehiculo())
                {
                    if (frmRegistro.ShowDialog(this) == DialogResult.OK)
                    {
                        // ← AGREGAMOS ESTA LÍNEA: Recargamos la lista después de guardar
                        CargarVehiculos("");
                    }
                }
            }
        }
    }
}
