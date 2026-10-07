using BlockBlast.Module;
using System;
using System.Collections.Generic;

namespace BlockBlast
{
    public class GameManager
    {
        public GameBoard Board { get; private set; }
        public int CurrentScore { get; private set; }
        public int HighScore { get; private set; }
        public bool IsPaused { get; set; }

        private readonly Random rand = new Random();
        public const int BOARD_SIZE = 8;
        private int[,] grid = new int[BOARD_SIZE, BOARD_SIZE];

        // Hàm trả về mảng 8x8 để fGamePlay vẽ
        public int[,] GetGrid()
        {
            return grid;
        }
        public GameManager()
        {
            Board = new GameBoard();
            CurrentScore = 0;
            HighScore = 0;
        }

        public BlockPiece CreateRandomPiece()
        {
            // Logic sinh khối ngẫu nhiên của bạn...
            int[,] shape = new int[,] { { 1, 1 }, { 1, 1 } }; // Ví dụ
            Random rand = new Random();
            int colorIndex = rand.Next(1, 6);
            return new BlockPiece(shape, colorIndex);
        }

        public void AddScore(int points)
        {
            CurrentScore += points;
            if (CurrentScore > HighScore)
            {
                HighScore = CurrentScore;
            }
        }

        public void ResetGame()
        {
            Board.Clear();
            CurrentScore = 0;
        }

        public bool CheckGameOver(List<BlockPiece> remainingPieces)
        {
            if (remainingPieces == null || remainingPieces.Count == 0) return false;

            foreach (var piece in remainingPieces)
            {
                for (int r = 0; r < GameBoard.BOARD_SIZE; r++)
                {
                    for (int c = 0; c < GameBoard.BOARD_SIZE; c++)
                    {
                        if (Board.CanPlaceShape(piece.Shape, r, c))
                            return false; // Vẫn còn vị trí đặt hợp lệ
                    }
                }
            }
            return true; // Không còn chỗ đặt -> Game Over
        }
    }
}