namespace Hacktube
{
    partial class Form1
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
            this.dgvDescargas = new System.Windows.Forms.DataGridView();
            this.CNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAccion = new System.Windows.Forms.DataGridViewButtonColumn();
            this.cEliminar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tbAnnadir = new System.Windows.Forms.TextBox();
            this.btnAnnadir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDescargas)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDescargas
            // 
            this.dgvDescargas.AllowUserToAddRows = false;
            this.dgvDescargas.AllowUserToDeleteRows = false;
            this.dgvDescargas.AllowUserToResizeRows = false;
            this.dgvDescargas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDescargas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDescargas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CNombre,
            this.CEstado,
            this.cAccion,
            this.cEliminar});
            this.dgvDescargas.Location = new System.Drawing.Point(12, 38);
            this.dgvDescargas.Name = "dgvDescargas";
            this.dgvDescargas.Size = new System.Drawing.Size(777, 407);
            this.dgvDescargas.TabIndex = 3;
            this.dgvDescargas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDescargas_CellContentClick);
            // 
            // CNombre
            // 
            this.CNombre.HeaderText = "Nombre";
            this.CNombre.Name = "CNombre";
            this.CNombre.Width = 400;
            // 
            // CEstado
            // 
            this.CEstado.HeaderText = "Estado";
            this.CEstado.Name = "CEstado";
            // 
            // cAccion
            // 
            this.cAccion.HeaderText = "Acción";
            this.cAccion.Name = "cAccion";
            // 
            // cEliminar
            // 
            this.cEliminar.HeaderText = "Eliminar";
            this.cEliminar.Name = "cEliminar";
            // 
            // tbAnnadir
            // 
            this.tbAnnadir.Location = new System.Drawing.Point(12, 12);
            this.tbAnnadir.Name = "tbAnnadir";
            this.tbAnnadir.Size = new System.Drawing.Size(289, 20);
            this.tbAnnadir.TabIndex = 1;
            this.tbAnnadir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbAnnadir_KeyPress);
            // 
            // btnAnnadir
            // 
            this.btnAnnadir.Location = new System.Drawing.Point(307, 9);
            this.btnAnnadir.Name = "btnAnnadir";
            this.btnAnnadir.Size = new System.Drawing.Size(75, 23);
            this.btnAnnadir.TabIndex = 2;
            this.btnAnnadir.Text = "Añadir";
            this.btnAnnadir.UseVisualStyleBackColor = true;
            this.btnAnnadir.Click += new System.EventHandler(this.btnAnnadir_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 458);
            this.Controls.Add(this.tbAnnadir);
            this.Controls.Add(this.btnAnnadir);
            this.Controls.Add(this.dgvDescargas);
            this.Name = "Form1";
            this.Text = "Hacktube";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDescargas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvDescargas;
        private System.Windows.Forms.TextBox tbAnnadir;
        private System.Windows.Forms.Button btnAnnadir;
        private System.Windows.Forms.DataGridViewTextBoxColumn CNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn CEstado;
        private System.Windows.Forms.DataGridViewButtonColumn cAccion;
        private System.Windows.Forms.DataGridViewButtonColumn cEliminar;
    }
}

