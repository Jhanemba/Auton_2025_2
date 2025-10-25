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
        public int NuevoIdModelo { get; private set; }

        private TextBox txtNombre;
        private TextBox txtDescripcion;

        public FrmNuevoModelo(int idMarca, int idTipo)
        {
            InitializeComponent();
            this.idMarca = idMarca;
            this.idTipo = idTipo;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "...::: Auton - Registrar Nuevo Modelo :::...";
            this.Size = new Size(600, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 10F);

            Panel pnlPrincipal = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(584, 250),
                Padding = new Padding(25),
                BackColor = Color.White
            };

            int yPos = 10;
            int lineHeight = 55;
            int labelWidth = 150;
            int ctrlWidth = 380;
            int colX = 160;

            // Título
            Label lblTitulo = new Label
            {
                Text = "🚙 NUEVO MODELO (SIMPLIFICADO)",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(25, yPos),
                Size = new Size(530, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 50;

            // Info
            Label lblInfo = new Label
            {
                Text = "ℹ️ Solo registra el nombre del modelo. Las características específicas\n" +
                       "    (generación, país, transmisión, etc) se definen al registrar cada vehículo.",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.DarkBlue,
                Location = new Point(25, yPos),
                Size = new Size(530, 35)
            };
            pnlPrincipal.Controls.Add(lblInfo);
            yPos += 45;

            // Nombre del Modelo
            AgregarLabel(pnlPrincipal, "Nombre Modelo:", 25, yPos, labelWidth);
            txtNombre = new TextBox
            {
                Location = new Point(colX+20, yPos),
                Size = new Size(ctrlWidth, 28),
                Font = new Font("Segoe UI", 11F),
                MaxLength = 100
            };
            pnlPrincipal.Controls.Add(txtNombre);

            Label lblEjemplo = new Label
            {
                Text = "Ejemplos: Corolla, Civic, F-150, Versa, Baleno",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(colX, yPos + 32),
                Size = new Size(ctrlWidth, 18)
            };
            pnlPrincipal.Controls.Add(lblEjemplo);
            yPos += lineHeight;

            // Descripción (Opcional)
            AgregarLabel(pnlPrincipal, "Descripción:", 25, yPos, labelWidth);
            txtDescripcion = new TextBox
            {
                Location = new Point(colX+20, yPos),
                Size = new Size(ctrlWidth, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F)
            };
            pnlPrincipal.Controls.Add(txtDescripcion);

            // Panel de botones
            Panel pnlBotones = new Panel
            {
                Location = new Point(0, 250),
                Size = new Size(584, 70),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnGuardar = new Button
            {
                Text = "💾 Guardar Modelo",
                Size = new Size(180, 45),
                Location = new Point(150, 12),
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
                Location = new Point(350, 12),
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

            // Focus en nombre
            txtNombre.Focus();
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
                txtNombre.Focus();
                return false;
            }

            // ✅ VALIDACIÓN SIMPLE: Solo nombre + marca
            try
            {
                var todosLosModelos = ModeloVehiculoCln.listar();
                if (todosLosModelos != null && todosLosModelos.Count > 0)
                {
                    string nombreBuscado = txtNombre.Text.Trim();

                    var modeloExistente = todosLosModelos
                        .Where(m => m.idMarca == idMarca && m.estado != -1)
                        .FirstOrDefault(m =>
                            m.nombre != null &&
                            m.nombre.Equals(nombreBuscado, StringComparison.OrdinalIgnoreCase)
                        );

                    if (modeloExistente != null)
                    {
                        MessageBox.Show($"⚠️ El modelo '{nombreBuscado}' ya existe para esta marca.\n\n" +
                                      $"💡 Si necesitas diferentes variantes del mismo modelo\n" +
                                      $"   (Ej: Corolla Gen 12 Japón vs Corolla Gen 11 Brasil),\n" +
                                      $"   usa el mismo modelo y especifica las diferencias al\n" +
                                      $"   registrar cada vehículo individual.",
                                      "Modelo Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtNombre.Focus();
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
                    // Construir el objeto antes de insertar
                    var nuevoModelo = new ModeloVehiculo
                    {
                        idMarca = this.idMarca,
                        idTipoVehiculo = this.idTipo,
                        nombre = txtNombre.Text.Trim(),
                        descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                        estado = 1,
                        usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : Environment.UserName,
                        fechaRegistro = DateTime.Now
                    };

                    // Insertar y obtener ID
                    NuevoIdModelo = ModeloVehiculoCln.insertar(nuevoModelo);

                    if (NuevoIdModelo > 0)
                    {
                        MessageBox.Show($"✅ Modelo registrado exitosamente\n\n" +
                                      $"ID: {NuevoIdModelo}\n" +
                                      $"Nombre: {txtNombre.Text}\n\n" +
                                      $"💡 Ahora puedes registrar vehículos específicos de este modelo\n" +
                                      $"   especificando generación, país, transmisión, etc.",
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
