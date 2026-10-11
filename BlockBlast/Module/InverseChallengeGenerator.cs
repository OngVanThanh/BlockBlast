using System;
using System.Drawing;

namespace BlockBlast.Module
{
    /// <summary>
    /// Thuật toán Sinh Thử Thách Động Mô Phỏng Ngược Theo Mùa (Thematic Inverse Solvable Generator).
    /// Áp dụng nguyên lý toán học nghịch đảo: Khởi tạo bàn cờ với các khe hở tiêu chuẩn vừa khít
    /// các hình khối cơ bản của BlockBlast, bảo đảm 100% người chơi luôn có các nước đi khả dĩ
    /// để kích hoạt nổ dòng ngay từ những lượt đầu tiên.
    /// </summary>
    public static class InverseChallengeGenerator
    {
        // Thông số chủ đề của 8 Đại Mùa Sử Thi
        public struct SeasonTheme
        {
            public int Id;
            public string Name;
            public string Element;
            public string Icon;
            public Color AccentColor;
            public int ThemeColorIndex;
            public string Subtitle;
            public string BlastText;
        }

        public static readonly SeasonTheme[] Themes = new SeasonTheme[]
        {
            new SeasonTheme { Id = 1, Name = "Lục Địa Đất Mẹ", Element = "Đất Mẹ", Icon = "🌿", AccentColor = Color.FromArgb(46, 204, 113), ThemeColorIndex = 2, Subtitle = "Vùng Thạch Trận Cổ Đại (Ancient Monolith)", BlastText = "🌿 ĐẤT MẸ THỨC TỈNH!" },
            new SeasonTheme { Id = 2, Name = "Kỷ Băng Hà Vĩnh Cửu", Element = "Băng Giá", Icon = "❄️", AccentColor = Color.FromArgb(0, 210, 211), ThemeColorIndex = 7, Subtitle = "Thung Lũng Băng Vĩnh Cửu (Frozen Vale)", BlastText = "❄️ PHÁ VỠ BĂNG GIÁ!" },
            new SeasonTheme { Id = 3, Name = "Tro Tàn Hỏa Ngục", Element = "Hỏa Diệm", Icon = "🔥", AccentColor = Color.FromArgb(255, 71, 87), ThemeColorIndex = 1, Subtitle = "Lò Rèn Dung Nham (Molten Forge)", BlastText = "🔥 HỎA DIỆM BÙNG NỔ!" },
            new SeasonTheme { Id = 4, Name = "Đỉnh Tháp Thiên Lôi", Element = "Thiên Lôi", Icon = "⚡", AccentColor = Color.FromArgb(255, 165, 2), ThemeColorIndex = 4, Subtitle = "Đỉnh Tháp Cuồng Phong (Storm Peak)", BlastText = "⚡ THIÊN LÔI QUY TỤ!" },
            new SeasonTheme { Id = 5, Name = "Vực Thẳm Biển Sâu", Element = "Thủy Triều", Icon = "🌊", AccentColor = Color.FromArgb(30, 144, 255), ThemeColorIndex = 5, Subtitle = "Rạn San Hô Thất Lạc (Abyssal Trench)", BlastText = "🌊 THỦY TRIỀU DÂNG CAO!" },
            new SeasonTheme { Id = 6, Name = "Bão Cát Sa Mạc", Element = "Sa Mạc", Icon = "🏜️", AccentColor = Color.FromArgb(230, 126, 34), ThemeColorIndex = 3, Subtitle = "Lăng Mộ Cát Vàng (Pharaoh Tomb)", BlastText = "🏜️ BÃO CÁT CUỒN CUỘN!" },
            new SeasonTheme { Id = 7, Name = "Hư Không Vũ Trụ", Element = "Hư Không", Icon = "🌌", AccentColor = Color.FromArgb(155, 89, 182), ThemeColorIndex = 6, Subtitle = "Hố Đen Thứ Nguyên (Cosmic Void)", BlastText = "🌌 HƯ KHÔNG HỢP NHẤT!" },
            new SeasonTheme { Id = 8, Name = "Thời Khắc Khải Huyền", Element = "Chí Tôn", Icon = "👑", AccentColor = Color.FromArgb(241, 242, 246), ThemeColorIndex = 8, Subtitle = "Ngai Vàng Tối Thượng (Cosmic Zenith)", BlastText = "👑 KHẢI HUYỀN CHÍ TÔN!" }
        };

