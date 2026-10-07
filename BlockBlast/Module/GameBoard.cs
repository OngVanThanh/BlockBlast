using System.Collections.Generic;

namespace BlockBlast
{
    public class GameBoard
    {
        public const int BOARD_SIZE = 8;
        private readonly int[,] matrix = new int[BOARD_SIZE, BOARD_SIZE];

        public int GetCell(int r, int c) => matrix[r, c];

        public void Clear()
        {
            for (int r = 0; r < BOARD_SIZE; r++)
                for (int c = 0; c < BOARD_SIZE; c++)
                    matrix[r, c] = 0;
        }

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
                        int targetR = startRow + r;
                        int targetC = startCol + c;

                        if (targetR < 0 || targetR >= BOARD_SIZE || targetC < 0 || targetC >= BOARD_SIZE)
                            return false;

                        if (matrix[targetR, targetC] != 0)
                            return false;
                    }
                }
            }
            return true;
        }

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
                        matrix[startRow + r, startCol + c] = colorIndex;
                    }
                }
            }
        }

        public int ClearAndScoreLines()
        {
            List<int> fullRows = new List<int>();
            List<int> fullCols = new List<int>();

            for (int r = 0; r < BOARD_SIZE; r++)
            {
                bool full = true;
                for (int c = 0; c < BOARD_SIZE; c++) if (matrix[r, c] == 0) { full = false; break; }
                if (full) fullRows.Add(r);
            }

            for (int c = 0; c < BOARD_SIZE; c++)
            {
                bool full = true;
                for (int r = 0; r < BOARD_SIZE; r++) if (matrix[r, c] == 0) { full = false; break; }
                if (full) fullCols.Add(c);
            }

            foreach (int r in fullRows) for (int c = 0; c < BOARD_SIZE; c++) matrix[r, c] = 0;
            foreach (int c in fullCols) for (int r = 0; r < BOARD_SIZE; r++) matrix[r, c] = 0;

            int totalCleared = fullRows.Count + fullCols.Count;
            if (totalCleared > 0)
            {
                return totalCleared * 100 + (totalCleared - 1) * 50;
            }
            return 0;
        }
    }
}