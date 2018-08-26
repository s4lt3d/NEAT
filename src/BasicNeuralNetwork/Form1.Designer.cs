namespace BasicNeuralNetwork
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
            this.forceGraphVisualizer1 = new ForceDirected.ForceGraphVisualizer();
            this.forceGraphVisualizer2 = new ForceDirected.ForceGraphVisualizer();
            this.forceGraphVisualizer3 = new ForceDirected.ForceGraphVisualizer();
            this.SuspendLayout();
            // 
            // forceGraphVisualizer1
            // 
            this.forceGraphVisualizer1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.forceGraphVisualizer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.forceGraphVisualizer1.Location = new System.Drawing.Point(12, 12);
            this.forceGraphVisualizer1.Name = "forceGraphVisualizer1";
            this.forceGraphVisualizer1.Size = new System.Drawing.Size(302, 485);
            this.forceGraphVisualizer1.TabIndex = 1;
            // 
            // forceGraphVisualizer2
            // 
            this.forceGraphVisualizer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.forceGraphVisualizer2.Location = new System.Drawing.Point(320, 12);
            this.forceGraphVisualizer2.Name = "forceGraphVisualizer2";
            this.forceGraphVisualizer2.Size = new System.Drawing.Size(365, 485);
            this.forceGraphVisualizer2.TabIndex = 2;
            // 
            // forceGraphVisualizer3
            // 
            this.forceGraphVisualizer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.forceGraphVisualizer3.Location = new System.Drawing.Point(691, 12);
            this.forceGraphVisualizer3.Name = "forceGraphVisualizer3";
            this.forceGraphVisualizer3.Size = new System.Drawing.Size(334, 485);
            this.forceGraphVisualizer3.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 509);
            this.Controls.Add(this.forceGraphVisualizer3);
            this.Controls.Add(this.forceGraphVisualizer2);
            this.Controls.Add(this.forceGraphVisualizer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ForceDirected.ForceGraphVisualizer forceGraphVisualizer1;
        private ForceDirected.ForceGraphVisualizer forceGraphVisualizer2;
        private ForceDirected.ForceGraphVisualizer forceGraphVisualizer3;
    }
}

