namespace FinalProject.Controls
{
    partial class LayerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.visible_Button = new System.Windows.Forms.Button();
            this.locked_Button = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.up_Btn = new System.Windows.Forms.Button();
            this.down_Btn = new System.Windows.Forms.Button();
            this.remove_Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // visible_Button
            // 
            this.visible_Button.Location = new System.Drawing.Point(3, 36);
            this.visible_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.visible_Button.Name = "visible_Button";
            this.visible_Button.Size = new System.Drawing.Size(86, 31);
            this.visible_Button.TabIndex = 0;
            this.visible_Button.Text = "Visible";
            this.visible_Button.UseVisualStyleBackColor = true;
            this.visible_Button.Click += new System.EventHandler(this.visible_Clicked);
            // 
            // locked_Button
            // 
            this.locked_Button.Location = new System.Drawing.Point(95, 36);
            this.locked_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.locked_Button.Name = "locked_Button";
            this.locked_Button.Size = new System.Drawing.Size(86, 31);
            this.locked_Button.TabIndex = 1;
            this.locked_Button.Text = "Locked";
            this.locked_Button.UseVisualStyleBackColor = true;
            this.locked_Button.Click += new System.EventHandler(this.locked_Clicked);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(96, 85);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(177, 27);
            this.textBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Layer Name:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // up_Btn
            // 
            this.up_Btn.Location = new System.Drawing.Point(187, 36);
            this.up_Btn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.up_Btn.Name = "up_Btn";
            this.up_Btn.Size = new System.Drawing.Size(86, 31);
            this.up_Btn.TabIndex = 4;
            this.up_Btn.Text = "Send Up";
            this.up_Btn.UseVisualStyleBackColor = true;
            this.up_Btn.Click += new System.EventHandler(this.sendUp_Clicked);
            // 
            // down_Btn
            // 
            this.down_Btn.Location = new System.Drawing.Point(279, 36);
            this.down_Btn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.down_Btn.Name = "down_Btn";
            this.down_Btn.Size = new System.Drawing.Size(95, 31);
            this.down_Btn.TabIndex = 5;
            this.down_Btn.Text = "Send Down";
            this.down_Btn.UseVisualStyleBackColor = true;
            this.down_Btn.Click += new System.EventHandler(this.sendDown_Clicked);
            // 
            // remove_Btn
            // 
            this.remove_Btn.Location = new System.Drawing.Point(288, 83);
            this.remove_Btn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.remove_Btn.Name = "remove_Btn";
            this.remove_Btn.Size = new System.Drawing.Size(86, 31);
            this.remove_Btn.TabIndex = 6;
            this.remove_Btn.Text = "Remove";
            this.remove_Btn.UseVisualStyleBackColor = true;
            this.remove_Btn.Click += new System.EventHandler(this.remove_Clicked);
            // 
            // LayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.remove_Btn);
            this.Controls.Add(this.down_Btn);
            this.Controls.Add(this.up_Btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.locked_Button);
            this.Controls.Add(this.visible_Button);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LayerControl";
            this.Size = new System.Drawing.Size(418, 123);
            this.Click += new System.EventHandler(this.form_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button visible_Button;
        private Button locked_Button;
        private TextBox textBox1;
        private Label label1;
        private Button up_Btn;
        private Button down_Btn;
        private Button remove_Btn;
    }
}
