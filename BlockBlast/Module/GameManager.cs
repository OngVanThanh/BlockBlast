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

        public GameManager()
        {
            Board = new GameBoard();
            CurrentScore = 0;
            HighScore = 0;
        }

        public BlockPiece CreateRandomPiece()
        {
            int shapeIdx = rand.Next(BlockPiece.AllShapes.Count);
            int colorIdx = rand.Next(1, 8);
            return new BlockPiece(BlockPiece.AllShapes[shapeIdx], colorIdx);
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
            if (remainingPieces.Count == 0) return false;

            foreach (var piece in remainingPieces)
            {
                for (int r = 0; r < GameBoard.BOARD_SIZE; r++)
                {
                    for (int c = 0; c < GameBoard.BOARD_SIZE; c++)
                    {
                        if (Board.CanPlaceShape(piece.Shape, r, c))
                        {
                            return false; // Còn vị trí đặt
                        }
                    }
                }
            }
            return true; // Không còn vị trí nào hợp lệ -> Game Over
        }
    }
}