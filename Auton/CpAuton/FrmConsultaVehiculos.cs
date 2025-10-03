using CadAuton;
using ClnAuton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpAuton
{
    public partial class FrmConsultaVehiculos : Form
    {
        public FrmConsultaVehiculos()
        {
            InitializeComponent();
        }

        private void FrmConsultaVehiculos_Load(object sender, EventArgs e)
        {
            // Configurar el DataGridView al cargar el formulario
            ConfigurarDataGridView();
            // Cargar todos los vehículos al inicio
            CargarVehiculos("");
        }

        private void ConfigurarDataGridView()
        {
            // Configuración del DataGridView
            dgvVehiculos.AutoGenerateColumns = false;
            dgvVehiculos.AllowUserToAddRows = false;
            dgvVehiculos.AllowUserToDeleteRows = false;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculos.MultiSelect = false;
            dgvVehiculos.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            // Limpiar columnas existentes
            dgvVehiculos.Columns.Clear();

            // Configurar columnas manualmente
            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                DataPropertyName = "id",
                HeaderText = "ID",
                Width = 50,
                Visible = false
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMarca",
                DataPropertyName = "marca",
                HeaderText = "Marca",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelo",
                DataPropertyName = "modelo",
                HeaderText = "Modelo",
                Width = 150
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProcedencia",
                DataPropertyName = "procedencia",
                HeaderText = "Procedencia",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTipoVehiculo",
                DataPropertyName = "tipoVehiculo",
                HeaderText = "Tipo",
                Width = 80
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAnio",
                DataPropertyName = "anioFabricacion",
                HeaderText = "Año",
                Width = 60
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colConfiabilidad",
                DataPropertyName = "confiabilidad",
                HeaderText = "Confiabilidad",
                Width = 100
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPuntuacion",
                DataPropertyName = "puntuacion",
                HeaderText = "Puntuación",
                Width = 80,
                DefaultCellStyle = { Format = "N1" }
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colKilometraje",
                DataPropertyName = "kilometraje",
                HeaderText = "Km",
                Width = 80,
                DefaultCellStyle = { Format = "N0" }
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPropietario",
                DataPropertyName = "propietario",
                HeaderText = "Propietario",
                Width = 120
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMotor",
                DataPropertyName = "motor",
                HeaderText = "Motor",
                Width = 80
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCombustible",
                DataPropertyName = "combustible",
                HeaderText = "Combustible",
                Width = 80
            });
        }

        private void CargarVehiculos(string parametro)
        {
            try
            {
                // Usar la CLN para obtener los datos
                var vehiculos = ConsultaVehiculosCln.listarPa(parametro ?? "");

                // Asignar los datos al DataGridView
                dgvVehiculos.DataSource = vehiculos;

                //Mostrar total de registros
                if (lblTotal != null)
                {
                    lblTotal.Text = $"Total de vehículos: {vehiculos.Count}";
                }

                // Colorear filas según confiabilidad
                ColorearFilasPorConfiabilidad();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los vehículos: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorearFilasPorConfiabilidad()
        {
            foreach (DataGridViewRow row in dgvVehiculos.Rows)
            {
                if (row.Cells["colConfiabilidad"].Value != null)
                {
                    string confiabilidad = row.Cells["colConfiabilidad"].Value.ToString();
                    switch (confiabilidad)
                    {
                        case "Excelente":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        case "Bueno":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                            break;
                        case "Regular":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                            break;
                        case "Malo":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                            break;
                    }
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarVehiculos(txtBuscar.Text.Trim());
        }

        //private void btnLimpiar_Click(object sender, EventArgs e)
        //{
        //    txtBuscar.Clear();
        //    CargarVehiculos("");
        //}

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvVehiculos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Mostrar detalles del vehículo seleccionado
                MostrarDetallesVehiculo();
            }
        }

        private void MostrarDetallesVehiculo()
        {
            if (dgvVehiculos.CurrentRow != null)
            {
                var vehiculo = (paVehiculoListar_Result)dgvVehiculos.CurrentRow.DataBoundItem;

                // Crear un formulario personalizado para mostrar detalles
                Form frmDetalles = new Form
                {
                    Text = "Detalles del Vehículo",
                    Size = new Size(500, 600),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                // Crear un panel con scroll
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(20)
                };

                // Variables para posicionamiento
                int yPos = 20;
                int lineHeight = 25;

                // Función para agregar etiquetas
                Action<string, string, bool> AgregarEtiqueta = (titulo, valor, esObservacion) =>
                {
                    Label lblTitulo = new Label
                    {
                        Text = titulo + ":",
                        Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold),
                        Location = new Point(20, yPos),
                        Size = new Size(120, 20),
                        ForeColor = Color.DarkBlue
                    };

                    if (esObservacion)
                    {
                        // Para observaciones, usar un TextBox multilínea
                        TextBox txtObservacion = new TextBox
                        {
                            Text = string.IsNullOrEmpty(valor) ? "Sin observaciones" : valor,
                            Location = new Point(150, yPos),
                            Size = new Size(300, 80),
                            Multiline = true,
                            ReadOnly = true,
                            ScrollBars = ScrollBars.Vertical,
                            BackColor = Color.WhiteSmoke,
                            Font = new Font("Microsoft Sans Serif", 9)
                        };
                        panel.Controls.Add(lblTitulo);
                        panel.Controls.Add(txtObservacion);
                        yPos += 90;
                    }
                    else
                    {
                        Label lblValor = new Label
                        {
                            Text = string.IsNullOrEmpty(valor) ? "N/A" : valor,
                            Location = new Point(150, yPos),
                            Size = new Size(300, 20),
                            Font = new Font("Microsoft Sans Serif", 9)
                        };
                        panel.Controls.Add(lblTitulo);
                        panel.Controls.Add(lblValor);
                        yPos += lineHeight;
                    }
                };

                // Agregar información del vehículo
                AgregarEtiqueta("Marca", vehiculo.marca, false);
                AgregarEtiqueta("Modelo", vehiculo.modelo, false);
                AgregarEtiqueta("Procedencia", vehiculo.procedencia, false);
                AgregarEtiqueta("Tipo", vehiculo.tipoVehiculo, false);
                AgregarEtiqueta("Año", vehiculo.anioFabricacion.ToString(), false);
                AgregarEtiqueta("Confiabilidad", vehiculo.confiabilidad, false);
                AgregarEtiqueta("Puntuación", vehiculo.puntuacion.ToString("N1"), false);

                // Manejo del kilometraje - convertir directamente a string
                string kmTexto = vehiculo.kilometraje != null ? vehiculo.kilometraje.ToString() + " km" : "N/A";
                AgregarEtiqueta("Kilometraje", kmTexto, false);

                AgregarEtiqueta("Propietario", vehiculo.propietario, false);
                AgregarEtiqueta("Motor", vehiculo.motor, false);
                AgregarEtiqueta("Combustible", vehiculo.combustible, false);

                // Agregar observaciones como campo especial
                AgregarEtiqueta("Observaciones", vehiculo.observaciones, true);

                // Agregar información de registro
                yPos += 10;
                AgregarEtiqueta("Registrado por", vehiculo.usuarioRegistro, false);

                // La fecha no es nullable, se puede usar directamente
                AgregarEtiqueta("Fecha Registro", vehiculo.fechaRegistro.ToString("dd/MM/yyyy HH:mm"), false);

                // Botón Cerrar
                Button btnCerrar = new Button
                {
                    Text = "Cerrar",
                    Size = new Size(100, 30),
                    Location = new Point(350, yPos + 20),
                    DialogResult = DialogResult.OK
                };

                panel.Controls.Add(btnCerrar);
                frmDetalles.Controls.Add(panel);
                frmDetalles.AcceptButton = btnCerrar;

                // Ajustar altura del formulario según el contenido
                frmDetalles.Height = Math.Min(700, yPos + 100);

                frmDetalles.ShowDialog(this);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            // Función para exportar a CSV (beta)
            ExportarACSV();
        }

        private void ExportarACSV()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = $"Vehiculos_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.Text.StringBuilder csv = new System.Text.StringBuilder();

                    // Encabezados
                    csv.AppendLine("Marca,Modelo,Procedencia,Tipo,Año,Confiabilidad,Puntuación,Kilometraje,Propietario,Motor,Combustible");

                    // Datos
                    foreach (DataGridViewRow row in dgvVehiculos.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            csv.AppendLine($"{row.Cells["colMarca"].Value}," +
                                         $"{row.Cells["colModelo"].Value}," +
                                         $"{row.Cells["colProcedencia"].Value}," +
                                         $"{row.Cells["colTipoVehiculo"].Value}," +
                                         $"{row.Cells["colAnio"].Value}," +
                                         $"{row.Cells["colConfiabilidad"].Value}," +
                                         $"{row.Cells["colPuntuacion"].Value}," +
                                         $"{row.Cells["colKilometraje"].Value}," +
                                         $"{row.Cells["colPropietario"].Value}," +
                                         $"{row.Cells["colMotor"].Value}," +
                                         $"{row.Cells["colCombustible"].Value}");
                        }
                    }

                    System.IO.File.WriteAllText(saveFileDialog.FileName, csv.ToString(), System.Text.Encoding.UTF8);
                    MessageBox.Show("Datos exportados exitosamente.", "Exportar",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
