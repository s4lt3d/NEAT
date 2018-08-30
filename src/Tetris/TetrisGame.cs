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
        int piece = 6;
        int pieceRotation = 0;
        Image canvas;
        Random r = new Random();
        public enum Actions { RotateLeft, RotateRight, MoveLeft, MoveRight, MoveDown, Drop };

        Point pieceLocation = new Point();

        public TetrisGame(int w, int h, Image canvas) {
            InitPieces();

            width = w + 2;
            height = h + 2;
            InitGrid();
            this.canvas = canvas;
            pieceLocation = new Point(5, 0);
        }

        public void InitGrid() {
            grid = new int[width, height];
            state = new int[width, height];
            for (int i = 0; i < width; i++) {
                grid[i, height - 1] = 5;
            }

            for (int j = 0; j < height; j++) {
                grid[0, j] = 5;
                grid[width-1, j] = 5;
            }
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


        private void RotatePiece(int direction = 0) {
            pieceRotation += direction;
            if (pieceRotation < 0)
                pieceRotation = 3;
            else if (pieceRotation >= 4)
                pieceRotation = 0;
        }

        public void Move(Actions action) {
            int prevRotation = pieceRotation;
            Point prevPosition = pieceLocation;
            bool reset = false;
            switch (action) {
                case Actions.MoveLeft:
                    pieceLocation.X -= 1;
                    reset = !CheckMove();
                    break;
                case Actions.MoveRight:
                    pieceLocation.X += 1;
                    reset = !CheckMove();
                    break;
                case Actions.RotateLeft:
                    RotatePiece(1);
                    reset = !CheckMove();
                    break;
                case Actions.RotateRight:
                    RotatePiece(-1);
                    reset = !CheckMove();
                    break;
                case Actions.MoveDown:
                    pieceLocation.Y++;
                    if (CheckMove() == false) {
                        pieceLocation = prevPosition;
                        pieceRotation = prevRotation;
                        GetNextPiece();
                    }
                    break;
            }

            if (reset) {
                pieceLocation = prevPosition;
                pieceRotation = prevRotation;
            }

        }

        private void GetNextPiece() {
            int[,] p = pieces[piece * 4 + pieceRotation];

            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    if(p[x,y] > 0)
                        grid[x + pieceLocation.X, y + pieceLocation.Y] = p[x,y];
                }
            }

            piece = r.Next(pieces.Count / 4);
            pieceLocation.X = 6;
            pieceLocation.Y = 2;
            pieceRotation = 0;
        }

        public bool CheckMove() {
            int[,] moveState = GetGridState();
            foreach (int i in moveState) {
                if (i > 1) {
                    return false;
                }
            }
            return true;
        }

        private int Clamp(int val, int min, int max) {
            return Math.Max(Math.Min(val, max), min);
        }
        
        public int[,] GetGridState() {
            int[,] p = pieces[piece * 4 + pieceRotation];
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    state[x, y] = 0;
                }
            }

            int i = 0;
            int j = 0;
            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    try {
                        i = Clamp(x + pieceLocation.X, 0, width);
                        j = Clamp(y + pieceLocation.Y, 0, height);
                        state[i, j] = (p[x, y] > 0) ? 1 : 0;
                    }
                    catch { }
                }
            }

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (grid[x, y] > 0) {
                        state[x, y] += 1;
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
            int[,] p = pieces[piece * 4 + pieceRotation];

            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    DrawCell(g, p[x,y],  x + pieceLocation.X, y + pieceLocation.Y);
                }
            }
        }

        public void Draw() {
            float cellWidth = (float)(canvas.Width-2) / (width);
            float cellHeight = (float)(canvas.Height-2) / (height);
            using (Graphics g = Graphics.FromImage(canvas)) {
                g.FillRectangle(Brushes.Black, 0,0, canvas.Width, canvas.Height);
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
        }

        public void DrawCell(Graphics g, int type, int x, int y) {
            float w = (float)(canvas.Width) / width;
            float h = (float)(canvas.Height) / height;
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
                case 5:
                    c = Pens.Black; b = Brushes.Black;
                    break;
                default:
                    
                    break;
            }
            if (type > 0) {
                g.FillRectangle(b, x1, y1, w1, h1);
                g.DrawRectangle(c, x1, y1, w1, h1);
            }
        }
    }
}