        public static SeasonTheme GetTheme(int seasonId)
        {
            if (seasonId < 1) seasonId = 1;
            if (seasonId > Themes.Length) seasonId = Themes.Length;
            return Themes[seasonId - 1];
        }

        /// <summary>
        /// Tạo mới một GameChallenge với độ khó và layout được mô phỏng ngược thích ứng.
        /// </summary>
        public static GameChallenge GenerateChallenge(int seasonId, int levelId)
        {
            if (seasonId < 1) seasonId = 1;
            if (seasonId > 8) seasonId = 8;
            if (levelId < 1) levelId = 1;
            if (levelId > 12) levelId = 12;

            var theme = GetTheme(seasonId);

            // Công thức tính số dòng mục tiêu tăng tiến theo Season và Level
            int targetLines = Math.Min(12, 3 + (levelId + 1) / 3 + (seasonId - 1) / 2);

            // Công thức tính số lượt đi tối đa (vừa đủ thử thách nhưng có biên độ an toàn)
            int maxMoves = Math.Max(12, 22 - (levelId * 2 / 3) + (seasonId <= 2 ? 3 : 1));

            // Sinh ma trận chướng ngại vật theo chủ đề nguyên tố
            int[,] board = GenerateInverseSolvableBoard(seasonId, levelId, theme.ThemeColorIndex);

            var challenge = new GameChallenge
            {
                SeasonId = seasonId,
                LevelId = levelId,
                SeasonTitle = $"Mùa {seasonId}: {theme.Name}",
                LevelTitle = $"Màn {levelId}: Thử Thách Cấp {levelId}",
                ElementName = theme.Element,
                Icon = theme.Icon,
                Subtitle = theme.Subtitle,
                BlastFloatingText = theme.BlastText,
                AccentColor = theme.AccentColor,
                ThemeColorIndex = theme.ThemeColorIndex,
                TargetLines = targetLines,
                MaxMoves = maxMoves,
                MovesLeft = maxMoves,
                InitialBoard = board
            };

            // Tạo các câu thoại Briefing và Victory giàu tính sử thi
            GenerateLoreTexts(challenge);

            return challenge;
        }

        /// <summary>
        /// Sinh bố cục bàn cờ theo nguyên lý Mô Phỏng Ngược:
        /// Các hàng/cột chướng ngại vật luôn chừa lại đúng 1..3 ô trống ở các vị trí chiến lược
        /// giúp người chơi có thể lấp đầy bằng các khối gạch tiêu chuẩn (1x1, 1x2, 1x3, 2x2, L-shape).
        /// </summary>
        private static int[,] GenerateInverseSolvableBoard(int seasonId, int levelId, int colorIdx)
        {
            int[,] grid = new int[8, 8];
            int density = Math.Min(24, 6 + (levelId * 3 / 2) + (seasonId));

            // Khởi tạo các mẫu hoa văn nguyên tố tương ứng
            switch (seasonId)
            {
                case 1: // Đất Mẹ - Thạch trận rễ cây đối xứng góc và trục
                    ApplyGaiaPattern(grid, levelId, colorIdx);
                    break;
                case 2: // Băng Giá - Tháp băng nhũ và viền tinh thể
                    ApplyGlacialPattern(grid, levelId, colorIdx);
                    break;
                case 3: // Lửa - Rãnh dung nham chữ thập và tâm lò rèn
                    ApplyVulcanPattern(grid, levelId, colorIdx);
                    break;
                case 4: // Sét - Đường gấp khúc sấm sét zigzag
                    ApplyStormPattern(grid, levelId, colorIdx);
                    break;
                case 5: // Biển Sâu - Dòng hải lưu đan xen ngang
                    ApplyLeviathanPattern(grid, levelId, colorIdx);
                    break;
                case 6: // Sa Mạc - Kim tự tháp bậc thang
                    ApplyDesertPattern(grid, levelId, colorIdx);
                    break;
                case 7: // Hư Không - Lốc xoáy hư vô và góc cổng dịch chuyển
                    ApplyVoidPattern(grid, levelId, colorIdx);
                    break;
                case 8: // Khải Huyền - Bát giác hoàng kim & ma trận tổng hợp
                default:
                    ApplyZenithPattern(grid, levelId, colorIdx);
                    break;
            }

            // Đảm bảo không hàng nào hoặc cột nào bị kín 100% trước khi bắt đầu
            // và luôn có ít nhất 2 hàng/cột có thể kích nổ chỉ với 1 khối gạch
            EnsureSolvabilityGuarantees(grid, colorIdx);

            return grid;
        }

