using CadAuton;
using ClnAuton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpAuton
{
    public partial class FrmDetallesVehiculo : Form
    {
        private paVehiculoListar_Result vehiculoResumen;
        private Vehiculo vehiculoCompleto;
        private Action onVehiculoActualizado;

        private List<Procedencia> procedencias;
        private List<Marca> marcas;
        private List<ModeloVehiculo> modelos;
        private List<TipoVehiculo> tipos;

        private Panel pnlPrincipal;
        private PictureBox pbFotografia;
        private Button btnCambiarFoto;
        private Button btnEliminarFotoDetalle;
        private Button btnVerFotoGrande;
        private Button btnBuscarFotoWeb;
        private Button btnEditar;
        private Button btnEliminar;
        private Image imagenCargada = null;
        private bool fotoEliminada = false;
        private bool fotoExisteEnBD = false;
        private bool editarActivo = false;
        private bool hayCambios = false;
        private Dictionary<string, object> valoresOriginales = new Dictionary<string, object>();
        private Action verificarCambios;

        // Referencias a controles para acceso global
        private TextBox txtChasis;
        private ComboBox cboProcedencia;
        private ComboBox cboMarca;
        private ComboBox cboModelo;
        private ComboBox cboTipo;
        private ComboBox cboVersion;
        private NumericUpDown nudAnio;
        private TextBox txtGeneracion;
        private ComboBox cboPaisFabricacion;
        private ComboBox cboTransmision;
        private NumericUpDown nudVelocidades;
        private Label lblInfoVelocidades;
        private CheckBox chkTieneTurbo;
        private TextBox txtMotor;
        private ComboBox cboCombustible;
        private ComboBox cboFrenos;
        private NumericUpDown nudAsientos;
        private ComboBox cboPuertas;
        private ComboBox cboTraccion;
        private NumericUpDown nudAirbags;
        private ComboBox cboOrganizacion;
        private ComboBox cboEstrellas;
        private ComboBox cboConfiabilidad;
        private NumericUpDown nudPuntuacion;
        private TextBox txtObservaciones;
        private Button btnAddMarca;
        private Button btnAddModelo;
        private Button btnEditMarca;
        private Button btnEditModelo;
        private bool permitirEventos = false;

        public FrmDetallesVehiculo(paVehiculoListar_Result vehiculo, Action onActualizado = null)
        {
            InitializeComponent();
            this.vehiculoResumen = vehiculo;
            this.onVehiculoActualizado = onActualizado;

            this.vehiculoCompleto = ConsultaVehiculosCln.obtenerUno(vehiculo.id);

            if (this.vehiculoCompleto == null)
            {
                MessageBox.Show("❌ No se pudo cargar el vehículo", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = $"Detalles del Vehículo - {vehiculoResumen.marca} {vehiculoResumen.modelo}";
            //this.Size = new Size(1200, 850);
            //this.StartPosition = FormStartPosition.CenterParent;
            //this.FormBorderStyle = FormBorderStyle.FixedDialog;
            //this.MaximizeBox = false;
            //this.MinimizeBox = false;
            //this.Font = new Font("Segoe UI", 10F);
            //this.AutoScroll = true;
            //this.ShowInTaskbar = false;

            pnlPrincipal = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1184, 700 - 120),
                Padding = new Padding(20),
                BackColor = Color.PapayaWhip,
                AutoScroll = true
            };
            this.Controls.Add(pnlPrincipal);
            // EVENTO SHOWN PARA RESETEAR SCROLL AL INICIO
            this.Shown += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (pnlPrincipal != null && pnlPrincipal.AutoScroll)
                    {
                        pnlPrincipal.AutoScrollPosition = new Point(0, 0);
                    }
                }));
            };

            CrearInterfaz();
            CargarDatos();
        }

        private void CrearInterfaz()
        {
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
                Name = "lblEstado",
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

            // ==================== FOTOGRAFÍA ====================
            yPos += 35;
            Panel pnlFotografia = new Panel
            {
                Location = new Point(20 + 575, yPos + 500 + 100 + 45 + 90),
                Size = new Size(540, 320),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTituloFoto = new Label
            {
                Text = "📷 FOTOGRAFÍA DEL VEHÍCULO",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(10, 10),
                Size = new Size(520, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlFotografia.Controls.Add(lblTituloFoto);

            pbFotografia = new PictureBox
            {
                Location = new Point(10, 45),
                Size = new Size(520, 220),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250),
                SizeMode = PictureBoxSizeMode.Zoom,
                AllowDrop = false
            };

            pbFotografia.DragEnter += (s, ev) =>
            {
                if (pbFotografia.AllowDrop && ev.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] archivos = (string[])ev.Data.GetData(DataFormats.FileDrop);
                    if (archivos.Length > 0 && ImagenHelper.EsImagenValida(archivos[0]))
                    {
                        ev.Effect = DragDropEffects.Copy;
                        pbFotografia.BackColor = Color.FromArgb(220, 255, 220);
                    }
                }
            };

            pbFotografia.DragDrop += (s, ev) =>
            {
                pbFotografia.BackColor = Color.FromArgb(250, 250, 250);
                if (pbFotografia.AllowDrop && ev.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    try
                    {
                        string[] archivos = (string[])ev.Data.GetData(DataFormats.FileDrop);
                        if (archivos.Length > 0)
                        {
                            if (imagenCargada != null) imagenCargada.Dispose();
                            imagenCargada = ImagenHelper.CargarImagenDesdeArchivo(archivos[0]);
                            ImagenHelper.AjustarImagenPictureBox(pbFotografia, imagenCargada);
                            btnEliminarFotoDetalle.Enabled = true;
                            btnCambiarFoto.Text = "📂 Cambiar Foto";
                            btnVerFotoGrande.Visible = true;
                            fotoEliminada = false;
                            verificarCambios();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar imagen: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            pnlFotografia.Controls.Add(pbFotografia);

            // Botones de control de foto
            Panel pnlBotonesFoto = new Panel
            {
                Location = new Point(10, 275),
                Size = new Size(520, 35)
            };

            btnCambiarFoto = new Button
            {
                Text = "📂 Cambiar Foto",
                Size = new Size(150, 30),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Visible = false
            };
            btnCambiarFoto.FlatAppearance.BorderSize = 0;
            btnCambiarFoto.Click += (s, ev) =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (imagenCargada != null) imagenCargada.Dispose();
                            imagenCargada = ImagenHelper.CargarImagenDesdeArchivo(ofd.FileName);
                            ImagenHelper.AjustarImagenPictureBox(pbFotografia, imagenCargada);
                            btnEliminarFotoDetalle.Enabled = true;
                            btnCambiarFoto.Text = "📂 Cambiar Foto";
                            btnVerFotoGrande.Visible = true;
                            fotoEliminada = false;
                            verificarCambios();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };
            pnlBotonesFoto.Controls.Add(btnCambiarFoto);

            btnEliminarFotoDetalle = new Button
            {
                Text = "🗑️ Quitar Foto",
                Size = new Size(150, 30),
                Location = new Point(160, 0),
                BackColor = Color.FromArgb(217, 83, 79),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Visible = false,
                Enabled = false
            };
            btnEliminarFotoDetalle.FlatAppearance.BorderSize = 0;
            btnEliminarFotoDetalle.Click += (s, ev) =>
            {
                if (imagenCargada != null) imagenCargada.Dispose();
                imagenCargada = null;
                pbFotografia.Image = null;
                ImagenHelper.AjustarImagenPictureBox(pbFotografia, null);
                btnEliminarFotoDetalle.Enabled = false;
                btnVerFotoGrande.Visible = false;
                btnCambiarFoto.Text = "📂 Añadir Foto";
                fotoEliminada = true;
                verificarCambios();
                fotoExisteEnBD = false;
            };
            pnlBotonesFoto.Controls.Add(btnEliminarFotoDetalle);

            btnVerFotoGrande = new Button
            {
                Name = "btnVerFotoGrande",
                Text = "👁️ Ver Foto",
                Size = new Size(120, 30),
                Location = new Point(320, 0),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Visible = false
            };
            btnVerFotoGrande.FlatAppearance.BorderSize = 0;
            btnVerFotoGrande.Click += (s, ev) =>
            {
                try
                {
                    if (imagenCargada != null)
                    {
                        using (var frmFull = new FullscreenImageForm((Image)imagenCargada.Clone()))
                        {
                            frmFull.ShowDialog(this);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay fotografía disponible para mostrar.", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al mostrar fotografía: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            pnlBotonesFoto.Controls.Add(btnVerFotoGrande);

            btnBuscarFotoWeb = new Button
            {
                Name = "btnBuscarFotoWeb",
                Text = "🌐 Buscar Web",
                Size = new Size(150, 30),
                Location = new Point(320, 0),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = true
            };
            btnBuscarFotoWeb.FlatAppearance.BorderSize = 0;
            btnBuscarFotoWeb.Click += BtnBuscarFotoWeb_Click;
            pnlBotonesFoto.Controls.Add(btnBuscarFotoWeb);

            pnlFotografia.Controls.Add(pnlBotonesFoto);
            pnlPrincipal.Controls.Add(pnlFotografia);

            // ==================== COLUMNA IZQUIERDA ====================
            AgregarSeccion(pnlPrincipal, "📋 IDENTIFICACIÓN DEL MODELO", 20, yPos, 540);
            int yPosLeft = yPos + 38;

            // Nro de Chasis
            AgregarLabel(pnlPrincipal, "Nro. Chasis:", 20, yPosLeft, labelWidth);
            txtChasis = new TextBox
            {
                Name = "txtChasis",
                Location = new Point(colLeftX, yPosLeft),
                //Size = new Size(ctrlWidth - 44, 28),
                Size = new Size(ctrlWidth, 28),
                Font = new Font("Segoe UI", 10F),
                Text = vehiculoCompleto.nroChasis,
                CharacterCasing = CharacterCasing.Upper,
                MaxLength = 17,
                ReadOnly = true,
                BackColor = Color.WhiteSmoke
            };
            pnlPrincipal.Controls.Add(txtChasis);
            yPosLeft += lineHeight;

            // Procedencia
            Label lblProcedencia = new Label
            {
                Name = "lblProcedencia",
                Text = "Procedencia:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, yPosLeft + 3),
                Size = new Size(labelWidth, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(33, 37, 41)
            };
            pnlPrincipal.Controls.Add(lblProcedencia);

            cboProcedencia = new ComboBox
            {
                Name = "cboProcedencia",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboProcedencia.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            pnlPrincipal.Controls.Add(cboProcedencia);
            yPosLeft += lineHeight;

            // Marca
            AgregarLabel(pnlPrincipal, "Marca:", 20, yPosLeft, labelWidth);
            cboMarca = new ComboBox
            {
                Name = "cboMarca",
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboMarca.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };

            btnAddMarca = new Button
            {
                Name = "btnAddMarca",
                Text = "+",
                Size = new Size(30, 28),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            btnAddMarca.FlatAppearance.BorderSize = 0;
            btnAddMarca.Click += BtnAddMarca_Click;

            btnEditMarca = new Button
            {
                Name = "btnEditMarca",
                Text = "✏️",
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(255, 193, 7),
                Margin = new Padding(8, 0, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false,
                Enabled = true
            };
            btnEditMarca.FlatAppearance.BorderSize = 0;
            btnEditMarca.Click += BtnEditMarca_Click;

            Button btnVerMarcaDesc = new Button
            {
                Name = "btnVerMarcaDesc",
                Text = "👁️",
                Size = new Size(36, 28),
                Margin = new Padding(8, 0, 0, 0),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = true
            };
            btnVerMarcaDesc.FlatAppearance.BorderSize = 0;
            btnVerMarcaDesc.Click += BtnVerMarcaDesc_Click;

            FlowLayoutPanel flpMarcaRow = new FlowLayoutPanel
            {
                Name = "flpMarcaRow",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth + 50 + 35, 28),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false,
            };
            flpMarcaRow.Controls.Add(cboMarca);
            flpMarcaRow.Controls.Add(btnAddMarca);
            flpMarcaRow.Controls.Add(btnEditMarca);
            flpMarcaRow.Controls.Add(btnVerMarcaDesc);
            pnlPrincipal.Controls.Add(flpMarcaRow);
            cboMarca.TextChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosLeft += lineHeight;

            // Modelo
            AgregarLabel(pnlPrincipal, "Modelo:", 20, yPosLeft, labelWidth);
            cboModelo = new ComboBox
            {
                Name = "cboModelo",
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboModelo.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };

            btnAddModelo = new Button
            {
                Name = "btnAddModelo",
                Text = "+",
                Size = new Size(30, 28),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            btnAddModelo.FlatAppearance.BorderSize = 0;
            btnAddModelo.Click += BtnAddModelo_Click;

            btnEditModelo = new Button
            {
                Name = "btnEditModelo",
                Text = "✏️",
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(255, 193, 7),
                Margin = new Padding(8, 0, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false,
                Enabled = true
            };
            btnEditModelo.FlatAppearance.BorderSize = 0;
            btnEditModelo.Click += BtnEditModelo_Click;

            Button btnVerModeloDesc = new Button
            {
                Name = "btnVerModeloDesc",
                Text = "👁️",
                Size = new Size(36, 28),
                Margin = new Padding(8, 0, 0, 0),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = true
            };
            btnVerModeloDesc.FlatAppearance.BorderSize = 0;
            btnVerModeloDesc.Click += BtnVerModeloDesc_Click;

            FlowLayoutPanel flpModeloRow = new FlowLayoutPanel
            {
                Name = "flpModeloRow",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth + 50 + 35, 28),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false
            };
            flpModeloRow.Controls.Add(cboModelo);
            flpModeloRow.Controls.Add(btnAddModelo);
            flpModeloRow.Controls.Add(btnEditModelo);
            flpModeloRow.Controls.Add(btnVerModeloDesc);
            pnlPrincipal.Controls.Add(flpModeloRow);
            cboModelo.SelectedIndexChanged += (s, e) => ActualizarBotonesBusqueda();
            cboModelo.TextChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosLeft += lineHeight;

            // Versión
            AgregarLabel(pnlPrincipal, "Versión:", 20, yPosLeft, labelWidth);
            cboVersion = new ComboBox
            {
                Name = "cboVersion",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboVersion.Items.AddRange(new[] { "Básica", "Intermedia", "Full" });
            cboVersion.Text = vehiculoCompleto.version;
            cboVersion.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            pnlPrincipal.Controls.Add(cboVersion);

            Button btnBuscarVersion = new Button
            {
                Name = "btnBuscarVersion",
                Text = "🔍",
                Location = new Point(colLeftX + ctrlWidth + 8 + 6, yPosLeft),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscarVersion.FlatAppearance.BorderSize = 0;
            btnBuscarVersion.Click += BtnBuscarVersion_Click;
            try { var ttV = new ToolTip(); ttV.SetToolTip(btnBuscarVersion, "Buscar información de la versión"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarVersion);
            btnBuscarVersion.BringToFront();
            cboVersion.SelectedIndexChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosLeft += lineHeight + 20;

            // Tipo
            AgregarLabel(pnlPrincipal, "Tipo:", 20, yPosLeft, labelWidth);
            cboTipo = new ComboBox
            {
                Name = "cboTipo",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboTipo.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            pnlPrincipal.Controls.Add(cboTipo);
            yPosLeft += lineHeight;

            // Año
            AgregarLabel(pnlPrincipal, "Año Fabricación:", 20, yPosLeft, labelWidth);
            nudAnio = new NumericUpDown
            {
                Name = "nudAnio",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(120, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = vehiculoCompleto.anioFabricacion,
                TextAlign = HorizontalAlignment.Center,
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            try
            {
                var tb = nudAnio.Controls.OfType<TextBox>().FirstOrDefault();
                if (tb != null)
                {
                    tb.ReadOnly = true;
                    tb.BackColor = Color.WhiteSmoke;
                }
            }
            catch { }
            pnlPrincipal.Controls.Add(nudAnio);
            nudAnio.ValueChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosLeft += lineHeight;

            // Agregar el botón de búsqueda del chasis aquí (después de declarar nudAnio y antes de usarlo)
            Button btnBuscarChasis = new Button
            {
                Name = "btnBuscarChasis",
                Text = "🔍",
                //Location = new Point(colLeftX + (ctrlWidth - 44) + 8, yPosLeft - (lineHeight * 6)), // Posición al lado del txtChasis
                Location = new Point(colLeftX + ctrlWidth - 44 + 8 + 50, yPosLeft - lineHeight - 290), // Posición al lado del txtChasis
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = true
            };
            btnBuscarChasis.FlatAppearance.BorderSize = 0;
            btnBuscarChasis.Click += BtnBuscarChasis_Click;
            try
            {
                var ttChasis = new ToolTip();
                ttChasis.SetToolTip(btnBuscarChasis, "Buscar información sobre dígitos del VIN/Chasis");
            }
            catch { }
            pnlPrincipal.Controls.Add(btnBuscarChasis);
            btnBuscarChasis.BringToFront();

            // ==================== COLUMNA DERECHA ====================
            AgregarSeccion(pnlPrincipal, "⚙️ CARACTERÍSTICAS ESPECÍFICAS", 590, yPos, 560);
            int yPosRight = yPos + 38;

            // Generación
            AgregarLabel(pnlPrincipal, "Generación:", 590, yPosRight, labelWidth);
            txtGeneracion = new TextBox
            {
                Name = "txtGeneracion",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                Font = new Font("Segoe UI", 10F),
                Text = vehiculoCompleto.generacion,
                ReadOnly = true,
                BackColor = Color.WhiteSmoke
            };
            pnlPrincipal.Controls.Add(txtGeneracion);

            Button btnBuscarGeneracion = new Button
            {
                Name = "btnBuscarGeneracion",
                Text = "🔍",
                Location = new Point(colRightX + 80 + ctrlWidth + 8, yPosRight),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscarGeneracion.FlatAppearance.BorderSize = 0;
            btnBuscarGeneracion.Click += BtnBuscarGeneracion_Click;
            try { var ttG = new ToolTip(); ttG.SetToolTip(btnBuscarGeneracion, "Buscar generación en la web"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarGeneracion);
            txtGeneracion.BringToFront();
            yPosRight += lineHeight;

            // País de Fabricación
            AgregarLabel(pnlPrincipal, "País Fabricación:", 590, yPosRight, labelWidth);
            cboPaisFabricacion = new ComboBox
            {
                Name = "cboPaisFabricacion",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboPaisFabricacion.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboPaisFabricacion.Items.AddRange(new[] {
                "Japón", "Alemania", "Estados Unidos", "Corea del Sur", "China",
                "India", "Brasil", "México", "España", "Francia", "Italia",
                "Reino Unido", "Tailandia", "Hungría", "República Checa",
                "Turquía", "Argentina", "Sudáfrica", "Australia", "Canadá"
            });
            cboPaisFabricacion.Text = vehiculoCompleto.paisFabricacion;
            pnlPrincipal.Controls.Add(cboPaisFabricacion);

            Button btnBuscarPaisFabricacion = new Button
            {
                Name = "btnBuscarPaisFabricacion",
                Text = "🔍",
                Location = new Point(colRightX + 80 + ctrlWidth + 8, yPosRight),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscarPaisFabricacion.FlatAppearance.BorderSize = 0;
            btnBuscarPaisFabricacion.Click += BtnBuscarPaisFabricacion_Click;
            try { var ttpPais = new ToolTip(); ttpPais.SetToolTip(btnBuscarPaisFabricacion, "Verificar país de fabricación"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarPaisFabricacion);
            cboPaisFabricacion.BringToFront();
            btnBuscarPaisFabricacion.BringToFront();
            cboPaisFabricacion.SelectedIndexChanged += (s, e) => ActualizarBotonesBusqueda();
            cboPaisFabricacion.TextChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosRight += lineHeight;

            // Transmisión
            AgregarLabel(pnlPrincipal, "Transmisión:", 590, yPosRight, labelWidth);
            cboTransmision = new ComboBox
            {
                Name = "cboTransmision",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboTransmision.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboTransmision.Items.AddRange(new[] { "Manual", "Automática", "CVT", "Semiautomática", "Doble Embrague" });
            cboTransmision.Text = vehiculoCompleto.transmision;
            pnlPrincipal.Controls.Add(cboTransmision);
            cboTransmision.BringToFront();
            yPosRight += lineHeight;

            // Velocidades
            AgregarLabel(pnlPrincipal, "Velocidades:", 590, yPosRight, labelWidth);
            nudVelocidades = new NumericUpDown
            {
                Name = "nudVelocidades",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(100, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 4,
                Maximum = 10,
                Value = vehiculoCompleto.nroVelocidades ?? 5,
                TextAlign = HorizontalAlignment.Center,
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            try
            {
                var tbv = nudVelocidades.Controls.OfType<TextBox>().FirstOrDefault();
                if (tbv != null)
                {
                    tbv.ReadOnly = true;
                    tbv.BackColor = Color.WhiteSmoke;
                }
            }
            catch { }

            lblInfoVelocidades = new Label
            {
                Name = "lblInfoVelocidades",
                Location = new Point(colRightX + 190 + 35, yPosRight + 3),
                Size = new Size(180, 25),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = true
            };

            pnlPrincipal.Controls.Add(nudVelocidades);
            pnlPrincipal.Controls.Add(lblInfoVelocidades);

            Button btnBuscarVelocidades = new Button
            {
                Name = "btnBuscarVelocidades",
                Text = "🔍",
                Location = new Point(colRightX + 80 + 100 + 8, yPosRight),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscarVelocidades.FlatAppearance.BorderSize = 0;
            btnBuscarVelocidades.Click += BtnBuscarVelocidades_Click;
            try { var ttVel = new ToolTip(); ttVel.SetToolTip(btnBuscarVelocidades, "Buscar información sobre velocidades"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarVelocidades);
            nudVelocidades.BringToFront();
            btnBuscarVelocidades.BringToFront();
            //nudVelocidades.ValueChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosRight += lineHeight;

            ConfigurarVelocidades(cboTransmision.Text, nudVelocidades, lblInfoVelocidades, vehiculoCompleto.nroVelocidades, false);

            cboTransmision.SelectedIndexChanged += (s, ev) =>
            {
                if (cboTransmision.Enabled)
                {
                    ConfigurarVelocidades(cboTransmision.Text, nudVelocidades, lblInfoVelocidades, null, true);
                }
                else
                {
                    ConfigurarVelocidades(cboTransmision.Text, nudVelocidades, lblInfoVelocidades, null, false);
                }
                ActualizarBotonesBusqueda();
            };

            // Turbo
            chkTieneTurbo = new CheckBox
            {
                Name = "chkTieneTurbo",
                Text = "🚀 Tiene Turbo",
                Location = new Point(colRightX + 80, yPosRight + 3),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.FromArgb(220, 53, 69),
                Checked = vehiculoCompleto.tieneTurbo,
                Enabled = true,
                AutoCheck = false
            };
            pnlPrincipal.Controls.Add(chkTieneTurbo);
            chkTieneTurbo.Click += (s, e) =>
            {
                if (editarActivo)
                {
                    verificarCambios();
                }
            };
            yPosRight += lineHeight;

            // Motor
            AgregarLabel(pnlPrincipal, "Motor:", 590, yPosRight, labelWidth);
            txtMotor = new TextBox
            {
                Name = "txtMotor",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                Font = new Font("Segoe UI", 10F),
                Text = vehiculoCompleto.motor,
                ReadOnly = true,
                BackColor = Color.WhiteSmoke
            };
            pnlPrincipal.Controls.Add(txtMotor);

            Button btnBuscarMotor = new Button
            {
                Name = "btnBuscarMotor",
                Text = "🔍",
                Location = new Point(colRightX + 80 + ctrlWidth + 8, yPosRight),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscarMotor.FlatAppearance.BorderSize = 0;
            btnBuscarMotor.Click += BtnBuscarMotor_Click;
            try { var ttM = new ToolTip(); ttM.SetToolTip(btnBuscarMotor, "Buscar si el vehículo trae ese motor"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarMotor);
            txtMotor.BringToFront();
            btnBuscarMotor.BringToFront();
            txtMotor.TextChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosRight += lineHeight;

            // Combustible
            AgregarLabel(pnlPrincipal, "Combustible:", 590, yPosRight, labelWidth);
            cboCombustible = new ComboBox
            {
                Name = "cboCombustible",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboCombustible.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboCombustible.Items.AddRange(new[] { "Gasolina", "Diesel", "Híbrido", "Eléctrico", "GLP", "GNC" });
            cboCombustible.Text = vehiculoCompleto.combustible;
            pnlPrincipal.Controls.Add(cboCombustible);
            cboCombustible.BringToFront();
            yPosRight += lineHeight;

            // Frenos
            AgregarLabel(pnlPrincipal, "Frenos:", 590, yPosRight, labelWidth);
            cboFrenos = new ComboBox
            {
                Name = "cboFrenos",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboFrenos.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboFrenos.Items.AddRange(new[] {
                "Disco en las 4 ruedas",
                "Disco delanteros y tambor atrás",
                "Tambor delantero y disco atrás"
            });
            cboFrenos.Text = vehiculoCompleto.frenos;
            pnlPrincipal.Controls.Add(cboFrenos);

            Button btnBuscarFrenos = new Button
            {
                Name = "btnBuscarFrenos",
                Text = "🔍",
                Location = new Point(colRightX + 80 + ctrlWidth + 8, yPosRight),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscarFrenos.FlatAppearance.BorderSize = 0;
            btnBuscarFrenos.Click += BtnBuscarFrenos_Click;
            try { var ttF = new ToolTip(); ttF.SetToolTip(btnBuscarFrenos, "Buscar información sobre frenos"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarFrenos);
            cboFrenos.BringToFront();
            btnBuscarFrenos.BringToFront();
            cboFrenos.SelectedIndexChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosRight += lineHeight;

            // Nro de Asientos
            AgregarLabel(pnlPrincipal, "Nro. Asientos:", 590, yPosRight, labelWidth);
            nudAsientos = new NumericUpDown
            {
                Name = "nudAsientos",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(100, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 3,
                Maximum = 8,
                Value = vehiculoCompleto.nroAsientos ?? 5,
                TextAlign = HorizontalAlignment.Center,
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            try { var tba = nudAsientos.Controls.OfType<TextBox>().FirstOrDefault(); if (tba != null) tba.ReadOnly = true; } catch { }
            pnlPrincipal.Controls.Add(nudAsientos);
            nudAsientos.BringToFront();
            yPosRight += lineHeight;

            // Nro de Puertas
            AgregarLabel(pnlPrincipal, "Nro. Puertas:", 590, yPosRight, labelWidth);
            cboPuertas = new ComboBox
            {
                Name = "cboPuertas",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboPuertas.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboPuertas.Items.AddRange(new object[] { 3, 4, 5 });
            cboPuertas.SelectedItem = vehiculoCompleto.nroPuertas;
            pnlPrincipal.Controls.Add(cboPuertas);

            Label lblInfoPuertas = new Label
            {
                Text = vehiculoCompleto.nroPuertas == 5 ? "(Común)" : "(Menos frecuente)",
                Location = new Point(colRightX + 240, yPosRight + 3),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = vehiculoCompleto.nroPuertas == 5 ? Color.Green : Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlPrincipal.Controls.Add(lblInfoPuertas);
            cboPuertas.SelectedIndexChanged += (s, e) =>
            {
                if (cboPuertas.SelectedItem != null)
                {
                    int puertas = (int)cboPuertas.SelectedItem;
                    switch (puertas)
                    {
                        case 3:
                            lblInfoPuertas.Text = "(Menos frecuente)";
                            lblInfoPuertas.ForeColor = Color.Gray;
                            break;
                        case 4:
                            lblInfoPuertas.Text = "(Menos frecuente)";
                            lblInfoPuertas.ForeColor = Color.Gray;
                            break;
                        case 5:
                            lblInfoPuertas.Text = "(Común)";
                            lblInfoPuertas.ForeColor = Color.Green;
                            break;
                    }
                }
            };
            cboPuertas.BringToFront();
            yPosRight += lineHeight;

            // Tracción
            AgregarLabel(pnlPrincipal, "Tracción:", 590, yPosRight, labelWidth);
            cboTraccion = new ComboBox
            {
                Name = "cboTraccion",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth + 50, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboTraccion.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboTraccion.Items.AddRange(new[] {
            "Delantera (4x2 FWD)",
            "Trasera (4x2 RWD)",
            "Integral permanente (AWD 4x4)",
            "Doble tracción seleccionable (4WD 4x4)"
            });
            cboTraccion.Text = vehiculoCompleto.traccion;
            pnlPrincipal.Controls.Add(cboTraccion);
            Button btnBuscarTraccion = new Button
            {
                Name = "btnBuscarTraccion",
                Text = "🔍",
                Location = new Point(colRightX + 80 + ctrlWidth + 50 + 8, yPosRight),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscarTraccion.FlatAppearance.BorderSize = 0;
            btnBuscarTraccion.Click += BtnBuscarTraccion_Click;
            try { var ttT = new ToolTip(); ttT.SetToolTip(btnBuscarTraccion, "Buscar información sobre tracción"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarTraccion);
            btnBuscarTraccion.BringToFront();

            Label lblInfoTraccion = new Label
            {
                Name = "lblInfoTraccion",
                Text = ObtenerInfoTraccion(vehiculoCompleto.traccion),
                Location = new Point(colRightX + 80 + ctrlWidth + 60 + 35, yPosRight + 3),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = ObtenerColorTraccion(vehiculoCompleto.traccion),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlPrincipal.Controls.Add(lblInfoTraccion);
            cboTraccion.SelectedIndexChanged += (s, e) =>
            {
                if (cboTraccion.SelectedIndex >= 0)
                {
                    switch (cboTraccion.SelectedIndex)
                    {
                        case 0:
                            lblInfoTraccion.Text = "Sedán/Hatchback";
                            lblInfoTraccion.ForeColor = Color.FromArgb(0, 123, 255);
                            break;
                        case 1:
                            lblInfoTraccion.Text = "Deportivos/Coupé";
                            lblInfoTraccion.ForeColor = Color.FromArgb(220, 53, 69);
                            break;
                        case 2:
                            lblInfoTraccion.Text = "SUV moderna";
                            lblInfoTraccion.ForeColor = Color.FromArgb(40, 167, 69);
                            break;
                        case 3:
                            lblInfoTraccion.Text = "Pickup/Off-road";
                            lblInfoTraccion.ForeColor = Color.FromArgb(255, 193, 7);
                            break;
                    }
                }
            };
            cboTraccion.BringToFront();
            cboTraccion.SelectedIndexChanged += (s, e) => ActualizarBotonesBusqueda();
            yPosRight += lineHeight;

            // ==================== SEGURIDAD ====================
            yPos = Math.Max(yPosLeft, yPosRight) + 15;
            AgregarSeccion(pnlPrincipal, "🛡️ SEGURIDAD", 20, yPos, 540);
            yPosLeft = yPos + 38;

            // Airbags
            AgregarLabel(pnlPrincipal, "Airbags:", 20, yPosLeft, labelWidth);
            nudAirbags = new NumericUpDown
            {
                Name = "nudAirbags",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(100, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 12,
                Value = vehiculoCompleto.nroAirbags,
                TextAlign = HorizontalAlignment.Center,
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            pnlPrincipal.Controls.Add(nudAirbags);

            Button btnBuscarAirbags = new Button
            {
                Name = "btnBuscarAirbags",
                Text = "🔍",
                Location = new Point(colLeftX + 100 + 8, yPosLeft),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscarAirbags.FlatAppearance.BorderSize = 0;
            btnBuscarAirbags.Click += BtnBuscarAirbags_Click;
            try { var ttA = new ToolTip(); ttA.SetToolTip(btnBuscarAirbags, "Buscar información sobre airbags"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarAirbags);
            btnBuscarAirbags.BringToFront();
            yPosLeft += lineHeight;

            // Organización
            AgregarLabel(pnlPrincipal, "Organización:", 20, yPosLeft, labelWidth);
            cboOrganizacion = new ComboBox
            {
                Name = "cboOrganizacion",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboOrganizacion.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboOrganizacion.Items.AddRange(new[] { "Latin NCAP", "Euro NCAP", "IIHS", "NHTSA", "ANCAP", "Sin calificación" });
            cboOrganizacion.Text = vehiculoCompleto.organizacionSeguridad;
            pnlPrincipal.Controls.Add(cboOrganizacion);

            Button btnBuscarOrganizacion = new Button
            {
                Name = "btnBuscarOrganizacion",
                Text = "🔍",
                Location = new Point(colLeftX + ctrlWidth + 8, yPosLeft),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false

            };
            btnBuscarOrganizacion.FlatAppearance.BorderSize = 0;
            btnBuscarOrganizacion.Click += BtnBuscarOrganizacion_Click;
            try { var ttO = new ToolTip(); ttO.SetToolTip(btnBuscarOrganizacion, "Buscar calificación de seguridad"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarOrganizacion);
            btnBuscarOrganizacion.BringToFront();
            yPosLeft += lineHeight;

            // Estrellas
            AgregarLabel(pnlPrincipal, "Estrellas:", 20, yPosLeft, labelWidth);
            cboEstrellas = new ComboBox
            {
                Name = "cboEstrellas",
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboEstrellas.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboEstrellas.Items.AddRange(new[] {
            "⭐⭐⭐⭐⭐ (5 estrellas)",
            "⭐⭐⭐⭐ (4 estrellas)",
            "⭐⭐⭐ (3 estrellas)",
            "⭐⭐ (2 estrellas)",
            "⭐ (1 estrella)",
            "☆ (0 estrellas)",
            "Sin calificación"
        });

            int indexEstrellas = vehiculoCompleto.seguridadChoques < 0 || vehiculoCompleto.seguridadChoques > 5
                ? 6
                : (5 - vehiculoCompleto.seguridadChoques);
            if (indexEstrellas >= 0 && indexEstrellas < cboEstrellas.Items.Count)
                cboEstrellas.SelectedIndex = indexEstrellas;

            // Validación: si organización es "Sin calificación", deshabilitar estrellas
            if (cboOrganizacion.Text == "Sin calificación")
            {
                cboEstrellas.SelectedIndex = 6;
                cboEstrellas.Enabled = false;
            }

            pnlPrincipal.Controls.Add(cboEstrellas);

            // Evento para organización que controla estrellas
            cboOrganizacion.SelectedIndexChanged += (s, e) =>
            {
                if (cboOrganizacion.SelectedIndex >= 0)
                {
                    string organizacion = cboOrganizacion.Text;

                    if (organizacion == "Sin calificación")
                    {
                        cboEstrellas.SelectedIndex = 6;
                        cboEstrellas.Enabled = false;
                    }
                    else
                    {
                        cboEstrellas.Enabled = editarActivo; // Solo habilitar si estamos en modo edición
                        if (cboEstrellas.SelectedIndex == 6)
                        {
                            cboEstrellas.SelectedIndex = -1;
                        }
                    }
                }
                ActualizarBotonesBusqueda();
                verificarCambios();
            };

            Button btnBuscarEstrellas = new Button
            {
                Name = "btnBuscarEstrellas",
                Text = "🔍",
                Location = new Point(colLeftX + ctrlWidth + 8, yPosLeft),
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnBuscarEstrellas.FlatAppearance.BorderSize = 0;
            btnBuscarEstrellas.Click += BtnBuscarEstrellas_Click;
            try { var ttE = new ToolTip(); ttE.SetToolTip(btnBuscarEstrellas, "Buscar calificación de estrellas"); } catch { }
            pnlPrincipal.Controls.Add(btnBuscarEstrellas);
            btnBuscarEstrellas.BringToFront();
            yPosLeft += lineHeight;

            // ==================== EVALUACIÓN ====================
            AgregarSeccion(pnlPrincipal, "⭐ EVALUACIÓN", 590, yPos, 560);
            yPosRight = yPos + 38;

            // Confiabilidad
            AgregarLabel(pnlPrincipal, "Confiabilidad:", 590, yPosRight, labelWidth);
            cboConfiabilidad = new ComboBox
            {
                Name = "cboConfiabilidad",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(ctrlWidth, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            cboConfiabilidad.DropDown += (s, e) => { if (!editarActivo) ((ComboBox)s).DroppedDown = false; };
            cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
            cboConfiabilidad.Text = vehiculoCompleto.confiabilidad;
            pnlPrincipal.Controls.Add(cboConfiabilidad);
            cboConfiabilidad.BringToFront();
            yPosRight += lineHeight;

            // Puntuación
            AgregarLabel(pnlPrincipal, "Puntuación:", 590, yPosRight, labelWidth);
            nudPuntuacion = new NumericUpDown
            {
                Name = "nudPuntuacion",
                Location = new Point(colRightX + 80, yPosRight),
                Size = new Size(120, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1.0m,
                Maximum = 10.0m,
                DecimalPlaces = 1,
                Increment = 0.5m,
                Value = vehiculoCompleto.puntuacion,
                TextAlign = HorizontalAlignment.Center,
                Enabled = false,
                BackColor = Color.WhiteSmoke
            };
            pnlPrincipal.Controls.Add(nudPuntuacion);
            nudPuntuacion.BringToFront();
            yPosRight += lineHeight;

            // ==================== OBSERVACIONES ====================
            yPos = Math.Max(yPosLeft, yPosRight) + 15;
            AgregarLabel(pnlPrincipal, "Observaciones:", 20, yPos, labelWidth);
            txtObservaciones = new TextBox
            {
                Name = "txtObservaciones",
                Location = new Point(colLeftX, yPos),
                Size = new Size(990 / 2 - 100, 65 + 200),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F),
                Text = vehiculoCompleto.observaciones ?? "",
                ReadOnly = true,
                BackColor = Color.WhiteSmoke
            };
            pnlPrincipal.Controls.Add(txtObservaciones);

            // Inicializar verificarCambios
            verificarCambios = () =>
            {
                try
                {
                    hayCambios = false;

                    if (txtChasis.Text.Trim().ToUpper() != valoresOriginales["nroChasis"].ToString()) hayCambios = true;
                    if (cboModelo.SelectedValue != null && (int)cboModelo.SelectedValue != (int)valoresOriginales["idModeloVehiculo"]) hayCambios = true;
                    if (cboTipo.SelectedValue != null && (int)cboTipo.SelectedValue != (int)valoresOriginales["idTipoVehiculo"]) hayCambios = true;
                    if (cboVersion.Text != valoresOriginales["version"].ToString()) hayCambios = true;
                    if ((int)nudAnio.Value != (int)valoresOriginales["anioFabricacion"]) hayCambios = true;
                    if (txtGeneracion.Text.Trim() != valoresOriginales["generacion"].ToString()) hayCambios = true;
                    if (cboPaisFabricacion.Text.Trim() != valoresOriginales["paisFabricacion"].ToString()) hayCambios = true;
                    if (cboTransmision.Text != valoresOriginales["transmision"].ToString()) hayCambios = true;
                    if (cboTraccion.Text != valoresOriginales["traccion"].ToString()) hayCambios = true;

                    int? velActual = cboTransmision.Text == "CVT" ? (int?)null : (int)nudVelocidades.Value;
                    int? velOriginal = valoresOriginales["nroVelocidades"] as int?;
                    if (velActual != velOriginal) hayCambios = true;

                    if ((int)nudAsientos.Value != (int)valoresOriginales["nroAsientos"]) hayCambios = true;
                    if ((int)cboPuertas.SelectedItem != (int)valoresOriginales["nroPuertas"]) hayCambios = true;
                    if (chkTieneTurbo.Checked != (bool)valoresOriginales["tieneTurbo"]) hayCambios = true;
                    if (txtMotor.Text.Trim() != valoresOriginales["motor"].ToString()) hayCambios = true;
                    if (cboCombustible.Text != valoresOriginales["combustible"].ToString()) hayCambios = true;
                    if (cboFrenos.Text != valoresOriginales["frenos"].ToString()) hayCambios = true;
                    if ((int)nudAirbags.Value != (int)valoresOriginales["nroAirbags"]) hayCambios = true;
                    if (cboOrganizacion.Text != valoresOriginales["organizacionSeguridad"].ToString()) hayCambios = true;
                    if (cboEstrellas.SelectedIndex != (int)valoresOriginales["cboEstrellas_SelectedIndex"]) hayCambios = true;
                    if (cboConfiabilidad.Text != valoresOriginales["confiabilidad"].ToString()) hayCambios = true;
                    if (nudPuntuacion.Value != (decimal)valoresOriginales["puntuacion"]) hayCambios = true;

                    string obsActual = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? "" : txtObservaciones.Text.Trim();
                    string obsOriginal = valoresOriginales["observaciones"]?.ToString() ?? "";
                    if (obsActual != obsOriginal) hayCambios = true;

                    bool fotoOriginalExistia = valoresOriginales.ContainsKey("fotoExiste") && (bool)valoresOriginales["fotoExiste"];
                    bool fotoCambio = false;

                    if (imagenCargada != null)
                    {
                        fotoCambio = true;
                    }
                    else if (fotoEliminada)
                    {
                        fotoCambio = fotoOriginalExistia;
                    }

                    if (fotoCambio) hayCambios = true;

                    if (btnEditar != null) btnEditar.Enabled = hayCambios;
                }
                catch { }
            };

            // EVENTO: Limpiar selección visual DESPUÉS de que el formulario se muestre
            this.Shown += (s, ev) =>
            {
                try
                {
                    if (cboMarca.SelectedIndex >= 0)
                    {
                        cboMarca.SelectionStart = 0;
                        cboMarca.SelectionLength = 0;
                    }

                    if (cboModelo.SelectedIndex >= 0)
                    {
                        cboModelo.SelectionStart = 0;
                        cboModelo.SelectionLength = 0;
                    }
                }
                catch { }
            };

            CrearPanelBotones();
        }

        private void CargarDatos()
        {
            try
            {
                procedencias = ProcedenciaCln.listar().Where(p => p.estado != -1).OrderBy(p => p.descripcion).ToList();
                tipos = TipoVehiculoCln.listar().Where(t => t.estado != -1).OrderBy(t => t.descripcion).ToList();
                marcas = MarcaCln.listar().Where(m => m.estado != -1).OrderBy(m => m.nombre).ToList();

                var modeloActual = ModeloVehiculoCln.obtenerUno(vehiculoCompleto.idModeloVehiculo);
                if (modeloActual != null)
                {
                    modelos = ModeloVehiculoCln.listar()
                        .Where(m => m.idMarca == modeloActual.idMarca && m.estado != -1)
                        .OrderBy(m => m.nombre)
                        .ToList();

                    // DESHABILITAR EVENTOS TEMPORALMENTE
                    permitirEventos = false;

                    // Cargar Procedencias
                    cboProcedencia.DisplayMember = "descripcion";
                    cboProcedencia.ValueMember = "id";
                    cboProcedencia.DataSource = procedencias;

                    var marcaDelModelo = marcas.FirstOrDefault(m => m.id == modeloActual.idMarca);
                    if (marcaDelModelo != null)
                        cboProcedencia.SelectedValue = marcaDelModelo.idProcedencia;

                    // Cargar Marcas
                    cboMarca.DisplayMember = "nombre";
                    cboMarca.ValueMember = "id";
                    cboMarca.DataSource = marcas;
                    cboMarca.SelectedValue = modeloActual.idMarca;

                    // Cargar Tipos
                    cboTipo.DisplayMember = "descripcion";
                    cboTipo.ValueMember = "id";
                    cboTipo.DataSource = tipos;
                    cboTipo.SelectedValue = modeloActual.idTipoVehiculo;

                    // Cargar Modelos
                    cboModelo.DisplayMember = "nombre";
                    cboModelo.ValueMember = "id";
                    cboModelo.DataSource = modelos;
                    cboModelo.SelectedValue = vehiculoCompleto.idModeloVehiculo;

                    // AGREGAR EVENTO PARA CAMBIO DE MARCA (cargar modelos dinámicamente)
                    // CONFIGURAR EVENTOS (SOLO UNA VEZ)
                    ConfigurarEventosCascada();

                    // HABILITAR EVENTOS (pero no dispararán porque los controles están deshabilitados)

                    permitirEventos = true;
                }

                CargarFotografia();

                // INICIALIZAR ESTADO DE BOTONES

                try
                {
                    ActualizarBotonesBusqueda();
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarEventosCascada()
        {
            // EVENTO: Cuando cambia PROCEDENCIA → cargar MARCAS de esa procedencia
            cboProcedencia.SelectedIndexChanged += (s, e) =>
            {
                try
                {
                    if (!permitirEventos) return;
                    if (!cboProcedencia.Enabled) return;
                    if (!editarActivo) return; // Solo en modo edición

                    var sel = cboProcedencia.SelectedValue;
                    if (sel == null) return;

                    int idProc = Convert.ToInt32(sel);
                    var marcasFiltradas = MarcaCln.listar()
                        .Where(m => m.idProcedencia == idProc && m.estado != -1)
                        .OrderBy(m => m.nombre)
                        .ToList();

                    permitirEventos = false;
                    cboMarca.DataSource = null;
                    cboMarca.DisplayMember = "nombre";
                    cboMarca.ValueMember = "id";
                    cboMarca.DataSource = marcasFiltradas;
                    cboMarca.BindingContext = new BindingContext();

                    if (marcasFiltradas.Any())
                        cboMarca.SelectedIndex = 0;
                    else
                        cboMarca.SelectedIndex = -1;

                    permitirEventos = true;
                }
                catch { }
            };

            // EVENTO: Cuando cambia MARCA → cargar MODELOS de esa marca
            cboMarca.SelectedIndexChanged += (s, e) =>
            {
                if (permitirEventos && cboMarca.Enabled && cboMarca.SelectedValue != null && editarActivo)
                {
                    try
                    {
                        int idMarca = Convert.ToInt32(cboMarca.SelectedValue);
                        var modelosNuevos = ModeloVehiculoCln.listar()
                            .Where(m => m.idMarca == idMarca && m.estado != -1)
                            .OrderBy(m => m.nombre)
                            .ToList();

                        permitirEventos = false;
                        cboModelo.DataSource = null;
                        cboModelo.DisplayMember = "nombre";
                        cboModelo.ValueMember = "id";
                        cboModelo.DataSource = modelosNuevos;
                        cboModelo.BindingContext = new BindingContext();
                        permitirEventos = true;

                        if (modelosNuevos.Any())
                        {
                            cboModelo.SelectedIndex = 0;
                        }
                    }
                    catch { }
                }
            };
        }

        private void ActualizarBotonesBusqueda()
        {
            bool tieneMarca = cboMarca != null && !string.IsNullOrWhiteSpace(cboMarca.Text);
            bool tieneModelo = cboModelo != null && !string.IsNullOrWhiteSpace(cboModelo.Text);
            bool tienePais = cboPaisFabricacion != null && !string.IsNullOrWhiteSpace(cboPaisFabricacion.Text);
            bool tieneVersion = cboVersion != null && cboVersion.SelectedIndex >= 0;
            bool tieneOrganizacion = cboOrganizacion != null &&
                                    cboOrganizacion.SelectedIndex >= 0 &&
                                    cboOrganizacion.Text != "Sin calificación";
            bool tieneMotor = txtMotor != null && !string.IsNullOrWhiteSpace(txtMotor.Text);
            bool tieneAnio = nudAnio != null && nudAnio.Value > 0;
            bool tieneFrenos = cboFrenos != null && cboFrenos.SelectedIndex >= 0;
            bool tieneTransmision = cboTransmision != null && cboTransmision.SelectedIndex >= 0;
            // Verificar si nudVelocidades está visible Y habilitado (no CVT)
            // NO importa si está en modo edición o lectura
            bool tieneVelocidadesValidas = cboTransmision != null &&
                                           cboTransmision.SelectedIndex >= 0 &&
                                           cboTransmision.Text != "CVT" &&
                                           nudVelocidades != null &&
                                           nudVelocidades.Value >= nudVelocidades.Minimum;

            // Botón Airbags
            var btnBuscarAirbags = pnlPrincipal.Controls.Find("btnBuscarAirbags", true).FirstOrDefault() as Button;
            if (btnBuscarAirbags != null)
                btnBuscarAirbags.Enabled = tieneMarca && tieneModelo && tienePais && tieneVersion;

            // Botón Organización: habilitado si hay marca, modelo y versión
            var btnBuscarOrganizacion = pnlPrincipal.Controls.Find("btnBuscarOrganizacion", true).FirstOrDefault() as Button;
            if (btnBuscarOrganizacion != null)
                btnBuscarOrganizacion.Enabled = tieneMarca && tieneModelo && tieneVersion && tieneOrganizacion;

            // Botón Estrellas: habilitado solo si hay organización válida (no "Sin calificación")
            var btnBuscarEstrellas = pnlPrincipal.Controls.Find("btnBuscarEstrellas", true).FirstOrDefault() as Button;
            if (btnBuscarEstrellas != null)
                btnBuscarEstrellas.Enabled = tieneMarca && tieneModelo && tieneOrganizacion && tieneVersion;

            // Botón Motor
            var btnBuscarMotor = pnlPrincipal.Controls.Find("btnBuscarMotor", true).FirstOrDefault() as Button;
            if (btnBuscarMotor != null)
                btnBuscarMotor.Enabled = tieneMarca && tieneModelo && tieneAnio && tieneMotor;

            // Botón Velocidades
            var btnBuscarVelocidades = pnlPrincipal.Controls.Find("btnBuscarVelocidades", true).FirstOrDefault() as Button;
            if (btnBuscarVelocidades != null)
                btnBuscarVelocidades.Enabled = tieneMarca && tieneModelo && tieneAnio && tieneVelocidadesValidas;

            // Botón Tracción
            var btnBuscarTraccion = pnlPrincipal.Controls.Find("btnBuscarTraccion", true).FirstOrDefault() as Button;
            if (btnBuscarTraccion != null)
                btnBuscarTraccion.Enabled = cboTraccion != null && cboTraccion.SelectedIndex >= 0;

            // Botón País de Fabricación
            var btnBuscarPaisFabricacion = pnlPrincipal.Controls.Find("btnBuscarPaisFabricacion", true).FirstOrDefault() as Button;
            if (btnBuscarPaisFabricacion != null)
                btnBuscarPaisFabricacion.Enabled = tieneMarca && tieneModelo && tieneVersion && tienePais;

            // Botón Versión
            var btnBuscarVersion = pnlPrincipal.Controls.Find("btnBuscarVersion", true).FirstOrDefault() as Button;
            if (btnBuscarVersion != null)
                btnBuscarVersion.Enabled = tieneMarca && tieneModelo && tieneAnio && tieneVersion;

            // Botón Frenos
            var btnBuscarFrenos = pnlPrincipal.Controls.Find("btnBuscarFrenos", true).FirstOrDefault() as Button;
            if (btnBuscarFrenos != null)
                btnBuscarFrenos.Enabled = tieneMarca && tieneModelo && tieneAnio && tieneVersion && tieneFrenos;

            // Botón Buscar Foto Web
            var btnBuscarFotoWeb = pnlPrincipal.Controls.Find("btnBuscarFotoWeb", true).FirstOrDefault() as Button;
            if (btnBuscarFotoWeb != null)
                btnBuscarFotoWeb.Enabled = tieneMarca && tieneModelo && tieneAnio && tieneVersion;
        }

        private void CargarFotografia()
        {
            try
            {
                byte[] fotoBytes = ConsultaVehiculosCln.obtenerFotografia(vehiculoResumen.id);
                if (fotoBytes != null && fotoBytes.Length > 0)
                {
                    imagenCargada = ImagenHelper.ConvertirBytesAImagen(fotoBytes);
                    if (pbFotografia != null)
                    {
                        ImagenHelper.AjustarImagenPictureBox(pbFotografia, imagenCargada);
                    }
                    btnVerFotoGrande.Visible = true;
                    btnCambiarFoto.Text = "📂 Cambiar Foto";
                    fotoExisteEnBD = true;
                }
                else
                {
                    ImagenHelper.AjustarImagenPictureBox(pbFotografia, null);
                    btnVerFotoGrande.Visible = false;
                    btnCambiarFoto.Text = "📂 Añadir Foto";
                    fotoExisteEnBD = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar fotografía: {ex.Message}", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ImagenHelper.AjustarImagenPictureBox(pbFotografia, null);
                btnVerFotoGrande.Visible = false;
                btnCambiarFoto.Text = "📂 Añadir Foto";
                fotoExisteEnBD = false;
            }
        }

        private void CrearPanelBotones()
        {
            Panel pnlBotones = new Panel
            {
                Location = new Point(0, 650 - 65),
                Size = new Size(1184, 70 - 05),
                BackColor = Color.FromArgb(52, 58, 64),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnIniciarSesionDetalle = new Button
            {
                Name = "btnIniciarSesionDetalle",
                Text = Util.usuario != null ? "Cerrar Sesión" : "Iniciar Sesión",
                Size = new Size(120, 35),
                Location = new Point(10, 10),
                BackColor = Util.usuario != null ? Color.FromArgb(217, 83, 79) : Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Visible = Util.usuario == null
            };
            btnIniciarSesionDetalle.FlatAppearance.BorderSize = 0;
            btnIniciarSesionDetalle.Click += BtnIniciarSesion_Click;
            this.Controls.Add(btnIniciarSesionDetalle);
            btnIniciarSesionDetalle.BringToFront();

            btnEditar = new Button
            {
                Name = "btnEditar",
                Text = "✏️ Editar",
                Size = new Size(180, 45),
                Location = new Point(350, 12),
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = Util.usuario != null
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += BtnEditar_Click;
            pnlBotones.Controls.Add(btnEditar);

            btnEliminar = new Button
            {
                Name = "btnEliminar",
                Text = "🗑️ Eliminar",
                Size = new Size(180, 45),
                Location = new Point(550, 12),
                BackColor = Color.FromArgb(217, 83, 79),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = Util.usuario != null
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;
            pnlBotones.Controls.Add(btnEliminar);

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

            this.Controls.Add(pnlBotones);
            this.AcceptButton = btnCerrar;
        }

        // ==================== EVENTOS DE BOTONES ====================

        private void BtnIniciarSesion_Click(object sender, EventArgs e)
        {
            if (Util.usuario != null)
            {
                ((Button)sender).Visible = false;
                btnEditar.Visible = true;
                btnEliminar.Visible = true;
                return;
            }

            using (var frmAuth = new FrmAutenticacion())
            {
                frmAuth.StartPosition = FormStartPosition.CenterParent;
                if (frmAuth.ShowDialog(this) == DialogResult.OK)
                {
                    ((Button)sender).Visible = false;
                    btnEditar.Visible = true;
                    btnEliminar.Visible = true;
                    MessageBox.Show("✅ Autenticación exitosa.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (btnEditar.Text == "✏️ Editar")
            {
                ActivarModoEdicion();
            }
            else
            {
                GuardarCambios();
            }
        }

        private void ActivarModoEdicion()
        {
            editarActivo = true;

            // HABILITAR EVENTOS PARA QUE MARCA/MODELO FUNCIONEN
            permitirEventos = true;

            GuardarValoresOriginales();

            // Habilitar controles de foto
            pbFotografia.AllowDrop = true;
            btnCambiarFoto.Visible = true;
            btnEliminarFotoDetalle.Visible = true;

            bool hayImagen = (imagenCargada != null) || fotoExisteEnBD;
            btnEliminarFotoDetalle.Enabled = hayImagen;

            // Habilitar todos los controles
            txtChasis.ReadOnly = false;
            txtChasis.BackColor = Color.White;
            cboMarca.Enabled = true;
            cboModelo.Enabled = true;
            cboTipo.Enabled = true;
            cboVersion.Enabled = true;
            cboFrenos.Enabled = true;
            cboProcedencia.Visible = false;

            try
            {
                foreach (Control ctrl in pnlPrincipal.Controls)
                {
                    if (ctrl is Label l && !string.IsNullOrEmpty(l.Text) && l.Text.Trim().StartsWith("Procedencia"))
                        l.Visible = false;
                }
            }
            catch { }

            nudAnio.Enabled = true;
            try { var t = nudAnio.Controls.OfType<TextBox>().FirstOrDefault(); if (t != null) t.ReadOnly = false; } catch { }

            txtGeneracion.ReadOnly = false;
            txtGeneracion.BackColor = Color.White;
            cboPaisFabricacion.Enabled = true;
            cboTransmision.Enabled = true;

            ConfigurarVelocidades(cboTransmision.Text, nudVelocidades, lblInfoVelocidades, vehiculoCompleto.nroVelocidades, true);
            try { var tv = nudVelocidades.Controls.OfType<TextBox>().FirstOrDefault(); if (tv != null) tv.ReadOnly = false; } catch { }

            nudAsientos.Enabled = true;
            try { var ta = nudAsientos.Controls.OfType<TextBox>().FirstOrDefault(); if (ta != null) ta.ReadOnly = false; } catch { }

            cboTraccion.Enabled = true;
            cboPuertas.Enabled = true;
            chkTieneTurbo.AutoCheck = true;

            txtMotor.ReadOnly = false;
            txtMotor.BackColor = Color.White;
            cboCombustible.Enabled = true;

            nudAirbags.Enabled = true;
            try { var tbair = nudAirbags.Controls.OfType<TextBox>().FirstOrDefault(); if (tbair != null) tbair.ReadOnly = false; } catch { }

            cboOrganizacion.Enabled = true;
            cboEstrellas.Enabled = true;

            if (cboOrganizacion.Text == "Sin calificación")
            {
                cboEstrellas.Enabled = false;
            }

            cboConfiabilidad.Enabled = true;

            nudPuntuacion.Enabled = true;
            try { var tbp3 = nudPuntuacion.Controls.OfType<TextBox>().FirstOrDefault(); if (tbp3 != null) tbp3.ReadOnly = false; } catch { }

            txtObservaciones.ReadOnly = false;
            txtObservaciones.BackColor = Color.White;

            btnAddMarca.Visible = true;
            btnAddModelo.Visible = true;
            btnEditMarca.Visible = true;
            btnEditModelo.Visible = true;

            ActualizarBotonesBusqueda();


            var lblEstado = pnlPrincipal.Controls.Find("lblEstado", true).FirstOrDefault() as Label;
            if (lblEstado != null)
            {
                lblEstado.Text = "✏️ MODO EDICIÓN - Modifique los campos necesarios";
                lblEstado.ForeColor = Color.Green;
                lblEstado.BackColor = Color.FromArgb(230, 255, 230);
            }

            btnEditar.Text = "💾 Guardar";
            btnEditar.BackColor = Color.FromArgb(40, 167, 69);
            btnEditar.Enabled = false;

            // Limpiar selección visual de ComboBox
            if (cboMarca.SelectedIndex >= 0)
            {
                cboMarca.SelectionStart = 0;
                cboMarca.SelectionLength = 0;
            }

            if (cboModelo.SelectedIndex >= 0)
            {
                cboModelo.SelectionStart = 0;
                cboModelo.SelectionLength = 0;
            }

            // Conectar eventos
            txtChasis.TextChanged += (ss, ee) => verificarCambios();
            cboModelo.SelectedIndexChanged += (ss, ee) => verificarCambios();
            cboTipo.SelectedIndexChanged += (ss, ee) => verificarCambios();
            cboVersion.SelectedIndexChanged += (ss, ee) => verificarCambios();
            nudAnio.ValueChanged += (ss, ee) => verificarCambios();
            txtGeneracion.TextChanged += (ss, ee) => verificarCambios();
            cboPaisFabricacion.SelectedIndexChanged += (ss, ee) => verificarCambios();
            cboPaisFabricacion.TextChanged += (ss, ee) => verificarCambios();
            cboTransmision.SelectedIndexChanged += (ss, ee) => verificarCambios();
            cboTraccion.SelectedIndexChanged += (ss, ee) => verificarCambios();
            nudVelocidades.ValueChanged += (ss, ee) => verificarCambios();
            nudAsientos.ValueChanged += (ss, ee) => verificarCambios();
            cboPuertas.SelectedIndexChanged += (ss, ee) => verificarCambios();
            chkTieneTurbo.CheckedChanged += (ss, ee) => verificarCambios();
            txtMotor.TextChanged += (ss, ee) => verificarCambios();
            cboCombustible.SelectedIndexChanged += (ss, ee) => verificarCambios();
            cboFrenos.SelectedIndexChanged += (ss, ee) => verificarCambios();
            nudAirbags.ValueChanged += (ss, ee) => verificarCambios();
            cboOrganizacion.SelectedIndexChanged += (ss, ee) => verificarCambios();
            cboEstrellas.SelectedIndexChanged += (ss, ee) => verificarCambios();
            cboConfiabilidad.SelectedIndexChanged += (ss, ee) => verificarCambios();
            nudPuntuacion.ValueChanged += (ss, ee) => verificarCambios();
            txtObservaciones.TextChanged += (ss, ee) => verificarCambios();
        }

        private void GuardarValoresOriginales()
        {
            valoresOriginales.Clear();
            valoresOriginales["nroChasis"] = txtChasis.Text.Trim().ToUpper();
            valoresOriginales["idModeloVehiculo"] = cboModelo.SelectedValue ?? 0;
            valoresOriginales["idTipoVehiculo"] = cboTipo.SelectedValue ?? 0;
            valoresOriginales["version"] = cboVersion.Text;
            valoresOriginales["anioFabricacion"] = (int)nudAnio.Value;
            valoresOriginales["generacion"] = txtGeneracion.Text.Trim();
            valoresOriginales["paisFabricacion"] = cboPaisFabricacion.Text.Trim();
            valoresOriginales["transmision"] = cboTransmision.Text;
            valoresOriginales["traccion"] = cboTraccion.Text;
            valoresOriginales["nroVelocidades"] = cboTransmision.Text == "CVT" ? (int?)null : (int)nudVelocidades.Value;
            valoresOriginales["nroAsientos"] = (int)nudAsientos.Value;
            valoresOriginales["nroPuertas"] = (int)cboPuertas.SelectedItem;
            valoresOriginales["tieneTurbo"] = chkTieneTurbo.Checked;
            valoresOriginales["motor"] = txtMotor.Text.Trim();
            valoresOriginales["combustible"] = cboCombustible.Text;
            valoresOriginales["frenos"] = cboFrenos.Text;
            valoresOriginales["nroAirbags"] = (int)nudAirbags.Value;
            valoresOriginales["organizacionSeguridad"] = cboOrganizacion.Text;
            valoresOriginales["cboEstrellas_SelectedIndex"] = cboEstrellas.SelectedIndex;
            valoresOriginales["confiabilidad"] = cboConfiabilidad.Text;
            valoresOriginales["puntuacion"] = nudPuntuacion.Value;
            valoresOriginales["observaciones"] = txtObservaciones.Text?.Trim() ?? "";
            valoresOriginales["fotoExiste"] = fotoExisteEnBD;
        }

        private void GuardarCambios()
        {
            if (!ValidarEdicion())
                return;

            try
            {
                if (cboModelo.SelectedValue == null || cboTipo.SelectedValue == null)
                {
                    MessageBox.Show("⚠️ Debe seleccionar un modelo y un tipo válidos",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int nuevoIdModelo = Convert.ToInt32(cboModelo.SelectedValue);
                int nuevoIdTipo = Convert.ToInt32(cboTipo.SelectedValue);

                var modeloSeleccionado = ModeloVehiculoCln.obtenerUno(nuevoIdModelo);

                if (modeloSeleccionado == null)
                {
                    MessageBox.Show("❌ Error: No se pudo cargar el modelo seleccionado",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (modeloSeleccionado.idTipoVehiculo != nuevoIdTipo)
                {
                    modeloSeleccionado.idTipoVehiculo = nuevoIdTipo;
                    modeloSeleccionado.usuarioRegistro = Util.usuario.usuario;
                    //modeloSeleccionado.usuarioModificacion = Util.usuario.usuario;
                    //modeloSeleccionado.fechaModificacion = DateTime.Now;

                    int resultadoModelo = ModeloVehiculoCln.actualizar(modeloSeleccionado);

                    if (resultadoModelo == 0)
                    {
                        MessageBox.Show("❌ Error al actualizar el tipo del modelo",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Procesar fotografía
                if (imagenCargada != null)
                {
                    byte[] fotoBytes = ImagenHelper.ConvertirImagenABytes(imagenCargada);
                    vehiculoCompleto.fotografia = fotoBytes;
                }
                else if (fotoEliminada)
                {
                    vehiculoCompleto.fotografia = null;
                }

                // Actualizar vehículo
                vehiculoCompleto.nroChasis = txtChasis.Text.Trim().ToUpper();
                vehiculoCompleto.idModeloVehiculo = nuevoIdModelo;
                vehiculoCompleto.version = cboVersion.Text;
                vehiculoCompleto.anioFabricacion = (int)nudAnio.Value;
                vehiculoCompleto.generacion = txtGeneracion.Text.Trim();
                vehiculoCompleto.paisFabricacion = cboPaisFabricacion.Text.Trim();
                vehiculoCompleto.transmision = cboTransmision.Text;
                vehiculoCompleto.traccion = cboTraccion.Text;

                if (cboTransmision.Text == "CVT")
                {
                    vehiculoCompleto.nroVelocidades = null;
                }
                else
                {
                    vehiculoCompleto.nroVelocidades = (int)nudVelocidades.Value;
                }

                vehiculoCompleto.nroAsientos = (int)nudAsientos.Value;
                vehiculoCompleto.nroPuertas = (int)cboPuertas.SelectedItem;
                vehiculoCompleto.tieneTurbo = chkTieneTurbo.Checked;
                vehiculoCompleto.motor = txtMotor.Text.Trim();
                vehiculoCompleto.combustible = cboCombustible.Text;
                vehiculoCompleto.frenos = cboFrenos.Text;
                vehiculoCompleto.nroAirbags = (int)nudAirbags.Value;
                vehiculoCompleto.organizacionSeguridad = cboOrganizacion.Text;
                vehiculoCompleto.seguridadChoques = cboEstrellas.SelectedIndex < 6 ? (5 - cboEstrellas.SelectedIndex) : 0;
                vehiculoCompleto.confiabilidad = cboConfiabilidad.Text;
                vehiculoCompleto.puntuacion = nudPuntuacion.Value;
                vehiculoCompleto.observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ?
                    null : txtObservaciones.Text.Trim();
                vehiculoCompleto.usuarioRegistro = Util.usuario.usuario;
                vehiculoCompleto.usuarioModificacion = Util.usuario.usuario;
                vehiculoCompleto.fechaModificacion = DateTime.Now;

                int resultado = ConsultaVehiculosCln.actualizar(vehiculoCompleto);

                if (resultado > 0)
                {
                    DesactivarModoEdicion();

                    MessageBox.Show("✅ Vehículo actualizado exitosamente",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    onVehiculoActualizado?.Invoke();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ No se pudo actualizar el vehículo",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ValidarEdicion()
        {
            if (string.IsNullOrWhiteSpace(txtChasis.Text))
            {
                MessageBox.Show("⚠️ El número de chasis es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtChasis.Text.Length != 17)
            {
                MessageBox.Show("⚠️ El número de chasis debe tener exactamente 17 caracteres", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboModelo.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar un modelo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGeneracion.Text))
            {
                MessageBox.Show("⚠️ La generación es obligatoria", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboPaisFabricacion.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el país de fabricación", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboConfiabilidad.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el nivel de confiabilidad", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboVersion.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar la versión del vehículo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboFrenos.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el tipo de frenos", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboTraccion.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el tipo de tracción", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void DesactivarModoEdicion()
        {
            editarActivo = false;

            // DESHABILITAR EVENTOS
            permitirEventos = false;

            pbFotografia.AllowDrop = false;
            btnCambiarFoto.Visible = false;
            btnEliminarFotoDetalle.Visible = false;

            txtChasis.ReadOnly = true;
            txtChasis.BackColor = Color.WhiteSmoke;
            cboMarca.Enabled = false;
            cboModelo.Enabled = false;
            cboVersion.Enabled = false;
            cboTipo.Enabled = false;
            cboProcedencia.Visible = true;

            try
            {
                foreach (Control ctrl in pnlPrincipal.Controls)
                {
                    if (ctrl is Label l && !string.IsNullOrEmpty(l.Text) && l.Text.Trim().StartsWith("Procedencia"))
                        l.Visible = true;
                }
            }
            catch { }

            nudAnio.Enabled = false;
            txtGeneracion.ReadOnly = true;
            txtGeneracion.BackColor = Color.WhiteSmoke;
            cboPaisFabricacion.Enabled = false;
            cboTransmision.Enabled = false;

            ConfigurarVelocidades(cboTransmision.Text, nudVelocidades, lblInfoVelocidades, vehiculoCompleto.nroVelocidades, false);

            // EVENTO: Reconfigurar velocidades cuando cambia transmisión
            cboTransmision.SelectedIndexChanged += (s, ev) =>
            {
                if (cboTransmision.Enabled && editarActivo) // Solo en modo edición
                {
                    ConfigurarVelocidades(cboTransmision.Text, nudVelocidades, lblInfoVelocidades, null, true);
                }
                else
                {
                    ConfigurarVelocidades(cboTransmision.Text, nudVelocidades, lblInfoVelocidades, null, false);
                }
            };

            nudAsientos.Enabled = false;
            cboTraccion.Enabled = false;
            cboPuertas.Enabled = false;
            chkTieneTurbo.AutoCheck = false;
            txtMotor.ReadOnly = true;
            txtMotor.BackColor = Color.WhiteSmoke;
            cboCombustible.Enabled = false;
            cboFrenos.Enabled = false;
            nudAirbags.Enabled = false;
            cboOrganizacion.Enabled = false;
            cboEstrellas.Enabled = false;
            cboConfiabilidad.Enabled = false;
            nudPuntuacion.Enabled = false;
            txtObservaciones.ReadOnly = true;
            txtObservaciones.BackColor = Color.WhiteSmoke;

            btnAddMarca.Visible = false;
            btnAddModelo.Visible = false;
            btnEditMarca.Visible = false;
            btnEditModelo.Visible = false;

            var lblEstado = pnlPrincipal.Controls.Find("lblEstado", true).FirstOrDefault() as Label;
            if (lblEstado != null)
            {
                lblEstado.Text = "🔒 MODO LECTURA";
                lblEstado.ForeColor = Color.Red;
                lblEstado.BackColor = Color.FromArgb(255, 230, 230);
            }

            btnEditar.Text = "✏️ Editar";
            btnEditar.BackColor = Color.FromArgb(51, 122, 183);
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                $"⚠️ ¿Está seguro que desea eliminar este vehículo?\n\n" +
                $"Marca: {vehiculoResumen.marca}\n" +
                $"Modelo: {vehiculoResumen.modelo}\n\n" +
                $"Esta acción no se puede deshacer.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    int resultado_op = ConsultaVehiculosCln.eliminar(vehiculoResumen.id, Util.usuario.usuario);
                    if (resultado_op > 0)
                    {
                        onVehiculoActualizado?.Invoke();
                        MessageBox.Show("✅ Vehículo eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al eliminar: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==================== EVENTOS DE BÚSQUEDA ====================

        private void BtnBuscarChasis_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string pais = cboPaisFabricacion?.Text.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(pais))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo y País de Fabricación para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"¿El {marca} {modelo} {anioVal} fabricado en {pais} con qué dígitos comienza según su VIN o número de chasis?";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);

                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarVersion_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string version = cboVersion?.Text ?? "";

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(version))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo y Versión para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"{marca} {modelo} {anioVal} versión {version}";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarGeneracion_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;

                if (string.IsNullOrWhiteSpace(marca) && string.IsNullOrWhiteSpace(modelo))
                {
                    MessageBox.Show("⚠️ Debe especificar al menos la Marca o el Modelo para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"¿Qué generación es el {marca} {modelo} {anioVal} y entre que años comprende?".Trim();
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);

                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarPaisFabricacion_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                string version = cboVersion?.Text ?? "";
                string pais = cboPaisFabricacion?.Text ?? "";
                int anioVal = (int)nudAnio.Value;

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(version) || string.IsNullOrWhiteSpace(pais))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo, Versión y País para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"¿El {marca} {modelo} versión {version} año {anioVal} es fabricado en {pais}?";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarVelocidades_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string transmision = cboTransmision?.Text ?? "";
                int velocidades = (int)nudVelocidades.Value;

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(transmision))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo y Transmisión para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"¿El {marca} {modelo} {anioVal} con transmisión {transmision} tiene {velocidades} velocidades?";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarMotor_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string motor = txtMotor?.Text.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(motor))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo y Motor para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"¿El {marca} {modelo} {anioVal} trae motor {motor}?";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarFrenos_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string version = cboVersion?.Text ?? "";
                string frenos = cboFrenos?.Text ?? "";

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(frenos))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo y tipo de Frenos para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"¿El {marca} {modelo} {anioVal} versión {version} trae frenos de {frenos}?";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarTraccion_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string traccion = cboTraccion?.Text ?? "";

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(traccion))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo y tipo de Tracción para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"¿El {marca} {modelo} {anioVal} tiene tracción {traccion}?";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarAirbags_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string version = cboVersion?.Text ?? "";
                string pais = cboPaisFabricacion?.Text ?? "";

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca y Modelo para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"¿Cuántos airbags tiene el {marca} {modelo} {anioVal} versión {version} fabricado en {pais}?";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarOrganizacion_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string version = cboVersion?.Text ?? "";
                string organizacion = cboOrganizacion?.Text ?? "";

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(organizacion))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo y Organización para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"Prueba de choque {marca} {modelo} {anioVal} versión {version} según {organizacion}";
                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarEstrellas_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string version = cboVersion?.Text ?? "";
                string organizacion = cboOrganizacion?.Text ?? "";

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca y Modelo para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = string.IsNullOrWhiteSpace(organizacion)
                    ? $"{marca} {modelo} {anioVal} safety rating stars"
                    : $"¿Cuántas estrellas de seguridad tiene el {marca} {modelo} {anioVal} versión {version} según {organizacion}?";

                string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarFotoWeb_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = cboMarca?.Text.Trim() ?? "";
                string modelo = cboModelo?.Text.Trim() ?? "";
                int anioVal = (int)nudAnio.Value;
                string version = cboVersion?.Text ?? "";
                string tipoVehiculo = cboTipo?.Text ?? "";
                int nroPuertas = cboPuertas != null && cboPuertas.SelectedItem != null ? (int)cboPuertas.SelectedItem : 0;

                if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(version))
                {
                    MessageBox.Show("⚠️ Debe especificar Marca, Modelo y Versión para realizar la búsqueda.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = $"{marca} {modelo} año {anioVal} versión {version} tipo {tipoVehiculo} de {nroPuertas} puertas fotos";
                string url = "https://www.google.com/search?tbm=isch&q=" + Uri.EscapeDataString(query);

                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ No se pudo abrir el navegador:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== EVENTOS DE MARCA Y MODELO ====================

        private void BtnAddMarca_Click(object sender, EventArgs e)
        {
            if (Util.usuario == null)
            {
                MessageBox.Show("Debe autenticarse para agregar una nueva marca", "Autenticación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var frm = new FrmNuevaMarca(0))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        int nuevoIdMarca = frm.NuevoIdMarca;
                        var marcasActualizadas = MarcaCln.listar()
                            .Where(m => m.estado != -1)
                            .OrderBy(m => m.nombre)
                            .ToList();

                        cboMarca.DataSource = null;
                        cboMarca.DisplayMember = "nombre";
                        cboMarca.ValueMember = "id";
                        cboMarca.DataSource = marcasActualizadas;
                        cboMarca.BindingContext = new BindingContext();

                        if (nuevoIdMarca > 0 && marcasActualizadas.Any(m => m.id == nuevoIdMarca))
                            cboMarca.SelectedValue = nuevoIdMarca;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar marca: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditMarca_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboMarca.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione primero una marca para editar", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idMarcaSel = Convert.ToInt32(cboMarca.SelectedValue);
                using (var frm = new FrmGestionarMarcasModelos())
                {
                    frm.AbrirEnMarca(idMarcaSel);
                    frm.ShowDialog(this);
                }

                var marcasActualizadas = MarcaCln.listar()
                    .Where(m => m.estado != -1)
                    .OrderBy(m => m.nombre)
                    .ToList();

                cboMarca.DataSource = null;
                cboMarca.DisplayMember = "nombre";
                cboMarca.ValueMember = "id";
                cboMarca.DataSource = marcasActualizadas;
                cboMarca.BindingContext = new BindingContext();

                if (marcasActualizadas.Any(m => m.id == idMarcaSel))
                    cboMarca.SelectedValue = idMarcaSel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar marca: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVerMarcaDesc_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcion = null;
                if (cboMarca.SelectedValue != null)
                {
                    int idSel = Convert.ToInt32(cboMarca.SelectedValue);
                    if (cboMarca.DataSource is System.Collections.IEnumerable ds)
                    {
                        foreach (var item in ds)
                        {
                            var propId = item.GetType().GetProperty("id");
                            if (propId != null && Convert.ToInt32(propId.GetValue(item)) == idSel)
                            {
                                var propDesc = item.GetType().GetProperty("descripcion");
                                if (propDesc != null)
                                    descripcion = propDesc.GetValue(item) as string;
                                break;
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(descripcion))
                    {
                        var marcaObj = MarcaCln.listar().FirstOrDefault(m => m.id == idSel);
                        if (marcaObj != null)
                            descripcion = marcaObj.descripcion;
                    }
                }

                if (string.IsNullOrWhiteSpace(descripcion))
                    descripcion = "(Sin descripción disponible)";

                MessageBox.Show(descripcion, "Descripción de la Marca", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener descripción de la marca: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddModelo_Click(object sender, EventArgs e)
        {
            if (Util.usuario == null)
            {
                MessageBox.Show("Debe autenticarse para agregar un nuevo modelo", "Autenticación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (cboMarca.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione primero una marca para asociar el nuevo modelo", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idMarcaSeleccionada = Convert.ToInt32(cboMarca.SelectedValue);
                int idTipoSeleccionado = cboTipo.SelectedValue != null ? Convert.ToInt32(cboTipo.SelectedValue) : 0;

                using (var frm = new FrmNuevoModelo(idMarcaSeleccionada, idTipoSeleccionado))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        int nuevoIdModelo = frm.NuevoIdModelo;

                        var modelosActualizados = ModeloVehiculoCln.listar()
                            .Where(m => m.idMarca == idMarcaSeleccionada && m.estado != -1)
                            .OrderBy(m => m.nombre)
                            .ToList();

                        cboModelo.DataSource = null;
                        cboModelo.DisplayMember = "nombre";
                        cboModelo.ValueMember = "id";
                        cboModelo.DataSource = modelosActualizados;
                        cboModelo.BindingContext = new BindingContext();

                        if (nuevoIdModelo > 0 && modelosActualizados.Any(m => m.id == nuevoIdModelo))
                            cboModelo.SelectedValue = nuevoIdModelo;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar modelo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditModelo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboModelo.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione primero un modelo para editar", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idModeloSel = Convert.ToInt32(cboModelo.SelectedValue);
                using (var frm = new FrmGestionarMarcasModelos())
                {
                    frm.AbrirEnModelo(idModeloSel);
                    frm.ShowDialog(this);
                }

                if (cboMarca.SelectedValue != null)
                {
                    int idMarcaSel = Convert.ToInt32(cboMarca.SelectedValue);
                    var modelosActualizados = ModeloVehiculoCln.listar()
                        .Where(m => m.idMarca == idMarcaSel && m.estado != -1)
                        .OrderBy(m => m.nombre)
                        .ToList();

                    cboModelo.DataSource = null;
                    cboModelo.DisplayMember = "nombre";
                    cboModelo.ValueMember = "id";
                    cboModelo.DataSource = modelosActualizados;
                    cboModelo.BindingContext = new BindingContext();

                    if (modelosActualizados.Any(m => m.id == idModeloSel))
                        cboModelo.SelectedValue = idModeloSel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar modelo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVerModeloDesc_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcion = null;
                if (cboModelo.SelectedValue != null)
                {
                    int idSel = Convert.ToInt32(cboModelo.SelectedValue);
                    if (cboModelo.DataSource is System.Collections.IEnumerable ds)
                    {
                        foreach (var item in ds)
                        {
                            var propId = item.GetType().GetProperty("id");
                            if (propId != null && Convert.ToInt32(propId.GetValue(item)) == idSel)
                            {
                                var propDesc = item.GetType().GetProperty("descripcion");
                                if (propDesc != null)
                                    descripcion = propDesc.GetValue(item) as string;
                                break;
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(descripcion))
                    {
                        var modeloObj = ModeloVehiculoCln.obtenerUno(idSel);
                        if (modeloObj != null)
                            descripcion = modeloObj.descripcion;
                    }
                }

                if (string.IsNullOrWhiteSpace(descripcion))
                    descripcion = "(Sin descripción disponible)";

                MessageBox.Show(descripcion, "Descripción del Modelo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener descripción del modelo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== MÉTODOS AUXILIARES ====================

        private void ConfigurarVelocidades(string tipoTransmision, NumericUpDown nudVel, Label lblInfo, int? valorActual, bool permitirEdicion)
        {
            lblInfo.Visible = true;

            Button btnBuscarVel = null;
            try
            {
                var found = nudVel.Parent.Controls.Find("btnBuscarVelocidades", true);
                if (found.Length > 0 && found[0] is Button)
                    btnBuscarVel = (Button)found[0];
            }
            catch { }

            Label lblPlaceholder = null;
            try
            {
                var found = nudVel.Parent.Controls.Find("lblVelocidadesPlaceholder", true);
                if (found.Length > 0 && found[0] is Label)
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

                    decimal valManual = valorActual.HasValue ? (decimal)valorActual.Value : 5m;
                    valManual = Math.Max(nudVel.Minimum, Math.Min(nudVel.Maximum, valManual));
                    nudVel.Value = valManual;

                    lblInfo.Text = "(Típico: 5-6 velocidades)";
                    lblInfo.ForeColor = Color.FromArgb(40, 167, 69);
                    if (btnBuscarVel != null) btnBuscarVel.Visible = true;
                    break;

                case "Automática":
                    nudVel.Visible = true;
                    lblPlaceholder.Visible = false;
                    nudVel.Minimum = 4;
                    nudVel.Maximum = 10;
                    nudVel.Enabled = permitirEdicion;

                    decimal valAuto = valorActual.HasValue ? (decimal)valorActual.Value : 6m;
                    valAuto = Math.Max(nudVel.Minimum, Math.Min(nudVel.Maximum, valAuto));
                    nudVel.Value = valAuto;

                    lblInfo.Text = "(Típico: 6-10 velocidades)";
                    lblInfo.ForeColor = Color.FromArgb(0, 123, 255);
                    if (btnBuscarVel != null) btnBuscarVel.Visible = true;
                    break;

                case "CVT":
                    nudVel.Visible = false;
                    nudVel.Enabled = false;
                    lblPlaceholder.Text = "—";
                    lblPlaceholder.Visible = true;

                    lblInfo.Text = "(Sin velocidades fijas)";
                    lblInfo.ForeColor = Color.FromArgb(108, 117, 125);
                    if (btnBuscarVel != null) btnBuscarVel.Visible = false;
                    break;

                case "Semiautomática":
                case "Doble Embrague":
                    nudVel.Visible = true;
                    lblPlaceholder.Visible = false;
                    nudVel.Minimum = 6;
                    nudVel.Maximum = 8;
                    nudVel.Enabled = permitirEdicion;

                    decimal valDoble = valorActual.HasValue ? (decimal)valorActual.Value : 7m;
                    valDoble = Math.Max(nudVel.Minimum, Math.Min(nudVel.Maximum, valDoble));
                    nudVel.Value = valDoble;

                    lblInfo.Text = "(Típico: 7-8 velocidades)";
                    lblInfo.ForeColor = Color.FromArgb(255, 193, 7);
                    if (btnBuscarVel != null) btnBuscarVel.Visible = true;
                    break;

                default:
                    nudVel.Visible = true;
                    lblPlaceholder.Visible = false;
                    nudVel.Enabled = false;
                    lblInfo.Text = "(Tipo no especificado)";
                    lblInfo.ForeColor = Color.Gray;
                    if (btnBuscarVel != null) btnBuscarVel.Visible = false;
                    break;
            }
        }

        private string ObtenerInfoTraccion(string traccion)
        {
            if (string.IsNullOrEmpty(traccion)) return "";

            if (traccion.Contains("Delantera")) return "Sedán/Hatchback";
            if (traccion.Contains("Trasera")) return "Deportivos/Coupé";
            if (traccion.Contains("Integral")) return "SUV moderna";
            if (traccion.Contains("Doble")) return "Pickup/Off-road";

            return "";
        }

        private Color ObtenerColorTraccion(string traccion)
        {
            if (string.IsNullOrEmpty(traccion)) return Color.Gray;

            if (traccion.Contains("Delantera")) return Color.FromArgb(0, 123, 255);
            if (traccion.Contains("Trasera")) return Color.FromArgb(220, 53, 69);
            if (traccion.Contains("Integral")) return Color.FromArgb(40, 167, 69);
            if (traccion.Contains("Doble")) return Color.FromArgb(255, 193, 7);

            return Color.Gray;
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (imagenCargada != null)
            {
                imagenCargada.Dispose();
                imagenCargada = null;
            }
            base.OnFormClosing(e);
        }
    }
}
