using CadAuton;
using ClnAuton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpAuton
{
    public partial class FrmRegistroVehiculo : Form
    {
        private List<Procedencia> procedencias;
        private List<Marca> marcas;
        private List<ModeloVehiculo> modelos;
        private List<TipoVehiculo> tipos;

        // COLUMNA IZQUIERDA
        private ComboBox cboProcedencia;
        private ComboBox cboMarca;
        private ComboBox cboTipo;
        private ComboBox cboModelo;
        private Button btnAgregarMarca;
        private Button btnAgregarModelo;
        private TextBox txtNroChasis;
        private NumericUpDown nudAnio;

        // COLUMNA DERECHA - Características Específicas
        private TextBox txtGeneracion;
        private ComboBox cboPaisFabricacion;
        private ComboBox cboTransmision;
        private CheckBox chkTieneTurbo;
        private TextBox txtMotor;
        private ComboBox cboCombustible;

        // SEGURIDAD
        private NumericUpDown nudAirbags;
        private ComboBox cboOrganizacionSeguridad;
        private ComboBox cboSeguridadChoques;

        // EVALUACIÓN
        private ComboBox cboConfiabilidad;
        private NumericUpDown nudPuntuacion;
        //private NumericUpDown nudKilometraje;
        private CheckBox chkKilometrajeDesconocido;
        private TextBox txtObservaciones;

        private PictureBox pbFotografia;
        private Label lblFotoInfo;
        private Button btnSeleccionarFoto;
        private Button btnEliminarFoto;
        private Image imagenActual = null;

        public FrmRegistroVehiculo()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "...::: Auton - Registrar Nuevo Vehículo :::...";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 10F);
            //this.AutoScroll = true;
            //this.AutoScrollMinSize = new Size(0, 1150);  // Fuerza scroll

            Panel pnlContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            this.Controls.Add(pnlContenedor);

            Panel pnlPrincipal = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1184, 750+270),
                Padding = new Padding(20),
                BackColor = Color.PapayaWhip,
                //AutoScroll = true
            };

            int yPos = 15;
            int lineHeight = 45;
            int labelWidth = 150;
            int ctrlWidth = 250;
            int colLeftX = 170;
            int colRightX = 650;

            // Título principal
            Label lblTitulo = new Label
            {
                Text = "🚗 REGISTRO COMPLETO DE VEHÍCULO",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(20, yPos),
                Size = new Size(1140, 32),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 45;

            // ==================== COLUMNA IZQUIERDA ====================
            AgregarSeccion(pnlPrincipal, "📋 IDENTIFICACIÓN DEL MODELO", 20, yPos, 540);
            int yPosLeft = yPos + 38;

            // Procedencia
            AgregarLabel(pnlPrincipal, "Procedencia:", 20, yPosLeft, labelWidth);
            cboProcedencia = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboProcedencia.SelectedIndexChanged += CboProcedencia_SelectedIndexChanged;
            pnlPrincipal.Controls.Add(cboProcedencia);
            yPosLeft += lineHeight;

            // Marca
            AgregarLabel(pnlPrincipal, "Marca:", 20, yPosLeft, labelWidth);
            cboMarca = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(210, 28),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 10F),
                Enabled = false
            };
            cboMarca.SelectedIndexChanged += CboMarca_SelectedIndexChanged;
            pnlPrincipal.Controls.Add(cboMarca);

            btnAgregarMarca = new Button
            {
                Text = "+",
                Location = new Point(colLeftX + 215, yPosLeft),
                Size = new Size(35, 28),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnAgregarMarca.FlatAppearance.BorderSize = 0;
            btnAgregarMarca.Click += BtnAgregarMarca_Click;
            pnlPrincipal.Controls.Add(btnAgregarMarca);
            yPosLeft += lineHeight;

            // Tipo
            AgregarLabel(pnlPrincipal, "Tipo:", 20, yPosLeft, labelWidth);
            cboTipo = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(cboTipo);
            yPosLeft += lineHeight;

            // Modelo (SIMPLE - Solo nombre)
            AgregarLabel(pnlPrincipal, "Modelo:", 20, yPosLeft, labelWidth);
            cboModelo = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(210, 28),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 10F),
                Enabled = false
            };
            pnlPrincipal.Controls.Add(cboModelo);

            btnAgregarModelo = new Button
            {
                Text = "+",
                Location = new Point(colLeftX + 215, yPosLeft),
                Size = new Size(35, 28),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnAgregarModelo.FlatAppearance.BorderSize = 0;
            btnAgregarModelo.Click += BtnAgregarModelo_Click;
            pnlPrincipal.Controls.Add(btnAgregarModelo);
            yPosLeft += lineHeight;

            // Nro. de Chasis
            AgregarLabel(pnlPrincipal, "Nro. de Chasis:", 20, yPosLeft, labelWidth);
            txtNroChasis = new TextBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                Font = new Font("Segoe UI", 10F),
                CharacterCasing = CharacterCasing.Upper,
                MaxLength = 17
            };
            pnlPrincipal.Controls.Add(txtNroChasis);
            SetCueBanner(txtNroChasis, "17 caracteres");
            yPosLeft += lineHeight;

            // Año de Fabricación
            AgregarLabel(pnlPrincipal, "Año Fabricación:", 20, yPosLeft, labelWidth);
            nudAnio = new NumericUpDown
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(120, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year,
                TextAlign = HorizontalAlignment.Center
            };
            pnlPrincipal.Controls.Add(nudAnio);
            yPosLeft += lineHeight;

            // ==================== COLUMNA DERECHA ====================
            AgregarSeccion(pnlPrincipal, "⚙️ CARACTERÍSTICAS ESPECÍFICAS", 580, yPos, 580);
            int yPosRight = yPos + 38;

            // Generación
            AgregarLabel(pnlPrincipal, "Generación:", 580, yPosRight, labelWidth);
            txtGeneracion = new TextBox
            {
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                Font = new Font("Segoe UI", 10F),
                MaxLength = 50
            };
            pnlPrincipal.Controls.Add(txtGeneracion);
            SetCueBanner(txtGeneracion, "Ej: Gen 12, 11va Gen");
            yPosRight += lineHeight;

            // País de Fabricación
            AgregarLabel(pnlPrincipal, "País Fabricación:", 580, yPosRight, labelWidth);
            cboPaisFabricacion = new ComboBox
            {
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboPaisFabricacion.Items.AddRange(new[] {
                "Japón", "Alemania", "Estados Unidos", "Corea del Sur", "China",
                "India", "Brasil", "México", "España", "Francia", "Italia",
                "Reino Unido", "Tailandia", "Hungría", "República Checa",
                "Turquía", "Argentina", "Sudáfrica", "Australia", "Canadá"
            });
            cboPaisFabricacion.Sorted = true;
            pnlPrincipal.Controls.Add(cboPaisFabricacion);
            yPosRight += lineHeight;

            // Transmisión
            AgregarLabel(pnlPrincipal, "Transmisión:", 580, yPosRight, labelWidth);
            cboTransmision = new ComboBox
            {
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboTransmision.Items.AddRange(new[] { "Manual", "Automática", "CVT", "Semiautomática", "Doble Embrague" });
            pnlPrincipal.Controls.Add(cboTransmision);
            yPosRight += lineHeight;

            // Turbo
            chkTieneTurbo = new CheckBox
            {
                Text = "🚀 Tiene Turbo",
                Location = new Point(colRightX + 80, yPosRight + 3),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.FromArgb(220, 53, 69)
            };
            pnlPrincipal.Controls.Add(chkTieneTurbo);
            yPosRight += lineHeight;

            // Motor
            AgregarLabel(pnlPrincipal, "Motor:", 580, yPosRight, labelWidth);
            txtMotor = new TextBox
            {
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(txtMotor);
            SetCueBanner(txtMotor, "Ej: 2.0L, 1.8L Turbo");
            yPosRight += lineHeight;

            // Combustible
            AgregarLabel(pnlPrincipal, "Combustible:", 580, yPosRight, labelWidth);
            cboCombustible = new ComboBox
            {
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboCombustible.Items.AddRange(new[] { "Gasolina", "Diesel", "Híbrido", "Eléctrico", "GLP", "GNC" });
            pnlPrincipal.Controls.Add(cboCombustible);
            yPosRight += lineHeight;

            // ==================== SEGURIDAD ====================
            yPos = Math.Max(yPosLeft, yPosRight) + 15;
            AgregarSeccion(pnlPrincipal, "🛡️ SEGURIDAD", 20, yPos, 540);
            yPosLeft = yPos + 38;

            // Airbags
            AgregarLabel(pnlPrincipal, "Airbags:", 20, yPosLeft, labelWidth);
            nudAirbags = new NumericUpDown
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(100, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 12,
                Value = 2,
                TextAlign = HorizontalAlignment.Center
            };
            pnlPrincipal.Controls.Add(nudAirbags);
            yPosLeft += lineHeight;

            // Organización
            AgregarLabel(pnlPrincipal, "Organización:", 20, yPosLeft, labelWidth);
            cboOrganizacionSeguridad = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboOrganizacionSeguridad.Items.AddRange(new[] {
                "Latin NCAP", "Euro NCAP", "IIHS", "NHTSA", "ANCAP", "Sin calificación"
            });
            pnlPrincipal.Controls.Add(cboOrganizacionSeguridad);
            yPosLeft += lineHeight;

            // Estrellas
            AgregarLabel(pnlPrincipal, "Estrellas:", 20, yPosLeft, labelWidth);
            cboSeguridadChoques = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboSeguridadChoques.Items.AddRange(new[] {
                "⭐⭐⭐⭐⭐ (5 estrellas)",
                "⭐⭐⭐⭐ (4 estrellas)",
                "⭐⭐⭐ (3 estrellas)",
                "⭐⭐ (2 estrellas)",
                "⭐ (1 estrella)",
                "☆ (0 estrellas)",
                "Sin calificación"
            });
            pnlPrincipal.Controls.Add(cboSeguridadChoques);
            yPosLeft += lineHeight;

            // ==================== EVALUACIÓN ====================
            AgregarSeccion(pnlPrincipal, "⭐ EVALUACIÓN", 580, yPos, 580);
            yPosRight = yPos + 38;

            // Confiabilidad
            AgregarLabel(pnlPrincipal, "Confiabilidad:", 580, yPosRight, labelWidth);
            cboConfiabilidad = new ComboBox
            {
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
            pnlPrincipal.Controls.Add(cboConfiabilidad);
            yPosRight += lineHeight;

            // Puntuación
            AgregarLabel(pnlPrincipal, "Puntuación:", 580, yPosRight, labelWidth);
            nudPuntuacion = new NumericUpDown
            {
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(120, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1.0m,
                Maximum = 10.0m,
                DecimalPlaces = 1,
                Increment = 0.5m,
                Value = 7.5m,
                TextAlign = HorizontalAlignment.Center
            };
            pnlPrincipal.Controls.Add(nudPuntuacion);
            yPosRight += lineHeight;

            // Kilometraje
            //AgregarLabel(pnlPrincipal, "Kilometraje:", 580, yPosRight, labelWidth);
            //nudKilometraje = new NumericUpDown
            //{
            //    Location = new Point(colRightX + 80, yPosRight),
            //    Size = new Size(150, 28),
            //    Font = new Font("Segoe UI", 10F),
            //    Minimum = 0,
            //    Maximum = 999999,
            //    ThousandsSeparator = true,
            //    Value = 0,
            //    TextAlign = HorizontalAlignment.Right
            //};
            //pnlPrincipal.Controls.Add(nudKilometraje);

            //chkKilometrajeDesconocido = new CheckBox
            //{
            //    Text = "Desconocido",
            //    Location = new Point(colRightX + 240, yPosRight + 5),
            //    Font = new Font("Segoe UI", 9F),
            //    AutoSize = true
            //};
            //chkKilometrajeDesconocido.CheckedChanged += (s, e) =>
            //{
            //    nudKilometraje.Enabled = !chkKilometrajeDesconocido.Checked;
            //    if (chkKilometrajeDesconocido.Checked) nudKilometraje.Value = 0;
            //};
            //pnlPrincipal.Controls.Add(chkKilometrajeDesconocido);
            //yPosRight += lineHeight;

            // ==================== OBSERVACIONES ====================
            yPos = Math.Max(yPosLeft, yPosRight) + 15;
            AgregarLabel(pnlPrincipal, "Observaciones:", 20, yPos, labelWidth);
            txtObservaciones = new TextBox
            {
                Location = new Point(colLeftX, yPos),
                Size = new Size(990, 65),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(txtObservaciones);
            SetCueBanner(txtObservaciones, "Detalles adicionales del vehículo...");

            // ==================== FOTOGRAFÍA ====================
            yPos = Math.Max(yPosLeft, yPosRight) + 80;
            AgregarSeccion(pnlPrincipal, "📷 FOTOGRAFÍA DEL VEHÍCULO", 20, yPos, 1140);
            yPos += 40;

            // PictureBox para mostrar la foto
            pbFotografia = new PictureBox
            {
                Location = new Point(40, yPos),
                Size = new Size(400, 300),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250),
                SizeMode = PictureBoxSizeMode.Zoom,
                AllowDrop = true  // ⭐ HABILITAR DRAG & DROP
            };

            // ⭐ EVENTOS DE DRAG & DROP
            pbFotografia.DragEnter += PbFotografia_DragEnter;
            pbFotografia.DragDrop += PbFotografia_DragDrop;
            pbFotografia.Paint += PbFotografia_Paint;  // Para dibujar instrucciones

            pnlPrincipal.Controls.Add(pbFotografia);

            // Panel de controles de foto
            Panel pnlControlesFoto = new Panel
            {
                Location = new Point(460, yPos),
                Size = new Size(700, 300),
                BackColor = Color.White
            };

            Label lblInstrucciones = new Label
            {
                Text = "📋 CÓMO AGREGAR UNA FOTOGRAFÍA:\n\n" +
                       "1️⃣ Arrastra una imagen desde tu explorador de archivos\n" +
                       "2️⃣ O haz clic en 'Seleccionar Archivo' para buscar\n\n" +
                       "✅ Formatos aceptados: JPG, PNG, BMP, GIF\n" +
                       "⚠️ Tamaño máximo recomendado: 5 MB\n" +
                       "💡 La imagen se optimizará automáticamente",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(10, 10),
                Size = new Size(680, 150),
                TextAlign = ContentAlignment.TopLeft
            };
            pnlControlesFoto.Controls.Add(lblInstrucciones);

            // Botones de control
            btnSeleccionarFoto = new Button
            {
                Text = "📂 Seleccionar Archivo",
                Size = new Size(200, 40),
                Location = new Point(10, 170), // Fixed: was 170-300 which placed the button outside the panel
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSeleccionarFoto.FlatAppearance.BorderSize = 0;
            btnSeleccionarFoto.Click += BtnSeleccionarFoto_Click;
            pnlControlesFoto.Controls.Add(btnSeleccionarFoto);
            btnSeleccionarFoto.BringToFront();

            btnEliminarFoto = new Button
            {
                Text = "🗑️ Quitar Foto",
                Size = new Size(200, 40),
                Location = new Point(220, 170),
                BackColor = Color.FromArgb(217, 83, 79),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnEliminarFoto.FlatAppearance.BorderSize = 0;
            btnEliminarFoto.Click += (s, e) => EliminarFoto();
            pnlControlesFoto.Controls.Add(btnEliminarFoto);

            // Label de información
            lblFotoInfo = new Label
            {
                Text = "📷 Sin fotografía seleccionada",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(10, 220),
                Size = new Size(680, 50),
                TextAlign = ContentAlignment.TopLeft
            };
            pnlControlesFoto.Controls.Add(lblFotoInfo);

            pnlPrincipal.Controls.Add(pnlControlesFoto);


            // Panel de botones
            Panel pnlBotones = new Panel
            {
                Location = new Point(0, 750+250),
                Size = new Size(1184, 70+30),
                BackColor = Color.FromArgb(52, 58, 64),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnGuardar = new Button
            {
                Text = "💾 Guardar Vehículo",
                Size = new Size(200, 45),
                Location = new Point(450-90, 12),
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            Button btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Size = new Size(200, 45),
                Location = new Point(670-50, 12),
                BackColor = Color.FromArgb(217, 83, 79),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            pnlBotones.Controls.Add(btnGuardar);
            pnlBotones.Controls.Add(btnCancelar);

            pnlContenedor.Controls.Add(pnlPrincipal);
            pnlContenedor.Controls.Add(pnlBotones);
            this.Load += (s, e) =>
            {
                pnlBotones.Top = pnlPrincipal.Bottom + 20;
            };

            this.Shown += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    pnlContenedor.VerticalScroll.Value = 0;
                }));
            };


        }

        private void AgregarSeccion(Panel panel, string titulo, int x, int yPos, int width)
        {
            Panel pnlSeccion = new Panel
            {
                Location = new Point(x, yPos),
                Size = new Size(width, 32),
                BackColor = Color.FromArgb(233, 236, 239)
            };

            Label lblSeccion = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(10, 7),
                AutoSize = true
            };
            pnlSeccion.Controls.Add(lblSeccion);
            panel.Controls.Add(pnlSeccion);
        }

        private void AgregarLabel(Panel panel, string texto, int x, int yPos, int width)
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

        private void FrmRegistroVehiculo_Load(object sender, EventArgs e)
        {
            CargarProcedencias();
            CargarTipos();
        }

        private void CargarProcedencias()
        {
            try
            {
                procedencias = ProcedenciaCln.listar();
                cboProcedencia.DataSource = procedencias;
                cboProcedencia.DisplayMember = "descripcion";
                cboProcedencia.ValueMember = "id";
                cboProcedencia.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar procedencias: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTipos()
        {
            try
            {
                tipos = TipoVehiculoCln.listar();
                cboTipo.DataSource = tipos;
                cboTipo.DisplayMember = "descripcion";
                cboTipo.ValueMember = "id";
                cboTipo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar tipos: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboProcedencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProcedencia.SelectedValue is int idProcedencia)
            {
                try
                {
                    marcas = MarcaCln.listar().Where(m => m.idProcedencia == idProcedencia).ToList();
                    cboMarca.DataSource = marcas;
                    cboMarca.DisplayMember = "nombre";
                    cboMarca.ValueMember = "id";
                    cboMarca.Enabled = true;
                    btnAgregarMarca.Enabled = true;
                    cboMarca.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar marcas: {ex.Message}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMarca.SelectedValue is int idMarca)
            {
                try
                {
                    // SIMPLIFICADO: Solo cargamos modelos (solo nombre)
                    modelos = ModeloVehiculoCln.listar().Where(m => m.idMarca == idMarca && m.estado != -1).ToList();

                    cboModelo.DataSource = modelos;
                    cboModelo.DisplayMember = "nombre"; // Solo nombre
                    cboModelo.ValueMember = "id";
                    cboModelo.Enabled = true;
                    btnAgregarModelo.Enabled = true;
                    cboModelo.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar modelos: {ex.Message}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAgregarMarca_Click(object sender, EventArgs e)
        {
            if (cboProcedencia.SelectedValue is int idProcedencia)
            {
                using (var frmNuevaMarca = new FrmNuevaMarca(idProcedencia))
                {
                    if (frmNuevaMarca.ShowDialog(this) == DialogResult.OK)
                    {
                        CboProcedencia_SelectedIndexChanged(null, null);
                        cboMarca.SelectedValue = frmNuevaMarca.NuevoIdMarca;
                    }
                }
            }
        }

        private void BtnAgregarModelo_Click(object sender, EventArgs e)
        {
            if (cboMarca.SelectedValue is int idMarca && cboTipo.SelectedValue is int idTipo)
            {
                // SIMPLIFICADO: Solo pasamos marca y tipo
                using (var frmNuevoModelo = new FrmNuevoModelo(idMarca, idTipo))
                {
                    if (frmNuevoModelo.ShowDialog(this) == DialogResult.OK)
                    {
                        CboMarca_SelectedIndexChanged(null, null);
                        cboModelo.SelectedValue = frmNuevoModelo.NuevoIdModelo;
                    }
                }
            }
            else
            {
                MessageBox.Show("⚠️ Debe seleccionar una Marca y un Tipo de Vehículo primero",
                              "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool Validar()
        {
            if (cboProcedencia.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar una procedencia", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboProcedencia.Focus();
                return false;
            }

            if (cboMarca.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar una marca", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMarca.Focus();
                return false;
            }

            if (cboTipo.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar un tipo de vehículo", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTipo.Focus();
                return false;
            }

            if (cboModelo.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar un modelo", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboModelo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNroChasis.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar el número de chasis", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNroChasis.Focus();
                return false;
            }

            if (txtNroChasis.Text.Length != 17)
            {
                MessageBox.Show("⚠️ El número de chasis debe tener exactamente 17 caracteres", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNroChasis.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGeneracion.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar la generación", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGeneracion.Focus();
                return false;
            }

            if (cboPaisFabricacion.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el país de fabricación", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPaisFabricacion.Focus();
                return false;
            }

            if (cboTransmision.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar la transmisión", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTransmision.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMotor.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar el tipo de motor", "Validación",
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

            if (cboConfiabilidad.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el nivel de confiabilidad", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboConfiabilidad.Focus();
                return false;
            }

            return true;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    int idModelo = (int)cboModelo.SelectedValue;
                    int anio = (int)nudAnio.Value;
                    decimal puntuacion = nudPuntuacion.Value;
                    //int? kilometraje = chkKilometrajeDesconocido.Checked ? (int?)null : (int)nudKilometraje.Value;

                    // Extraer estrellas
                    int estrellas = 0;
                    if (cboSeguridadChoques.SelectedIndex < 6)
                    {
                        estrellas = 5 - cboSeguridadChoques.SelectedIndex;
                    }

                    // Convertir imagen a bytes si existe
                    byte[] fotografiaBytes = null;
                    if (imagenActual != null)
                    {
                        try
                        {
                            fotografiaBytes = ImagenHelper.ConvertirImagenABytes(imagenActual);
                            double tamanoKB = ImagenHelper.ObtenerTamanoKB(fotografiaBytes);

                            // Advertir si la imagen es muy grande
                            if (tamanoKB > 2048) // Más de 2MB
                            {
                                DialogResult result = MessageBox.Show(
                                    $"⚠️ La imagen optimizada pesa {Math.Round(tamanoKB / 1024, 2)} MB.\n\n" +
                                    $"Se recomienda usar imágenes más pequeñas.\n" +
                                    $"¿Desea continuar?",
                                    "Imagen Grande",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);

                                if (result != DialogResult.Yes)
                                    return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"❌ Error al procesar la fotografía:\n{ex.Message}\n\n" +
                                          "El vehículo se guardará sin fotografía.",
                                          "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            fotografiaBytes = null;
                        }
                    }

                    // NUEVO: Vehículo con TODAS las características
                    var nuevoVehiculo = new Vehiculo
                    {
                        nroChasis = txtNroChasis.Text.Trim().ToUpper(),
                        idModeloVehiculo = idModelo,
                        anioFabricacion = anio,
                        generacion = txtGeneracion.Text.Trim(),
                        paisFabricacion = cboPaisFabricacion.SelectedItem.ToString(),
                        transmision = cboTransmision.SelectedItem.ToString(),
                        tieneTurbo = chkTieneTurbo.Checked,
                        motor = txtMotor.Text.Trim(),
                        combustible = cboCombustible.SelectedItem.ToString(),
                        nroAirbags = (int)nudAirbags.Value,
                        seguridadChoques = estrellas,
                        organizacionSeguridad = cboOrganizacionSeguridad.SelectedItem?.ToString() ?? "Sin calificación",
                        confiabilidad = cboConfiabilidad.SelectedItem.ToString(),
                        puntuacion = puntuacion,
                        observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim(),
                        fotografia = fotografiaBytes,  // ⭐ AGREGAMOS ESTA LÍNEA
                        estado = 1,
                        usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : Environment.UserName,
                        fechaRegistro = DateTime.Now
                    };


                    int resultado = VehiculoCln.insertar(nuevoVehiculo);

                    if (resultado > 0)
                    {
                        string turboInfo = chkTieneTurbo.Checked ? " 🚀 CON TURBO" : "";
                        string modeloNombre = cboModelo.Text;

                        MessageBox.Show($"✅ Vehículo registrado exitosamente\n\n" +
                                      $"Chasis: {txtNroChasis.Text}\n" +
                                      $"Modelo: {modeloNombre}\n" +
                                      $"Generación: {txtGeneracion.Text}\n" +
                                      $"País: {cboPaisFabricacion.Text}\n" +
                                      $"Transmisión: {cboTransmision.Text}{turboInfo}",
                                      "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al guardar el vehículo:\n\n{ex.Message}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;

        private void SetCueBanner(TextBox tb, string text)
        {
            if (tb == null) return;
            try
            {
                SendMessage(tb.Handle, EM_SETCUEBANNER, (IntPtr)1, text);
            }
            catch { }
        }


        // Evento cuando se arrastra algo sobre el PictureBox
        private void PbFotografia_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] archivos = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (archivos.Length > 0 && ImagenHelper.EsImagenValida(archivos[0]))
                {
                    e.Effect = DragDropEffects.Copy;
                    pbFotografia.BackColor = Color.FromArgb(220, 255, 220); // Verde claro
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    pbFotografia.BackColor = Color.FromArgb(255, 220, 220); // Rojo claro
                }
            }
        }

        // Evento cuando se suelta el archivo en el PictureBox
        private void PbFotografia_DragDrop(object sender, DragEventArgs e)
        {
            pbFotografia.BackColor = Color.FromArgb(250, 250, 250); // Restaurar color

            try
            {
                string[] archivos = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (archivos.Length > 0)
                {
                    CargarImagen(archivos[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al cargar la imagen:\n{ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Dibuja instrucciones cuando no hay imagen
        private void PbFotografia_Paint(object sender, PaintEventArgs e)
        {
            if (pbFotografia.Image == null)
            {
                string texto = "📷\n\nArrastra aquí\nuna imagen\n\n🖱️ o haz clic en\n'Seleccionar Archivo'";
                using (Font font = new Font("Segoe UI", 12, FontStyle.Bold))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(texto, font, Brushes.Gray,
                        new RectangleF(0, 0, pbFotografia.Width, pbFotografia.Height), sf);
                }

                // Dibujar borde punteado
                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, 5, 5, pbFotografia.Width - 10, pbFotografia.Height - 10);
                }
            }
        }

        // Botón para seleccionar archivo manualmente
        private void BtnSeleccionarFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Seleccionar Fotografía del Vehículo";
                ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos los archivos|*.*";
                ofd.FilterIndex = 1;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        CargarImagen(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al cargar la imagen:\n{ex.Message}",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Carga una imagen desde un archivo
        private void CargarImagen(string rutaArchivo)
        {
            if (!ImagenHelper.EsImagenValida(rutaArchivo))
            {
                MessageBox.Show("⚠️ El archivo seleccionado no es una imagen válida",
                              "Archivo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Liberar imagen anterior si existe
            if (imagenActual != null)
            {
                imagenActual.Dispose();
                imagenActual = null;
            }

            // Cargar nueva imagen
            imagenActual = ImagenHelper.CargarImagenDesdeArchivo(rutaArchivo);
            ImagenHelper.AjustarImagenPictureBox(pbFotografia, imagenActual);

            // Actualizar información
            FileInfo fi = new FileInfo(rutaArchivo);
            double tamanoMB = Math.Round(fi.Length / (1024.0 * 1024.0), 2);

            lblFotoInfo.Text = $"✅ Imagen cargada: {Path.GetFileName(rutaArchivo)}\n" +
                              $"📏 Dimensiones: {imagenActual.Width} x {imagenActual.Height} px\n" +
                              $"💾 Tamaño original: {tamanoMB} MB";
            lblFotoInfo.ForeColor = Color.Green;

            btnEliminarFoto.Enabled = true;

            // Forzar repintado
            pbFotografia.Invalidate();
        }

        // Elimina la foto cargada
        private void EliminarFoto()
        {
            if (imagenActual != null)
            {
                imagenActual.Dispose();
                imagenActual = null;
            }

            pbFotografia.Image = null;
            lblFotoInfo.Text = "📷 Sin fotografía seleccionada";
            lblFotoInfo.ForeColor = Color.Gray;
            btnEliminarFoto.Enabled = false;

            // Forzar repintado para mostrar instrucciones
            pbFotografia.Invalidate();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmRegistroVehiculo
            // 
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(1162, 994);
            this.Name = "FrmRegistroVehiculo";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.FrmRegistroVehiculo_Load);
            this.ResumeLayout(false);

        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (imagenActual != null)
        //        {
        //            imagenActual.Dispose();
        //            imagenActual = null;
        //        }
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