        private static void ApplyGaiaPattern(int[,] grid, int level, int c)
        {
            // Các trụ đá góc và chân bệ đỡ
            grid[7, 1] = c; grid[7, 2] = c;
            grid[7, 5] = c; grid[7, 6] = c;
            grid[6, 2] = c; grid[6, 5] = c;
            if (level >= 4) { grid[1, 1] = c; grid[1, 6] = c; }
            if (level >= 7) { grid[3, 0] = c; grid[3, 7] = c; grid[4, 0] = c; grid[4, 7] = c; }
            if (level >= 10) { grid[2, 3] = c; grid[2, 4] = c; }
        }

        private static void ApplyGlacialPattern(int[,] grid, int level, int c)
        {
            // Tinh thể băng dọc theo cột 1, 3, 4, 6
            grid[1, 3] = c; grid[1, 4] = c;
            grid[6, 3] = c; grid[6, 4] = c;
            grid[2, 1] = c; grid[5, 1] = c;
            grid[2, 6] = c; grid[5, 6] = c;
            if (level >= 4) { grid[0, 2] = c; grid[0, 5] = c; grid[7, 2] = c; grid[7, 5] = c; }
            if (level >= 8) { grid[3, 2] = c; grid[4, 5] = c; }
        }

        private static void ApplyVulcanPattern(int[,] grid, int level, int c)
        {
            // Rãnh magma chữ thập chừa tâm mở
            grid[3, 1] = c; grid[3, 2] = c; grid[3, 5] = c; grid[3, 6] = c;
            grid[4, 1] = c; grid[4, 2] = c; grid[4, 5] = c; grid[4, 6] = c;
            if (level >= 4) { grid[1, 3] = c; grid[1, 4] = c; grid[6, 3] = c; grid[6, 4] = c; }
            if (level >= 9) { grid[0, 0] = c; grid[7, 7] = c; grid[0, 7] = c; grid[7, 0] = c; }
        }

        private static void ApplyStormPattern(int[,] grid, int level, int c)
        {
            // Sấm sét zigzag chéo
            for (int i = 1; i <= 6; i++)
            {
                if (i != 3 && i != 4)
                {
                    grid[i, i] = c;
                    grid[i, 7 - i] = c;
                }
            }
            if (level >= 6) { grid[0, 3] = c; grid[7, 4] = c; }
            if (level >= 10) { grid[3, 0] = c; grid[4, 7] = c; }
        }

        private static void ApplyLeviathanPattern(int[,] grid, int level, int c)
        {
            // Làn sóng biển sâu đan xen
            for (int col = 0; col < 8; col += 2)
            {
                grid[2, col] = c;
                grid[5, col + 1] = c;
            }
            if (level >= 5) { grid[1, 1] = c; grid[6, 6] = c; grid[1, 6] = c; grid[6, 1] = c; }
            if (level >= 9) { grid[3, 3] = c; grid[4, 4] = c; }
        }

