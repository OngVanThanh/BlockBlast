using System.Collections.Generic;

namespace BlockBlast.Module
{
    public class StoryChapter
    {
        public int ChapterNumber { get; set; }
        public string Title { get; set; }
        public string ElementName { get; set; }
        public string Icon { get; set; }
        public string Subtitle { get; set; }
        public string StoryLore { get; set; }
        public string BriefingQuote { get; set; }
        public string VictoryLore { get; set; }
        public string BlastFloatingText { get; set; }
        public int TargetLines { get; set; }
        public int MaxMoves { get; set; }
        public int[,] InitialBoard { get; set; }
        public int ThemeColorIndex { get; set; }
        public System.Drawing.Color AccentColor { get; set; }
    }

    public static class StoryManager
    {
        public static List<StoryChapter> Chapters { get; private set; } = new List<StoryChapter>();

        static StoryManager()
        {
            InitChapters();
        }

        private static void InitChapters()
        {
            // ==========================================
            // CHƯƠNG 1: ĐẤT MẸ (Earth Core)
            // ==========================================
            int[,] b1 = new int[8, 8];
            b1[7, 1] = 2; b1[7, 2] = 2; b1[7, 5] = 2; b1[7, 6] = 2;
            b1[6, 2] = 2; b1[6, 5] = 2;

            Chapters.Add(new StoryChapter
            {
                ChapterNumber = 1,
                Title = "Chương 1: Lõi Đất Mẹ",
                ElementName = "Đất Mẹ",
                Icon = "🌿",
                Subtitle = "Vùng Thạch Trận Cổ Đại (Ancient Monolith)",
                StoryLore = "Hàn gắn các phiến đá cổ đại và đánh thức dòng sinh khí lục địa ngầm.",
                BriefingQuote = "\"Kẻ thức tỉnh thân mến, các đại thạch trận đang vỡ vụn dưới lòng đất sâu. Hãy dùng các khối nguyên tố để khép kín mạch địa chấn và giải phóng nguồn sống cho muôn loài!\"",
                VictoryLore = "Dòng sinh khí ngầm đã bừng tỉnh! Cây cối đâm chồi, mặt đất rung chuyển mở ra lối mòn tiến về Thung lũng Băng Vĩnh Cửu...",
                BlastFloatingText = "🌿 ĐẤT MẸ THỨC TỈNH!",
                TargetLines = 4,
                MaxMoves = 15,
                ThemeColorIndex = 2,
                AccentColor = System.Drawing.Color.FromArgb(46, 204, 113),
                InitialBoard = b1
            });

            // ==========================================
            // CHƯƠNG 2: BĂNG GIÁ (Frost Core)
            // ==========================================
            int[,] b2 = new int[8, 8];
            for (int c = 0; c < 8; c++)
            {
                if (c == 0 || c == 3 || c == 4 || c == 7)
                {
                    b2[1, c] = 5;
                    b2[6, c] = 5;
                }
            }

            Chapters.Add(new StoryChapter
            {
                ChapterNumber = 2,
                Title = "Chương 2: Lõi Băng Giá",
                ElementName = "Băng Giá",
                Icon = "❄️",
                Subtitle = "Thung Lũng Băng Vĩnh Cửu (Frozen Vale)",
                StoryLore = "Phá vỡ lớp băng vĩnh cửu để giải phóng dòng suối nguồn nguyên sơ.",
                BriefingQuote = "\"Băng tuyết ngàn năm đang bóp nghẹt dòng chảy thời gian. Hãy tạo nên những vụ nổ dây chuyền để làm tan chảy phong ấn băng hà vĩnh cửu!\"",
                VictoryLore = "Lớp băng vĩnh cửu đã nứt toác! Dòng suối nguyên sơ tuôn trào đưa nhiệt lượng khổng lồ bốc lên từ Miệng Núi Lửa Hỏa Diệm...",
                BlastFloatingText = "❄️ PHÁ VỠ BĂNG GIÁ!",
                TargetLines = 6,
                MaxMoves = 18,
                ThemeColorIndex = 7, // Xanh ngọc Cyan
                AccentColor = System.Drawing.Color.FromArgb(0, 210, 211),
                InitialBoard = b2
            });

            // ==========================================
            // CHƯƠNG 3: LỬA ĐỊA NGỤC (Fire Core)
            // ==========================================
            int[,] b3 = new int[8, 8];
            for (int r = 2; r <= 5; r++)
            {
                b3[r, 0] = 1;
                b3[r, 7] = 1;
            }
            b3[3, 3] = 1; b3[4, 4] = 1;

            Chapters.Add(new StoryChapter
            {
                ChapterNumber = 3,
                Title = "Chương 3: Lõi Lửa Địa Ngục",
                ElementName = "Hỏa Diệm",
                Icon = "🔥",
                Subtitle = "Lò Rèn Dung Nham (Molten Forge)",
                StoryLore = "Thuần hóa dòng dung nham cuộn trào để kích hoạt lò rèn nguyên tố tối thượng.",
                BriefingQuote = "\"Sức nóng địa ngục đang chực chờ nuốt chửng tất cả. Chỉ có sự bình tĩnh và chiến thuật dứt khoát mới có thể thuần hóa ngọn lửa cuồng nộ này!\"",
                VictoryLore = "Ngọn lửa cuồng nộ đã được quy phục vào khuôn đúc! Nhiệt lượng tối thượng bốc lên tầng khí quyển, thu hút cơn cuồng phong sét đánh kinh hoàng...",
                BlastFloatingText = "🔥 HỎA DIỆM BÙNG NỔ!",
                TargetLines = 8,
                MaxMoves = 20,
                ThemeColorIndex = 1,
                AccentColor = System.Drawing.Color.FromArgb(255, 71, 87),
                InitialBoard = b3
            });

            // ==========================================
            // CHƯƠNG 4: SẤM SÉT (Thunder Core)
            // ==========================================
            int[,] b4 = new int[8, 8];
            for (int i = 1; i < 7; i++)
            {
                if (i != 3 && i != 4)
                {
                    b4[i, i] = 3;
                    b4[i, 7 - i] = 3;
                }
            }

            Chapters.Add(new StoryChapter
            {
                ChapterNumber = 4,
                Title = "Chương 4: Lõi Sấm Sét",
                ElementName = "Thiên Lôi",
                Icon = "⚡",
                Subtitle = "Đỉnh Tháp Cuồng Phong (Storm Peak)",
                StoryLore = "Hội tụ năng lượng sét nguyên thủy để khôi phục hoàn toàn Trật Tự Thế Giới!",
                BriefingQuote = "\"Đây là bước thử thách cuối cùng trước ngai vàng của Đấng Kiến Tạo! Hãy điều phối các tia sét nguyên thủy để kích hoạt Trật Tự Thế Giới tối thượng!\"",
                VictoryLore = "TRẬT TỰ THẾ GIỚI ĐÃ ĐƯỢC PHỤC HỒI HOÀN TOÀN! Toàn bộ 4 Lõi Nguyên Tố đã liên kết bền vững. Bạn chính là Bậc Thầy Kiến Tạo Vĩ Đại!",
                BlastFloatingText = "⚡ THIÊN LÔI QUY TỤ!",
                TargetLines = 10,
                MaxMoves = 22,
                ThemeColorIndex = 4,
                AccentColor = System.Drawing.Color.FromArgb(255, 165, 2),
                InitialBoard = b4
            });
        }

        public static StoryChapter GetChapter(int chapterNumber)
        {
            if (chapterNumber < 1) chapterNumber = 1;
            if (chapterNumber > Chapters.Count) chapterNumber = Chapters.Count;
            return Chapters[chapterNumber - 1];
        }
    }
}
