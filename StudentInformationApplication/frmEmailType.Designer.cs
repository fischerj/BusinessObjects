namespace StudentInformationApplication
{
    partial class frmEmailType
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
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvEmailType = new System.Windows.Forms.DataGridView();
            this.btnGet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmailType)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(102, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvEmailType
            // 
            this.dgvEmailType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmailType.Location = new System.Drawing.Point(3, 30);
            this.dgvEmailType.Name = "dgvEmailType";
            this.dgvEmailType.Size = new System.Drawing.Size(509, 231);
            this.dgvEmailType.TabIndex = 6;
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(-9, 1);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(115, 23);
            this.btnGet.TabIndex = 5;
            this.btnGet.Text = "Get Students";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // frmEmailType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 262);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvEmailType);
            this.Controls.Add(this.btnGet);
            this.Name = "frmEmailType";
            this.Text = "EmailType";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmailType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvEmailType;
        private System.Windows.Forms.Button btnGet;
    }
}