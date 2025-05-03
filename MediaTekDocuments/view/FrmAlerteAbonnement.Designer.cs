namespace MediaTekDocuments.view
{
    partial class FrmAlerteAbonnement
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
            this.lbRevueFinAbonnement = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbRevueFinAbonnement
            // 
            this.lbRevueFinAbonnement.FormattingEnabled = true;
            this.lbRevueFinAbonnement.Location = new System.Drawing.Point(12, 38);
            this.lbRevueFinAbonnement.Name = "lbRevueFinAbonnement";
            this.lbRevueFinAbonnement.Size = new System.Drawing.Size(553, 238);
            this.lbRevueFinAbonnement.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(509, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Attention : les abonnements de ces revues arrivent à échéance dans moins de 30 jo" +
    "urs !";
            // 
            // FrmAlerteAbonnement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 284);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbRevueFinAbonnement);
            this.Name = "FrmAlerteAbonnement";
            this.Text = "Alerte Abonnement";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbRevueFinAbonnement;
        private System.Windows.Forms.Label label1;
    }
}