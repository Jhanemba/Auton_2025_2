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
    public partial class FrmEstadisticas : Form
    {
        private ConsultaVehiculosCln.EstadisticasGenerales stats;

        public FrmEstadisticas()
        {
            InitializeComponent();
        }

        private void FrmEstadisticas_Load(object sender, EventArgs e)
        {
            CargarEstadisticas();
        }

        private void CargarEstadisticas()
        {
            try
            {
                stats = ConsultaVehiculosCln.obtenerEstadisticasGenerales();
                MostrarEstadisticas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar estadísticas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarEstadisticas()
        {
            this.Text = "📊 Estadísticas del Sistema Auton";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Panel principal con scroll
            Panel pnlPrincipal = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1184, 620),
                AutoScroll = true,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            int yPos = 20;
            int margin = 20;
            int cardWidth = 360;
            int cardHeight = 180;

            // ===== TÍTULO PRINCIPAL =====
            Label lblTitulo = new Label
            {
                Text = "📊 ESTADÍSTICAS GENERALES DEL SISTEMA",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                Location = new Point(margin, yPos),
                Size = new Size(1100, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTitulo);
            yPos += 60;

            // ===== CARD PRINCIPAL: RESUMEN GENERAL =====
            Panel cardResumen = CrearCard("📈 RESUMEN GENERAL", margin, yPos, 1120, 120, Color.FromArgb(51, 122, 183));

            Label lblTotalVehiculos = new Label
            {
                Text = $"🚗 TOTAL DE VEHÍCULOS: {stats.TotalVehiculos}",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 50),
                Size = new Size(400, 40)
            };
            cardResumen.Controls.Add(lblTotalVehiculos);

            Label lblPromPuntuacion = new Label
            {
                Text = $"⭐ Promedio Puntuación: {stats.PromedioPuntuacion:N2}/10",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(450, 55),
                Size = new Size(300, 30)
            };
            cardResumen.Controls.Add(lblPromPuntuacion);

            Label lblPromAirbags = new Label
            {
                Text = $"🛡️ Promedio Airbags: {stats.PromedioAirbags:N1}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(770, 55),
                Size = new Size(300, 30)
            };
            cardResumen.Controls.Add(lblPromAirbags);

            pnlPrincipal.Controls.Add(cardResumen);
            yPos += 140;

            // ===== FILA 1: CONFIABILIDAD =====
            int xPos = margin;

            // Card Excelente
            Panel cardExcelente = CrearCard("⭐ EXCELENTE", xPos, yPos, cardWidth, cardHeight, Color.FromArgb(40, 167, 69));
            AgregarEstadisticaCard(cardExcelente, stats.Excelentes, stats.PorcentajeExcelentes);
            pnlPrincipal.Controls.Add(cardExcelente);
            xPos += cardWidth + margin;

            // Card Bueno
            Panel cardBueno = CrearCard("👍 BUENO", xPos, yPos, cardWidth, cardHeight, Color.FromArgb(23, 162, 184));
            AgregarEstadisticaCard(cardBueno, stats.Buenos, stats.PorcentajeBuenos);
            pnlPrincipal.Controls.Add(cardBueno);
            xPos += cardWidth + margin;

            // Card Regular
            Panel cardRegular = CrearCard("⚠️ REGULAR", xPos, yPos, cardWidth, cardHeight, Color.FromArgb(255, 193, 7));
            AgregarEstadisticaCard(cardRegular, stats.Regulares, stats.PorcentajeRegulares);
            pnlPrincipal.Controls.Add(cardRegular);

            yPos += cardHeight + margin;

            // Card Malo (centrado)
            xPos = margin + (1120 - cardWidth) / 2;
            Panel cardMalo = CrearCard("❌ MALO", xPos, yPos, cardWidth, cardHeight, Color.FromArgb(220, 53, 69));
            AgregarEstadisticaCard(cardMalo, stats.Malos, stats.PorcentajeMalos);
            pnlPrincipal.Controls.Add(cardMalo);
            yPos += cardHeight + margin;

            // ===== FILA 2: POR PROCEDENCIA =====
            Label lblProcedencia = new Label
            {
                Text = "🌍 DISTRIBUCIÓN POR PROCEDENCIA",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(margin, yPos),
                Size = new Size(1100, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblProcedencia);
            yPos += 45;

            xPos = margin;
            int smallCardWidth = 180;
            int smallCardHeight = 100;

            // Japonés
            Panel cardJapones = CrearCardPequeno("🇯🇵 Japonés", stats.Japoneses, xPos, yPos, smallCardWidth, smallCardHeight, Color.FromArgb(220, 53, 69));
            pnlPrincipal.Controls.Add(cardJapones);
            xPos += smallCardWidth + margin;

            // Americano
            Panel cardAmericano = CrearCardPequeno("🇺🇸 Americano", stats.Americanos, xPos, yPos, smallCardWidth, smallCardHeight, Color.FromArgb(0, 123, 255));
            pnlPrincipal.Controls.Add(cardAmericano);
            xPos += smallCardWidth + margin;

            // Chino
            Panel cardChino = CrearCardPequeno("🇨🇳 Chino", stats.Chinos, xPos, yPos, smallCardWidth, smallCardHeight, Color.FromArgb(255, 193, 7));
            pnlPrincipal.Controls.Add(cardChino);
            xPos += smallCardWidth + margin;

            // Alemán
            Panel cardAleman = CrearCardPequeno("🇩🇪 Alemán", stats.Alemanes, xPos, yPos, smallCardWidth, smallCardHeight, Color.FromArgb(52, 58, 64));
            pnlPrincipal.Controls.Add(cardAleman);
            xPos += smallCardWidth + margin;

            // Europeo
            Panel cardEuropeo = CrearCardPequeno("🇪🇺 Europeo", stats.Europeos, xPos, yPos, smallCardWidth, smallCardHeight, Color.FromArgb(0, 51, 153));
            pnlPrincipal.Controls.Add(cardEuropeo);
            xPos += smallCardWidth + margin;

            // Coreano
            Panel cardCoreano = CrearCardPequeno("🇰🇷 Coreano", stats.Coreanos, xPos, yPos, smallCardWidth, smallCardHeight, Color.FromArgb(23, 162, 184));
            pnlPrincipal.Controls.Add(cardCoreano);

            yPos += smallCardHeight + margin;

            // ===== TOP MARCAS =====
            Label lblTopMarcas = new Label
            {
                Text = "🏆 TOP 5 MARCAS MÁS REGISTRADAS",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(margin, yPos),
                Size = new Size(1100, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblTopMarcas);
            yPos += 45;

            Panel panelTopMarcas = new Panel
            {
                Location = new Point(margin, yPos),
                Size = new Size(1120, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            int topY = 15;
            int posicion = 1;
            foreach (var marca in stats.TopMarcas.OrderByDescending(m => m.Value))
            {
                string emoji = posicion == 1 ? "🥇" : posicion == 2 ? "🥈" : posicion == 3 ? "🥉" : "🏅";

                Label lblMarca = new Label
                {
                    Text = $"{emoji} {posicion}. {marca.Key}",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Location = new Point(30, topY),
                    Size = new Size(400, 25)
                };
                panelTopMarcas.Controls.Add(lblMarca);

                Label lblCantidad = new Label
                {
                    Text = $"{marca.Value} vehículos",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(108, 117, 125),
                    Location = new Point(450, topY),
                    Size = new Size(200, 25)
                };
                panelTopMarcas.Controls.Add(lblCantidad);

                // Barra de progreso visual
                int barraWidth = (int)(marca.Value * 400.0 / stats.TopMarcas.Values.Max());
                Panel barra = new Panel
                {
                    Location = new Point(670, topY + 5),
                    Size = new Size(barraWidth, 15),
                    BackColor = Color.FromArgb(51, 122, 183)
                };
                panelTopMarcas.Controls.Add(barra);

                topY += 28;
                posicion++;
            }

            pnlPrincipal.Controls.Add(panelTopMarcas);
            yPos += 170;

            // ===== OTROS DATOS =====
            Label lblOtros = new Label
            {
                Text = "🔧 OTROS DATOS",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                Location = new Point(margin, yPos),
                Size = new Size(1100, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrincipal.Controls.Add(lblOtros);
            yPos += 45;

            Panel cardTurbo = CrearCard("🚀 TURBO", margin, yPos, 540, 100, Color.FromArgb(220, 53, 69));
            Label lblTurbo = new Label
            {
                Text = $"Con Turbo: {stats.ConTurbo} | Sin Turbo: {stats.SinTurbo}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 50),
                Size = new Size(500, 30)
            };
            cardTurbo.Controls.Add(lblTurbo);
            pnlPrincipal.Controls.Add(cardTurbo);

            Panel cardRango = CrearCard("📅 RANGO DE AÑOS", margin + 560, yPos, 540, 100, Color.FromArgb(108, 117, 125));
            Label lblRango = new Label
            {
                Text = $"{stats.AnioMasAntiguo} - {stats.AnioMasReciente}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 50),
                Size = new Size(500, 30)
            };
            cardRango.Controls.Add(lblRango);
            pnlPrincipal.Controls.Add(cardRango);

            // Añadimos espacio en blanco debajo de TURBO y RANGO
            yPos += 125 + margin;
            Panel spacerTurboRango = new Panel
            {
                Location = new Point(margin, yPos),
                Size = new Size(1120, 20),
                BackColor = pnlPrincipal.BackColor,
                BorderStyle = BorderStyle.None
            };
            pnlPrincipal.Controls.Add(spacerTurboRango);
            yPos += 20;

            

            // ===== BOTÓN CERRAR =====
            Button btnCerrar = new Button
            {
                Text = "✅ Cerrar",
                Size = new Size(200, 50),
                Location = new Point(490, 550),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();

            this.Controls.Add(pnlPrincipal);
            this.Controls.Add(btnCerrar);
            btnCerrar.BringToFront(); // Esto lo pone sobre el panel
        }

        private Panel CrearCard(string titulo, int x, int y, int width, int height, Color colorFondo)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = colorFondo,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(width - 20, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblTitulo);

            return card;
        }

        private void AgregarEstadisticaCard(Panel card, int cantidad, double porcentaje)
        {
            Label lblCantidad = new Label
            {
                Text = cantidad.ToString(),
                Font = new Font("Segoe UI", 40, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 60),
                Size = new Size(card.Width - 20, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblCantidad);

            Label lblPorcentaje = new Label
            {
                Text = $"{porcentaje:N1}%",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 130),
                Size = new Size(card.Width - 20, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblPorcentaje);
        }

        private Panel CrearCardPequeno(string titulo, int cantidad, int x, int y, int width, int height, Color colorFondo)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = colorFondo,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(5, 10),
                Size = new Size(width - 10, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblTitulo);

            Label lblCantidad = new Label
            {
                Text = cantidad.ToString(),
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(5, 40),
                Size = new Size(width - 10, 45),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblCantidad);

            return card;
        }
    }
}
