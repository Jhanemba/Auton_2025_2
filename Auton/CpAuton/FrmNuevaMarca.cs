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
    public partial class FrmNuevaMarca : Form
    {
        private int idProcedencia;
        public int NuevoIdMarca { get; private set; }

        public FrmNuevaMarca(int idProcedencia)
        {
            InitializeComponent();
            this.idProcedencia = idProcedencia;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "...::: Auton - Nueva Marca :::...";
            this.Size = new Size(450, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Panel pnlPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            int yPos = 20;

            // Título
            Label lblTitulo = new Label
            {
                Text = "NUEVA MARCA",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(20, yPos),
                Size = new Size(350, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 40;

            // Nombre
            Label lblNombre = new Label
            {
                Text = "Nombre Marca:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(100, 20)
            };
            txtNombre = new TextBox
            {
                Location = new Point(130, yPos),
                Size = new Size(280, 23)
            };
            pnlPrincipal.Controls.Add(lblNombre);
            pnlPrincipal.Controls.Add(txtNombre);
            yPos += 35;

            // Descripción
            Label lblDescripcion = new Label
            {
                Text = "Descripción:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(100, 20)
            };
            txtDescripcion = new TextBox
            {
                Location = new Point(130, yPos),
                Size = new Size(280, 70),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            pnlPrincipal.Controls.Add(lblDescripcion);
            pnlPrincipal.Controls.Add(txtDescripcion);
            yPos += 80;

            // Botones
            Button btnGuardar = new Button
            {
                Text = "Guardar",
                Size = new Size(100, 35),
                Location = new Point(130, yPos),
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnGuardar.Click += BtnGuardar_Click;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(100, 35),
                Location = new Point(240, yPos),
                BackColor = Color.FromArgb(217, 83, 79),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnCancelar.Click += (s, e) => this.Close();

            pnlPrincipal.Controls.Add(btnGuardar);
            pnlPrincipal.Controls.Add(btnCancelar);

            this.Controls.Add(pnlPrincipal);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    var nuevaMarca = new Marca
                    {
                        idProcedencia = idProcedencia,
                        nombre = txtNombre.Text.Trim(),
                        descripcion = string.IsNullOrEmpty(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                        estado = 1,
                        usuarioRegistro = Util.usuario != null ? Util.usuario.usuario : "sistema",
                        fechaRegistro = DateTime.Now  // IMPORTANTE ESTO    
                    };

                    NuevoIdMarca = MarcaCln.insertar(nuevaMarca);

                    if (NuevoIdMarca > 0)
                    {
                        MessageBox.Show($"Marca registrada exitosamente",
                                      "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar la marca: {ex.Message}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool Validar()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar el nombre de la marca", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (txtNombre.Text.Length > 50)
            {
                MessageBox.Show("El nombre no puede exceder 50 caracteres", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // VALIDACIÓN: Verificar si la marca ya existe en esta procedencia
            var marcaExistente = MarcaCln.listar()
                .FirstOrDefault(m => m.idProcedencia == idProcedencia &&
                                     m.nombre.ToUpper() == txtNombre.Text.Trim().ToUpper() &&
                                     m.estado != -1);

            if (marcaExistente != null)
            {
                MessageBox.Show($"La marca '{txtNombre.Text.Trim()}' ya existe en esta procedencia",
                              "Marca Duplicada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            return true;
        }

        private TextBox txtNombre;
        private TextBox txtDescripcion;

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    this.Name = "FrmNuevaMarca";
        //    this.ResumeLayout(false);
        //}
    }
}
