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
    public partial class FrmGestionarUsuarios : Form
    {
        private bool modoEdicion = false;
        private int usuarioSeleccionadoId = 0;

        // Controles del formulario
        private ComboBox cboEmpleado;
        private TextBox txtUsuario;
        private TextBox txtClave;
        private TextBox txtConfirmarClave;
        private CheckBox chkEsAdministrador;
        private CheckBox chkMostrarClave;
        private DataGridView dgvUsuarios;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnCancelar;
        private Button btnEliminar;
        private Button btnResetearClave;
        private TextBox txtBuscar;
        private Label lblTotal;

        public FrmGestionarUsuarios()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "👥 Gestión de Usuarios - Sistema Auton";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Panel usuario actual
            Panel pnlUsuarioActual = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(52, 58, 64),
                BorderStyle = BorderStyle.FixedSingle
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
                Size = new Size(450, 630),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            int yPos = 15;

            // Título
            Label lblTitulo = new Label
            {
                Text = "📝 DATOS DEL USUARIO",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(15, yPos),
                Size = new Size(420, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlForm.Controls.Add(lblTitulo);
            yPos += 50;

            // Empleado
            AgregarLabel(pnlForm, "Empleado:", 15, yPos, 120);
            cboEmpleado = new ComboBox
            {
                Location = new Point(140, yPos),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            pnlForm.Controls.Add(cboEmpleado);

            // Botón agregar empleado
            Button btnAgregarEmpleado = new Button
            {
                Text = "+",
                Location = new Point(395, yPos),
                Size = new Size(35, 25),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnAgregarEmpleado.FlatAppearance.BorderSize = 0;
            btnAgregarEmpleado.Click += BtnAgregarEmpleado_Click;
            pnlForm.Controls.Add(btnAgregarEmpleado);

            yPos += 40;

            // Usuario
            AgregarLabel(pnlForm, "Usuario:", 15, yPos, 120);
            txtUsuario = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 20,
                CharacterCasing = CharacterCasing.Lower
            };
            pnlForm.Controls.Add(txtUsuario);
            yPos += 40;

            // Clave
            AgregarLabel(pnlForm, "Contraseña:", 15, yPos, 120);
            txtClave = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true,
                MaxLength = 50
            };
            pnlForm.Controls.Add(txtClave);
            yPos += 40;

            // Confirmar Clave
            AgregarLabel(pnlForm, "Confirmar:", 15, yPos, 120);
            txtConfirmarClave = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true,
                MaxLength = 50
            };
            pnlForm.Controls.Add(txtConfirmarClave);
            yPos += 40;

            // Mostrar contraseña
            chkMostrarClave = new CheckBox
            {
                Text = "👁️ Mostrar contraseña",
                Location = new Point(140, yPos),
                Font = new Font("Segoe UI", 9),
                AutoSize = true
            };
            chkMostrarClave.CheckedChanged += (s, e) =>
            {
                txtClave.UseSystemPasswordChar = !chkMostrarClave.Checked;
                txtConfirmarClave.UseSystemPasswordChar = !chkMostrarClave.Checked;
            };
            pnlForm.Controls.Add(chkMostrarClave);
            yPos += 45;

            // Es Administrador
            chkEsAdministrador = new CheckBox
            {
                Text = "🔑 Usuario Administrador",
                Location = new Point(140, yPos),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69),
                AutoSize = true
            };
            pnlForm.Controls.Add(chkEsAdministrador);
            yPos += 50;

            // Panel de advertencia
            Panel pnlAdvertencia = new Panel
            {
                Location = new Point(15, yPos),
                Size = new Size(415, 80),
                BackColor = Color.FromArgb(255, 243, 205),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblAdvertencia = new Label
            {
                Text = "⚠️ IMPORTANTE:\n" +
                       "• Los administradores pueden gestionar usuarios\n" +
                       "• Los usuarios normales solo consultan vehículos\n" +
                       "• La contraseña se encripta automáticamente",
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(5, 5),
                Size = new Size(400, 70)
            };
            pnlAdvertencia.Controls.Add(lblAdvertencia);
            pnlForm.Controls.Add(pnlAdvertencia);
            yPos += 95;

            // Botones
            btnNuevo = new Button
            {
                Text = "➕ Nuevo",
                Size = new Size(130, 40),
                Location = new Point(15, yPos),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += BtnNuevo_Click;
            pnlForm.Controls.Add(btnNuevo);

            btnGuardar = new Button
            {
                Text = "💾 Guardar",
                Size = new Size(130, 40),
                Location = new Point(160, yPos),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;
            pnlForm.Controls.Add(btnGuardar);

            btnCancelar = new Button
            {
                Text = "🚫 Cancelar",
                Size = new Size(130, 40),
                Location = new Point(305, yPos),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;
            pnlForm.Controls.Add(btnCancelar);

            // Panel derecho - Grid
            Panel pnlGrid = new Panel
            {
                Location = new Point(470, 60),
                Size = new Size(810, 630),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTituloGrid = new Label
            {
                Text = "📊 LISTA DE USUARIOS",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(15, 15),
                Size = new Size(780, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlGrid.Controls.Add(lblTituloGrid);

            // Búsqueda
            Label lblBuscar = new Label
            {
                Text = "🔍 Buscar:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 55),
                Size = new Size(80, 25)
            };
            pnlGrid.Controls.Add(lblBuscar);

            txtBuscar = new TextBox
            {
                Location = new Point(100, 55),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10)
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            pnlGrid.Controls.Add(txtBuscar);

            lblTotal = new Label
            {
                Text = "Total: 0 usuarios",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(520, 55),
                Size = new Size(250, 25)
            };
            pnlGrid.Controls.Add(lblTotal);

            // DataGridView
            dgvUsuarios = new DataGridView
            {
                Location = new Point(15, 90),
                Size = new Size(780, 440),
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9)
            };

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                DataPropertyName = "id",
                HeaderText = "ID",
                Width = 60
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEmpleado",
                DataPropertyName = "nombreCompleto",
                HeaderText = "Empleado",
                Width = 250
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCargo",
                DataPropertyName = "cargo",
                HeaderText = "Cargo",
                Width = 150
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colUsuario",
                DataPropertyName = "usuario",
                HeaderText = "Usuario",
                Width = 150
            });

            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colAdmin",
                DataPropertyName = "esAdministrador",
                HeaderText = "Admin",
                Width = 70
            });

            dgvUsuarios.SelectionChanged += DgvUsuarios_SelectionChanged;
            pnlGrid.Controls.Add(dgvUsuarios);

            // Botones de acción
            btnResetearClave = new Button
            {
                Text = "🔑 Resetear Contraseña",
                Size = new Size(180, 40),
                Location = new Point(15, 545),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnResetearClave.FlatAppearance.BorderSize = 0;
            btnResetearClave.Click += BtnResetearClave_Click;
            pnlGrid.Controls.Add(btnResetearClave);

            btnEliminar = new Button
            {
                Text = "🗑️ Eliminar Usuario",
                Size = new Size(180, 40),
                Location = new Point(210, 545),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;
            pnlGrid.Controls.Add(btnEliminar);

            // Botón cerrar
            Button btnCerrar = new Button
            {
                Text = "❌ Cerrar",
                Size = new Size(150, 45),
                Location = new Point(1120, 700),
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

        private void FrmGestionarUsuarios_Load(object sender, EventArgs e)
        {
            // Validar autenticación
            if (Util.usuario == null || !Util.EsAdministrador())
            {
                MessageBox.Show("⚠️ Solo los administradores pueden gestionar usuarios.",
                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            CargarEmpleados();
            CargarUsuarios();
        }

        private void CargarEmpleados()
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    if (modoEdicion)
                    {
                        // En modo edición: cargar TODOS los empleados
                        var todosEmpleados = context.Empleado
                            .Where(e => e.estado != -1)
                            .ToList() // IMPORTANTE: Ejecutar consulta primero
                            .Select(e => new EmpleadoDto
                            {
                                id = e.id,
                                nombreCompleto = e.nombres + " " +
                                               (e.primerApellido ?? "") + " " +
                                               (e.segundoApellido ?? "")
                            })
                            .OrderBy(e => e.nombreCompleto)
                            .ToList();

                        cboEmpleado.DataSource = todosEmpleados;
                    }
                    else
                    {
                        // En modo nuevo: cargar solo empleados SIN usuario
                        var empleadosConUsuario = context.Usuario
                            .Where(u => u.estado != -1)
                            .Select(u => u.idEmpleado)
                            .ToList();

                        var empleadosDisponibles = context.Empleado
                            .Where(e => e.estado != -1 && !empleadosConUsuario.Contains(e.id))
                            .ToList() // IMPORTANTE: Ejecutar consulta primero
                            .Select(e => new EmpleadoDto
                            {
                                id = e.id,
                                nombreCompleto = e.nombres + " " +
                                               (e.primerApellido ?? "") + " " +
                                               (e.segundoApellido ?? "")
                            })
                            .OrderBy(e => e.nombreCompleto)
                            .ToList();

                        cboEmpleado.DataSource = empleadosDisponibles;

                        // ADVERTENCIA si no hay empleados disponibles
                        if (empleadosDisponibles.Count == 0)
                        {
                            MessageBox.Show(
                                "⚠️ No hay empleados disponibles para crear usuarios.\n\n" +
                                "Todos los empleados ya tienen usuarios asignados.\n\n" +
                                "💡 Soluciones:\n" +
                                "   1. Haga clic en el botón '+' para crear un nuevo empleado\n" +
                                "   2. Eliminar un usuario existente para liberar un empleado",
                                "Sin Empleados Disponibles",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    }

                    cboEmpleado.DisplayMember = "nombreCompleto";
                    cboEmpleado.ValueMember = "id";
                    cboEmpleado.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar empleados:\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clase auxiliar para el DTO
        public class EmpleadoDto
        {
            public int id { get; set; }
            public string nombreCompleto { get; set; }
        }

        private void CargarUsuarios()
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    var usuarios = context.Usuario
                        .Where(u => u.estado != -1)
                        .Select(u => new
                        {
                            u.id,
                            nombreCompleto = u.Empleado.nombres + " " +
                                           (u.Empleado.primerApellido ?? "") + " " +
                                           (u.Empleado.segundoApellido ?? ""),
                            cargo = u.Empleado.Cargo.descripcion,
                            usuario = u.usuario1,
                            u.esAdministrador
                        })
                        .OrderBy(u => u.nombreCompleto)
                        .ToList();

                    dgvUsuarios.DataSource = usuarios;
                    lblTotal.Text = $"Total: {usuarios.Count} usuarios";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            usuarioSeleccionadoId = 0;
            LimpiarFormulario();
            CargarEmpleados();

            btnGuardar.Text = "💾 Guardar";
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            cboEmpleado.Enabled = true;
            txtUsuario.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                Usuario usuario;

                if (modoEdicion)
                {
                    usuario = UsuarioCln.obtenerUno(usuarioSeleccionadoId);
                    if (usuario == null)
                    {
                        MessageBox.Show("❌ No se encontró el usuario", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    usuario = new Usuario
                    {
                        estado = 1,
                        fechaRegistro = DateTime.Now
                    };
                }

                usuario.idEmpleado = (int)cboEmpleado.SelectedValue;
                usuario.usuario1 = txtUsuario.Text.Trim().ToLower();

                // Solo actualizar clave si se ingresó una nueva
                if (!string.IsNullOrWhiteSpace(txtClave.Text))
                {
                    usuario.clave = Util.Encrypt(txtClave.Text);
                }

                usuario.esAdministrador = chkEsAdministrador.Checked;
                usuario.usuarioRegistro = Util.usuario.usuario;

                int resultado;
                string mensaje;

                if (modoEdicion)
                {
                    resultado = UsuarioCln.actualizar(usuario);
                    mensaje = resultado > 0 ?
                        "✅ Usuario actualizado exitosamente" :
                        "❌ No se pudo actualizar el usuario";
                }
                else
                {
                    resultado = UsuarioCln.insertar(usuario);
                    mensaje = resultado > 0 ?
                        "✅ Usuario registrado exitosamente" :
                        "❌ No se pudo registrar el usuario";
                }

                if (resultado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarUsuarios();
                    CargarEmpleados();
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
            usuarioSeleccionadoId = 0;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            dgvUsuarios.ClearSelection();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            int idUsuario = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            string nombreUsuario = dgvUsuarios.CurrentRow.Cells["colUsuario"].Value.ToString();

            // No permitir eliminar el usuario actual
            if (idUsuario == Util.usuario.id)
            {
                MessageBox.Show("⚠️ No puede eliminar su propio usuario",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado = MessageBox.Show(
                $"⚠️ ¿Está seguro que desea eliminar el usuario '{nombreUsuario}'?\n\n" +
                $"Esta acción no se puede deshacer.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    int resultadoOp = UsuarioCln.eliminar(idUsuario, Util.usuario.usuario);

                    if (resultadoOp > 0)
                    {
                        MessageBox.Show("✅ Usuario eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                        CargarEmpleados();
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

        private void BtnResetearClave_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            int idUsuario = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
            string nombreUsuario = dgvUsuarios.CurrentRow.Cells["colUsuario"].Value.ToString();

            DialogResult resultado = MessageBox.Show(
                $"¿Resetear la contraseña del usuario '{nombreUsuario}'?\n\n" +
                $"Nueva contraseña: 123456\n\n" +
                $"El usuario deberá cambiarla en su primer inicio de sesión.",
                "Confirmar Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    var usuario = UsuarioCln.obtenerUno(idUsuario);
                    usuario.clave = Util.Encrypt("123456");
                    usuario.usuarioRegistro = Util.usuario.usuario;

                    int resultadoOp = UsuarioCln.actualizar(usuario);

                    if (resultadoOp > 0)
                    {
                        MessageBox.Show("✅ Contraseña reseteada a: 123456", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow != null)
            {
                btnEliminar.Enabled = true;
                btnResetearClave.Enabled = true;

                int idUsuario = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["colId"].Value);
                CargarDatosUsuario(idUsuario);
            }
            else
            {
                btnEliminar.Enabled = false;
                btnResetearClave.Enabled = false;
            }
        }

        private void CargarDatosUsuario(int id)
        {
            try
            {
                var usuario = UsuarioCln.obtenerUno(id);
                if (usuario == null) return;

                modoEdicion = true;
                usuarioSeleccionadoId = id;

                // Cargar TODOS los empleados para edición
                using (var context = new AutonEntities())
                {
                    var empleados = context.Empleado
                        .Where(e => e.estado != -1)
                        .Select(e => new
                        {
                            e.id,
                            nombreCompleto = e.nombres + " " +
                                           (e.primerApellido ?? "") + " " +
                                           (e.segundoApellido ?? "")
                        })
                        .OrderBy(e => e.nombreCompleto)
                        .ToList();

                    cboEmpleado.DataSource = empleados;
                    cboEmpleado.DisplayMember = "nombreCompleto";
                    cboEmpleado.ValueMember = "id";
                }

                cboEmpleado.SelectedValue = usuario.idEmpleado;
                cboEmpleado.Enabled = false; // No permitir cambiar empleado
                txtUsuario.Text = usuario.usuario1;
                txtClave.Clear();
                txtConfirmarClave.Clear();
                chkEsAdministrador.Checked = usuario.esAdministrador;

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
                    var usuarios = context.Usuario
                        .Where(u => u.estado != -1 &&
                               (u.usuario1.ToLower().Contains(filtro) ||
                                u.Empleado.nombres.ToLower().Contains(filtro) ||
                                u.Empleado.primerApellido.ToLower().Contains(filtro) ||
                                u.Empleado.Cargo.descripcion.ToLower().Contains(filtro)))
                        .Select(u => new
                        {
                            u.id,
                            nombreCompleto = u.Empleado.nombres + " " +
                                           (u.Empleado.primerApellido ?? "") + " " +
                                           (u.Empleado.segundoApellido ?? ""),
                            cargo = u.Empleado.Cargo.descripcion,
                            usuario = u.usuario1,
                            u.esAdministrador
                        })
                        .OrderBy(u => u.nombreCompleto)
                        .ToList();

                    dgvUsuarios.DataSource = usuarios;
                    lblTotal.Text = $"Total: {usuarios.Count} usuarios";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en búsqueda: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Validar()
        {
            if (cboEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar un empleado", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboEmpleado.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar el nombre de usuario", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return false;
            }

            if (txtUsuario.Text.Length < 4)
            {
                MessageBox.Show("⚠️ El usuario debe tener al menos 4 caracteres",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return false;
            }

            // Validar clave solo si es nuevo o si se ingresó una
            if (!modoEdicion || !string.IsNullOrWhiteSpace(txtClave.Text))
            {
                if (string.IsNullOrWhiteSpace(txtClave.Text))
                {
                    MessageBox.Show("⚠️ Debe ingresar la contraseña", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Focus();
                    return false;
                }

                if (txtClave.Text.Length < 6)
                {
                    MessageBox.Show("⚠️ La contraseña debe tener al menos 6 caracteres",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Focus();
                    return false;
                }

                if (txtClave.Text != txtConfirmarClave.Text)
                {
                    MessageBox.Show("⚠️ Las contraseñas no coinciden", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmarClave.Focus();
                    return false;
                }
            }

            // CORRECCIÓN: Verificar si el usuario ya existe (solo ACTIVOS)
            if (!modoEdicion)
            {
                try
                {
                    using (var context = new AutonEntities())
                    {
                        var usuarioExistente = context.Usuario
                            .FirstOrDefault(u => u.usuario1.ToLower() == txtUsuario.Text.Trim().ToLower()
                                              && u.estado != -1); // SOLO usuarios activos

                        if (usuarioExistente != null)
                        {
                            MessageBox.Show($"⚠️ El usuario '{txtUsuario.Text}' ya existe y está activo.\n\n" +
                                          "Por favor, elija otro nombre de usuario.",
                                "Usuario Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtUsuario.Focus();
                            return false;
                        }

                        // ADVERTENCIA: Usuario eliminado
                        var usuarioEliminado = context.Usuario
                            .FirstOrDefault(u => u.usuario1.ToLower() == txtUsuario.Text.Trim().ToLower()
                                              && u.estado == -1);

                        if (usuarioEliminado != null)
                        {
                            DialogResult resultado = MessageBox.Show(
                                $"⚠️ El usuario '{txtUsuario.Text}' existe pero fue eliminado anteriormente.\n\n" +
                                $"¿Desea REACTIVAR este usuario en lugar de crear uno nuevo?\n\n" +
                                $"• SÍ: Reactivar el usuario existente\n" +
                                $"• NO: Continuar con el error (no recomendado)\n" +
                                $"• CANCELAR: Elegir otro nombre de usuario",
                                "Usuario Eliminado Encontrado",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question);

                            if (resultado == DialogResult.Yes)
                            {
                                // Reactivar usuario
                                ReactivarUsuario(usuarioEliminado.id);
                                return false; // No continuar con el guardado normal
                            }
                            else if (resultado == DialogResult.Cancel)
                            {
                                txtUsuario.Focus();
                                return false;
                            }
                            // Si elige NO, continúa y dejará que falle (no recomendado)
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al validar usuario: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void ReactivarUsuario(int idUsuario)
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    var usuario = context.Usuario.Find(idUsuario);
                    if (usuario == null) return;

                    // Reactivar
                    usuario.estado = 1;
                    usuario.idEmpleado = (int)cboEmpleado.SelectedValue;
                    usuario.esAdministrador = chkEsAdministrador.Checked;
                    usuario.usuarioRegistro = Util.usuario.usuario;
                    usuario.fechaRegistro = DateTime.Now;

                    // Solo actualizar clave si se ingresó una nueva
                    if (!string.IsNullOrWhiteSpace(txtClave.Text))
                    {
                        usuario.clave = Util.Encrypt(txtClave.Text);
                    }

                    context.SaveChanges();

                    MessageBox.Show(
                        $"✅ Usuario '{usuario.usuario1}' reactivado exitosamente\n\n" +
                        $"El usuario se ha vinculado al empleado seleccionado.",
                        "Usuario Reactivado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LimpiarFormulario();
                    CargarUsuarios();
                    CargarEmpleados();
                    btnGuardar.Enabled = false;
                    btnCancelar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reactivar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnAgregarEmpleado_Click(object sender, EventArgs e)
        {
            using (var frmEmpleado = new FrmNuevoEmpleado())
            {
                if (frmEmpleado.ShowDialog(this) == DialogResult.OK)
                {
                    CargarEmpleados();
                    // Seleccionar el empleado recién creado
                    if (frmEmpleado.NuevoIdEmpleado > 0)
                    {
                        cboEmpleado.SelectedValue = frmEmpleado.NuevoIdEmpleado;
                    }
                }
            }
        }

        private void LimpiarFormulario()
        {
            cboEmpleado.SelectedIndex = -1;
            txtUsuario.Clear();
            txtClave.Clear();
            txtConfirmarClave.Clear();
            chkEsAdministrador.Checked = false;
            chkMostrarClave.Checked = false;
            cboEmpleado.Enabled = true;
        }
    }
}
