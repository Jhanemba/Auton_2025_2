using CadAuton;
using ClnAuton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private ComboBox cboProcedencia;
        private ComboBox cboMarca;
        private ComboBox cboTipo;
        private ComboBox cboModelo;
        private NumericUpDown nudAnio;
        private ComboBox cboConfiabilidad;
        private NumericUpDown nudPuntuacion;
        private NumericUpDown nudKilometraje;
        private TextBox txtPropietario;
        private TextBox txtObservaciones;
        private TextBox txtNroChasis;
        private Button btnAgregarMarca;
        private Button btnAgregarModelo;
        private CheckBox chkKilometrajeDesconocido;

        public FrmRegistroVehiculo()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Registrar Nuevo Vehículo";
            this.Size = new Size(1150, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 10F);

            Panel pnlPrincipal = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1134, 610),
                Padding = new Padding(20),
                BackColor = Color.White
            };

            int yPos = 15;
            int lineHeight = 48;
            int labelWidth = 180;
            int ctrlWidthLeft = 380;
            int ctrlWidthRight = 380;
            int colLeftX = 200;
            int colRightX = 650;

            // Título principal
            Label lblTitulo = new Label
            {
                Text = "🚗 REGISTRO DE NUEVO VEHÍCULO",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(20, yPos),
                Size = new Size(1090, 32),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 45;

            // COLUMNA IZQUIERDA
            AgregarSeccionCompacta(pnlPrincipal, "📋 DATOS DEL MODELO", 20, yPos, 560);
            int yPosLeft = yPos + 38;

            // Procedencia
            AgregarLabelCompacto(pnlPrincipal, "Procedencia:", 20, yPosLeft, labelWidth);
            cboProcedencia = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidthLeft, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboProcedencia.SelectedIndexChanged += CboProcedencia_SelectedIndexChanged;
            pnlPrincipal.Controls.Add(cboProcedencia);
            yPosLeft += lineHeight;

            // Marca
            AgregarLabelCompacto(pnlPrincipal, "Marca:", 20, yPosLeft, labelWidth);
            cboMarca = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(280, 28),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 10F),
                Enabled = false
            };
            cboMarca.SelectedIndexChanged += CboMarca_SelectedIndexChanged;
            pnlPrincipal.Controls.Add(cboMarca);

            btnAgregarMarca = new Button
            {
                Text = "+",
                Location = new Point(colLeftX + 285, yPosLeft),
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

            // Tipo Vehículo
            AgregarLabelCompacto(pnlPrincipal, "Tipo de Vehículo:", 20, yPosLeft, labelWidth);
            cboTipo = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidthLeft, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(cboTipo);
            yPosLeft += lineHeight;

            // Modelo ⚠️ MEJORADO: Ahora muestra transmisión y turbo
            AgregarLabelCompacto(pnlPrincipal, "Modelo:", 20, yPosLeft, labelWidth);
            cboModelo = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(280, 28),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 10F),
                Enabled = false
            };
            pnlPrincipal.Controls.Add(cboModelo);

            btnAgregarModelo = new Button
            {
                Text = "+",
                Location = new Point(colLeftX + 285, yPosLeft),
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
            AgregarLabelCompacto(pnlPrincipal, "Nro. de Chasis:", 20, yPosLeft, labelWidth);
            txtNroChasis = new TextBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidthLeft, 28),
                Font = new Font("Segoe UI", 10F),
                CharacterCasing = CharacterCasing.Upper,
                MaxLength = 17
            };
            pnlPrincipal.Controls.Add(txtNroChasis);
            SetCueBanner(txtNroChasis, "Ej: 1HGBH41JXMN109186 (17 caracteres)");
            yPosLeft += lineHeight;

            // COLUMNA DERECHA
            AgregarSeccionCompacta(pnlPrincipal, "⚙️ CARACTERÍSTICAS", 590, yPos, 520);
            int yPosRight = yPos + 38;

            // Año
            AgregarLabelCompacto(pnlPrincipal, "Año:", 590, yPosRight, 120);
            nudAnio = new NumericUpDown
            {
                Location = new Point(colRightX + 60, yPosRight),
                Size = new Size(120, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year,
                TextAlign = HorizontalAlignment.Center
            };
            pnlPrincipal.Controls.Add(nudAnio);
            yPosRight += lineHeight;

            // Confiabilidad
            AgregarLabelCompacto(pnlPrincipal, "Confiabilidad:", 590, yPosRight, 120);
            cboConfiabilidad = new ComboBox
            {
                Location = new Point(colRightX + 60, yPosRight),
                Size = new Size(200, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
            cboConfiabilidad.SelectedIndex = 1;
            pnlPrincipal.Controls.Add(cboConfiabilidad);
            yPosRight += lineHeight;

            // Puntuación
            AgregarLabelCompacto(pnlPrincipal, "Puntuación:", 590, yPosRight, 120);
            nudPuntuacion = new NumericUpDown
            {
                Location = new Point(colRightX + 60, yPosRight),
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
            AgregarLabelCompacto(pnlPrincipal, "Kilometraje:", 590, yPosRight, 120);
            nudKilometraje = new NumericUpDown
            {
                Location = new Point(colRightX + 60, yPosRight),
                Size = new Size(150, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 999999,
                ThousandsSeparator = true,
                Value = 0,
                TextAlign = HorizontalAlignment.Right
            };
            pnlPrincipal.Controls.Add(nudKilometraje);

            chkKilometrajeDesconocido = new CheckBox
            {
                Text = "Desconocido",
                Location = new Point(colRightX + 220, yPosRight + 5),
                Font = new Font("Segoe UI", 9F),
                AutoSize = true
            };
            chkKilometrajeDesconocido.CheckedChanged += (s, e) =>
            {
                nudKilometraje.Enabled = !chkKilometrajeDesconocido.Checked;
                if (chkKilometrajeDesconocido.Checked) nudKilometraje.Value = 0;
            };
            pnlPrincipal.Controls.Add(chkKilometrajeDesconocido);
            yPosRight += lineHeight;

            // Propietario
            AgregarLabelCompacto(pnlPrincipal, "Propietario:", 590, yPosRight, 120);
            txtPropietario = new TextBox
            {
                Location = new Point(colRightX + 60, yPosRight),
                Size = new Size(ctrlWidthRight, 28),
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(txtPropietario);
            SetCueBanner(txtPropietario, "Nombre del propietario (opcional)");
            yPosRight += lineHeight;

            // OBSERVACIONES - Ancho completo
            yPos = Math.Max(yPosLeft, yPosRight) + 15;
            AgregarLabelCompacto(pnlPrincipal, "Observaciones:", 20, yPos, labelWidth);
            txtObservaciones = new TextBox
            {
                Location = new Point(colLeftX, yPos),
                Size = new Size(910, 65),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(txtObservaciones);
            SetCueBanner(txtObservaciones, "Detalles adicionales del vehículo...");

            // Panel de botones
            Panel pnlBotones = new Panel
            {
                Location = new Point(0, 610),
                Size = new Size(1134, 70),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnGuardar = new Button
            {
                Text = "💾 Guardar Vehículo",
                Size = new Size(200, 45),
                Location = new Point(400, 12),
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
                Location = new Point(620, 12),
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

            this.Controls.Add(pnlBotones);
            this.Controls.Add(pnlPrincipal);
        }

        private void AgregarSeccionCompacta(Panel panel, string titulo, int x, int yPos, int width)
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

        private void AgregarLabelCompacto(Panel panel, string texto, int x, int yPos, int width)
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
                    modelos = ModeloVehiculoCln.listar().Where(m => m.idMarca == idMarca && m.estado != -1).ToList();

                    // ✅ MEJORADO: Formato descriptivo que incluye transmisión y turbo
                    var modelosFormateados = modelos.Select(m => new
                    {
                        m.id,
                        DisplayText = FormatearNombreModelo(m)
                    }).ToList();

                    cboModelo.DataSource = modelosFormateados;
                    cboModelo.DisplayMember = "DisplayText";
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

        /// <summary>
        /// ✅ MEJORADO: Formatea el nombre del modelo mostrando país, turbo y transmisión
        /// Ejemplos:
        /// - "Baleno (India) SIN Turbo - Manual" - 2 airbags
        /// - "Baleno (Hungría) SIN Turbo - Manual" - 6 airbags
        /// - "F-150 (EE.UU.) SIN Turbo - Manual"
        /// - "F-150 (EE.UU.) 🚀 CON Turbo - Manual"
        /// - "Corolla (Japón) - CVT" - Premium
        /// - "Corolla (Brasil) - CVT" - Versión Latino
        /// </summary>
        private string FormatearNombreModelo(ModeloVehiculo modelo)
        {
            string nombre = modelo.nombre ?? "Sin nombre";
            string transmision = modelo.transmision ?? "N/A";
            string pais = modelo.paisFabricacion ?? "N/A";

            // ✅ Abreviar países largos para mejor visualización
            string paisAbreviado = AbrevarPais(pais);

            // ✅ Mostrar explícitamente si tiene turbo o no
            string infoTurbo = "";
            if (modelo.tieneTurbo.HasValue)
            {
                if (modelo.tieneTurbo.Value)
                {
                    infoTurbo = " 🚀 CON Turbo";
                }
                else
                {
                    infoTurbo = " SIN Turbo";
                }
            }

            // Formato: "Baleno (India) SIN Turbo - Manual"
            return $"{nombre} ({paisAbreviado}){infoTurbo} - {transmision}";
        }

        /// <summary>
        /// Abrevia nombres de países largos para mejor visualización
        /// </summary>
        private string AbrevarPais(string pais)
        {
            var abreviaturas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Estados Unidos", "EE.UU." },
                { "Reino Unido", "UK" },
                { "República Checa", "R. Checa" },
                { "Corea del Sur", "Corea S." },
                { "Sudáfrica", "Sudáfr." }
            };

            return abreviaturas.ContainsKey(pais) ? abreviaturas[pais] : pais;
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
                int anioVehiculo = (int)nudAnio.Value;
                using (var frmNuevoModelo = new FrmNuevoModelo(idMarca, idTipo, anioVehiculo))
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
                    int? kilometraje = chkKilometrajeDesconocido.Checked ? (int?)null : (int)nudKilometraje.Value;

                    var nuevoVehiculo = new Vehiculo
                    {
                        nroChasis = txtNroChasis.Text.Trim().ToUpper(),
                        idModeloVehiculo = idModelo,
                        anioFabricacion = anio,
                        confiabilidad = cboConfiabilidad.SelectedItem.ToString(),
                        puntuacion = puntuacion,
                        kilometraje = kilometraje,
                        propietario = string.IsNullOrWhiteSpace(txtPropietario.Text) ? null : txtPropietario.Text.Trim(),
                        observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim(),
                        estado = 1,
                        usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : Environment.UserName,
                        fechaRegistro = DateTime.Now
                    };

                    int resultado = VehiculoCln.insertar(nuevoVehiculo);

                    if (resultado > 0)
                    {
                        // Obtener el modelo seleccionado para mostrar info completa
                        var modeloSeleccionado = modelos.FirstOrDefault(m => m.id == idModelo);
                        string infoModelo = modeloSeleccionado != null ? FormatearNombreModelo(modeloSeleccionado) : "";

                        MessageBox.Show($"✅ Vehículo registrado exitosamente\n\n" +
                                      $"Chasis: {txtNroChasis.Text}\n" +
                                      $"Modelo: {infoModelo}",
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "FrmRegistroVehiculo";
            this.Load += new System.EventHandler(this.FrmRegistroVehiculo_Load);
            this.ResumeLayout(false);
        }
    }
}
