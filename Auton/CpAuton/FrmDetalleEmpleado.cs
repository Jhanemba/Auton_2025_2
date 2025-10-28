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
    public partial class FrmDetalleEmpleado : Form
    {
        private int idEmpleado;

        public FrmDetalleEmpleado(int idEmpleado)
        {
            this.idEmpleado = idEmpleado;
            InitializeComponent();
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    this.Text = "...::: Detalle del Empleado :::...";
        //    this.ClientSize = new System.Drawing.Size(600, 500);
        //    this.StartPosition = FormStartPosition.CenterParent;
        //    this.FormBorderStyle = FormBorderStyle.FixedDialog;
        //    this.MaximizeBox = false;
        //    this.MinimizeBox = false;
        //    this.BackColor = Color.White;
        //    this.Load += new System.EventHandler(this.FrmDetalleEmpleado_Load);
        //    this.ResumeLayout(false);
        //}

        private void FrmDetalleEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                using (var context = new AutonEntities())
                {
                    var empleado = context.Empleado.Find(idEmpleado);
                    if (empleado == null)
                    {
                        MessageBox.Show("❌ Empleado no encontrado", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    var usuario = context.Usuario.FirstOrDefault(u => u.idEmpleado == idEmpleado && u.estado != -1);

                    // Panel principal
                    Panel pnlPrincipal = new Panel
                    {
                        Location = new Point(20, 20),
                        Size = new Size(560, 420),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White
                    };

                    int yPos = 20;

                    // Título
                    Label lblTitulo = new Label
                    {
                        Text = "INFORMACIÓN DEL EMPLEADO",
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = Color.FromArgb(51, 122, 183),
                        Location = new Point(20, yPos),
                        Size = new Size(520, 30),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    pnlPrincipal.Controls.Add(lblTitulo);
                    yPos += 50;

                    // Información
                    string primerApellido = empleado.primerApellido != null ? empleado.primerApellido : "";
                    string segundoApellido = empleado.segundoApellido != null ? empleado.segundoApellido : "";
                    string nombreCompleto = (empleado.nombres + " " + primerApellido + " " + segundoApellido).Trim();

                    AgregarInfo(pnlPrincipal, "CI:", empleado.cedulaIdentidad, ref yPos);
                    AgregarInfo(pnlPrincipal, "Nombre:", nombreCompleto, ref yPos);
                    AgregarInfo(pnlPrincipal, "Cargo:", empleado.Cargo.descripcion, ref yPos);
                    AgregarInfo(pnlPrincipal, "Celular:", empleado.celular.ToString(), ref yPos);
                    AgregarInfo(pnlPrincipal, "Dirección:", empleado.direccion, ref yPos);
                    AgregarInfo(pnlPrincipal, "Fecha Registro:", empleado.fechaRegistro.ToString("dd/MM/yyyy"), ref yPos);

                    yPos += 10;

                    // Estado del usuario
                    Label lblUsuarioEstado = new Label
                    {
                        Text = usuario != null ?
                            "✅ TIENE USUARIO: " + usuario.usuario1 + " (" + (usuario.esAdministrador ? "Admin" : "Normal") + ")" :
                            "⚠️ NO TIENE USUARIO ASIGNADO",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = usuario != null ? Color.Green : Color.Orange,
                        Location = new Point(20, yPos),
                        Size = new Size(520, 30),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = usuario != null ? Color.LightGreen : Color.LightYellow
                    };
                    pnlPrincipal.Controls.Add(lblUsuarioEstado);

                    this.Controls.Add(pnlPrincipal);

                    // Botón cerrar
                    Button btnCerrar = new Button
                    {
                        Text = "Cerrar",
                        Size = new Size(120, 35),
                        Location = new Point(240, 455),
                        BackColor = Color.FromArgb(51, 122, 183),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        DialogResult = DialogResult.OK
                    };
                    btnCerrar.FlatAppearance.BorderSize = 0;
                    this.Controls.Add(btnCerrar);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void AgregarInfo(Panel panel, string etiqueta, string valor, ref int yPos)
        {
            Label lblEtiqueta = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(40, yPos),
                Size = new Size(120, 25)
            };
            panel.Controls.Add(lblEtiqueta);

            Label lblValor = new Label
            {
                Text = valor,
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, yPos),
                Size = new Size(370, 25)
            };
            panel.Controls.Add(lblValor);

            yPos += 35;
        }
    }
}
