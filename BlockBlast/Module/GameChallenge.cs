using System;
using System.Drawing;

namespace BlockBlast.Module
{
    /// <summary>
    /// Đóng gói độc lập toàn bộ dữ liệu và logic luật chơi của một màn Thử Thách Cốt Truyện / Mùa Sử Thi.
    /// Tách biệt hoàn toàn với GameBoard (chỉ quản lý ô cờ và tính điểm vật lý).
    /// </summary>
    public class GameChallenge
    {
        // Thông tin định danh & cốt truyện
        public int SeasonId { get; set; } = 1;
        public int LevelId { get; set; } = 1;
        public string SeasonTitle { get; set; } = "Mùa 1: Lục Địa Đất Mẹ";
        public string LevelTitle { get; set; } = "Màn 1: Khởi Nguyên Sinh Khí";
        public string ElementName { get; set; } = "Đất Mẹ";
        public string Icon { get; set; } = "🌿";
        public string Subtitle { get; set; } = "Vùng Thạch Trận Cổ Đại";
        public string StoryLore { get; set; } = "Hàn gắn các phiến đá cổ đại và đánh thức dòng sinh khí lục địa ngầm.";
        public string BriefingQuote { get; set; } = "\"Hãy kết nối các khối cờ nguyên tố để phục hồi địa chấn!\"";
        public string VictoryLore { get; set; } = "Dòng sinh khí ngầm đã bừng tỉnh! Lối mòn mới đã được khai thông.";
        public string BlastFloatingText { get; set; } = "🌿 ĐẤT MẸ THỨC TỈNH!";

        // Thuộc tính hình ảnh & Theme
        public int ThemeColorIndex { get; set; } = 2; // Màu gạch chướng ngại vật
        public Color AccentColor { get; set; } = Color.FromArgb(46, 204, 113); // Màu chủ đạo của mùa

        // Luật chơi & Điều kiện hoàn thành
        public int TargetLines { get; set; } = 4;
        public int LinesCleared { get; set; } = 0;
        public int MaxMoves { get; set; } = 15;
        public int MovesLeft { get; set; } = 15;

        // Ma trận chướng ngại vật ban đầu (8x8)
        public int[,] InitialBoard { get; set; } = new int[8, 8];

        public GameChallenge()
        {
        }

        /// <summary>
        /// Khởi tạo thử thách mới hoặc thiết lập lại trạng thái ván đấu
        /// </summary>
        public void ResetProgress()
        {
            LinesCleared = 0;
            MovesLeft = MaxMoves;
        }

        /// <summary>
        /// Ghi nhận 1 nước đi hợp lệ đã đặt gạch
        /// </summary>
        /// <returns>True nếu còn nước đi hợp lệ hoặc đã chiến thắng</returns>
        public bool RecordMove()
        {
            if (MovesLeft > 0)
            {
                MovesLeft--;
            }
            return MovesLeft > 0 || IsTargetReached;
        }

        /// <summary>
        /// Ghi nhận số dòng vừa nổ trong nước đi
        /// </summary>
        public void AddClearedLines(int count)
        {
            if (count > 0)
            {
                LinesCleared += count;
            }
        }

        /// <summary>
        /// Đã hoàn thành mục tiêu số dòng hay chưa
        /// </summary>
        public bool IsTargetReached => LinesCleared >= TargetLines;

        /// <summary>
        /// Đã hết lượt đi nhưng chưa đạt mục tiêu
        /// </summary>
        public bool IsOutOfMoves => MovesLeft <= 0 && !IsTargetReached;

        /// <summary>
        /// Tỷ lệ hoàn thành mục tiêu (0.0 -> 1.0)
        /// </summary>
        public float ProgressRatio
        {
            get
            {
                if (TargetLines <= 0) return 1.0f;
                return Math.Min(1.0f, (float)LinesCleared / TargetLines);
            }
        }

        /// <summary>
        /// Clone sâu để bảo toàn dữ liệu khi chơi lại ván
        /// </summary>
        public GameChallenge Clone()
        {
            var copy = new GameChallenge
            {
                SeasonId = this.SeasonId,
                LevelId = this.LevelId,
                SeasonTitle = this.SeasonTitle,
                LevelTitle = this.LevelTitle,
                ElementName = this.ElementName,
                Icon = this.Icon,
                Subtitle = this.Subtitle,
                StoryLore = this.StoryLore,
                BriefingQuote = this.BriefingQuote,
                VictoryLore = this.VictoryLore,
                BlastFloatingText = this.BlastFloatingText,
                ThemeColorIndex = this.ThemeColorIndex,
                AccentColor = this.AccentColor,
                TargetLines = this.TargetLines,
                LinesCleared = this.LinesCleared,
                MaxMoves = this.MaxMoves,
                MovesLeft = this.MovesLeft,
                InitialBoard = new int[8, 8]
            };

            if (this.InitialBoard != null)
            {
                for (int r = 0; r < 8; r++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        copy.InitialBoard[r, c] = this.InitialBoard[r, c];
                    }
                }
            }

            return copy;
        }
    }
}
