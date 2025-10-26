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
    public partial class FrmBusquedaAvanzada : Form
    {
        public List<dynamic> ResultadosBusqueda { get; private set; }

        public FrmBusquedaAvanzada()
        {
            InitializeComponent();
        }

        private void FrmBusquedaAvanzada_Load(object sender, EventArgs e)
        {
            ConfigurarFormulario();
            CargarFiltros();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "🔍 Búsqueda Avanzada de Vehículos";
            //this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 10F);

            // Panel principal
            Panel pnlPrincipal = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(840, 550),
                BorderStyle = BorderStyle.FixedSingle,
                //BackColor = Color.White
            };

            // Título
            Label lblTitulo = new Label
            {
                Text = "🔍 BÚSQUEDA AVANZADA",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(20, 15),
                Size = new Size(800, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);

            int yPos = 60;
            int labelWidth = 150;
            int controlWidth = 200;
            int leftCol = 30;
            int rightCol = 450;

            // ===== COLUMNA IZQUIERDA =====

            // Procedencia
            Label lblProcedencia = new Label
            {
                Text = "Procedencia:",
                Location = new Point(leftCol, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            pnlPrincipal.Controls.Add(lblProcedencia);

            ComboBox cboProcedencia = new ComboBox
            {
                Name = "cboProcedencia",
                Location = new Point(leftCol + 160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboProcedencia.Items.AddRange(new[] {
                "Todos", "Japonés", "Americano", "Chino", "Alemán", "Europeo", "Coreano"
            });
            cboProcedencia.SelectedIndex = 0;
            pnlPrincipal.Controls.Add(cboProcedencia);
            yPos += 45;

            // Confiabilidad
            Label lblConfiabilidad = new Label
            {
                Text = "Confiabilidad:",
                Location = new Point(leftCol, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            pnlPrincipal.Controls.Add(lblConfiabilidad);

            ComboBox cboConfiabilidad = new ComboBox
            {
                Name = "cboConfiabilidad",
                Location = new Point(leftCol + 160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboConfiabilidad.Items.AddRange(new[] {
                "Todos", "Excelente", "Bueno", "Regular", "Malo"
            });
            cboConfiabilidad.SelectedIndex = 0;
            pnlPrincipal.Controls.Add(cboConfiabilidad);
            yPos += 45;

            // Transmisión
            Label lblTransmision = new Label
            {
                Text = "Transmisión:",
                Location = new Point(leftCol, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            pnlPrincipal.Controls.Add(lblTransmision);

            ComboBox cboTransmision = new ComboBox
            {
                Name = "cboTransmision",
                Location = new Point(leftCol + 160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboTransmision.Items.AddRange(new[] {
                "Todos", "Manual", "Automática", "CVT", "Semiautomática", "Doble Embrague"
            });
            cboTransmision.SelectedIndex = 0;
            pnlPrincipal.Controls.Add(cboTransmision);
            yPos += 45;

            // Combustible
            Label lblCombustible = new Label
            {
                Text = "Combustible:",
                Location = new Point(leftCol, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            pnlPrincipal.Controls.Add(lblCombustible);

            ComboBox cboCombustible = new ComboBox
            {
                Name = "cboCombustible",
                Location = new Point(leftCol + 160, yPos),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboCombustible.Items.AddRange(new[] {
                "Todos", "Gasolina", "Diesel", "Híbrido", "Eléctrico", "GLP", "GNC"
            });
            cboCombustible.SelectedIndex = 0;
            pnlPrincipal.Controls.Add(cboCombustible);

            // ===== COLUMNA DERECHA =====
            yPos = 60;

            // Año desde
            Label lblAnioDesde = new Label
            {
                Text = "Año desde:",
                Location = new Point(rightCol, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            pnlPrincipal.Controls.Add(lblAnioDesde);

            NumericUpDown nudAnioDesde = new NumericUpDown
            {
                Name = "nudAnioDesde",
                Location = new Point(rightCol + 160, yPos),
                Size = new Size(120, 25),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = 2000,
                Font = new Font("Segoe UI", 10)
            };
            pnlPrincipal.Controls.Add(nudAnioDesde);
            yPos += 45;

            // Año hasta
            Label lblAnioHasta = new Label
            {
                Text = "Año hasta:",
                Location = new Point(rightCol, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            pnlPrincipal.Controls.Add(lblAnioHasta);

            NumericUpDown nudAnioHasta = new NumericUpDown
            {
                Name = "nudAnioHasta",
                Location = new Point(rightCol + 160, yPos),
                Size = new Size(120, 25),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year,
                Font = new Font("Segoe UI", 10)
            };
            pnlPrincipal.Controls.Add(nudAnioHasta);
            yPos += 45;

            // Puntuación mínima
            Label lblPuntuacion = new Label
            {
                Text = "Puntuación mínima:",
                Location = new Point(rightCol, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            pnlPrincipal.Controls.Add(lblPuntuacion);

            NumericUpDown nudPuntuacion = new NumericUpDown
            {
                Name = "nudPuntuacion",
                Location = new Point(rightCol + 160, yPos),
                Size = new Size(120, 25),
                Minimum = 1.0m,
                Maximum = 10.0m,
                DecimalPlaces = 1,
                Increment = 0.5m,
                Value = 1.0m,
                Font = new Font("Segoe UI", 10)
            };
            pnlPrincipal.Controls.Add(nudPuntuacion);
            yPos += 45;

            // Tiene Turbo
            CheckBox chkTurbo = new CheckBox
            {
                Name = "chkTurbo",
                Text = "Solo con Turbo",
                Location = new Point(rightCol + 160, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69)
            };
            pnlPrincipal.Controls.Add(chkTurbo);

            // ===== RESULTADOS =====
            yPos = 260;
            Label lblResultados = new Label
            {
                Name = "lblResultados",
                Text = "Resultados: 0 vehículos encontrados",
                Location = new Point(leftCol, yPos),
                Size = new Size(780, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183)
            };
            pnlPrincipal.Controls.Add(lblResultados);
            yPos += 30;

            // DataGridView de resultados
            DataGridView dgvResultados = new DataGridView
            {
                Name = "dgvResultados",
                Location = new Point(leftCol, yPos),
                Size = new Size(780, 180),
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9)
            };

            dgvResultados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMarca",
                DataPropertyName = "marca",
                HeaderText = "Marca",
                Width = 100
            });

            dgvResultados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelo",
                DataPropertyName = "modelo",
                HeaderText = "Modelo",
                Width = 120
            });

            dgvResultados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAnio",
                DataPropertyName = "anioFabricacion",
                HeaderText = "Año",
                Width = 60
            });

            dgvResultados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProcedencia",
                DataPropertyName = "procedencia",
                HeaderText = "Procedencia",
                Width = 100
            });

            dgvResultados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colConfiabilidad",
                DataPropertyName = "confiabilidad",
                HeaderText = "Confiabilidad",
                Width = 100
            });

            dgvResultados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPuntuacion",
                DataPropertyName = "puntuacion",
                HeaderText = "Puntuación",
                Width = 80,
                DefaultCellStyle = { Format = "N1" }
            });

            dgvResultados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTransmision",
                DataPropertyName = "transmision",
                HeaderText = "Transmisión",
                Width = 100
            });

            dgvResultados.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colTurbo",
                DataPropertyName = "tieneTurbo",
                HeaderText = "Turbo",
                Width = 60
            });

            pnlPrincipal.Controls.Add(dgvResultados);

            // ===== BOTONES =====
            yPos = 475;

            Button btnBuscar = new Button
            {
                Text = "🔍 Buscar",
                Location = new Point(250, yPos),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += (s, e) => RealizarBusqueda(
                cboProcedencia, cboConfiabilidad, cboTransmision, cboCombustible,
                nudAnioDesde, nudAnioHasta, nudPuntuacion, chkTurbo,
                dgvResultados, lblResultados
            );
            pnlPrincipal.Controls.Add(btnBuscar);

            Button btnLimpiar = new Button
            {
                Text = "🗑️ Limpiar",
                Location = new Point(420, yPos),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Click += (s, e) => LimpiarFiltros(
                cboProcedencia, cboConfiabilidad, cboTransmision, cboCombustible,
                nudAnioDesde, nudAnioHasta, nudPuntuacion, chkTurbo,
                dgvResultados, lblResultados
            );
            pnlPrincipal.Controls.Add(btnLimpiar);

            this.Controls.Add(pnlPrincipal);
        }

        private void CargarFiltros()
        {
            // Método para cargar datos adicionales si es necesario
        }

        private void RealizarBusqueda(
            ComboBox cboProcedencia, ComboBox cboConfiabilidad, ComboBox cboTransmision,
            ComboBox cboCombustible, NumericUpDown nudAnioDesde, NumericUpDown nudAnioHasta,
            NumericUpDown nudPuntuacion, CheckBox chkTurbo,
            DataGridView dgvResultados, Label lblResultados)
        {
            try
            {
                // Obtener vehículos según procedencia
                List<dynamic> resultados;
                string procedencia = cboProcedencia.Text;

                // Siempre usar el método unificado
                resultados = ConsultaVehiculosCln.listarPorProcedencia(procedencia, "");

                // Aplicar filtros adicionales
                if (cboConfiabilidad.Text != "Todos")
                    resultados = resultados.Where(v => v.confiabilidad == cboConfiabilidad.Text).ToList();

                if (cboTransmision.Text != "Todos")
                    resultados = resultados.Where(v => v.transmision == cboTransmision.Text).ToList();

                if (cboCombustible.Text != "Todos")
                    resultados = resultados.Where(v => v.combustible == cboCombustible.Text).ToList();

                resultados = resultados.Where(v =>
                    v.anioFabricacion >= nudAnioDesde.Value &&
                    v.anioFabricacion <= nudAnioHasta.Value
                ).ToList();

                resultados = resultados.Where(v => v.puntuacion >= nudPuntuacion.Value).ToList();

                if (chkTurbo.Checked)
                    resultados = resultados.Where(v => v.tieneTurbo).ToList();

                // Mostrar resultados
                dgvResultados.DataSource = resultados;
                lblResultados.Text = $"Resultados: {resultados.Count} vehículos encontrados";

                ResultadosBusqueda = resultados;

                ColorearFilas(dgvResultados);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        case "Excelente":
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            break;
                        case "Bueno":
                            row.DefaultCellStyle.BackColor = Color.LightBlue;
                            break;
                        case "Regular":
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            break;
                        case "Malo":
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                            break;
                    }
                }
            }
        }

        private void LimpiarFiltros(
            ComboBox cboProcedencia, ComboBox cboConfiabilidad, ComboBox cboTransmision,
            ComboBox cboCombustible, NumericUpDown nudAnioDesde, NumericUpDown nudAnioHasta,
            NumericUpDown nudPuntuacion, CheckBox chkTurbo,
            DataGridView dgvResultados, Label lblResultados)
        {
            cboProcedencia.SelectedIndex = 0;
            cboConfiabilidad.SelectedIndex = 0;
            cboTransmision.SelectedIndex = 0;
            cboCombustible.SelectedIndex = 0;
            nudAnioDesde.Value = 2000;
            nudAnioHasta.Value = DateTime.Now.Year;
            nudPuntuacion.Value = 1.0m;
            chkTurbo.Checked = false;
            dgvResultados.DataSource = null;
            lblResultados.Text = "Resultados: 0 vehículos encontrados";
            ResultadosBusqueda = null;
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    this.ClientSize = new System.Drawing.Size(900, 650);
        //    this.Name = "FrmBusquedaAvanzada";
        //    this.Load += new System.EventHandler(this.FrmBusquedaAvanzada_Load);
        //    this.ResumeLayout(false);
        //}
    }
}
