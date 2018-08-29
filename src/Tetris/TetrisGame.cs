using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    class TetrisGame {

        int width, height = 5;
        int margin = 1;
        int[,] grid;
        int[,] piece;
        int[,] state;
        Image canvas;
        Random r = new Random();

        public TetrisGame(int w, int h, Image canvas) {
            width = w;
            height = h;
            grid = new int[w, h];
            piece = new int[w, h];
            state = new int[w, h];
            this.canvas = canvas;
        }

        public void Move() {

        }

        public int[,] GetGridState() {
            
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (grid[x, y] > 0) {
                        state[x, y] = 1;
                    }
                    else if (piece[x, y] > 0) {
                        state[x, y] = 2;
                    }
                    else {
                        state[x, y] = 0;
                    }
                }
            }

            return state;
        }


        public void Draw() {
            float cellWidth = (float)(canvas.Width-2) / width;
            float cellHeight = (float)(canvas.Height-2) / height;
            using (Graphics g = Graphics.FromImage(canvas)) {

                // Draw grid on the canvas
                for (int x = 0; x <= width; x++) {
                    g.DrawLine(Pens.Black, x * cellWidth, 0, x * cellWidth, height * cellHeight);
                }
                for (int y = 0; y <= height; y++) {
                    g.DrawLine(Pens.Black, 0, y * cellHeight, width * cellWidth, y * cellHeight);
                }

                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        grid[x, y] = r.Next(0, 5);
                        if(grid[x,y] == 1)
                            DrawCell(g, Pens.LightSalmon, Brushes.DarkRed, x, y);
                        else if (grid[x, y] == 2)
                            DrawCell(g, Pens.LightBlue, Brushes.DarkBlue, x, y);
                        else if (grid[x, y] == 3)
                            DrawCell(g, Pens.LightCyan, Brushes.DarkCyan, x, y);
                        else if (grid[x, y] == 4)
                            DrawCell(g, Pens.LightYellow, Brushes.DarkGoldenrod, x, y);
                    }
                }
            }
        }

        public void DrawCell(Graphics g, Pen c, Brush b, int x, int y) {
            float w = (float)(canvas.Width - 2) / width;
            float h = (float)(canvas.Height - 2) / height;
            float x1 = x * w + margin;
            float y1 = y * h + margin;
            float w1 = w - margin * 2;
            float h1 = h - margin * 2;
            g.FillRectangle(b, x1, y1, w1, h1);
            g.DrawRectangle(c, x1, y1, w1, h1);
        }
    }
}
