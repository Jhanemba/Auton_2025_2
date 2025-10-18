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
    public partial class FrmNuevoModelo : Form
    {
        private int idMarca;
        private int idTipo;
        private int anioSugerido;
        public int NuevoIdModelo { get; private set; }

        private TextBox txtNombre;
        private NumericUpDown nudAnioInicio;
        private NumericUpDown nudAnioFin;
        private CheckBox chkEnProduccion;
        private TextBox txtMotor;
        private ComboBox cboCombustible;
        private ComboBox cboTransmision;
        private CheckBox chkTieneTurbo;
        private NumericUpDown nudAirbags;
        private ComboBox cboSeguridadChoques;
        private ComboBox cboOrganizacionSeguridad;
        private TextBox txtDescripcion;

        public FrmNuevoModelo(int idMarca, int idTipo) : this(idMarca, idTipo, DateTime.Now.Year) { }

        public FrmNuevoModelo(int idMarca, int idTipo, int anioSugerido)
        {
            InitializeComponent();
            this.idMarca = idMarca;
            this.idTipo = idTipo;
            this.anioSugerido = anioSugerido;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Registrar Nuevo Modelo";
            this.Size = new Size(1000, 720);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 10F);

            Panel pnlPrincipal = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(984, 610),
                Padding = new Padding(25),
                BackColor = Color.White
            };

            int yPos = 10;
            int lineHeight = 48;
            int labelWidth = 150;
            int ctrlWidthLeft = 300;
            int ctrlWidthRight = 300;
            int colLeftX = 180;
            int colRightX = 580;

            // Título
            Label lblTitulo = new Label
            {
                Text = "🚙 NUEVO MODELO DE VEHÍCULO",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(25, yPos),
                Size = new Size(930, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 45;

            // COLUMNA IZQUIERDA
            AgregarSeccion(pnlPrincipal, "📋 IDENTIFICACIÓN", 25, yPos, 440);
            int yPosLeft = yPos + 38;

            // Nombre
            AgregarLabel(pnlPrincipal, "Nombre:", 25, yPosLeft, labelWidth);
            txtNombre = new TextBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidthLeft, 28),
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(txtNombre);
            yPosLeft += lineHeight;

            // Año Inicio
            AgregarLabel(pnlPrincipal, "Año Inicio:", 25, yPosLeft, labelWidth);
            nudAnioInicio = new NumericUpDown
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(120, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = anioSugerido,
                TextAlign = HorizontalAlignment.Center
            };
            pnlPrincipal.Controls.Add(nudAnioInicio);

            chkEnProduccion = new CheckBox
            {
                Text = "✓ En producción",
                Location = new Point(colLeftX + 130, yPosLeft + 3),
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Checked = true
            };
            chkEnProduccion.CheckedChanged += (s, e) =>
            {
                nudAnioFin.Enabled = !chkEnProduccion.Checked;
            };
            pnlPrincipal.Controls.Add(chkEnProduccion);
            yPosLeft += lineHeight;

            // Año Fin
            AgregarLabel(pnlPrincipal, "Año Fin:", 25, yPosLeft, labelWidth);
            nudAnioFin = new NumericUpDown
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(120, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year,
                TextAlign = HorizontalAlignment.Center,
                Enabled = false
            };
            pnlPrincipal.Controls.Add(nudAnioFin);
            yPosLeft += lineHeight;

            // Motor
            AgregarLabel(pnlPrincipal, "Motor:", 25, yPosLeft, labelWidth);
            txtMotor = new TextBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(ctrlWidthLeft, 28),
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(txtMotor);
            yPosLeft += lineHeight;

            // Combustible
            AgregarLabel(pnlPrincipal, "Combustible:", 25, yPosLeft, labelWidth);
            cboCombustible = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboCombustible.Items.AddRange(new[] { "Gasolina", "Diesel", "Híbrido", "Eléctrico", "GLP", "GNC" });
            cboCombustible.SelectedIndex = 0;
            pnlPrincipal.Controls.Add(cboCombustible);
            yPosLeft += lineHeight;

            // Transmisión ⚠️ IMPORTANTE: Ahora es ÚNICA por modelo
            AgregarLabel(pnlPrincipal, "Transmisión:", 25, yPosLeft, labelWidth);
            cboTransmision = new ComboBox
            {
                Location = new Point(colLeftX, yPosLeft),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboTransmision.Items.AddRange(new[] { "Manual", "Automática", "CVT", "Semiautomática", "Doble Embrague" });
            cboTransmision.SelectedIndex = 0;
            pnlPrincipal.Controls.Add(cboTransmision);

            // Label informativo sobre transmisión
            Label lblInfoTransmision = new Label
            {
                Text = "ℹ️ La transmisión identifica modelos únicos",
                Location = new Point(colLeftX, yPosLeft + 32),
                Size = new Size(300, 18),
                Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                ForeColor = Color.DarkBlue
            };
            pnlPrincipal.Controls.Add(lblInfoTransmision);
            yPosLeft += lineHeight;

            // Turbo
            chkTieneTurbo = new CheckBox
            {
                Text = "🚀 Tiene Turbo",
                Location = new Point(colLeftX, yPosLeft + 3),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.FromArgb(220, 53, 69)
            };
            pnlPrincipal.Controls.Add(chkTieneTurbo);
            yPosLeft += lineHeight;

            // COLUMNA DERECHA - SEGURIDAD
            AgregarSeccion(pnlPrincipal, "🛡️ SEGURIDAD", 490, yPos, 470);
            int yPosRight = yPos + 38;

            // Airbags (SIN "Nro." para evitar espacio)
            AgregarLabel(pnlPrincipal, "Airbags:", 490, yPosRight, labelWidth);
            nudAirbags = new NumericUpDown
            {
                Location = new Point(colRightX, yPosRight),
                Size = new Size(100, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 12,
                Value = 2,
                TextAlign = HorizontalAlignment.Center
            };
            pnlPrincipal.Controls.Add(nudAirbags);
            yPosRight += lineHeight;

            // Organización de Seguridad
            AgregarLabel(pnlPrincipal, "Organización:", 490, yPosRight, labelWidth);
            cboOrganizacionSeguridad = new ComboBox
            {
                Location = new Point(colRightX, yPosRight),
                Size = new Size(ctrlWidthRight, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboOrganizacionSeguridad.Items.AddRange(new[] {
                "Latin NCAP", "Euro NCAP", "IIHS", "NHTSA", "ANCAP", "Sin calificación"
            });
            cboOrganizacionSeguridad.SelectedIndex = 0;
            pnlPrincipal.Controls.Add(cboOrganizacionSeguridad);
            yPosRight += lineHeight;

            // Seguridad en Choques (Estrellas) - AHORA CON 0 ESTRELLAS
            AgregarLabel(pnlPrincipal, "Estrellas:", 490, yPosRight, labelWidth);
            cboSeguridadChoques = new ComboBox
            {
                Location = new Point(colRightX, yPosRight),
                Size = new Size(250, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboSeguridadChoques.Items.AddRange(new[] {
                "⭐⭐⭐⭐⭐ (5 estrellas)",
                "⭐⭐⭐⭐ (4 estrellas)",
                "⭐⭐⭐ (3 estrellas)",
                "⭐⭐ (2 estrellas)",
                "⭐ (1 estrella)",
                "☆ (0 estrellas - Sin seguridad)",
                "Sin calificación"
            });
            cboSeguridadChoques.SelectedIndex = 2;
            pnlPrincipal.Controls.Add(cboSeguridadChoques);
            yPosRight += lineHeight;

            // Descripción (ancho completo)
            yPos = Math.Max(yPosLeft, yPosRight) + 15;
            AgregarLabel(pnlPrincipal, "Descripción:", 25, yPos, labelWidth);
            txtDescripcion = new TextBox
            {
                Location = new Point(colLeftX, yPos),
                Size = new Size(780, 70),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(txtDescripcion);

            // Panel de botones
            Panel pnlBotones = new Panel
            {
                Location = new Point(0, 610),
                Size = new Size(984, 70),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnGuardar = new Button
            {
                Text = "💾 Guardar Modelo",
                Size = new Size(180, 45),
                Location = new Point(330, 12),
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
                Size = new Size(180, 45),
                Location = new Point(530, 12),
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

        private bool Validar()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("⚠️ Debe ingresar el nombre del modelo", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (txtNombre.Text.Length > 100)
            {
                MessageBox.Show("⚠️ El nombre no puede exceder 100 caracteres", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int anioInicio = (int)nudAnioInicio.Value;
            int anioFin = (int)nudAnioFin.Value;

            if (!chkEnProduccion.Checked && anioFin < anioInicio)
            {
                MessageBox.Show($"⚠️ El año fin ({anioFin}) debe ser mayor o igual al año inicio ({anioInicio})",
                              "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudAnioFin.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMotor.Text))
            {
                MessageBox.Show("⚠️ Debe especificar el tipo de motor", "Validación",
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

            if (cboTransmision.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Debe seleccionar el tipo de transmisión", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTransmision.Focus();
                return false;
            }

            // ✅ VALIDACIÓN CRÍTICA: Mismo modelo + transmisión = DUPLICADO
            try
            {
                var todosLosModelos = ModeloVehiculoCln.listar();
                if (todosLosModelos != null && todosLosModelos.Count > 0)
                {
                    string nombreBuscado = txtNombre.Text.Trim();
                    string transmisionBuscada = cboTransmision.SelectedItem.ToString();

                    // Validamos modelo + transmisión (manejando NULLs)
                    var modeloExistente = todosLosModelos
                        .Where(m => m.idMarca == idMarca && m.estado != -1)
                        .FirstOrDefault(m =>
                            m.nombre != null &&
                            m.nombre.Equals(nombreBuscado, StringComparison.OrdinalIgnoreCase) &&
                            m.transmision != null &&
                            m.transmision.Equals(transmisionBuscada, StringComparison.OrdinalIgnoreCase) &&
                            //m.tieneTurbo.Value == tieneTurboBuscado
                        );

                    if (modeloExistente != null)
                    {
                        MessageBox.Show($"⚠️ El modelo '{nombreBuscado}' con transmisión '{transmisionBuscada}' ya existe para esta marca.\n\n" +
                                      $"💡 Nota: Si deseas registrar el mismo modelo con transmisión diferente (Ej: Manual vs CVT), " +
                                      $"selecciona otra opción de transmisión.",
                                      "Modelo Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cboTransmision.Focus();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Advertencia al validar duplicados: {ex.Message}\n\nSe procederá con el registro.",
                              "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return true;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    // Extraer el número de estrellas del texto (ahora incluye 0)
                    int estrellas = 0;
                    if (cboSeguridadChoques.SelectedIndex < 6) // Si no es "Sin calificación"
                    {
                        estrellas = 5 - cboSeguridadChoques.SelectedIndex;
                    }

                    var nuevoModelo = new ModeloVehiculo
                    {
                        idMarca = idMarca,
                        idTipoVehiculo = idTipo,
                        nombre = txtNombre.Text.Trim(),
                        anioInicio = (int)nudAnioInicio.Value,
                        anioFin = chkEnProduccion.Checked ? (int?)null : (int)nudAnioFin.Value,
                        motor = txtMotor.Text.Trim(),
                        combustible = cboCombustible.SelectedItem.ToString(),
                        transmision = cboTransmision.SelectedItem.ToString(),
                        tieneTurbo = chkTieneTurbo.Checked,
                        nroAirbags = (int)nudAirbags.Value,
                        seguridadChoques = estrellas,
                        organizacionSeguridad = cboOrganizacionSeguridad.SelectedItem.ToString(),
                        estado = 1,
                        usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : Environment.UserName,
                        fechaRegistro = DateTime.Now
                    };

                    NuevoIdModelo = ModeloVehiculoCln.insertar(nuevoModelo);

                    if (NuevoIdModelo > 0)
                    {
                        string turboInfo = chkTieneTurbo.Checked ? " 🚀 CON TURBO" : "";
                        string airbagInfo = $"\nAirbags: {nudAirbags.Value}";
                        string seguridadInfo = estrellas >= 0 ? $"\nSeguridad: {estrellas} estrellas ({cboOrganizacionSeguridad.SelectedItem})" : "";
                        string transmisionInfo = $"\nTransmisión: {cboTransmision.SelectedItem}";

                        MessageBox.Show($"✅ Modelo registrado exitosamente{turboInfo}\n\n" +
                                      $"ID: {NuevoIdModelo}\n" +
                                      $"Nombre: {txtNombre.Text}\n" +
                                      $"Motor: {txtMotor.Text}" +
                                      transmisionInfo +
                                      airbagInfo + seguridadInfo,
                                      "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("❌ No se pudo registrar el modelo. El ID retornado es 0.",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al guardar el modelo:\n\n{ex.Message}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
