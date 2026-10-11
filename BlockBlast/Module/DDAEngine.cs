using System;
using System.Collections.Generic;

namespace BlockBlast.Module
{
    public static class DDAEngine
    {
        private static readonly Random rand = new Random();

        // Đánh giá tỷ lệ lấp đầy bàn cờ (0.0 -> 1.0)
        public static double CalculateBoardFillRatio(GameBoard board)
        {
            if (board == null) return 0.0;
            int filled = 0;
            for (int r = 0; r < GameBoard.BOARD_SIZE; r++)
                for (int c = 0; c < GameBoard.BOARD_SIZE; c++)
                    if (board.GetCell(r, c) != 0) filled++;
            return (double)filled / (GameBoard.BOARD_SIZE * GameBoard.BOARD_SIZE);
        }

        // Sinh khối gạch có chủ đích dựa trên trạng thái bàn cờ
        public static BlockPiece GenerateAdaptivePiece(GameBoard board, int targetDifficulty = 1)
        {
            double fillRatio = CalculateBoardFillRatio(board);
            List<int[,]> candidateShapes = new List<int[,]>();

            // Bàn cờ nguy cấp (lấp đầy > 65%): Ưu tiên sinh các khối cứu hộ nhỏ gọn
            if (fillRatio > 0.65)
            {
                candidateShapes.Add(new int[,] { { 1 } }); // 1x1
                candidateShapes.Add(new int[,] { { 1, 1 } }); // 1x2
                candidateShapes.Add(new int[,] { { 1 }, { 1 } }); // 2x1
                candidateShapes.Add(new int[,] { { 1, 1 }, { 1, 1 } }); // 2x2
                candidateShapes.Add(new int[,] { { 1, 0 }, { 1, 1 } }); // corner mini L
            }
            // Bàn cờ thông thoáng: Tăng độ thử thách với các khối lớn
            else if (fillRatio < 0.35 && targetDifficulty > 1)
            {
                candidateShapes.Add(new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }); // 3x3
                candidateShapes.Add(new int[,] { { 1, 1, 1, 1, 1 } }); // 5x1
                candidateShapes.Add(new int[,] { { 1 }, { 1 }, { 1 }, { 1 }, { 1 } }); // 1x5
                candidateShapes.Add(new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }); // Z shape
                candidateShapes.Add(new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }); // S shape
                candidateShapes.Add(new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } }); // Big L
            }
            else
            {
                // Độ khó cân bằng: Bốc từ toàn bộ danh sách chuẩn
                candidateShapes = BlockPiece.AllShapes;
            }

            int idx = rand.Next(candidateShapes.Count);
            int color = rand.Next(1, 8);
            return new BlockPiece(candidateShapes[idx], color);
        }
    }
}
