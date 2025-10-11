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

            Button btnSesion = new Button
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
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel pnl)
                {
                    foreach (Control child in pnl.Controls)
                    {
                        if (child is Button btn && btn.Text.Contains("Sesión"))
                        {
                            btn.Text = Util.usuario != null ? "Cerrar Sesión" : "Iniciar Sesión";
                            btn.BackColor = Util.usuario != null ? Color.FromArgb(217, 83, 79) : Color.FromArgb(51, 122, 183);
                        }
                        if (child is Label lbl && lbl.Text.Contains("Usuario"))
                        {
                            lbl.Text = Util.usuario != null ? $"Usuario: {Util.usuario.nombreCompleto}" : "Usuario: No autenticado";
                        }
                    }
                }
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
                    Size = new Size(550, 650),
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

                // Información del vehículo
                AgregarEtiqueta(pnlPrincipal, "Marca", vehiculo.marca, ref yPos, false, null);
                AgregarEtiqueta(pnlPrincipal, "Modelo", vehiculo.modelo, ref yPos, false, null);
                AgregarEtiqueta(pnlPrincipal, "Procedencia", vehiculo.procedencia, ref yPos, false, null);
                AgregarEtiqueta(pnlPrincipal, "Tipo", vehiculo.tipoVehiculo, ref yPos, false, null);
                AgregarEtiqueta(pnlPrincipal, "Año", vehiculo.anioFabricacion.ToString(), ref yPos, false, null);

                // Confiabilidad (Editable)
                Label lblConfiabilidad = new Label
                {
                    Text = "Confiabilidad:",
                    Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold),
                    Location = new Point(20, yPos),
                    Size = new Size(120, 20),
                    ForeColor = Color.DarkBlue
                };
                ComboBox cboConfiabilidad = new ComboBox
                {
                    Location = new Point(150, yPos),
                    Size = new Size(300, 23),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Enabled = false
                };
                cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
                cboConfiabilidad.SelectedItem = vehiculoCompleto.confiabilidad;
                pnlPrincipal.Controls.Add(lblConfiabilidad);
                pnlPrincipal.Controls.Add(cboConfiabilidad);
                yPos += 25;

                // Puntuación (Editable)
                Label lblPuntuacion = new Label
                {
                    Text = "Puntuación:",
                    Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold),
                    Location = new Point(20, yPos),
                    Size = new Size(120, 20),
                    ForeColor = Color.DarkBlue
                };
                TextBox txtPuntuacion = new TextBox
                {
                    Location = new Point(150, yPos),
                    Size = new Size(300, 23),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke
                };
                txtPuntuacion.Text = vehiculoCompleto.puntuacion.ToString();
                pnlPrincipal.Controls.Add(lblPuntuacion);
                pnlPrincipal.Controls.Add(txtPuntuacion);
                yPos += 25;

                // Kilometraje (Editable)
                Label lblKilometraje = new Label
                {
                    Text = "Kilometraje:",
                    Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold),
                    Location = new Point(20, yPos),
                    Size = new Size(120, 20),
                    ForeColor = Color.DarkBlue
                };
                TextBox txtKilometraje = new TextBox
                {
                    Location = new Point(150, yPos),
                    Size = new Size(300, 23),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke
                };
                txtKilometraje.Text = vehiculoCompleto.kilometraje != null ? vehiculoCompleto.kilometraje.ToString() : "";
                pnlPrincipal.Controls.Add(lblKilometraje);
                pnlPrincipal.Controls.Add(txtKilometraje);
                yPos += 25;

                // Propietario (Editable)
                Label lblPropietario = new Label
                {
                    Text = "Propietario:",
                    Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold),
                    Location = new Point(20, yPos),
                    Size = new Size(120, 20),
                    ForeColor = Color.DarkBlue
                };
                TextBox txtPropietario = new TextBox
                {
                    Location = new Point(150, yPos),
                    Size = new Size(300, 23),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke
                };
                txtPropietario.Text = vehiculoCompleto.propietario ?? "";
                pnlPrincipal.Controls.Add(lblPropietario);
                pnlPrincipal.Controls.Add(txtPropietario);
                yPos += 25;

                // Observaciones (Editable)
                Label lblObservaciones = new Label
                {
                    Text = "Observaciones:",
                    Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold),
                    Location = new Point(20, yPos),
                    Size = new Size(120, 20),
                    ForeColor = Color.DarkBlue
                };
                TextBox txtObservaciones = new TextBox
                {
                    Location = new Point(150, yPos),
                    Size = new Size(300, 80),
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke
                };
                txtObservaciones.Text = vehiculoCompleto.observaciones ?? "";
                pnlPrincipal.Controls.Add(lblObservaciones);
                pnlPrincipal.Controls.Add(txtObservaciones);
                yPos += 90;

                // Información adicional (Solo lectura)
                AgregarEtiqueta(pnlPrincipal, "Motor", vehiculo.motor, ref yPos, false, null);
                AgregarEtiqueta(pnlPrincipal, "Combustible", vehiculo.combustible, ref yPos, false, null);
                AgregarEtiqueta(pnlPrincipal, "Registrado por", vehiculo.usuarioRegistro, ref yPos, false, null);
                AgregarEtiqueta(pnlPrincipal, "Fecha Registro", vehiculo.fechaRegistro.ToString("dd/MM/yyyy HH:mm"), ref yPos, false, null);

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
                            cboConfiabilidad.Enabled = true;
                            txtPuntuacion.ReadOnly = false;
                            txtKilometraje.ReadOnly = false;
                            txtPropietario.ReadOnly = false;
                            txtObservaciones.ReadOnly = false;
                            txtPuntuacion.BackColor = Color.White;
                            txtKilometraje.BackColor = Color.White;
                            txtPropietario.BackColor = Color.White;
                            txtObservaciones.BackColor = Color.White;
                            btnEditar.Text = "Guardar";
                            btnEditar.BackColor = Color.FromArgb(40, 167, 69);
                        }
                        else
                        {
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

                            vehiculoCompleto.confiabilidad = cboConfiabilidad.SelectedItem.ToString();
                            vehiculoCompleto.puntuacion = decimal.Parse(txtPuntuacion.Text);
                            vehiculoCompleto.kilometraje = string.IsNullOrEmpty(txtKilometraje.Text) ? (int?)null : int.Parse(txtKilometraje.Text);
                            vehiculoCompleto.propietario = string.IsNullOrEmpty(txtPropietario.Text) ? null : txtPropietario.Text;
                            vehiculoCompleto.observaciones = string.IsNullOrEmpty(txtObservaciones.Text) ? null : txtObservaciones.Text;
                            vehiculoCompleto.usuarioRegistro = Util.usuario.usuario;

                            int resultado = VehiculoCln.actualizar(vehiculoCompleto);

                            if (resultado > 0)
                            {
                                cboConfiabilidad.Enabled = false;
                                txtPuntuacion.ReadOnly = true;
                                txtKilometraje.ReadOnly = true;
                                txtPropietario.ReadOnly = true;
                                txtObservaciones.ReadOnly = true;
                                txtPuntuacion.BackColor = Color.WhiteSmoke;
                                txtKilometraje.BackColor = Color.WhiteSmoke;
                                txtPropietario.BackColor = Color.WhiteSmoke;
                                txtObservaciones.BackColor = Color.WhiteSmoke;
                                btnEditar.Text = "Editar";
                                btnEditar.BackColor = Color.FromArgb(51, 122, 183);
                                MessageBox.Show("Vehículo actualizado exitosamente", "Éxito",
                                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                                frmDetalles.Close();
                                CargarVehiculos("");
                            }
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
    }
}
