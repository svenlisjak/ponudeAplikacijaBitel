namespace ponudeAplikacijaBitel
{
    partial class editOpis
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
            this.opisEdit = new System.Windows.Forms.TextBox();
            this.Edit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // opisEdit
            // 
            this.opisEdit.Location = new System.Drawing.Point(47, 37);
            this.opisEdit.MaximumSize = new System.Drawing.Size(450, 250);
            this.opisEdit.MinimumSize = new System.Drawing.Size(450, 250);
            this.opisEdit.Name = "opisEdit";
            this.opisEdit.Size = new System.Drawing.Size(450, 20);
            this.opisEdit.TabIndex = 0;
            // 
            // Edit
            // 
            this.Edit.Location = new System.Drawing.Point(394, 328);
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(75, 23);
            this.Edit.TabIndex = 1;
            this.Edit.Text = "edit";
            this.Edit.UseVisualStyleBackColor = true;
            this.Edit.Click += new System.EventHandler(this.Edit_Click);
            // 
            // editOpis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 388);
            this.Controls.Add(this.Edit);
            this.Controls.Add(this.opisEdit);
            this.Name = "editOpis";
            this.Text = "editOpis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox opisEdit;
        private System.Windows.Forms.Button Edit;
    }
}