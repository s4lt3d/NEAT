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
        List<int[,]> pieces = new List<int[,]>();
        int[,] state;
        int piece = 1;
        int pieceRotation = 0;
        Image canvas;
        Random r = new Random();
        public enum Actions { RotateLeft, RotateRight, MoveLeft, MoveRight, MoveDown, Drop };

        Point pieceLocation = new Point();

        public TetrisGame(int w, int h, Image canvas) {
            InitPieces();
            width = w;
            height = h;
            grid = new int[w + 2, h + 2];
            state = new int[w + 2, h + 2];
            this.canvas = canvas;
            pieceLocation = new Point(3, 3);
        }

        public void InitPieces() {

            // rotate point around point 1, 1
            pieces.Add(new int[,] { {0,0,0,0}, 
                                    {1,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,0,0,0},
                                    {1,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,0,0,0},
                                    {1,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,0,0,0},
                                    {1,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0}});



            pieces.Add(new int[,] { {0,0,1,0},
                                    {0,0,1,0},
                                    {0,0,1,0},
                                    {0,0,1,0}});

            pieces.Add(new int[,] { {0,0,0,0},
                                    {1,1,1,1},
                                    {0,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,1,0,0},
                                    {0,1,0,0},
                                    {0,1,0,0},
                                    {0,1,0,0}});

            pieces.Add(new int[,] { {0,0,0,0},
                                    {1,1,1,1},
                                    {0,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {1,0,0,0},
                                    {1,1,0,0},
                                    {0,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,0,0,0},
                                    {0,1,1,0},
                                    {1,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {1,0,0,0},
                                    {1,1,0,0},
                                    {0,1,0,0},
                                    {0,0,0,0}});


            pieces.Add(new int[,] { {0,0,0,0},
                                    {0,1,1,0},
                                    {1,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,1,0,0},
                                    {1,1,0,0},
                                    {1,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {1,1,0,0},
                                    {0,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,1,0,0},
                                    {1,1,0,0},
                                    {1,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {1,1,0,0},
                                    {0,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,1,1,0},
                                    {0,1,0,0},
                                    {0,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {1,0,0,0},
                                    {1,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,1,0,0},
                                    {0,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,0,0,0},
                                    {1,1,1,0},
                                    {0,0,1,0},
                                    {0,0,0,0}});



            pieces.Add(new int[,] { {1,1,0,0},
                                    {0,1,0,0},
                                    {0,1,0,0},
                                    {0,0,0,0}});
            
            pieces.Add(new int[,] { {0,0,0,0},
                                    {1,1,1,0},
                                    {1,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,1,0,0},
                                    {0,1,0,0},
                                    {0,1,1,0},
                                    {0,0,0,0}});
            
            pieces.Add(new int[,] { {0,0,1,0},
                                    {1,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}});

            


            pieces.Add(new int[,] { {0,1,0,0},
                                    {1,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,1,0,0},
                                    {1,1,0,0},
                                    {0,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,0,0,0},
                                    {1,1,1,0},
                                    {0,1,0,0},
                                    {0,0,0,0}});

            pieces.Add(new int[,] { {0,1,0,0},
                                    {0,1,1,0},
                                    {0,1,0,0},
                                    {0,0,0,0}});




        }

        private bool CheckMove(Actions action) {

            switch (action) {
                case Actions.MoveLeft:

                    break;
            }

            return true;
        }

        private void RotatePiece(int direction = 0) {
            pieceRotation += direction;
            if (pieceRotation < 0)
                pieceRotation = 3;
            else if (pieceRotation >= 4)
                pieceRotation = 0;
        }

        public void Move(Actions action) {
            if (CheckMove(action)) {

            }

            switch (action) {
                case Actions.RotateLeft:

                    break;


            }
        }

       
        public int[,] GetGridState() {
            
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (grid[x, y] > 0) {
                        state[x, y] = 1;
                    }
                    else {
                        state[x, y] = 0;
                    }
                }
            }

            return state;
        }

        // things for drawing

        public void DrawPiece(Graphics g) {
            int[,] p = pieces[piece  + pieceRotation];

            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    DrawCell(g, p[x,y],  x + pieceLocation.X, y + pieceLocation.Y);
                }
            }
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
                        DrawCell(g, grid[x, y], x, y);
                    }
                }

                DrawPiece(g);
            }

            RotatePiece(0);

        }

        public void DrawCell(Graphics g, int type, int x, int y) {
            float w = (float)(canvas.Width - 2) / width;
            float h = (float)(canvas.Height - 2) / height;
            float x1 = x * w + margin;
            float y1 = y * h + margin;
            float w1 = w - margin * 2;
            float h1 = h - margin * 2;
            Brush b = Brushes.Gray;
            Pen c = Pens.White;

            switch (type) {
                case 1:
                    c = Pens.LightSalmon; b = Brushes.DarkRed;
                    break;
                case 2:
                    c = Pens.LightBlue; b = Brushes.DarkBlue;
                    break;
                case 3:
                    c = Pens.LightCyan; b = Brushes.DarkCyan;
                    break;
                case 4:
                    c = Pens.LightYellow; b = Brushes.DarkGoldenrod;
                    break;
                default:
                    break;
            }

            g.FillRectangle(b, x1, y1, w1, h1);
            g.DrawRectangle(c, x1, y1, w1, h1);
        }
    }
}
