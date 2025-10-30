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
    public partial class FrmGestionarVehiculos : Form
    {
        private bool modoEdicion = false;
        private int vehiculoSeleccionadoId = 0;

        public FrmGestionarVehiculos()
        {
            InitializeComponent();
        }

        private void FrmGestionarVehiculos_Load(object sender, EventArgs e)
        {
            // Validar que el usuario esté autenticado
            if (Util.usuario == null)
            {
                MessageBox.Show("⚠️ Debe iniciar sesión para gestionar vehículos.",
                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            ConfigurarFormulario();
            ConfigurarDataGridView();
            CargarVehiculos("");
            CargarCombos();

            // CAMBIO: Iniciar en modo edición habilitado
            HabilitarControles(true);

            // Deshabilitar solo el botón Guardar hasta que se haga click en Nuevo o se seleccione un registro
            var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
            var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
            var btnEliminar = this.Controls.Find("btnEliminar", true)[0] as Button;

            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void ConfigurarFormulario()
        {
            this.Text = "🔧 Gestión de Vehículos - Sistema Auton";
            this.Size = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Panel superior - Información de usuario
            Panel pnlUsuario = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(52, 58, 64),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblUsuario = new Label
            {
                Text = $"👤 Usuario: {Util.usuario.nombreCompleto} | {Util.usuario.cargo}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 12),
                AutoSize = true
            };
            pnlUsuario.Controls.Add(lblUsuario);

            // Panel izquierdo - Formulario de datos
            Panel pnlFormulario = new Panel
            {
                Location = new Point(10, 60),
                Size = new Size(450, 650-50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            CrearFormularioDatos(pnlFormulario);

            // Panel derecho - Grid y búsqueda
            Panel pnlGrid = new Panel
            {
                Location = new Point(470, 60),
                Size = new Size(910, 650-50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            CrearPanelGrid(pnlGrid);

            // Panel inferior - Botones principales (usar Dock bottom para que quede fuera del área scrolleable)
            Panel pnlBotones = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            CrearBotonesPrincipales(pnlBotones);

            // Añadir controles (pnlBotones al final) y asegurar que quede al frente
            this.Controls.Add(pnlUsuario);
            this.Controls.Add(pnlFormulario);
            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlBotones);
            pnlBotones.BringToFront();
        }

        private void CrearFormularioDatos(Panel panel)
        {
            int yPos = 15;
            int labelWidth = 140;
            int controlWidth = 270;

            // Título
            Label lblTitulo = new Label
            {
                Text = "📋 DATOS DEL VEHÍCULO",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(15, yPos),
                Size = new Size(420, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panel.Controls.Add(lblTitulo);
            yPos += 45;

            // Nro de Chasis
            AgregarLabel(panel, "Nro. Chasis:", 15, yPos, labelWidth);
            TextBox txtChasis = new TextBox
            {
                Name = "txtChasis",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 17
            };
            panel.Controls.Add(txtChasis);
            yPos += 35;

            // Marca
            AgregarLabel(panel, "Marca:", 15, yPos, labelWidth);
            ComboBox cboMarca = new ComboBox
            {
                Name = "cboMarca",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboMarca.SelectedIndexChanged += CboMarca_SelectedIndexChanged;
            panel.Controls.Add(cboMarca);
            yPos += 35;

            // Modelo
            AgregarLabel(panel, "Modelo:", 15, yPos, labelWidth);
            ComboBox cboModelo = new ComboBox
            {
                Name = "cboModelo",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            panel.Controls.Add(cboModelo);
            yPos += 35;

            // Generación
            AgregarLabel(panel, "Generación:", 15, yPos, labelWidth);
            TextBox txtGeneracion = new TextBox
            {
                Name = "txtGeneracion",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 50
            };
            panel.Controls.Add(txtGeneracion);
            yPos += 35;

            // País Fabricación
            AgregarLabel(panel, "País Fabricación:", 15, yPos, labelWidth);
            ComboBox cboPais = new ComboBox
            {
                Name = "cboPais",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 10)
            };
            cboPais.Items.AddRange(new[] { "Japón", "Alemania", "Estados Unidos", "Corea del Sur",
                "China", "India", "Brasil", "México", "España", "Francia", "Italia", "Reino Unido" });
            panel.Controls.Add(cboPais);
            yPos += 35;

            // Año Fabricación
            AgregarLabel(panel, "Año Fabricación:", 15, yPos, labelWidth);
            NumericUpDown nudAnio = new NumericUpDown
            {
                Name = "nudAnio",
                Location = new Point(160, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year
            };
            panel.Controls.Add(nudAnio);
            yPos += 35;

            // Transmisión
            AgregarLabel(panel, "Transmisión:", 15, yPos, labelWidth);
            ComboBox cboTransmision = new ComboBox
            {
                Name = "cboTransmision",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboTransmision.Items.AddRange(new[] { "Manual", "Automática", "CVT", "Semiautomática", "Doble Embrague" });
            panel.Controls.Add(cboTransmision);
            yPos += 35;

            // Tiene Turbo
            CheckBox chkTurbo = new CheckBox
            {
                Name = "chkTurbo",
                Text = "🚀 Tiene Turbo",
                Location = new Point(160, yPos),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69),
                AutoSize = true
            };
            panel.Controls.Add(chkTurbo);
            yPos += 35;

            // Motor
            AgregarLabel(panel, "Motor:", 15, yPos, labelWidth);
            TextBox txtMotor = new TextBox
            {
                Name = "txtMotor",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10),
                MaxLength = 50
            };
            panel.Controls.Add(txtMotor);
            yPos += 35;

            // Combustible
            AgregarLabel(panel, "Combustible:", 15, yPos, labelWidth);
            ComboBox cboCombustible = new ComboBox
            {
                Name = "cboCombustible",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboCombustible.Items.AddRange(new[] { "Gasolina", "Diesel", "Híbrido", "Eléctrico", "GLP", "GNC" });
            panel.Controls.Add(cboCombustible);
            yPos += 35;

            // Nro Airbags
            AgregarLabel(panel, "Nro. Airbags:", 15, yPos, labelWidth);
            NumericUpDown nudAirbags = new NumericUpDown
            {
                Name = "nudAirbags",
                Location = new Point(160, yPos),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 0,
                Maximum = 12,
                Value = 2
            };
            panel.Controls.Add(nudAirbags);
            yPos += 35;

            // Seguridad Choques
            AgregarLabel(panel, "Estrellas Seguridad:", 15, yPos, labelWidth);
            NumericUpDown nudEstrellas = new NumericUpDown
            {
                Name = "nudEstrellas",
                Location = new Point(160, yPos),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 0,
                Maximum = 5,
                Value = 0
            };
            panel.Controls.Add(nudEstrellas);
            yPos += 35;

            // Organización Seguridad
            AgregarLabel(panel, "Organización:", 15, yPos, labelWidth);
            ComboBox cboOrganizacion = new ComboBox
            {
                Name = "cboOrganizacion",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboOrganizacion.Items.AddRange(new[] { "Latin NCAP", "Euro NCAP", "IIHS", "NHTSA", "ANCAP", "Sin calificación" });
            panel.Controls.Add(cboOrganizacion);
            yPos += 35;

            // Confiabilidad
            AgregarLabel(panel, "Confiabilidad:", 15, yPos, labelWidth);
            ComboBox cboConfiabilidad = new ComboBox
            {
                Name = "cboConfiabilidad",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
            panel.Controls.Add(cboConfiabilidad);
            yPos += 35;

            // Puntuación
            AgregarLabel(panel, "Puntuación:", 15, yPos, labelWidth);
            NumericUpDown nudPuntuacion = new NumericUpDown
            {
                Name = "nudPuntuacion",
                Location = new Point(160, yPos),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 1.0m,
                Maximum = 10.0m,
                DecimalPlaces = 1,
                Increment = 0.5m,
                Value = 5.0m
            };
            panel.Controls.Add(nudPuntuacion);
            yPos += 35;

            // Observaciones
            AgregarLabel(panel, "Observaciones:", 15, yPos, labelWidth);
            yPos += 25;
            TextBox txtObservaciones = new TextBox
            {
                Name = "txtObservaciones",
                Location = new Point(15, yPos),
                Size = new Size(415, 80+100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10),
                MaxLength = 500
            };
            panel.Controls.Add(txtObservaciones);
            txtObservaciones.BringToFront();
        }

        private void CrearPanelGrid(Panel panel)
        {
            // Título
            Label lblTitulo = new Label
            {
                Text = "📊 LISTA DE VEHÍCULOS",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(15, 15),
                Size = new Size(880, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panel.Controls.Add(lblTitulo);

            // Búsqueda
            Label lblBuscar = new Label
            {
                Text = "🔍 Buscar:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 55),
                Size = new Size(80, 25)
            };
            panel.Controls.Add(lblBuscar);

            TextBox txtBuscar = new TextBox
            {
                Name = "txtBuscar",
                Location = new Point(100, 55),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10)
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            panel.Controls.Add(txtBuscar);

            // Label Total
            Label lblTotal = new Label
            {
                Name = "lblTotal",
                Text = "Total: 0 vehículos",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(520, 55),
                Size = new Size(300, 25)
            };
            panel.Controls.Add(lblTotal);

            // DataGridView
            DataGridView dgvVehiculos = new DataGridView
            {
                Name = "dgvVehiculos",
                Location = new Point(15, 90),
                Size = new Size(880, 540),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9)
            };
            dgvVehiculos.SelectionChanged += DgvVehiculos_SelectionChanged;
            panel.Controls.Add(dgvVehiculos);
        }

        private void CrearBotonesPrincipales(Panel panel)
        {
            // Botón Nuevo
            Button btnNuevo = new Button
            {
                Name = "btnNuevo",
                Text = "➕ Nuevo",
                Size = new Size(150, 45),
                Location = new Point(20, 8),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += BtnNuevo_Click;
            panel.Controls.Add(btnNuevo);
            //btnNuevo.BringToFront();

            // Botón Guardar
            Button btnGuardar = new Button
            {
                Name = "btnGuardar",
                Text = "💾 Guardar",
                Size = new Size(150, 45),
                Location = new Point(190, 8),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;
            panel.Controls.Add(btnGuardar);

            // Botón Cancelar
            Button btnCancelar = new Button
            {
                Name = "btnCancelar",
                Text = "🚫 Cancelar",
                Size = new Size(150, 45),
                Location = new Point(360, 8),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;
            panel.Controls.Add(btnCancelar);

            // Botón Eliminar
            Button btnEliminar = new Button
            {
                Name = "btnEliminar",
                Text = "🗑️ Eliminar",
                Size = new Size(150, 45),
                Location = new Point(530, 8),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;
            panel.Controls.Add(btnEliminar);

            // Botón Cerrar
            Button btnCerrar = new Button
            {
                Text = "❌ Cerrar",
                Size = new Size(150, 45),
                Location = new Point(1200-70, 8),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();
            panel.Controls.Add(btnCerrar);
        }

        private void ConfigurarDataGridView()
        {
            var dgv = this.Controls.Find("dgvVehiculos", true)[0] as DataGridView;

            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", DataPropertyName = "id", HeaderText = "ID", Width = 50, Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMarca", DataPropertyName = "marca", HeaderText = "Marca", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colModelo", DataPropertyName = "modelo", HeaderText = "Modelo", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGeneracion", DataPropertyName = "generacion", HeaderText = "Generación", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAnio", DataPropertyName = "anioFabricacion", HeaderText = "Año", Width = 60 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTransmision", DataPropertyName = "transmision", HeaderText = "Transmisión", Width = 100 });
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colTurbo", DataPropertyName = "tieneTurbo", HeaderText = "Turbo", Width = 60 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colConfiabilidad", DataPropertyName = "confiabilidad", HeaderText = "Confiabilidad", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPuntuacion", DataPropertyName = "puntuacion", HeaderText = "Puntuación", Width = 80, DefaultCellStyle = { Format = "N1" } });
        }

        private void CargarCombos()
        {
            try
            {
                // Cargar Marcas
                var cboMarca = this.Controls.Find("cboMarca", true)[0] as ComboBox;
                var marcas = MarcaCln.listar();
                cboMarca.DataSource = marcas;
                cboMarca.DisplayMember = "nombre";
                cboMarca.ValueMember = "id";
                cboMarca.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar combos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var cboMarca = sender as ComboBox;
                var cboModelo = this.Controls.Find("cboModelo", true)[0] as ComboBox;

                cboModelo.DataSource = null;
                cboModelo.Items.Clear();

                if (cboMarca.SelectedValue != null && cboMarca.SelectedValue is int)
                {
                    int idMarca = (int)cboMarca.SelectedValue;
                    var modelos = ModeloVehiculoCln.listarPorMarca(idMarca);

                    cboModelo.DataSource = modelos;
                    cboModelo.DisplayMember = "nombre";
                    cboModelo.ValueMember = "id";
                    cboModelo.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar modelos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarVehiculos(string parametro)
        {
            try
            {
                var vehiculos = ConsultaVehiculosCln.listarPa(parametro ?? "");
                var dgv = this.Controls.Find("dgvVehiculos", true)[0] as DataGridView;
                dgv.DataSource = vehiculos;

                var lblTotal = this.Controls.Find("lblTotal", true)[0] as Label;
                lblTotal.Text = $"Total: {vehiculos.Count} vehículos";

                ColorearFilas(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar vehículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorearFilas(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["colConfiabilidad"].Value != null)
                {
                    string conf = row.Cells["colConfiabilidad"].Value.ToString();
                    switch (conf)
                    {
                        case "Excelente": row.DefaultCellStyle.BackColor = Color.LightGreen; break;
                        case "Bueno": row.DefaultCellStyle.BackColor = Color.LightBlue; break;
                        case "Regular": row.DefaultCellStyle.BackColor = Color.LightYellow; break;
                        case "Malo": row.DefaultCellStyle.BackColor = Color.LightCoral; break;
                    }
                }
            }
        }

        private void AgregarLabel(Panel panel, string texto, int x, int y, int width)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(x, y + 3),
                Size = new Size(width, 25)
            };
            panel.Controls.Add(lbl);
        }

        private void HabilitarControles(bool habilitar)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel panel)
                {
                    foreach (Control subCtrl in panel.Controls)
                    {
                        if (subCtrl.Name.StartsWith("txt") || subCtrl.Name.StartsWith("cbo") ||
                            subCtrl.Name.StartsWith("nud") || subCtrl.Name.StartsWith("chk"))
                        {
                            subCtrl.Enabled = habilitar;
                        }
                    }
                }
            }

            var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
            var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            vehiculoSeleccionadoId = 0;
            LimpiarFormulario();
            HabilitarControles(true);

            // Habilitar botones de acción
            var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
            var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;

            btnGuardar.Text = "💾 Guardar";
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;

            // Habilitar chasis para nuevo registro
            var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
            txtChasis.ReadOnly = false;
            txtChasis.Focus();

            // Deseleccionar grid
            var dgv = this.Controls.Find("dgvVehiculos", true)[0] as DataGridView;
            dgv.ClearSelection();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos obligatorios
                if (!ValidarFormulario()) return;

                // Obtener controles
                var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
                var cboModelo = this.Controls.Find("cboModelo", true)[0] as ComboBox;
                var txtGeneracion = this.Controls.Find("txtGeneracion", true)[0] as TextBox;
                var cboPais = this.Controls.Find("cboPais", true)[0] as ComboBox;
                var nudAnio = this.Controls.Find("nudAnio", true)[0] as NumericUpDown;
                var cboTransmision = this.Controls.Find("cboTransmision", true)[0] as ComboBox;
                var chkTurbo = this.Controls.Find("chkTurbo", true)[0] as CheckBox;
                var txtMotor = this.Controls.Find("txtMotor", true)[0] as TextBox;
                var cboCombustible = this.Controls.Find("cboCombustible", true)[0] as ComboBox;
                var nudAirbags = this.Controls.Find("nudAirbags", true)[0] as NumericUpDown;
                var nudEstrellas = this.Controls.Find("nudEstrellas", true)[0] as NumericUpDown;
                var cboOrganizacion = this.Controls.Find("cboOrganizacion", true)[0] as ComboBox;
                var cboConfiabilidad = this.Controls.Find("cboConfiabilidad", true)[0] as ComboBox;
                var nudPuntuacion = this.Controls.Find("nudPuntuacion", true)[0] as NumericUpDown;
                var txtObservaciones = this.Controls.Find("txtObservaciones", true)[0] as TextBox;

                // Crear o actualizar vehículo
                Vehiculo vehiculo;

                if (modoEdicion)
                {
                    vehiculo = VehiculoCln.obtenerUno(vehiculoSeleccionadoId);
                    if (vehiculo == null)
                    {
                        MessageBox.Show("❌ No se encontró el vehículo a actualizar", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    vehiculo = new Vehiculo();
                    vehiculo.nroChasis = txtChasis.Text.Trim();
                }

                // Asignar valores
                vehiculo.idModeloVehiculo = (int)cboModelo.SelectedValue;
                vehiculo.generacion = txtGeneracion.Text.Trim();
                vehiculo.paisFabricacion = cboPais.Text.Trim();
                vehiculo.anioFabricacion = (int)nudAnio.Value;
                vehiculo.transmision = cboTransmision.Text;
                vehiculo.tieneTurbo = chkTurbo.Checked;
                vehiculo.motor = txtMotor.Text.Trim();
                vehiculo.combustible = cboCombustible.Text;
                vehiculo.nroAirbags = (int)nudAirbags.Value;
                vehiculo.seguridadChoques = (int)nudEstrellas.Value;
                vehiculo.organizacionSeguridad = cboOrganizacion.Text;
                vehiculo.confiabilidad = cboConfiabilidad.Text;
                vehiculo.puntuacion = nudPuntuacion.Value;
                vehiculo.observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ?
                    null : txtObservaciones.Text.Trim();
                vehiculo.usuarioRegistro = Util.usuario.usuario;

                int resultado;
                string mensaje;

                if (modoEdicion)
                {
                    resultado = VehiculoCln.actualizar(vehiculo);
                    mensaje = resultado > 0 ?
                        "✅ Vehículo actualizado exitosamente" :
                        "❌ No se pudo actualizar el vehículo";
                }
                else
                {
                    resultado = VehiculoCln.insertar(vehiculo);
                    mensaje = resultado > 0 ?
                        "✅ Vehículo registrado exitosamente" :
                        "❌ No se pudo registrar el vehículo";
                }

                if (resultado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarVehiculos("");

                    // Deshabilitar botones de acción después de guardar
                    var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
                    var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
                    btnGuardar.Enabled = false;
                    btnCancelar.Enabled = false;

                    modoEdicion = false;
                    vehiculoSeleccionadoId = 0;
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarFormulario()
        {
            var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
            var cboModelo = this.Controls.Find("cboModelo", true)[0] as ComboBox;
            var txtGeneracion = this.Controls.Find("txtGeneracion", true)[0] as TextBox;
            var cboPais = this.Controls.Find("cboPais", true)[0] as ComboBox;
            var cboTransmision = this.Controls.Find("cboTransmision", true)[0] as ComboBox;
            var txtMotor = this.Controls.Find("txtMotor", true)[0] as TextBox;
            var cboCombustible = this.Controls.Find("cboCombustible", true)[0] as ComboBox;
            var cboOrganizacion = this.Controls.Find("cboOrganizacion", true)[0] as ComboBox;
            var cboConfiabilidad = this.Controls.Find("cboConfiabilidad", true)[0] as ComboBox;

            if (string.IsNullOrWhiteSpace(txtChasis.Text) && !modoEdicion)
            {
                MessageBox.Show("⚠️ El número de chasis es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChasis.Focus();
                return false;
            }

            if (cboModelo.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar un modelo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboModelo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGeneracion.Text))
            {
                MessageBox.Show("⚠️ La generación es obligatoria", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGeneracion.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cboPais.Text))
            {
                MessageBox.Show("⚠️ Debe seleccionar o ingresar el país de fabricación", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPais.Focus();
                return false;
            }

            if (cboTransmision.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el tipo de transmisión", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTransmision.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMotor.Text))
            {
                MessageBox.Show("⚠️ El motor es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotor.Focus();
                return false;
            }

            if (cboCombustible.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el tipo de combustible", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCombustible.Focus();
                return false;
            }

            if (cboOrganizacion.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar la organización de seguridad", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboOrganizacion.Focus();
                return false;
            }

            if (cboConfiabilidad.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el nivel de confiabilidad", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboConfiabilidad.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            modoEdicion = false;
            vehiculoSeleccionadoId = 0;

            // Deshabilitar botones de acción
            var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
            var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
            var btnEliminar = this.Controls.Find("btnEliminar", true)[0] as Button;

            btnGuardar.Text = "💾 Guardar";
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnEliminar.Enabled = false;

            // Deseleccionar grid
            var dgv = this.Controls.Find("dgvVehiculos", true)[0] as DataGridView;
            dgv.ClearSelection();

            // Los campos siguen habilitados para que el usuario pueda explorar
            // pero no puede guardar hasta que haga click en Nuevo o seleccione un vehículo
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var dgv = this.Controls.Find("dgvVehiculos", true)[0] as DataGridView;

                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("⚠️ Debe seleccionar un vehículo para eliminar", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var vehiculo = (paVehiculoListar_Result)dgv.CurrentRow.DataBoundItem;

                DialogResult resultado = MessageBox.Show(
                    $"⚠️ ¿Está seguro que desea eliminar este vehículo?\n\n" +
                    $"Marca: {vehiculo.marca}\n" +
                    $"Modelo: {vehiculo.modelo}\n" +
                    $"Generación: {vehiculo.generacion}\n\n" +
                    $"Esta acción no se puede deshacer.",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    int resultadoOp = VehiculoCln.eliminar(vehiculo.id, Util.usuario.usuario);

                    if (resultadoOp > 0)
                    {
                        MessageBox.Show("✅ Vehículo eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarVehiculos("");
                        LimpiarFormulario();
                        HabilitarControles(false);
                    }
                    else
                    {
                        MessageBox.Show("❌ No se pudo eliminar el vehículo", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al eliminar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel panel)
                {
                    foreach (Control subCtrl in panel.Controls)
                    {
                        if (subCtrl is TextBox txt) txt.Clear();
                        else if (subCtrl is ComboBox cbo) cbo.SelectedIndex = -1;
                        else if (subCtrl is NumericUpDown nud) nud.Value = nud.Minimum;
                        else if (subCtrl is CheckBox chk) chk.Checked = false;
                    }
                }
            }
        }

        private void DgvVehiculos_SelectionChanged(object sender, EventArgs e)
        {
            var dgv = sender as DataGridView;
            var btnEliminar = this.Controls.Find("btnEliminar", true)[0] as Button;

            if (dgv.CurrentRow != null)
            {
                btnEliminar.Enabled = true;

                // Cargar datos del vehículo seleccionado en el formulario
                var vehiculo = (paVehiculoListar_Result)dgv.CurrentRow.DataBoundItem;
                CargarDatosVehiculo(vehiculo.id);
            }
            else
            {
                btnEliminar.Enabled = false;
            }
        }

        private void CargarDatosVehiculo(int id)
        {
            try
            {
                var vehiculo = VehiculoCln.obtenerUno(id);
                if (vehiculo == null) return;

                modoEdicion = true;
                vehiculoSeleccionadoId = id;

                // Cargar datos en los controles
                var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
                var cboMarca = this.Controls.Find("cboMarca", true)[0] as ComboBox;
                var cboModelo = this.Controls.Find("cboModelo", true)[0] as ComboBox;
                var txtGeneracion = this.Controls.Find("txtGeneracion", true)[0] as TextBox;
                var cboPais = this.Controls.Find("cboPais", true)[0] as ComboBox;
                var nudAnio = this.Controls.Find("nudAnio", true)[0] as NumericUpDown;
                var cboTransmision = this.Controls.Find("cboTransmision", true)[0] as ComboBox;
                var chkTurbo = this.Controls.Find("chkTurbo", true)[0] as CheckBox;
                var txtMotor = this.Controls.Find("txtMotor", true)[0] as TextBox;
                var cboCombustible = this.Controls.Find("cboCombustible", true)[0] as ComboBox;
                var nudAirbags = this.Controls.Find("nudAirbags", true)[0] as NumericUpDown;
                var nudEstrellas = this.Controls.Find("nudEstrellas", true)[0] as NumericUpDown;
                var cboOrganizacion = this.Controls.Find("cboOrganizacion", true)[0] as ComboBox;
                var cboConfiabilidad = this.Controls.Find("cboConfiabilidad", true)[0] as ComboBox;
                var nudPuntuacion = this.Controls.Find("nudPuntuacion", true)[0] as NumericUpDown;
                var txtObservaciones = this.Controls.Find("txtObservaciones", true)[0] as TextBox;

                // Asignar valores
                txtChasis.Text = vehiculo.nroChasis;
                txtChasis.ReadOnly = false; // El chasis no se puede modificar

                var modelo = ModeloVehiculoCln.obtenerUno(vehiculo.idModeloVehiculo);
                cboMarca.SelectedValue = modelo.idMarca;
                cboModelo.SelectedValue = vehiculo.idModeloVehiculo;

                txtGeneracion.Text = vehiculo.generacion;
                cboPais.Text = vehiculo.paisFabricacion;
                nudAnio.Value = vehiculo.anioFabricacion;
                cboTransmision.Text = vehiculo.transmision;
                chkTurbo.Checked = vehiculo.tieneTurbo;
                txtMotor.Text = vehiculo.motor;
                cboCombustible.Text = vehiculo.combustible;
                nudAirbags.Value = vehiculo.nroAirbags;
                nudEstrellas.Value = vehiculo.seguridadChoques;
                cboOrganizacion.Text = vehiculo.organizacionSeguridad;
                cboConfiabilidad.Text = vehiculo.confiabilidad;
                nudPuntuacion.Value = vehiculo.puntuacion;
                txtObservaciones.Text = vehiculo.observaciones ?? "";

                // Habilitar botón de guardar (para edición)
                var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
                var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
                btnGuardar.Text = "💾 Actualizar";
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del vehículo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            var txt = sender as TextBox;
            CargarVehiculos(txt.Text.Trim());
        }
    }
}
