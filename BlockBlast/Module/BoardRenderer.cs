using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BlockBlast
{
    public static class BoardRenderer
    {
        public const int CELL_SIZE = 56;
        public const int GAP = 4;
        public const int MARGIN = 8;

        public static void FillRoundedRect(Graphics g, Brush brush, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(x, y, radius, radius, 180, 90);
                path.AddArc(x + width - radius, y, radius, radius, 270, 90);
                path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
                path.AddArc(x, y + height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }

        public static void RenderBoard(Graphics g, GameBoard board, int[,] draggingShape, int draggingColorIndex, int hoverRow, int hoverCol)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Vẽ các ô trên bàn chơi
            for (int r = 0; r < GameBoard.BOARD_SIZE; r++)
            {
                for (int c = 0; c < GameBoard.BOARD_SIZE; c++)
                {
                    int x = MARGIN + c * (CELL_SIZE + GAP);
                    int y = MARGIN + r * (CELL_SIZE + GAP);

                    Color color = BlockPiece.GetColor(board.GetCell(r, c));
                    using (Brush brush = new SolidBrush(color))
                    {
                        FillRoundedRect(g, brush, x, y, CELL_SIZE, CELL_SIZE, 6);
                    }
                }
            }

            // 2. Vẽ ô xem trước (Hover Preview)
            if (draggingShape != null && hoverRow != -1 && hoverCol != -1)
            {
                int rows = draggingShape.GetLength(0);
                int cols = draggingShape.GetLength(1);

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (draggingShape[r, c] != 0)
                        {
                            int targetR = hoverRow + r;
                            int targetC = hoverCol + c;

                            if (targetR >= 0 && targetR < GameBoard.BOARD_SIZE && targetC >= 0 && targetC < GameBoard.BOARD_SIZE)
                            {
                                int x = MARGIN + targetC * (CELL_SIZE + GAP);
                                int y = MARGIN + targetR * (CELL_SIZE + GAP);

                                Color previewColor = Color.FromArgb(130, BlockPiece.GetColor(draggingColorIndex));
                                using (Brush brush = new SolidBrush(previewColor))
                                {
                                    FillRoundedRect(g, brush, x, y, CELL_SIZE, CELL_SIZE, 6);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void RenderDockPiece(Graphics g, Panel panel)
        {
            if (panel == null || panel.Tag == null) return;

            BlockPiece piece = (BlockPiece)panel.Tag;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int[,] shape = piece.Shape;
            int rows = shape.GetLength(0);
            int cols = shape.GetLength(1);

            int miniCell = 28;
            int miniGap = 3;

            int totalW = cols * (miniCell + miniGap);
            int totalH = rows * (miniCell + miniGap);
            int startX = (panel.Width - totalW) / 2;
            int startY = (panel.Height - totalH) / 2;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (shape[r, c] != 0)
                    {
                        int x = startX + c * (miniCell + miniGap);
                        int y = startY + r * (miniCell + miniGap);

                        using (Brush b = new SolidBrush(BlockPiece.GetColor(piece.ColorIndex)))
                        {
                            FillRoundedRect(g, b, x, y, miniCell, miniCell, 4);
                        }
                    }
                }
            }
        }
    }
}