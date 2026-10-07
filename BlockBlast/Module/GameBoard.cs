using System;
using System.Collections.Generic;

namespace BlockBlast.Module
{
    public class GameBoard
    {
        public const int BOARD_SIZE = 8;
        private int[,] grid = new int[BOARD_SIZE, BOARD_SIZE];

        public int GetCell(int r, int c) => grid[r, c];

        public int[,] GetGrid()
        {
            return grid;
        }

        // Xóa sạch toàn bộ bàn chơi
        public void Clear()
        {
            Array.Clear(grid, 0, grid.Length);
        }

        // Kiểm tra xem hình dạng có thể đặt vào vị trí (startRow, startCol) không
        public bool CanPlaceShape(int[,] shape, int startRow, int startCol)
        {
            int rows = shape.GetLength(0);
            int cols = shape.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (shape[r, c] != 0)
                    {
                        int targetRow = startRow + r;
                        int targetCol = startCol + c;

                        // Kiểm tra tràn viền bàn chơi (0 -> 7)
                        if (targetRow < 0 || targetRow >= BOARD_SIZE || targetCol < 0 || targetCol >= BOARD_SIZE)
                            return false;

                        // Kiểm tra ô đã có gạch chưa
                        if (grid[targetRow, targetCol] != 0)
                            return false;
                    }
                }
            }
            return true;
        }

        // Đặt khối gạch vào bàn chơi
        public void PlaceShape(int[,] shape, int colorIndex, int startRow, int startCol)
        {
            int rows = shape.GetLength(0);
            int cols = shape.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (shape[r, c] != 0)
                    {
                        grid[startRow + r, startCol + c] = colorIndex;
                    }
                }
            }
        }

        // Kiểm tra, xóa các dòng/cột đầy và trả về số điểm combo nhận được
        public int ClearAndScoreLines()
        {
            List<int> fullRows = new List<int>();
            List<int> fullCols = new List<int>();

            // Kiểm tra hàng đầy
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                bool full = true;
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    if (grid[r, c] == 0) { full = false; break; }
                }
                if (full) fullRows.Add(r);
            }

            // Kiểm tra cột đầy
            for (int c = 0; c < BOARD_SIZE; c++)
            {
                bool full = true;
                for (int r = 0; r < BOARD_SIZE; r++)
                {
                    if (grid[r, c] == 0) { full = false; break; }
                }
                if (full) fullCols.Add(c);
            }

            // Xóa hàng đầy
            foreach (int r in fullRows)
            {
                for (int c = 0; c < BOARD_SIZE; c++) grid[r, c] = 0;
            }

            // Xóa cột đầy
            foreach (int c in fullCols)
            {
                for (int r = 0; r < BOARD_SIZE; r++) grid[r, c] = 0;
            }

            int totalCleared = fullRows.Count + fullCols.Count;
            if (totalCleared > 0)
            {
                return totalCleared * 100 + (totalCleared - 1) * 50;
            }
            return 0;
        }

        // Nạp ma trận bàn chơi từ Tutorial / Level
        public void SetGrid(int[,] newGrid)
        {
            if (newGrid == null) return;

            int rows = newGrid.GetLength(0);
            int cols = newGrid.GetLength(1);

            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    if (r < rows && c < cols)
                    {
                        grid[r, c] = newGrid[r, c];
                    }
                    else
                    {
                        grid[r, c] = 0;
                    }
                }
            }
        }
    }
}