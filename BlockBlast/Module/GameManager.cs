using BlockBlast.Module;
using System;
using System.Collections.Generic;

namespace BlockBlast
{
    public class GameManager
    {
        public GameBoard Board { get; private set; }
        public int CurrentScore { get; set; }
        public int HighScore { get; set; }
        public int ComboCount { get; set; } = 0;
        public bool IsPaused { get; set; }

        private readonly Random rand = new Random();
        public const int BOARD_SIZE = 8;

        public GameManager()
        {
            Board = new GameBoard();
            CurrentScore = 0;
            HighScore = 0;
        }

        public BlockPiece CreateRandomPiece(int difficulty = 1)
        {
            return DDAEngine.GenerateAdaptivePiece(Board, difficulty);
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