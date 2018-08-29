using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris {
    public partial class Form1 : Form {

        TetrisGame tetris;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Image i = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = i;
            tetris = new TetrisGame(12, 20, i);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            tetris.Draw();
            pictureBox1.Invalidate();
        }
    }
}