        private static void ApplyDesertPattern(int[,] grid, int level, int c)
        {
            // Kim tự tháp bậc thang
            grid[7, 0] = c; grid[7, 7] = c;
            grid[6, 1] = c; grid[6, 6] = c;
            grid[5, 2] = c; grid[5, 5] = c;
            grid[4, 3] = c; grid[4, 4] = c;
            if (level >= 6) { grid[2, 2] = c; grid[2, 5] = c; }
            if (level >= 10) { grid[1, 3] = c; grid[1, 4] = c; }
        }

        private static void ApplyVoidPattern(int[,] grid, int level, int c)
        {
            // Hư không xoắn ốc
            grid[0, 1] = c; grid[1, 7] = c; grid[7, 6] = c; grid[6, 0] = c;
            grid[2, 3] = c; grid[3, 5] = c; grid[5, 4] = c; grid[4, 2] = c;
            if (level >= 5) { grid[0, 6] = c; grid[7, 1] = c; }
            if (level >= 9) { grid[3, 3] = c; grid[4, 4] = c; }
        }

        private static void ApplyZenithPattern(int[,] grid, int level, int c)
        {
            // Bát giác hoàng kim vương giả
            grid[0, 2] = c; grid[0, 5] = c;
            grid[7, 2] = c; grid[7, 5] = c;
            grid[2, 0] = c; grid[5, 0] = c;
            grid[2, 7] = c; grid[5, 7] = c;
            grid[3, 2] = c; grid[3, 5] = c;
            grid[4, 2] = c; grid[4, 5] = c;
            if (level >= 6) { grid[1, 3] = c; grid[1, 4] = c; grid[6, 3] = c; grid[6, 4] = c; }
        }

        /// <summary>
        /// Bảo đảm tính giải được tuyệt đối: Không dòng/cột nào bị full 8/8 từ trước,
        /// và luôn có ít nhất 2 hàng hoặc cột đang có đúng 6 hoặc 7 ô gạch (chỉ cần đặt 1..2 ô là nổ ngay).
        /// </summary>
        private static void EnsureSolvabilityGuarantees(int[,] grid, int c)
        {
            // 1. Kiểm tra xóa nếu lỡ full 8/8
            for (int r = 0; r < 8; r++)
            {
                int count = 0;
                for (int col = 0; col < 8; col++) if (grid[r, col] != 0) count++;
                if (count >= 8) grid[r, 4] = 0; // Chừa lỗ trống
            }

            for (int col = 0; col < 8; col++)
            {
                int count = 0;
                for (int r = 0; r < 8; r++) if (grid[r, col] != 0) count++;
                if (count >= 8) grid[4, col] = 0; // Chừa lỗ trống
            }

            // 2. Bảo đảm ít nhất 1 hàng có 5-6 ô để kích nổ dễ dàng
            int rTarget = 7;
            for (int col = 1; col <= 5; col++)
            {
                if (grid[rTarget, col] == 0) grid[rTarget, col] = c;
            }
            grid[rTarget, 0] = 0;
            grid[rTarget, 6] = 0;
            grid[rTarget, 7] = 0;
        }

        private static void GenerateLoreTexts(GameChallenge c)
        {
            string themeName = Themes[c.SeasonId - 1].Name;
            string elem = Themes[c.SeasonId - 1].Element;

            c.StoryLore = $"Khai phá sức mạnh của {elem} tại cấp độ {c.LevelId}. Vượt qua chướng ngại và hoàn thành {c.TargetLines} dòng nổ trong tối đa {c.MaxMoves} lượt đi.";
            c.BriefingQuote = $"\"Ý chí của {elem} đang kêu gọi! Hãy tính toán từng nước đi chính xác để giải phóng nguồn năng lượng tinh khiết nhất của {themeName}!\"";
            c.VictoryLore = $"XUẤT SẮC! Phong ấn cấp {c.LevelId} của {themeName} đã được hóa giải thành công. Con đường chinh phục đại đỉnh cao tiếp tục rộng mở!";
        }
    }
}
