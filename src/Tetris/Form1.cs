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
            tetris = new TetrisGame(10, 20, i);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            tetris.Draw();
            pictureBox1.Invalidate();
        }

        protected override bool IsInputKey(Keys keyData) {
            switch (keyData) {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {

            switch (e.KeyCode) {
                case Keys.Left:
                    tetris.Move(TetrisGame.Actions.MoveLeft);
                    break;
                case Keys.Right:
                    tetris.Move(TetrisGame.Actions.MoveRight);
                    break;
                case Keys.Up:
                    tetris.Move(TetrisGame.Actions.RotateLeft);
                    break;
                case Keys.Down:
                    tetris.Move(TetrisGame.Actions.MoveDown);
                    break;
            }
        }
    }
}
