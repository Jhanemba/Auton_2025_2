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
        private bool datosCopiados = false;
        private bool permitirEventos = true; // CRÍTICO: Control de eventos

        public FrmGestionarVehiculos()
        {
            InitializeComponent();
        }

        private void FrmGestionarVehiculos_Load(object sender, EventArgs e)
        {
            if (Util.usuario == null)
            {
                MessageBox.Show("⚠️ Debe iniciar sesión para gestionar vehículos.",
                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            ConfigurarFormulario();
            ConfigurarDataGridView();
            CargarCombos();
            CargarVehiculos("");

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

            Panel pnlFormulario = new Panel
            {
                Location = new Point(10, 60),
                Size = new Size(450, 600),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            CrearFormularioDatos(pnlFormulario);

            Panel pnlGrid = new Panel
            {
                Location = new Point(470, 60),
                Size = new Size(910, 600+80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            CrearPanelGrid(pnlGrid);

            Panel pnlBotones = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80-20,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            CrearBotonesPrincipales(pnlBotones);

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

            // NRO DE CHASIS
            AgregarLabel(panel, "Nro. Chasis:", 15, yPos, labelWidth);
            TextBox txtChasis = new TextBox
            {
                Name = "txtChasis",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10),
                CharacterCasing = CharacterCasing.Upper,
                MaxLength = 17
            };
            panel.Controls.Add(txtChasis);
            yPos += 35;

            // MARCA
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

            // MODELO
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

            // TIPO
            AgregarLabel(panel, "Tipo:", 15, yPos, labelWidth);
            ComboBox cboTipo = new ComboBox
            {
                Name = "cboTipo",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            panel.Controls.Add(cboTipo);
            yPos += 35;

            // GENERACIÓN
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

            // PAÍS FABRICACIÓN
            AgregarLabel(panel, "País Fabricación:", 15, yPos, labelWidth);
            ComboBox cboPais = new ComboBox
            {
                Name = "cboPais",
                Location = new Point(160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboPais.Items.AddRange(new[] { "Japón", "Alemania", "Estados Unidos", "Corea del Sur", "China",
                "India", "Brasil", "México", "España", "Francia", "Italia",
                "Reino Unido", "Tailandia", "Hungría", "República Checa",
                "Turquía", "Argentina", "Sudáfrica", "Australia", "Canadá" });
            panel.Controls.Add(cboPais);
            yPos += 35;

            // AÑO FABRICACIÓN
            AgregarLabel(panel, "Año Fabricación:", 15, yPos, labelWidth);
            NumericUpDown nudAnio = new NumericUpDown
            {
                Name = "nudAnio",
                Location = new Point(160, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = 2020
            };
            panel.Controls.Add(nudAnio);
            yPos += 35;

            // TRANSMISIÓN
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

            AgregarLabel(panel, "Velocidades:", 15, yPos, labelWidth);

            // NumericUpDown para velocidades
            NumericUpDown nudVelocidades = new NumericUpDown
            {
                Name = "nudVelocidades",
                Location = new Point(160, yPos),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 4,
                Maximum = 10,
                Value = 5,
                TextAlign = HorizontalAlignment.Center,
                Enabled = false // Deshabilitado por defecto hasta seleccionar transmisión
            };
            panel.Controls.Add(nudVelocidades);

            // Label informativo
            Label lblInfoVelocidades = new Label
            {
                Name = "lblInfoVelocidades",
                Location = new Point(270, yPos + 3),
                Size = new Size(160, 25),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Text = "Seleccione transmisión",
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(lblInfoVelocidades);
            yPos += 35;

            // EVENTO: Actualizar velocidades cuando cambia la transmisión
            cboTransmision.SelectedIndexChanged += (s, e) =>
            {
                if (!permitirEventos) return; // Control de eventos

                string tipoTrans = cboTransmision.Text;

                // Use centralized helper to configure vitesse control (shows placeholder for CVT)
                var nud = panel.Controls.Find("nudVelocidades", true).FirstOrDefault() as NumericUpDown;
                var lblInfo = panel.Controls.Find("lblInfoVelocidades", true).FirstOrDefault() as Label;
                if (nud != null && lblInfo != null)
                {
                    ConfigurarVelocidades(tipoTrans, nud, lblInfo, null, true);
                }
            };

            // TIENE TURBO
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

            // MOTOR
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

            // COMBUSTIBLE
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

            // NRO ASIENTOS
            AgregarLabel(panel, "Nro. Asientos:", 15, yPos, labelWidth);
            NumericUpDown nudAsientos = new NumericUpDown
            {
                Name = "nudAsientos",
                Location = new Point(160, yPos),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 3,
                Maximum = 8,
                Value = 5
            };
            panel.Controls.Add(nudAsientos);
            yPos += 35;

            // NRO AIRBAGS
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

            // SEGURIDAD CHOQUES
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

            // ORGANIZACIÓN SEGURIDAD
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

            // CONFIABILIDAD
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

            // PUNTUACIÓN
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

            // OBSERVACIONES
            AgregarLabel(panel, "Observaciones:", 15, yPos, labelWidth);
            yPos += 25;
            TextBox txtObservaciones = new TextBox
            {
                Name = "txtObservaciones",
                Location = new Point(15, yPos),
                Size = new Size(415, 100+100),
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

            DataGridView dgvVehiculos = new DataGridView
            {
                Name = "dgvVehiculos",
                Location = new Point(15, 90),
                Size = new Size(880, 490),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9)
            };
            dgvVehiculos.SelectionChanged += DgvVehiculos_SelectionChanged;
            panel.Controls.Add(dgvVehiculos);

        }

        private void CrearBotonesPrincipales(Panel panel)
        {
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

            Button btnMarcasModelos = new Button
            {
                Text = "🏢 Marcas/Modelos",
                Size = new Size(180, 45),
                Location = new Point(700, 8),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnMarcasModelos.FlatAppearance.BorderSize = 0;
            btnMarcasModelos.Click += (s, e) =>
            {
                // Snapshot form state
                var state = new Dictionary<string, object>();

                Control[] ctrls = this.Controls.Find("txtChasis", true);
                var txtChasis = ctrls != null && ctrls.Length > 0 ? ctrls[0] as TextBox : null;

                // TextBoxes
                foreach (var t in new[] { "txtChasis", "txtGeneracion", "txtMotor", "txtObservaciones" })
                {
                    var c = this.Controls.Find(t, true).FirstOrDefault() as TextBox;
                    state[t] = c != null ? (object)c.Text : null;
                }

                // ComboBoxes - store SelectedValue (if set) and Text as fallback
                foreach (var name in new[] { "cboMarca", "cboModelo", "cboTipo", "cboPais", "cboTransmision", "cboCombustible", "cboOrganizacion", "cboConfiabilidad" })
                {
                    var c = this.Controls.Find(name, true).FirstOrDefault() as ComboBox;
                    if (c != null)
                    {
                        state[name + "__SelectedValue"] = c.SelectedValue;
                        state[name + "__Text"] = c.Text;
                    }
                    else
                    {
                        state[name + "__SelectedValue"] = null;
                        state[name + "__Text"] = null;
                    }
                }

                // NumericUpDowns
                foreach (var n in new[] { "nudAnio", "nudVelocidades", "nudAsientos", "nudAirbags", "nudEstrellas", "nudPuntuacion" })
                {
                    var nud = this.Controls.Find(n, true).FirstOrDefault() as NumericUpDown;
                    state[n] = nud != null ? (object)nud.Value : null;
                }

                // CheckBoxes
                var chkTurbo = this.Controls.Find("chkTurbo", true).FirstOrDefault() as CheckBox;
                state["chkTurbo"] = chkTurbo != null ? (object)chkTurbo.Checked : null;

                // Buttons / flags
                var btnGuardarCtrl = this.Controls.Find("btnGuardar", true).FirstOrDefault() as Button;
                state["btnGuardar__Text"] = btnGuardarCtrl != null ? btnGuardarCtrl.Text : null;
                state["btnGuardar__Enabled"] = btnGuardarCtrl != null ? (object)btnGuardarCtrl.Enabled : null;
                state["modoEdicion"] = modoEdicion;
                state["datosCopiados"] = datosCopiados;
                state["vehiculoSeleccionadoId"] = vehiculoSeleccionadoId;

                // Open dialog
                using (var frmMarcasModelos = new FrmGestionarMarcasModelos())
                {
                    frmMarcasModelos.ShowDialog(this);
                }

                // After close: reload combos and list
                CargarCombos();
                CargarVehiculos("");

                // Restore state
                try
                {
                    permitirEventos = false;

                    // Restore combos: cboMarca, cboTipo first
                    var cboMarca = this.Controls.Find("cboMarca", true).FirstOrDefault() as ComboBox;
                    var cboTipo = this.Controls.Find("cboTipo", true).FirstOrDefault() as ComboBox;
                    var cboModelo = this.Controls.Find("cboModelo", true).FirstOrDefault() as ComboBox;

                    object marcaVal = state.ContainsKey("cboMarca__SelectedValue") ? state["cboMarca__SelectedValue"] : null;
                    object tipoVal = state.ContainsKey("cboTipo__SelectedValue") ? state["cboTipo__SelectedValue"] : null;
                    object modeloVal = state.ContainsKey("cboModelo__SelectedValue") ? state["cboModelo__SelectedValue"] : null;

                    if (cboMarca != null && marcaVal != null)
                    {
                        // Try to set SelectedValue only if present in list
                        try { cboMarca.SelectedValue = marcaVal; } catch { cboMarca.Text = state["cboMarca__Text"] as string; }
                    }
                    else if (cboMarca != null)
                    {
                        cboMarca.Text = state["cboMarca__Text"] as string;
                    }

                    if (cboTipo != null && tipoVal != null)
                    {
                        try { cboTipo.SelectedValue = tipoVal; } catch { cboTipo.Text = state["cboTipo__Text"] as string; }
                    }
                    else if (cboTipo != null)
                    {
                        cboTipo.Text = state["cboTipo__Text"] as string;
                    }

                    // Repopulate models for the restored marca
                    if (cboMarca != null && cboModelo != null)
                    {
                        try
                        {
                            int idMarcaForModels = 0;
                            if (marcaVal is int) idMarcaForModels = (int)marcaVal;
                            else if (marcaVal is short) idMarcaForModels = Convert.ToInt32(marcaVal);
                            else if (!string.IsNullOrEmpty(cboMarca.Text))
                            {
                                // try match by name
                                var match = MarcaCln.listar().FirstOrDefault(m => m.nombre == cboMarca.Text);
                                if (match != null) idMarcaForModels = match.id;
                            }

                            if (idMarcaForModels > 0)
                            {
                                var modelos = ModeloVehiculoCln.listar()
                                    .Where(m => m.idMarca == idMarcaForModels && m.estado != -1)
                                    .OrderBy(m => m.nombre)
                                    .ToList();

                                cboModelo.DataSource = null;
                                cboModelo.DisplayMember = "nombre";
                                cboModelo.ValueMember = "id";
                                cboModelo.DataSource = modelos;

                                if (modeloVal != null)
                                {
                                    try { cboModelo.SelectedValue = modeloVal; } catch { cboModelo.Text = state["cboModelo__Text"] as string; }
                                }
                                else
                                {
                                    cboModelo.Text = state["cboModelo__Text"] as string;
                                }
                            }
                            else
                            {
                                // no marca id, just restore text
                                cboModelo.Text = state["cboModelo__Text"] as string;
                            }
                        }
                        catch { }
                    }

                    // Restore other controls
                    foreach (var t in new[] { "txtChasis", "txtGeneracion", "txtMotor", "txtObservaciones" })
                    {
                        var c = this.Controls.Find(t, true).FirstOrDefault() as TextBox;
                        if (c != null && state.ContainsKey(t)) c.Text = state[t] as string ?? "";
                    }

                    foreach (var name in new[] { "cboPais", "cboTransmision", "cboCombustible", "cboOrganizacion", "cboConfiabilidad" })
                    {
                        var c = this.Controls.Find(name, true).FirstOrDefault() as ComboBox;
                        if (c != null)
                        {
                            object val = state.ContainsKey(name + "__SelectedValue") ? state[name + "__SelectedValue"] : null;
                            if (val != null)
                            {
                                try { c.SelectedValue = val; } catch { c.Text = state[name + "__Text"] as string; }
                            }
                            else
                            {
                                c.Text = state[name + "__Text"] as string;
                            }
                        }
                    }

                    // Numeric values
                    foreach (var n in new[] { "nudAnio", "nudVelocidades", "nudAsientos", "nudAirbags", "nudEstrellas", "nudPuntuacion" })
                    {
                        var nud = this.Controls.Find(n, true).FirstOrDefault() as NumericUpDown;
                        if (nud != null && state.ContainsKey(n) && state[n] != null)
                        {
                            try { nud.Value = Convert.ToDecimal(state[n]); } catch { }
                        }
                    }

                    // CheckBox
                    if (chkTurbo != null && state.ContainsKey("chkTurbo") && state["chkTurbo"] != null)
                        chkTurbo.Checked = Convert.ToBoolean(state["chkTurbo"]);

                    // Restore botones/flags
                    var btnGuardarNow = this.Controls.Find("btnGuardar", true).FirstOrDefault() as Button;
                    if (btnGuardarNow != null)
                    {
                        if (state.ContainsKey("btnGuardar__Text") && state["btnGuardar__Text"] != null)
                            btnGuardarNow.Text = state["btnGuardar__Text"] as string;
                        if (state.ContainsKey("btnGuardar__Enabled") && state["btnGuardar__Enabled"] != null)
                            btnGuardarNow.Enabled = Convert.ToBoolean(state["btnGuardar__Enabled"]);
                    }

                    if (state.ContainsKey("modoEdicion")) modoEdicion = Convert.ToBoolean(state["modoEdicion"]);
                    if (state.ContainsKey("datosCopiados")) datosCopiados = Convert.ToBoolean(state["datosCopiados"]);
                    if (state.ContainsKey("vehiculoSeleccionadoId") && state["vehiculoSeleccionadoId"] != null)
                        vehiculoSeleccionadoId = Convert.ToInt32(state["vehiculoSeleccionadoId"]);

                    // Ensure velocidad control state matches current transmisión and is editable
                    var nudVel = this.Controls.Find("nudVelocidades", true).FirstOrDefault() as NumericUpDown;
                    var lblInfoVel = this.Controls.Find("lblInfoVelocidades", true).FirstOrDefault() as Label;
                    var cboTransFinal = this.Controls.Find("cboTransmision", true).FirstOrDefault() as ComboBox;

                    if (nudVel != null && lblInfoVel != null && cboTransFinal != null)
                    {
                        int? velVal = null;
                        if (state.ContainsKey("nudVelocidades") && state["nudVelocidades"] != null)
                            velVal = Convert.ToInt32(state["nudVelocidades"]);

                        ConfigurarVelocidades(cboTransFinal.Text, nudVel, lblInfoVel, velVal, true);
                    }
                }
                finally
                {
                    permitirEventos = true;
                }
            };
            panel.Controls.Add(btnMarcasModelos);

            Button btnCerrar = new Button
            {
                Text = "❌ Cerrar",
                Size = new Size(150, 45),
                Location = new Point(900, 8),
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
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colChasis", DataPropertyName = "nroChasis", HeaderText = "Chasis", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMarca", DataPropertyName = "marca", HeaderText = "Marca", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colModelo", DataPropertyName = "modelo", HeaderText = "Modelo", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGeneracion", DataPropertyName = "generacion", HeaderText = "Generación", Width = 100 });
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
                permitirEventos = false;

                var cboMarca = this.Controls.Find("cboMarca", true)[0] as ComboBox;
                var cboTipo = this.Controls.Find("cboTipo", true)[0] as ComboBox;

                var marcas = MarcaCln.listar()
                    .Where(m => m.estado != -1)
                    .OrderBy(m => m.nombre)
                    .ToList();

                cboMarca.DataSource = null;
                cboMarca.DisplayMember = "nombre";
                cboMarca.ValueMember = "id";
                cboMarca.DataSource = marcas;
                cboMarca.SelectedIndex = -1;

                var tipos = TipoVehiculoCln.listar()
                    .Where(t => t.estado != -1)
                    .OrderBy(t => t.descripcion)
                    .ToList();

                cboTipo.DataSource = null;
                cboTipo.DisplayMember = "descripcion";
                cboTipo.ValueMember = "id";
                cboTipo.DataSource = tipos;
                cboTipo.SelectedIndex = -1;

                permitirEventos = true;
            }
            catch (Exception ex)
            {
                permitirEventos = true;
                MessageBox.Show($"Error al cargar combos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!permitirEventos) return;
            try
            {
                var cboMarca = sender as ComboBox;
                var cboModelo = this.Controls.Find("cboModelo", true)[0] as ComboBox;

                permitirEventos = false;

                cboModelo.DataSource = null;
                cboModelo.Items.Clear();

                if (cboMarca.SelectedValue != null && cboMarca.SelectedValue is int)
                {
                    int idMarca = (int)cboMarca.SelectedValue;
                    var modelos = ModeloVehiculoCln.listar()
                        .Where(m => m.idMarca == idMarca && m.estado != -1)
                        .OrderBy(m => m.nombre)
                        .ToList();

                    cboModelo.DisplayMember = "nombre";
                    cboModelo.ValueMember = "id";
                    cboModelo.DataSource = modelos;
                    cboModelo.SelectedIndex = -1;
                }

                permitirEventos = true;
            }
            catch (Exception ex)
            {
                permitirEventos = true;
                MessageBox.Show($"Error al cargar modelos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Ajusta la altura del DataGridView para mostrar hasta maxRows filas y activar scroll si hay más.
        private void AjustarAlturaGrid(DataGridView dgv, int maxRowsVisible)
        {
            if (dgv == null) return;
            dgv.ScrollBars = ScrollBars.Vertical;
            // Asegurar una altura de fila por defecto razonable
            int rowHeight = dgv.RowTemplate != null && dgv.RowTemplate.Height > 0 ? dgv.RowTemplate.Height : 22;
            int headerHeight = dgv.ColumnHeadersHeight > 0 ? dgv.ColumnHeadersHeight : 24;

            int totalRows = dgv.Rows.Count;
            int visibleRows = Math.Min(totalRows, maxRowsVisible);

            // Si no hay filas aún, dejar la altura por defecto
            if (totalRows == 0)
            {
                // mantener tamaño original
                return;
            }

            int newHeight = headerHeight + (visibleRows * rowHeight) + 4; // padding
            // Limitar la altura máxima al tamaño del contenedor
            var parent = dgv.Parent as Control;
            if (parent != null)
            {
                int maxAllowed = parent.ClientSize.Height - dgv.Location.Y - 20; // dejar espacio
                if (newHeight > maxAllowed) newHeight = maxAllowed;
            }

            dgv.Height = newHeight;
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

                // Mostrar como máximo 10 filas y activar scroll si hay más
                AjustarAlturaGrid(dgv, 18);

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

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            var dgv = this.Controls.Find("dgvVehiculos", true)[0] as DataGridView;
            var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
            var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
            var btnNuevo = sender as Button;

            // If already in 'copy' mode and user clicks Nuevo: clear the form for fresh entry
            if (datosCopiados)
            {
                LimpiarFormulario();
                dgv.ClearSelection();

                btnGuardar.Text = "💾 Guardar";
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
                btnNuevo.BackColor = Color.FromArgb(40, 167, 69);

                modoEdicion = false;
                vehiculoSeleccionadoId = 0;
                datosCopiados = false;

                var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
                txtChasis.Focus();
                return;
            }

            // If there is a selected row, copy its data into the form to create a variant
            DataGridViewRow selRow = null;
            if (dgv != null)
            {
                if (dgv.CurrentRow != null) selRow = dgv.CurrentRow;
                else if (dgv.SelectedRows != null && dgv.SelectedRows.Count > 0) selRow = dgv.SelectedRows[0];
            }

            if (selRow != null && selRow.DataBoundItem is paVehiculoListar_Result vehiculoData)
            {
                // Load data into form (this sets modoEdicion = true internally), then switch to copy mode
                CargarDatosVehiculo(vehiculoData.id);

                modoEdicion = false;            // we want to create a new record
                vehiculoSeleccionadoId = 0;
                datosCopiados = true;

                // Clear only the chasis to force new unique key and focus it
                var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
                if (txtChasis != null) txtChasis.Clear();
                if (txtChasis != null) txtChasis.Focus();

                btnGuardar.Text = "💾 Guardar Nuevo";
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
                btnNuevo.BackColor = Color.FromArgb(255, 193, 7);

                // Ensure velocidad control matches current transmisión and is editable for user
                var nudVel = this.Controls.Find("nudVelocidades", true)[0] as NumericUpDown;
                var lblInfoVel = this.Controls.Find("lblInfoVelocidades", true)[0] as Label;
                var cboTrans = this.Controls.Find("cboTransmision", true)[0] as ComboBox;
                if (nudVel != null && lblInfoVel != null && cboTrans != null)
                {
                    ConfigurarVelocidades(cboTrans.Text, nudVel, lblInfoVel, null, true);
                }

                MessageBox.Show("ℹ️ Datos copiados del vehículo seleccionado.\n\n" +
                                "• Ingrese un nuevo número de chasis\n" +
                                "• Modifique los campos necesarios\n" +
                                "• Click en 'Guardar Nuevo' para registrar\n" +
                                "• Click nuevamente en 'Nuevo' para limpiar todo",
                                "Modo Copiar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // No selection: prepare a blank form for new entry
                LimpiarFormulario();
                if (dgv != null) dgv.ClearSelection();

                modoEdicion = false;
                vehiculoSeleccionadoId = 0;
                datosCopiados = false;

                btnGuardar.Text = "💾 Guardar";
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
                btnNuevo.BackColor = Color.FromArgb(40, 167, 69);

                var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
                if (txtChasis != null) txtChasis.Focus();
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // CRÍTICO: Validar campos obligatorios PRIMERO
                if (!ValidarFormulario()) return;

                // Obtener controles
                var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
                var cboModelo = this.Controls.Find("cboModelo", true)[0] as ComboBox;
                var cboTipo = this.Controls.Find("cboTipo", true)[0] as ComboBox;
                var txtGeneracion = this.Controls.Find("txtGeneracion", true)[0] as TextBox;
                var cboPais = this.Controls.Find("cboPais", true)[0] as ComboBox;
                var nudAnio = this.Controls.Find("nudAnio", true)[0] as NumericUpDown;
                var cboTransmision = this.Controls.Find("cboTransmision", true)[0] as ComboBox;
                var nudVelocidades = this.Controls.Find("nudVelocidades", true)[0] as NumericUpDown;
                var nudAsientos = this.Controls.Find("nudAsientos", true)[0] as NumericUpDown;
                var chkTurbo = this.Controls.Find("chkTurbo", true)[0] as CheckBox;
                var txtMotor = this.Controls.Find("txtMotor", true)[0] as TextBox;
                var cboCombustible = this.Controls.Find("cboCombustible", true)[0] as ComboBox;
                var nudAirbags = this.Controls.Find("nudAirbags", true)[0] as NumericUpDown;
                var nudEstrellas = this.Controls.Find("nudEstrellas", true)[0] as NumericUpDown;
                var cboOrganizacion = this.Controls.Find("cboOrganizacion", true)[0] as ComboBox;
                var cboConfiabilidad = this.Controls.Find("cboConfiabilidad", true)[0] as ComboBox;
                var nudPuntuacion = this.Controls.Find("nudPuntuacion", true)[0] as NumericUpDown;
                var txtObservaciones = this.Controls.Find("txtObservaciones", true)[0] as TextBox;

                // CRÍTICO: Actualizar el tipo del modelo ANTES de guardar el vehículo
                int nuevoIdModelo = (int)cboModelo.SelectedValue;
                int nuevoIdTipo = (int)cboTipo.SelectedValue;

                var modeloSeleccionado = ModeloVehiculoCln.obtenerUno(nuevoIdModelo);

                if (modeloSeleccionado == null)
                {
                    MessageBox.Show("❌ Error: No se pudo cargar el modelo seleccionado",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Si el tipo cambió, actualizar el modelo
                if (modeloSeleccionado.idTipoVehiculo != nuevoIdTipo)
                {
                    modeloSeleccionado.idTipoVehiculo = nuevoIdTipo;
                    modeloSeleccionado.usuarioRegistro = Util.usuario.usuario;

                    int resultadoModelo = ModeloVehiculoCln.actualizar(modeloSeleccionado);

                    if (resultadoModelo == 0)
                    {
                        MessageBox.Show("❌ Error al actualizar el tipo del modelo",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

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
                    vehiculo = new Vehiculo
                    {
                        estado = 1,
                        fechaRegistro = DateTime.Now
                    };
                }

                // Asignar TODOS los valores
                vehiculo.nroChasis = txtChasis.Text.Trim().ToUpper();
                vehiculo.idModeloVehiculo = nuevoIdModelo;
                vehiculo.generacion = txtGeneracion.Text.Trim();
                vehiculo.paisFabricacion = cboPais.Text.Trim();
                vehiculo.anioFabricacion = (int)nudAnio.Value;
                vehiculo.transmision = cboTransmision.Text;
                if (cboTransmision.Text == "CVT")
                {
                    vehiculo.nroVelocidades = null;
                }
                else
                {
                    vehiculo.nroVelocidades = (int)nudVelocidades.Value;
                }
                vehiculo.nroAsientos = (int)nudAsientos.Value;
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

                    var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
                    var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
                    btnGuardar.Enabled = false;
                    btnCancelar.Enabled = false;

                    modoEdicion = false;
                    vehiculoSeleccionadoId = 0;
                    datosCopiados = false;
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarFormulario()
        {
            var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
            var cboMarca = this.Controls.Find("cboMarca", true)[0] as ComboBox;
            var cboModelo = this.Controls.Find("cboModelo", true)[0] as ComboBox;
            var cboTipo = this.Controls.Find("cboTipo", true)[0] as ComboBox;
            var txtGeneracion = this.Controls.Find("txtGeneracion", true)[0] as TextBox;
            var cboPais = this.Controls.Find("cboPais", true)[0] as ComboBox;
            var cboTransmision = this.Controls.Find("cboTransmision", true)[0] as ComboBox;
            var txtMotor = this.Controls.Find("txtMotor", true)[0] as TextBox;
            var cboCombustible = this.Controls.Find("cboCombustible", true)[0] as ComboBox;
            var cboOrganizacion = this.Controls.Find("cboOrganizacion", true)[0] as ComboBox;
            var cboConfiabilidad = this.Controls.Find("cboConfiabilidad", true)[0] as ComboBox;

            // VALIDAR CHASIS (solo en modo nuevo)
            if (!modoEdicion && string.IsNullOrWhiteSpace(txtChasis.Text))
            {
                MessageBox.Show("⚠️ El número de chasis es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChasis.Focus();
                return false;
            }

            if (!modoEdicion && txtChasis.Text.Trim().Length != 17)
            {
                MessageBox.Show("⚠️ El número de chasis debe tener exactamente 17 caracteres", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChasis.Focus();
                return false;
            }

            if (cboMarca.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar una marca", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMarca.Focus();
                return false;
            }

            if (cboModelo.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar un modelo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboModelo.Focus();
                return false;
            }

            if (cboTipo.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar un tipo de vehículo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTipo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGeneracion.Text))
            {
                MessageBox.Show("⚠️ La generación es obligatoria", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGeneracion.Focus();
                return false;
            }

            if (cboPais.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el país de fabricación", "Validación",
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

            var nudVelocidades = this.Controls.Find("nudVelocidades", true)[0] as NumericUpDown;
            if (cboTransmision.Text != "CVT" && !nudVelocidades.Enabled)
            {
                MessageBox.Show("⚠️ Debe especificar el número de velocidades", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMotor.Text))
            {
                MessageBox.Show("⚠️ El motor es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotor.Focus();
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
            datosCopiados = false;

            var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
            var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
            var btnEliminar = this.Controls.Find("btnEliminar", true)[0] as Button;
            var btnNuevo = this.Controls.Find("btnNuevo", true)[0] as Button;

            btnGuardar.Text = "💾 Guardar";
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnEliminar.Enabled = false;
            btnNuevo.BackColor = Color.FromArgb(40, 167, 69);

            var dgv = this.Controls.Find("dgvVehiculos", true)[0] as DataGridView;
            dgv.ClearSelection();
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
                    int resultadoOp = ConsultaVehiculosCln.eliminar(vehiculo.id, Util.usuario.usuario);

                    if (resultadoOp > 0)
                    {
                        MessageBox.Show("✅ Vehículo eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarVehiculos("");
                        LimpiarFormulario();

                        var btnEliminar = this.Controls.Find("btnEliminar", true)[0] as Button;
                        btnEliminar.Enabled = false;
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
            permitirEventos = false; // DESACTIVAR EVENTOS

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel panel)
                {
                    foreach (Control subCtrl in panel.Controls)
                    {
                        if (subCtrl is TextBox txt)
                            txt.Clear();
                        else if (subCtrl is ComboBox cbo)
                            cbo.SelectedIndex = -1;
                        else if (subCtrl is NumericUpDown nud)
                        {
                            if (nud.Name == "nudAnio")
                                nud.Value = 2020;
                            else if (nud.Name == "nudAirbags")
                                nud.Value = 2;
                            else if (nud.Name == "nudPuntuacion")
                                nud.Value = 5.0m;
                            else if (nud.Name == "nudVelocidades")  // New
                            {
                                nud.Value = 5;
                                nud.Enabled = false;
                            }
                            else if (nud.Name == "nudAsientos")
                                nud.Value = 5;
                            else
                                nud.Value = nud.Minimum;
                        }
                        else if (subCtrl is CheckBox chk)
                            chk.Checked = false;
                        else if (subCtrl is Label lbl && lbl.Name == "lblInfoVelocidades")  // New
                        {
                            lbl.Text = "Seleccione transmisión";
                            lbl.ForeColor = Color.Gray;
                        }
                    }
                }
            }

            permitirEventos = true; // REACTIVAR EVENTOS
        }

        private void DgvVehiculos_SelectionChanged(object sender, EventArgs e)
        {
            var dgv = sender as DataGridView;
            var btnEliminar = this.Controls.Find("btnEliminar", true)[0] as Button;

            if (dgv.CurrentRow != null)
            {
                btnEliminar.Enabled = true;
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
                permitirEventos = false; // DESACTIVAR EVENTOS

                var vehiculo = ConsultaVehiculosCln.obtenerUno(id);
                if (vehiculo == null) return;

                modoEdicion = true;
                vehiculoSeleccionadoId = id;

                var txtChasis = this.Controls.Find("txtChasis", true)[0] as TextBox;
                var cboMarca = this.Controls.Find("cboMarca", true)[0] as ComboBox;
                var cboModelo = this.Controls.Find("cboModelo", true)[0] as ComboBox;
                var cboTipo = this.Controls.Find("cboTipo", true)[0] as ComboBox;
                var txtGeneracion = this.Controls.Find("txtGeneracion", true)[0] as TextBox;
                var cboPais = this.Controls.Find("cboPais", true)[0] as ComboBox;
                var nudAnio = this.Controls.Find("nudAnio", true)[0] as NumericUpDown;
                var cboTransmision = this.Controls.Find("cboTransmision", true)[0] as ComboBox;
                var nudAsientos = this.Controls.Find("nudAsientos", true)[0] as NumericUpDown;
                var chkTurbo = this.Controls.Find("chkTurbo", true)[0] as CheckBox;
                var txtMotor = this.Controls.Find("txtMotor", true)[0] as TextBox;
                var cboCombustible = this.Controls.Find("cboCombustible", true)[0] as ComboBox;
                var nudAirbags = this.Controls.Find("nudAirbags", true)[0] as NumericUpDown;
                var nudEstrellas = this.Controls.Find("nudEstrellas", true)[0] as NumericUpDown;
                var cboOrganizacion = this.Controls.Find("cboOrganizacion", true)[0] as ComboBox;
                var cboConfiabilidad = this.Controls.Find("cboConfiabilidad", true)[0] as ComboBox;
                var nudPuntuacion = this.Controls.Find("nudPuntuacion", true)[0] as NumericUpDown;
                var txtObservaciones = this.Controls.Find("txtObservaciones", true)[0] as TextBox;

                txtChasis.Text = vehiculo.nroChasis;

                // Obtener modelo y cargar marca/modelos
                var modelo = ModeloVehiculoCln.obtenerUno(vehiculo.idModeloVehiculo);

                if (modelo != null)
                {
                    // Seleccionar marca
                    cboMarca.SelectedValue = modelo.idMarca;

                    // Cargar modelos de esa marca
                    var modelos = ModeloVehiculoCln.listar()
                        .Where(m => m.idMarca == modelo.idMarca && m.estado != -1)
                        .OrderBy(m => m.nombre)
                        .ToList();

                    cboModelo.DataSource = null;
                    cboModelo.DisplayMember = "nombre";
                    cboModelo.ValueMember = "id";
                    cboModelo.DataSource = modelos;
                    cboModelo.SelectedValue = vehiculo.idModeloVehiculo;

                    // Seleccionar tipo
                    cboTipo.SelectedValue = modelo.idTipoVehiculo;
                }

                txtGeneracion.Text = vehiculo.generacion;
                cboPais.Text = vehiculo.paisFabricacion;
                nudAnio.Value = vehiculo.anioFabricacion;
                cboTransmision.Text = vehiculo.transmision;
                var nudVelocidades = this.Controls.Find("nudVelocidades", true)[0] as NumericUpDown;
                var lblInfoVel = this.Controls.Find("lblInfoVelocidades", true)[0] as Label;
                nudAsientos.Value = vehiculo.nroAsientos ?? 5;


                // Use centralized helper to configure control (will hide for CVT)
                if (vehiculo.transmision == "CVT")
                {
                    ConfigurarVelocidades("CVT", nudVelocidades, lblInfoVel, null, true);
                }
                else
                {
                    ConfigurarVelocidades(vehiculo.transmision, nudVelocidades, lblInfoVel, vehiculo.nroVelocidades, true);
                }

                chkTurbo.Checked = vehiculo.tieneTurbo;
                txtMotor.Text = vehiculo.motor;
                cboCombustible.Text = vehiculo.combustible;
                nudAirbags.Value = vehiculo.nroAirbags;
                nudEstrellas.Value = vehiculo.seguridadChoques;
                cboOrganizacion.Text = vehiculo.organizacionSeguridad;
                cboConfiabilidad.Text = vehiculo.confiabilidad;
                nudPuntuacion.Value = vehiculo.puntuacion;
                txtObservaciones.Text = vehiculo.observaciones ?? "";

                permitirEventos = true; // REACTIVAR EVENTOS

                var btnGuardar = this.Controls.Find("btnGuardar", true)[0] as Button;
                var btnCancelar = this.Controls.Find("btnCancelar", true)[0] as Button;
                btnGuardar.Text = "💾 Actualizar";
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
            }
            catch (Exception ex)
            {
                permitirEventos = true;
                MessageBox.Show($"Error al cargar datos del vehículo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper to configure velocidades similar to FrmConsultaVehiculos/FrmRegistroVehiculo
        private void ConfigurarVelocidades(string tipoTransmision, NumericUpDown nudVel, Label lblInfo, int? valorActual, bool permitirEdicion)
        {
            Label lblPlaceholder = null;
            try
            {
                var found = nudVel.Parent.Controls.Find("lblVelocidadesPlaceholder", true);
                if (found != null && found.Length > 0 && found[0] is Label)
                    lblPlaceholder = (Label)found[0];
            }
            catch { }

            if (lblPlaceholder == null)
            {
                lblPlaceholder = new Label
                {
                    Name = "lblVelocidadesPlaceholder",
                    Location = nudVel.Location,
                    Size = nudVel.Size,
                    Font = new Font("Segoe UI", 10F),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    Visible = false
                };
                nudVel.Parent.Controls.Add(lblPlaceholder);
                lblPlaceholder.BringToFront();
            }

            switch (tipoTransmision)
            {
                case "Manual":
                    nudVel.Visible = true;
                    lblPlaceholder.Visible = false;
                    nudVel.Minimum = 4;
                    nudVel.Maximum = 6;
                    nudVel.Enabled = permitirEdicion;
                    {
                        decimal min = nudVel.Minimum;
                        decimal max = nudVel.Maximum;
                        decimal val = valorActual.HasValue ? (decimal)valorActual.Value : 5m;
                        if (val < min) val = min;
                        if (val > max) val = max;
                        nudVel.Value = val;
                    }
                    lblInfo.Text = "4-6 velocidades";
                    lblInfo.ForeColor = Color.FromArgb(40, 167, 69);
                    break;
                case "Automática":
                    nudVel.Visible = true;
                    lblPlaceholder.Visible = false;
                    nudVel.Minimum = 4;
                    nudVel.Maximum = 10;
                    nudVel.Enabled = permitirEdicion;
                    {
                        decimal min = nudVel.Minimum;
                        decimal max = nudVel.Maximum;
                        decimal val = valorActual.HasValue ? (decimal)valorActual.Value : 6m;
                        if (val < min) val = min;
                        if (val > max) val = max;
                        nudVel.Value = val;
                    }
                    lblInfo.Text = "4-10 velocidades";
                    lblInfo.ForeColor = Color.FromArgb(0, 123, 255);
                    break;
                case "CVT":
                    nudVel.Visible = false;
                    nudVel.Enabled = false;
                    lblPlaceholder.Text = "-";
                    lblPlaceholder.Visible = true;
                    lblInfo.Text = "Sin velocidades discretas";
                    lblInfo.ForeColor = Color.FromArgb(108, 117, 125);
                    break;
                case "Semiautomática":
                case "Doble Embrague":
                    nudVel.Visible = true;
                    lblPlaceholder.Visible = false;
                    nudVel.Minimum = 6;
                    nudVel.Maximum = 8;
                    nudVel.Enabled = permitirEdicion;
                    {
                        decimal min = nudVel.Minimum;
                        decimal max = nudVel.Maximum;
                        decimal val = valorActual.HasValue ? (decimal)valorActual.Value : 7m;
                        if (val < min) val = min;
                        if (val > max) val = max;
                        nudVel.Value = val;
                    }
                    lblInfo.Text = "6-8 velocidades";
                    lblInfo.ForeColor = Color.FromArgb(255, 193, 7);
                    break;
                default:
                    nudVel.Visible = true;
                    lblPlaceholder.Visible = false;
                    nudVel.Enabled = false;
                    lblInfo.Text = "Seleccione transmisión";
                    lblInfo.ForeColor = Color.Gray;
                    break;
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null) return;
            CargarVehiculos(txt.Text.Trim());
        }
    }
}
