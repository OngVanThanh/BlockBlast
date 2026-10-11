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

            FindFullLines(fullRows, fullCols);
            ClearLines(fullRows, fullCols);

            return CalculateLineScore(fullRows.Count + fullCols.Count);
        }

        // Công thức điểm: mỗi dòng 100 điểm + thưởng combo 50 điểm cho mỗi dòng thêm
        public static int CalculateLineScore(int totalCleared)
        {
            if (totalCleared <= 0) return 0;
            return totalCleared * 100 + (totalCleared - 1) * 50;
        }

        // Tìm các hàng/cột đang đầy (KHÔNG xóa) - dùng để chạy hoạt họa trước khi xóa
        public void FindFullLines(List<int> fullRows, List<int> fullCols)
        {
            fullRows.Clear();
            fullCols.Clear();

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
        }

        // Xóa các hàng/cột chỉ định
        public void ClearLines(IEnumerable<int> rows, IEnumerable<int> cols)
        {
            // Xóa hàng đầy
            foreach (int r in rows)
            {
                for (int c = 0; c < BOARD_SIZE; c++) grid[r, c] = 0;
            }

            // Xóa cột đầy
            foreach (int c in cols)
            {
                for (int r = 0; r < BOARD_SIZE; r++) grid[r, c] = 0;
            }
        }

        // Mô phỏng (không thay đổi bàn cờ): nếu đặt shape tại (startRow, startCol)
        // thì những hàng/cột nào sẽ được lấp đầy? Dùng cho hiệu ứng Line Preview Glow.
        public void GetLinesCompletedByPlacement(int[,] shape, int startRow, int startCol,
                                                 List<int> rows, List<int> cols)
        {
            rows.Clear();
            cols.Clear();
            if (shape == null) return;

            for (int r = 0; r < BOARD_SIZE; r++)
            {
                bool full = true;
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    if (grid[r, c] == 0 && !ShapeCovers(shape, startRow, startCol, r, c)) { full = false; break; }
                }
                if (full) rows.Add(r);
            }

            for (int c = 0; c < BOARD_SIZE; c++)
            {
                bool full = true;
                for (int r = 0; r < BOARD_SIZE; r++)
                {
                    if (grid[r, c] == 0 && !ShapeCovers(shape, startRow, startCol, r, c)) { full = false; break; }
                }
                if (full) cols.Add(c);
            }
        }

        private static bool ShapeCovers(int[,] shape, int startRow, int startCol, int r, int c)
        {
            int sr = r - startRow;
            int sc = c - startCol;
            return sr >= 0 && sr < shape.GetLength(0) &&
                   sc >= 0 && sc < shape.GetLength(1) &&
                   shape[sr, sc] != 0;
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