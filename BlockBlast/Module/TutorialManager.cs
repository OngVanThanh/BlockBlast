using System.Collections.Generic;

namespace BlockBlast.Module
{
    public class TutorialStep
    {
        public string Title { get; set; }
        public string Instruction { get; set; }
        public int[,] BoardLayout { get; set; }
        public int[,] DockPieceShape { get; set; }
        public int ColorIndex { get; set; }
    }

    public class TutorialManager
    {
        private List<TutorialStep> levels = new List<TutorialStep>();
        private int currentLevelIndex = 0;

        public bool IsTutorialActive => currentLevelIndex < levels.Count;

        public TutorialManager()
        {
            InitTutorialLevels();
        }

        private void InitTutorialLevels()
        {
            // Màn 1: Ngang đơn giản
            int[,] board1 = new int[8, 8];
            for (int c = 0; c < 8; c++) if (c != 3 && c != 4) board1[3, c] = 2;

            levels.Add(new TutorialStep
            {
                Title = "Màn 1",
                Instruction = "Kéo khối gạch lấp đầy hàng ngang!",
                BoardLayout = board1,
                DockPieceShape = new int[,] { { 1, 1 } },
                ColorIndex = 1
            });

            // Màn 2: Dọc đơn giản
            int[,] board2 = new int[8, 8];
            for (int r = 0; r < 8; r++) if (r != 2 && r != 3) board2[r, 4] = 3;

            levels.Add(new TutorialStep
            {
                Title = "Màn 2",
                Instruction = "Kéo khối gạch lấp đầy cột dọc!",
                BoardLayout = board2,
                DockPieceShape = new int[,] { { 1 }, { 1 } },
                ColorIndex = 2
            });

            // Màn 3: Combo Mega 3x3 (Đặt vào nổ sạch)
            int[,] board3 = new int[8, 8];
            for (int r = 1; r <= 3; r++)
                for (int c = 0; c < 8; c++) board3[r, c] = 5;

            for (int r = 1; r <= 3; r++)
                for (int c = 1; c <= 3; c++) board3[r, c] = 0;

            levels.Add(new TutorialStep
            {
                Title = "Màn 3",
                Instruction = "Thả khối 3x3 vào vùng trống để dọn sạch bàn!",
                BoardLayout = board3,
                DockPieceShape = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } },
                ColorIndex = 4
            });
        }

        public TutorialStep GetCurrentLevel()
        {
            if (IsTutorialActive) return levels[currentLevelIndex];
            return null;
        }

        public bool NextLevel()
        {
            currentLevelIndex++;
            return IsTutorialActive;
        }
    }
}