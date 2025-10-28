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
    public partial class FrmGestionarEmpleados : Form
    {
        private bool modoEdicion = false;
        private int empleadoSeleccionadoId = 0;

        // Controles
        private ComboBox cboCargo;
        private TextBox txtCI;
        private TextBox txtNombres;
        private TextBox txtPrimerApellido;
        private TextBox txtSegundoApellido;
        private TextBox txtDireccion;
        private TextBox txtCelular;
        private DataGridView dgvEmpleados;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnCancelar;
        private Button btnEliminar;
        private TextBox txtBuscar;
        private Label lblTotal;
        private Label lblEstadistica;

        public FrmGestionarEmpleados()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "👥 Gestión de Empleados - Sistema Auton";
            this.Size = new Size(1400, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Panel usuario actual
            Panel pnlUsuarioActual = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(52, 58, 64)
            };

            Label lblUsuarioActual = new Label
            {
                Text = $"👤 Usuario: {Util.usuario.nombreCompleto} | {Util.usuario.cargo}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 12),
                AutoSize = true
            };
            pnlUsuarioActual.Controls.Add(lblUsuarioActual);

            // Panel izquierdo - Formulario
            Panel pnlForm = new Panel
            {
                Location = new Point(10, 60),
                Size = new Size(480, 630),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            int yPos = 15;

            // Título
            Label lblTitulo = new Label
            {
                Text = "📝 DATOS DEL EMPLEADO",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(15, yPos),
                Size = new Size(450, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlForm.Controls.Add(lblTitulo);
            yPos += 50;

            // Cargo
            AgregarLabel(pnlForm, "Cargo:", 15, yPos, 130);
            cboCargo = new ComboBox
            {
                Location = new Point(150, yPos),
                Size = new Size(310, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            pnlForm.Controls.Add(cboCargo);
            yPos += 45;

            // CI
            AgregarLabel(pnlForm, "CI:", 15, yPos, 130);
            txtCI = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 12
            };
            txtCI.KeyPress += TxtSoloNumeros_KeyPress;
            pnlForm.Controls.Add(txtCI);
            yPos += 45;

            // Nombres
            AgregarLabel(pnlForm, "Nombres:", 15, yPos, 130);
            txtNombres = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(310, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 50,
                CharacterCasing = CharacterCasing.Upper
            };
            pnlForm.Controls.Add(txtNombres);
            yPos += 45;

            // Primer Apellido
            AgregarLabel(pnlForm, "Primer Apellido:", 15, yPos, 130);
            txtPrimerApellido = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(310, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 30,
                CharacterCasing = CharacterCasing.Upper
            };
            pnlForm.Controls.Add(txtPrimerApellido);
            yPos += 45;

            // Segundo Apellido
            AgregarLabel(pnlForm, "Segundo Apellido:", 15, yPos, 130);
            txtSegundoApellido = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(310, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 30,
                CharacterCasing = CharacterCasing.Upper
            };
            pnlForm.Controls.Add(txtSegundoApellido);

            Label lblOpcional = new Label
            {
                Text = "(Opcional)",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(150, yPos + 28),
                AutoSize = true
            };
            pnlForm.Controls.Add(lblOpcional);
            yPos += 45;

            // Dirección
            AgregarLabel(pnlForm, "Dirección:", 15, yPos, 130);
            txtDireccion = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(310, 60),
                Font = new Font("Segoe UI", 10),
                MaxLength = 250,
                Multiline = true
            };
            pnlForm.Controls.Add(txtDireccion);
            yPos += 75;

            // Celular
            AgregarLabel(pnlForm, "Celular:", 15, yPos, 130);
            txtCelular = new TextBox
            {
                Location = new Point(150, yPos),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 15
            };
            txtCelular.KeyPress += TxtSoloNumeros_KeyPress;
            pnlForm.Controls.Add(txtCelular);
            yPos += 60;

            // Botones
            btnNuevo = CrearBoton("➕ Nuevo", 15, yPos, 140, Color.FromArgb(40, 167, 69));
            btnNuevo.Click += BtnNuevo_Click;
            pnlForm.Controls.Add(btnNuevo);

            btnGuardar = CrearBoton("💾 Guardar", 170, yPos, 140, Color.FromArgb(0, 123, 255));
            btnGuardar.Click += BtnGuardar_Click;
            btnGuardar.Enabled = false;
            pnlForm.Controls.Add(btnGuardar);

            btnCancelar = CrearBoton("🚫 Cancelar", 325, yPos, 140, Color.FromArgb(108, 117, 125));
            btnCancelar.Click += BtnCancelar_Click;
            btnCancelar.Enabled = false;
            pnlForm.Controls.Add(btnCancelar);

            // Panel derecho - Grid
            Panel pnlGrid = new Panel
            {
                Location = new Point(500, 60),
                Size = new Size(880, 630),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTituloGrid = new Label
            {
                Text = "📊 LISTA DE EMPLEADOS",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(15, 15),
                Size = new Size(850, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlGrid.Controls.Add(lblTituloGrid);

            // Estadísticas
            lblEstadistica = new Label
            {
                Text = "📈 Estadísticas: ...",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                Location = new Point(15, 50),
                Size = new Size(850, 20)
            };
            pnlGrid.Controls.Add(lblEstadistica);

            // Búsqueda
            Label lblBuscar = new Label
            {
                Text = "🔍 Buscar:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 80),
                Size = new Size(80, 25)
            };
            pnlGrid.Controls.Add(lblBuscar);

            txtBuscar = new TextBox
            {
                Location = new Point(100, 80),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10)
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            pnlGrid.Controls.Add(txtBuscar);

            lblTotal = new Label
            {
                Text = "Total: 0 empleados",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(520, 80),
                Size = new Size(300, 25)
            };
            pnlGrid.Controls.Add(lblTotal);

            // DataGridView
            dgvEmpleados = new DataGridView
            {
                Location = new Point(15, 115),
                Size = new Size(850, 420),
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9)
            };

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                DataPropertyName = "id",
                HeaderText = "ID",
                Width = 50
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCI",
                DataPropertyName = "cedulaIdentidad",
                HeaderText = "CI",
                Width = 100
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombreCompleto",
                DataPropertyName = "nombreCompleto",
                HeaderText = "Nombre Completo",
                Width = 280
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCargo",
                DataPropertyName = "cargo",
                HeaderText = "Cargo",
                Width = 180
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCelular",
                DataPropertyName = "celular",
                HeaderText = "Celular",
                Width = 120
            });

            dgvEmpleados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTieneUsuario",
                DataPropertyName = "tieneUsuario",
                HeaderText = "Usuario",
                Width = 80
            });

            dgvEmpleados.SelectionChanged += DgvEmpleados_SelectionChanged;
            dgvEmpleados.CellFormatting += DgvEmpleados_CellFormatting;
            pnlGrid.Controls.Add(dgvEmpleados);

            // Botones de acción
            btnEliminar = CrearBoton("🗑️ Eliminar Empleado", 15, 550, 200, Color.FromArgb(220, 53, 69));
            btnEliminar.Click += BtnEliminar_Click;
            btnEliminar.Enabled = false;
            pnlGrid.Controls.Add(btnEliminar);

            Button btnVerDetalle = CrearBoton("📋 Ver Detalle Completo", 230, 550, 200, Color.FromArgb(23, 162, 184));
            btnVerDetalle.Click += BtnVerDetalle_Click;
            pnlGrid.Controls.Add(btnVerDetalle);

            Button btnExportar = CrearBoton("📊 Exportar a Excel", 445, 550, 200, Color.FromArgb(255, 193, 7));
            btnExportar.ForeColor = Color.Black;
            btnExportar.Click += BtnExportar_Click;
            pnlGrid.Controls.Add(btnExportar);

            // Botón cerrar
            Button btnCerrar = new Button
            {
                Text = "❌ Cerrar",
                Size = new Size(150, 45),
                Location = new Point(1220, 700),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();

            this.Controls.Add(pnlUsuarioActual);
            this.Controls.Add(pnlForm);
            this.Controls.Add(pnlGrid);
            this.Controls.Add(btnCerrar);
        }

        private Button CrearBoton(string texto, int x, int y, int width, Color color)
        {
            var btn = new Button
            {
                Text = texto,
                Size = new Size(width, 40),
                Location = new Point(x, y),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
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

        private void FrmGestionarEmpleados_Load(object sender, EventArgs e)
        {
            CargarCargos();
            CargarEmpleados();
            ActualizarEstadisticas();
        }

        private void CargarCargos()
        {
            try
            {
                var cargos = CargoCln.listar();
                cboCargo.DataSource = cargos;
                cboCargo.DisplayMember = "descripcion";
                cboCargo.ValueMember = "id";
                cboCargo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cargos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEmpleados()
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    var empleados = context.Empleado
                        .Where(emp => emp.estado != -1)
                        .ToList() // Ejecutar consulta primero
                        .Select(emp => new
                        {
                            id = emp.id,
                            cedulaIdentidad = emp.cedulaIdentidad,
                            nombreCompleto = emp.nombres + " " +
                                           (emp.primerApellido != null ? emp.primerApellido : "") + " " +
                                           (emp.segundoApellido != null ? emp.segundoApellido : ""),
                            cargo = emp.Cargo.descripcion,
                            celular = emp.celular,
                            tieneUsuario = context.Usuario.Any(u => u.idEmpleado == emp.id && u.estado != -1) ? "SÍ" : "NO"
                        })
                        .OrderBy(emp => emp.nombreCompleto)
                        .ToList();

                    dgvEmpleados.DataSource = empleados;
                    lblTotal.Text = "Total: " + empleados.Count.ToString() + " empleados";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarEstadisticas()
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    int totalEmpleados = context.Empleado.Count(e => e.estado != -1);
                    int conUsuario = context.Usuario.Count(u => u.estado != -1);
                    int sinUsuario = totalEmpleados - conUsuario;

                    var cargoMasComun = context.Empleado
                        .Where(e => e.estado != -1)
                        .GroupBy(e => e.Cargo.descripcion)
                        .OrderByDescending(g => g.Count())
                        .Select(g => new { Cargo = g.Key, Cantidad = g.Count() })
                        .FirstOrDefault();

                    lblEstadistica.Text = $"📈 Total: {totalEmpleados} empleados | " +
                                        $"✅ Con usuario: {conUsuario} | " +
                                        $"⚠️ Sin usuario: {sinUsuario} | " +
                                        $"🏆 Cargo más común: {cargoMasComun?.Cargo ?? "N/A"} ({cargoMasComun?.Cantidad ?? 0})";
                }
            }
            catch (Exception ex)
            {
                lblEstadistica.Text = "Error al cargar estadísticas";
            }
        }

        private void DgvEmpleados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEmpleados.Columns[e.ColumnIndex].Name == "colTieneUsuario")
            {
                if (e.Value != null && e.Value.ToString() == "SÍ")
                {
                    e.CellStyle.BackColor = Color.FromArgb(212, 237, 218);
                    e.CellStyle.ForeColor = Color.FromArgb(21, 87, 36);
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 243, 205);
                    e.CellStyle.ForeColor = Color.FromArgb(133, 100, 4);
                }
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            empleadoSeleccionadoId = 0;
            LimpiarFormulario();

            btnGuardar.Text = "💾 Guardar";
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            txtCI.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                Empleado empleado;

                if (modoEdicion)
                {
                    empleado = EmpleadoCln.obtenerUno(empleadoSeleccionadoId);
                    if (empleado == null)
                    {
                        MessageBox.Show("❌ No se encontró el empleado", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    empleado = new Empleado
                    {
                        estado = 1,
                        fechaRegistro = DateTime.Now
                    };
                }

                empleado.idCargo = (int)cboCargo.SelectedValue;
                empleado.cedulaIdentidad = txtCI.Text.Trim();
                empleado.nombres = txtNombres.Text.Trim();
                empleado.primerApellido = txtPrimerApellido.Text.Trim();
                empleado.segundoApellido = string.IsNullOrWhiteSpace(txtSegundoApellido.Text) ?
                    null : txtSegundoApellido.Text.Trim();
                empleado.direccion = txtDireccion.Text.Trim();
                empleado.celular = long.Parse(txtCelular.Text.Trim());
                empleado.usuarioRegistro = Util.usuario.usuario;

                int resultado;
                string mensaje;

                if (modoEdicion)
                {
                    resultado = EmpleadoCln.actualizar(empleado);
                    mensaje = resultado > 0 ?
                        "✅ Empleado actualizado exitosamente" :
                        "❌ No se pudo actualizar el empleado";
                }
                else
                {
                    resultado = EmpleadoCln.insertar(empleado);
                    mensaje = resultado > 0 ?
                        "✅ Empleado registrado exitosamente" :
                        "❌ No se pudo registrar el empleado";
                }

                if (resultado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarEmpleados();
                    ActualizarEstadisticas();
                    btnGuardar.Enabled = false;
                    btnCancelar.Enabled = false;
                }
                else
                {
                    MessageBox.Show(mensaje, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            modoEdicion = false;
            empleadoSeleccionadoId = 0;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            dgvEmpleados.ClearSelection();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow == null) return;

            int idEmpleado = Convert.ToInt32(dgvEmpleados.CurrentRow.Cells["colId"].Value);
            string nombreEmpleado = dgvEmpleados.CurrentRow.Cells["colNombreCompleto"].Value.ToString();

            // Verificar si tiene usuario asignado
            using (var context = new AutonEntities())
            {
                var tieneUsuario = context.Usuario.Any(u => u.idEmpleado == idEmpleado && u.estado != -1);

                if (tieneUsuario)
                {
                    MessageBox.Show(
                        $"⚠️ No se puede eliminar el empleado '{nombreEmpleado}'\n\n" +
                        $"Tiene un usuario asignado en el sistema.\n\n" +
                        $"Primero elimine el usuario asociado.",
                        "Empleado con Usuario",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            DialogResult resultado = MessageBox.Show(
                $"⚠️ ¿Está seguro que desea eliminar al empleado?\n\n" +
                $"Nombre: {nombreEmpleado}\n\n" +
                $"Esta acción no se puede deshacer.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    int resultadoOp = EmpleadoCln.eliminar(idEmpleado, Util.usuario.usuario);

                    if (resultadoOp > 0)
                    {
                        MessageBox.Show("✅ Empleado eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarEmpleados();
                        ActualizarEstadisticas();
                        LimpiarFormulario();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow == null)
            {
                MessageBox.Show("⚠️ Seleccione un empleado", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idEmpleado = Convert.ToInt32(dgvEmpleados.CurrentRow.Cells["colId"].Value);

            using (var frmDetalle = new FrmDetalleEmpleado(idEmpleado))
            {
                frmDetalle.ShowDialog(this);
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "🚧 Funcionalidad de exportación a Excel\n\n" +
                "Esta función estará disponible próximamente.\n\n" +
                "Exportará la lista completa de empleados con todos sus datos.",
                "En Desarrollo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void DgvEmpleados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow != null)
            {
                btnEliminar.Enabled = true;
                int idEmpleado = Convert.ToInt32(dgvEmpleados.CurrentRow.Cells["colId"].Value);
                CargarDatosEmpleado(idEmpleado);
            }
            else
            {
                btnEliminar.Enabled = false;
            }
        }

        private void CargarDatosEmpleado(int id)
        {
            try
            {
                var empleado = EmpleadoCln.obtenerUno(id);
                if (empleado == null) return;

                modoEdicion = true;
                empleadoSeleccionadoId = id;

                cboCargo.SelectedValue = empleado.idCargo;
                txtCI.Text = empleado.cedulaIdentidad;
                txtNombres.Text = empleado.nombres;
                txtPrimerApellido.Text = empleado.primerApellido;
                txtSegundoApellido.Text = empleado.segundoApellido;
                txtDireccion.Text = empleado.direccion;
                txtCelular.Text = empleado.celular.ToString();

                btnGuardar.Text = "💾 Actualizar";
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string filtro = txtBuscar.Text.Trim().ToLower();

                using (var context = new AutonEntities())
                {
                    var empleados = context.Empleado
                        .Where(emp => emp.estado != -1)
                        .ToList() // Ejecutar consulta primero
                        .Where(emp =>
                               emp.cedulaIdentidad.Contains(filtro) ||
                               emp.nombres.ToLower().Contains(filtro) ||
                               (emp.primerApellido != null && emp.primerApellido.ToLower().Contains(filtro)) ||
                               (emp.segundoApellido != null && emp.segundoApellido.ToLower().Contains(filtro)) ||
                               emp.Cargo.descripcion.ToLower().Contains(filtro))
                        .Select(emp => new
                        {
                            id = emp.id,
                            cedulaIdentidad = emp.cedulaIdentidad,
                            nombreCompleto = emp.nombres + " " +
                                           (emp.primerApellido != null ? emp.primerApellido : "") + " " +
                                           (emp.segundoApellido != null ? emp.segundoApellido : ""),
                            cargo = emp.Cargo.descripcion,
                            celular = emp.celular,
                            tieneUsuario = context.Usuario.Any(u => u.idEmpleado == emp.id && u.estado != -1) ? "SÍ" : "NO"
                        })
                        .OrderBy(emp => emp.nombreCompleto)
                        .ToList();

                    dgvEmpleados.DataSource = empleados;
                    lblTotal.Text = "Total: " + empleados.Count.ToString() + " empleados";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en búsqueda: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
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
                        e.estado != -1 &&
                        e.id != empleadoSeleccionadoId);

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
                MessageBox.Show($"Error al validar CI: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void LimpiarFormulario()
        {
            cboCargo.SelectedIndex = -1;
            txtCI.Clear();
            txtNombres.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            txtDireccion.Clear();
            txtCelular.Clear();
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    this.ClientSize = new System.Drawing.Size(1400, 750);
        //    this.Name = "FrmGestionarEmpleados";
        //    this.Load += new System.EventHandler(this.FrmGestionarEmpleados_Load);
        //    this.ResumeLayout(false);
        //}
    }
}
