using BlockBlast.Module;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BlockBlast
{
    public static class BoardRenderer
    {
        public const int BOARD_SIZE = 8; // Thay thế hoặc tham chiếu cố định
        public const int GAP = 4;
        public const int MARGIN = 8;

        // Hàm hỗ trợ tính toán CELL_SIZE phù hợp với kích thước Panel hiện tại
        public static int CalculateCellSize(Size panelSize)
        {
            // Tổng khoảng trống cố định cần trừ đi (2 lề + 7 khoảng cách giữa 8 ô)
            int totalSpacing = (MARGIN * 2) + (GAP * (BOARD_SIZE - 1));

            // Lấy chiều rộng và chiều cao khả dụng để vẽ 8 ô
            int availableWidth = panelSize.Width - totalSpacing;
            int availableHeight = panelSize.Height - totalSpacing;

            // Kích thước 1 ô sẽ lấy theo chiều nhỏ hơn để đảm bảo ô luôn là hình vuông
            int cellSize = Math.Min(availableWidth, availableHeight) / BOARD_SIZE;

            // Đảm bảo kích thước ô tối thiểu không bị nhỏ hơn 10px
            return Math.Max(cellSize, 10);
        }

        public static void FillRoundedRect(Graphics g, Brush brush, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int r = Math.Min(radius, Math.Min(width, height) / 2); // Tránh lỗi radius lớn hơn cell
                if (r < 1) r = 1;

                path.AddArc(x, y, r, r, 180, 90);
                path.AddArc(x + width - r, y, r, r, 270, 90);
                path.AddArc(x + width - r, y + height - r, r, r, 0, 90);
                path.AddArc(x, y + height - r, r, r, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }

        public static void RenderBoard(Graphics g, Size panelSize, GameBoard board, int[,] draggingShape, int draggingColorIndex, int hoverRow, int hoverCol)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Tính CELL_SIZE động
            int cellSize = CalculateCellSize(panelSize);

            // 1. Vẽ 64 ô của bàn chơi
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    int x = MARGIN + c * (cellSize + GAP);
                    int y = MARGIN + r * (cellSize + GAP);

                    Color color = BlockPiece.GetColor(board.GetCell(r, c));
                    using (Brush brush = new SolidBrush(color))
                    {
                        FillRoundedRect(g, brush, x, y, cellSize, cellSize, Math.Max(2, cellSize / 8));
                    }
                }
            }

            // 2. Vẽ hiệu ứng xem trước (Hover Preview)
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

                            if (targetR >= 0 && targetR < BOARD_SIZE && targetC >= 0 && targetC < BOARD_SIZE)
                            {
                                int x = MARGIN + targetC * (cellSize + GAP);
                                int y = MARGIN + targetR * (cellSize + GAP);

                                Color previewColor = Color.FromArgb(130, BlockPiece.GetColor(draggingColorIndex));
                                using (Brush brush = new SolidBrush(previewColor))
                                {
                                    FillRoundedRect(g, brush, x, y, cellSize, cellSize, Math.Max(2, cellSize / 8));
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

            int miniCell = Math.Min((panel.Width - 20) / 5, (panel.Height - 20) / 5);
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