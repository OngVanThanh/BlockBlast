using System.Collections.Generic;

namespace BlockBlast.Module
{
    // Class chứa dữ liệu của từng màn Hướng dẫn / Puzzle
    public class TutorialStep
    {
        public string Title { get; set; }
        public string Instruction { get; set; }
        public int[,] BoardLayout { get; set; }
        public int[,] DockPieceShape { get; set; }
        public int ColorIndex { get; set; }
    }

    // Class quản lý tiến trình các màn chơi
    public class TutorialManager
    {
        public int CurrentLevelIndex { get; private set; } = 0;
        public bool IsTutorialActive { get; set; } = true;

        private readonly List<TutorialStep> levels = new List<TutorialStep>();

        public TutorialManager()
        {
            InitPuzzleLevels();
        }

        private void InitPuzzleLevels()
        {
            // ==========================================
            // MÀN 1: HÌNH CHỮ THẬP (Thả khối 2x2 vào tâm nổ 2 hàng + 2 cột)
            // ==========================================
            int[,] board1 = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                board1[3, i] = 1; // Hàng 3
                board1[4, i] = 1; // Hàng 4
                board1[i, 3] = 1; // Cột 3
                board1[i, 4] = 1; // Cột 4
            }
            // Đục lỗ ở giữa tâm (2x2)
            board1[3, 3] = 0; board1[3, 4] = 0;
            board1[4, 3] = 0; board1[4, 4] = 0;

            levels.Add(new TutorialStep
            {
                Title = "Màn 1: Chữ Thập Combo",
                Instruction = "Kéo khối 2x2 thả vào TÂM chữ thập để nổ Combo!",
                BoardLayout = board1,
                DockPieceShape = new int[,] { { 1, 1 }, { 1, 1 } },
                ColorIndex = 2
            });

            // ==========================================
            // MÀN 2: LẤP ĐẦY KHUNG (Thả khối 1x4 để ăn dòng ngang)
            // ==========================================
            int[,] board2 = new int[8, 8];
            for (int c = 0; c < 8; c++)
            {
                if (c < 2 || c > 5) board2[2, c] = 3;
            }

            levels.Add(new TutorialStep
            {
                Title = "Màn 2: Lấp Đầy Khe Mới",
                Instruction = "Đặt khối 1x4 vào khe hở để hoàn thành hàng ngang!",
                BoardLayout = board2,
                DockPieceShape = new int[,] { { 1, 1, 1, 1 } },
                ColorIndex = 4
            });

            // ==========================================
            // MÀN 3: SIÊU COMBO 3 HÀNG (Thả khối 3x3 vào góc [1,1])
            // ==========================================
            int[,] board3 = new int[8, 8];
            for (int r = 1; r <= 3; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    // Đổ đầy 3 hàng (hàng 1, 2, 3), CHỈ để trống vùng 3x3 từ hàng 1-3, cột 1-3
                    if (!(c >= 1 && c <= 3))
                    {
                        board3[r, c] = 5;
                    }
                }
            }

            levels.Add(new TutorialStep
            {
                Title = "Màn 3: Siêu Combo Mega",
                Instruction = "Thả khối 3x3 vào góc trống để nổ tung 3 hàng cùng lúc!",
                BoardLayout = board3,
                DockPieceShape = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } },
                ColorIndex = 1
            });
        }

        public TutorialStep GetCurrentLevel()
        {
            if (CurrentLevelIndex >= 0 && CurrentLevelIndex < levels.Count)
                return levels[CurrentLevelIndex];
            return null;
        }

        public bool NextLevel()
        {
            CurrentLevelIndex++; // Chú ý chữ C viết hoa

            // Kiểm tra nếu đã vượt qua màn hướng dẫn cuối cùng
            if (CurrentLevelIndex >= levels.Count) // Sửa tutorialLevels -> levels
            {
                IsTutorialActive = false; // Tắt hoàn toàn chế độ Tutorial
                return false;             // Báo hiệu đã hết màn hướng dẫn
            }

            return true; // Tiếp tục sang màn hướng dẫn tiếp theo
        }

        // Hàm hỗ trợ reset tutorial từ đầu nếu người chơi muốn chơi lại
        public void ResetTutorial()
        {
            CurrentLevelIndex = 0;
            IsTutorialActive = true;
        }
    }
}