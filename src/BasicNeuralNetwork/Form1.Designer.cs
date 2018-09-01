namespace NEATNeuralNetwork
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.forceGraphVisualizer1 = new ForceDirected.ForceGraphVisualizer();
            this.textBoxFitness = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(849, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Run Generation";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // forceGraphVisualizer1
            // 
            this.forceGraphVisualizer1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.forceGraphVisualizer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.forceGraphVisualizer1.Location = new System.Drawing.Point(12, 12);
            this.forceGraphVisualizer1.Name = "forceGraphVisualizer1";
            this.forceGraphVisualizer1.Size = new System.Drawing.Size(831, 485);
            this.forceGraphVisualizer1.TabIndex = 1;
            // 
            // textBoxFitness
            // 
            this.textBoxFitness.Location = new System.Drawing.Point(923, 41);
            this.textBoxFitness.Name = "textBoxFitness";
            this.textBoxFitness.Size = new System.Drawing.Size(100, 20);
            this.textBoxFitness.TabIndex = 3;
            this.textBoxFitness.TextChanged += new System.EventHandler(this.textBoxFitness_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 509);
            this.Controls.Add(this.textBoxFitness);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.forceGraphVisualizer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ForceDirected.ForceGraphVisualizer forceGraphVisualizer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxFitness;
    }
}

