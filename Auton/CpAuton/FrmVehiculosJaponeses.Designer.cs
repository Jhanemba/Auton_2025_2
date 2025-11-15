namespace CpAuton
{
    partial class FrmVehiculosJaponeses
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVehiculosJaponeses));
            this.btnBuscar = new System.Windows.Forms.Button();
            this.erpPrecioVenta = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbxLista = new System.Windows.Forms.GroupBox();
            this.dgvVehiculos = new System.Windows.Forms.DataGridView();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblParametro = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlAcciones = new System.Windows.Forms.Panel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblEstadisticas = new System.Windows.Forms.Label();
            this.lblMarcas = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.erpPrecioVenta)).BeginInit();
            this.gbxLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehiculos)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBuscar
            // 
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(956, 62);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnBuscar.Size = new System.Drawing.Size(98, 40);
            this.btnBuscar.TabIndex = 18;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBuscar.UseVisualStyleBackColor = true;
            // 
            // erpPrecioVenta
            // 
            this.erpPrecioVenta.ContainerControl = this;
            // 
            // gbxLista
            // 
            this.gbxLista.Controls.Add(this.dgvVehiculos);
            this.gbxLista.Location = new System.Drawing.Point(8, 99);
            this.gbxLista.Name = "gbxLista";
            this.gbxLista.Size = new System.Drawing.Size(1249, 335);
            this.gbxLista.TabIndex = 19;
            this.gbxLista.TabStop = false;
            this.gbxLista.Text = "Lista de Vehículos";
            // 
            // dgvVehiculos
            // 
            this.dgvVehiculos.AllowUserToAddRows = false;
            this.dgvVehiculos.AllowUserToDeleteRows = false;
            this.dgvVehiculos.AllowUserToResizeRows = false;
            this.dgvVehiculos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvVehiculos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehiculos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVehiculos.Location = new System.Drawing.Point(3, 26);
            this.dgvVehiculos.Name = "dgvVehiculos";
            this.dgvVehiculos.ReadOnly = true;
            this.dgvVehiculos.RowHeadersWidth = 62;
            this.dgvVehiculos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVehiculos.Size = new System.Drawing.Size(1243, 306);
            this.dgvVehiculos.TabIndex = 0;
            this.dgvVehiculos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVehiculos_CellDoubleClick);
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(259, 71);
            this.txtBuscar.MaxLength = 100;
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(680, 30);
            this.txtBuscar.TabIndex = 17;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // lblParametro
            // 
            this.lblParametro.AutoSize = true;
            this.lblParametro.Location = new System.Drawing.Point(259, 52);
            this.lblParametro.Name = "lblParametro";
            this.lblParametro.Size = new System.Drawing.Size(583, 25);
            this.lblParametro.TabIndex = 16;
            this.lblParametro.Text = "Buscar por Marca, Modelo, País, Tansmisión, Motor, Confiabilidad\r\n";
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(254, 21);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(800, 31);
            this.lblTitulo.TabIndex = 15;
            this.lblTitulo.Text = "Vehículos";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.Controls.Add(this.btnNuevo);
            this.pnlAcciones.Controls.Add(this.btnExportar);
            this.pnlAcciones.Controls.Add(this.btnCerrar);
            this.pnlAcciones.Location = new System.Drawing.Point(138, 490);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(795, 47);
            this.pnlAcciones.TabIndex = 20;
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = global::CpAuton.Properties.Resources.add_77928;
            this.btnNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNuevo.Location = new System.Drawing.Point(164, 3);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnNuevo.Size = new System.Drawing.Size(111, 40);
            this.btnNuevo.TabIndex = 12;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Image = ((System.Drawing.Image)(resources.GetObject("btnExportar.Image")));
            this.btnExportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportar.Location = new System.Drawing.Point(389, 3);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnExportar.Size = new System.Drawing.Size(111, 40);
            this.btnExportar.TabIndex = 11;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrar.Location = new System.Drawing.Point(620, 4);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnCerrar.Size = new System.Drawing.Size(116, 40);
            this.btnCerrar.TabIndex = 9;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(982, 450);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(180, 25);
            this.lblTotal.TabIndex = 21;
            this.lblTotal.Text = "Total de Vehículos:";
            // 
            // lblEstadisticas
            // 
            this.lblEstadisticas.AutoSize = true;
            this.lblEstadisticas.Location = new System.Drawing.Point(384, 450);
            this.lblEstadisticas.Name = "lblEstadisticas";
            this.lblEstadisticas.Size = new System.Drawing.Size(122, 25);
            this.lblEstadisticas.TabIndex = 22;
            this.lblEstadisticas.Text = "Estadisticas:";
            // 
            // lblMarcas
            // 
            this.lblMarcas.AutoSize = true;
            this.lblMarcas.Location = new System.Drawing.Point(12, 450);
            this.lblMarcas.Name = "lblMarcas";
            this.lblMarcas.Size = new System.Drawing.Size(83, 25);
            this.lblMarcas.TabIndex = 23;
            this.lblMarcas.Text = "Marcas:";
            // 
            // FrmVehiculosJaponeses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(1264, 554);
            this.Controls.Add(this.lblMarcas);
            this.Controls.Add(this.lblEstadisticas);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.gbxLista);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.lblParametro);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.lblTotal);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmVehiculosJaponeses";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmVehiculosJaponeses";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmVehiculosChinos_FormClosed);
            this.Load += new System.EventHandler(this.FrmVehiculosJaponeses_Load);
            ((System.ComponentModel.ISupportInitialize)(this.erpPrecioVenta)).EndInit();
            this.gbxLista.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehiculos)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ErrorProvider erpPrecioVenta;
        private System.Windows.Forms.GroupBox gbxLista;
        private System.Windows.Forms.DataGridView dgvVehiculos;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label lblParametro;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblEstadisticas;
        private System.Windows.Forms.Label lblMarcas;
    }
}