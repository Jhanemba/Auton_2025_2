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
    public partial class FrmRegistroVehiculo : Form
    {
        private List<Procedencia> procedencias;
        private List<Marca> marcas;
        private List<ModeloVehiculo> modelos;
        private List<TipoVehiculo> tipos;

        public FrmRegistroVehiculo()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Registro de Nuevo Vehículo";
            this.Size = new Size(600, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Panel pnlPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            int yPos = 10;
            int lineHeight = 30;
            int lblWidth = 120;
            int ctrlWidth = 300;

            // Título
            Label lblTitulo = new Label
            {
                Text = "REGISTRO DE NUEVO VEHÍCULO",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(20, yPos),
                Size = new Size(500, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 40;

            // Procedencia
            Label lblProcedencia = new Label
            {
                Text = "Procedencia:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            cboProcedencia = new ComboBox
            {
                Name = "cboProcedencia",
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboProcedencia.SelectedIndexChanged += CboProcedencia_SelectedIndexChanged;
            pnlPrincipal.Controls.Add(lblProcedencia);
            pnlPrincipal.Controls.Add(cboProcedencia);
            yPos += lineHeight;

            // Marca con botón Agregar
            Label lblMarca = new Label
            {
                Text = "Marca:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            cboMarca = new ComboBox
            {
                Name = "cboMarca",
                Location = new Point(150, yPos),
                Size = new Size(250, 23),
                DropDownStyle = ComboBoxStyle.DropDown,
                Enabled = false
            };
            cboMarca.SelectedIndexChanged += CboMarca_SelectedIndexChanged;

            btnAgregarMarca = new Button
            {
                Text = "+",
                Location = new Point(410, yPos),
                Size = new Size(40, 23),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Enabled = false
            };
            btnAgregarMarca.Click += BtnAgregarMarca_Click;

            pnlPrincipal.Controls.Add(lblMarca);
            pnlPrincipal.Controls.Add(cboMarca);
            pnlPrincipal.Controls.Add(btnAgregarMarca);
            yPos += lineHeight;

            // Tipo Vehículo
            Label lblTipo = new Label
            {
                Text = "Tipo Vehículo:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            cboTipo = new ComboBox
            {
                Name = "cboTipo",
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            pnlPrincipal.Controls.Add(lblTipo);
            pnlPrincipal.Controls.Add(cboTipo);
            yPos += lineHeight;

            // Modelo con botón Agregar
            Label lblModelo = new Label
            {
                Text = "Modelo:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            cboModelo = new ComboBox
            {
                Name = "cboModelo",
                Location = new Point(150, yPos),
                Size = new Size(250, 23),
                DropDownStyle = ComboBoxStyle.DropDown,
                Enabled = false
            };

            btnAgregarModelo = new Button
            {
                Text = "+",
                Location = new Point(410, yPos),
                Size = new Size(40, 23),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Enabled = false
            };
            btnAgregarModelo.Click += BtnAgregarModelo_Click;

            pnlPrincipal.Controls.Add(lblModelo);
            pnlPrincipal.Controls.Add(cboModelo);
            pnlPrincipal.Controls.Add(btnAgregarModelo);
            yPos += lineHeight;

            // Año de Fabricación
            Label lblAnio = new Label
            {
                Text = "Año Fabricación:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtAnio = new TextBox
            {
                Name = "txtAnio",
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23)
            };
            pnlPrincipal.Controls.Add(lblAnio);
            pnlPrincipal.Controls.Add(txtAnio);
            yPos += lineHeight;

            // Confiabilidad
            Label lblConfiabilidad = new Label
            {
                Text = "Confiabilidad:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            cboConfiabilidad = new ComboBox
            {
                Name = "cboConfiabilidad",
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboConfiabilidad.Items.AddRange(new[] { "Excelente", "Bueno", "Regular", "Malo" });
            pnlPrincipal.Controls.Add(lblConfiabilidad);
            pnlPrincipal.Controls.Add(cboConfiabilidad);
            yPos += lineHeight;

            // Puntuación
            Label lblPuntuacion = new Label
            {
                Text = "Puntuación:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtPuntuacion = new TextBox
            {
                Name = "txtPuntuacion",
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23)
            };
            pnlPrincipal.Controls.Add(lblPuntuacion);
            pnlPrincipal.Controls.Add(txtPuntuacion);
            yPos += lineHeight;

            // Kilometraje
            Label lblKilometraje = new Label
            {
                Text = "Kilometraje:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtKilometraje = new TextBox
            {
                Name = "txtKilometraje",
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23)
            };
            pnlPrincipal.Controls.Add(lblKilometraje);
            pnlPrincipal.Controls.Add(txtKilometraje);
            yPos += lineHeight;

            // Propietario
            Label lblPropietario = new Label
            {
                Text = "Propietario:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtPropietario = new TextBox
            {
                Name = "txtPropietario",
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 23)
            };
            pnlPrincipal.Controls.Add(lblPropietario);
            pnlPrincipal.Controls.Add(txtPropietario);
            yPos += lineHeight;

            // Observaciones
            Label lblObservaciones = new Label
            {
                Text = "Observaciones:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(lblWidth, 20)
            };
            txtObservaciones = new TextBox
            {
                Name = "txtObservaciones",
                Location = new Point(150, yPos),
                Size = new Size(ctrlWidth, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            pnlPrincipal.Controls.Add(lblObservaciones);
            pnlPrincipal.Controls.Add(txtObservaciones);
            yPos += 110;

            // Botones
            Button btnGuardar = new Button
            {
                Text = "Guardar",
                Size = new Size(100, 35),
                Location = new Point(150, yPos),
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
                Location = new Point(260, yPos),
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
                    modelos = ModeloVehiculoCln.listar().Where(m => m.idMarca == idMarca).ToList();
                    cboModelo.DataSource = modelos;
                    cboModelo.DisplayMember = "nombre";
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
                using (var frmNuevoModelo = new FrmNuevoModelo(idMarca, idTipo))
                {
                    if (frmNuevoModelo.ShowDialog(this) == DialogResult.OK)
                    {
                        CboMarca_SelectedIndexChanged(null, null);
                        cboModelo.SelectedValue = frmNuevoModelo.NuevoIdModelo;
                    }
                }
            }
        }

        private bool Validar()
        {
            if (cboProcedencia.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una procedencia", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboProcedencia.Focus();
                return false;
            }

            if (cboMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una marca", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMarca.Focus();
                return false;
            }

            if (cboTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un tipo de vehículo", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTipo.Focus();
                return false;
            }

            if (cboModelo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un modelo", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboModelo.Focus();
                return false;
            }

            if (!int.TryParse(txtAnio.Text, out int anio) || anio < 1900 || anio > DateTime.Now.Year)
            {
                MessageBox.Show($"El año debe estar entre 1900 y {DateTime.Now.Year}", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnio.Focus();
                return false;
            }

            if (cboConfiabilidad.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar el nivel de confiabilidad", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboConfiabilidad.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPuntuacion.Text, out decimal puntuacion) || puntuacion < 1.0m || puntuacion > 10.0m)
            {
                MessageBox.Show("La puntuación debe estar entre 1.0 y 10.0", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPuntuacion.Focus();
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
                    int anio = int.Parse(txtAnio.Text);
                    decimal puntuacion = decimal.Parse(txtPuntuacion.Text);
                    int kilometraje = string.IsNullOrEmpty(txtKilometraje.Text) ? 0 : int.Parse(txtKilometraje.Text);

                    var nuevoVehiculo = new Vehiculo
                    {
                        idModeloVehiculo = idModelo,
                        anioFabricacion = anio,
                        confiabilidad = cboConfiabilidad.SelectedItem.ToString(),
                        puntuacion = puntuacion,
                        kilometraje = kilometraje,
                        propietario = string.IsNullOrEmpty(txtPropietario.Text) ? null : txtPropietario.Text,
                        observaciones = string.IsNullOrEmpty(txtObservaciones.Text) ? null : txtObservaciones.Text,
                        estado = 1
                    };

                    int resultado = VehiculoCln.insertar(nuevoVehiculo);

                    if (resultado > 0)
                    {
                        MessageBox.Show($"Vehículo registrado exitosamente con ID: {resultado}",
                                      "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el vehículo: {ex.Message}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private ComboBox cboProcedencia;
        private ComboBox cboMarca;
        private ComboBox cboTipo;
        private ComboBox cboModelo;
        private TextBox txtAnio;
        private ComboBox cboConfiabilidad;
        private TextBox txtPuntuacion;
        private TextBox txtKilometraje;
        private TextBox txtPropietario;
        private TextBox txtObservaciones;
        private Button btnAgregarMarca;
        private Button btnAgregarModelo;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "FrmRegistroVehiculo";
            this.Load += new System.EventHandler(this.FrmRegistroVehiculo_Load);
            this.ResumeLayout(false);
        }
    }
}
