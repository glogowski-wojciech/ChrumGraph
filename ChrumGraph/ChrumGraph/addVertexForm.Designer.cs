namespace ChrumGraph
{
    partial class AddVertexForm
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
            this.OKbutton = new System.Windows.Forms.Button();
            this.labelMain = new System.Windows.Forms.Label();
            this.labelInput = new System.Windows.Forms.TextBox();
            this.labelError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKbutton
            // 
            this.OKbutton.Location = new System.Drawing.Point(85, 79);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 23);
            this.OKbutton.TabIndex = 1;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // labelMain
            // 
            this.labelMain.AutoSize = true;
            this.labelMain.Location = new System.Drawing.Point(13, 15);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(102, 13);
            this.labelMain.TabIndex = 2;
            this.labelMain.Text = "Type in vertex label:";
            this.labelMain.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelInput
            // 
            this.labelInput.Location = new System.Drawing.Point(142, 12);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(100, 20);
            this.labelInput.TabIndex = 0;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelError.Location = new System.Drawing.Point(115, 52);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 13);
            this.labelError.TabIndex = 3;
            this.labelError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelError.Click += new System.EventHandler(this.label2_Click);
            // 
            // AddVertexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 114);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.labelMain);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.labelInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddVertexForm";
            this.Text = "Choose label";
            this.Load += new System.EventHandler(this.addVertexForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.TextBox labelInput;
        private System.Windows.Forms.Label labelError;
    }
}