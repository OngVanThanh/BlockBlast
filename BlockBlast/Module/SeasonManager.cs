using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;

namespace BlockBlast.Module
{
    public class SeasonLevelDto
    {
        public int LevelId { get; set; }
        public int TargetLines { get; set; }
        public int MaxMoves { get; set; }
        public int[][] InitialBoard { get; set; } // 8x8 matrix
        public string BriefingQuote { get; set; }
        public string VictoryLore { get; set; }
    }

    public class SeasonDto
    {
        public int SeasonId { get; set; }
        public string Title { get; set; }
        public string ElementName { get; set; }
        public string Icon { get; set; }
        public string Subtitle { get; set; }
        public string LoreDescription { get; set; }
        public string AccentColorHex { get; set; }
        public int ThemeColorIndex { get; set; }
        public List<SeasonLevelDto> Levels { get; set; } = new List<SeasonLevelDto>();
    }

    /// <summary>
    /// Quản lý Kho Lưu Trữ 8 Đại Mùa Sử Thi (8 Epic Lore Seasons Repository).
    /// Đọc/Ghi cấu hình từ file %AppData%/BlockBlast/seasons_data.json.
    /// Tự động sinh kho 96 màn chơi sử thi đảm bảo tính giải được 100% khi chưa có file.
    /// </summary>
    public static class SeasonManager
    {
        public const int TOTAL_SEASONS = 8;
        public const int LEVELS_PER_SEASON = 12;

        private static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlockBlast", "seasons_data.json");

        public static List<SeasonDto> Seasons { get; private set; } = new List<SeasonDto>();

        static SeasonManager()
        {
            LoadSeasons();
        }

        public static void LoadSeasons()
        {
            try
            {
                string dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    var serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;
                    Seasons = serializer.Deserialize<List<SeasonDto>>(json);
                }
            }
            catch
            {
                Seasons = null;
            }

            if (Seasons == null || Seasons.Count < TOTAL_SEASONS)
            {
                GenerateDefaultSeasons();
                SaveSeasons();
            }
        }

        public static void SaveSeasons()
        {
            try
            {
                string dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                string json = serializer.Serialize(Seasons);
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }

        /// <summary>
        /// Tự động sinh kho lưu trữ 8 Mùa Sử Thi và 96 Màn chơi theo thuật toán Inverse Solvable
        /// </summary>
        public static void GenerateDefaultSeasons()
        {
            Seasons = new List<SeasonDto>();

            for (int s = 1; s <= TOTAL_SEASONS; s++)
            {
                var theme = InverseChallengeGenerator.GetTheme(s);
                var sDto = new SeasonDto
                {
                    SeasonId = s,
                    Title = $"Mùa {s}: {theme.Name}",
                    ElementName = theme.Element,
                    Icon = theme.Icon,
                    Subtitle = theme.Subtitle,
                    LoreDescription = GetSeasonLore(s),
                    AccentColorHex = ColorTranslator.ToHtml(theme.AccentColor),
                    ThemeColorIndex = theme.ThemeColorIndex,
                    Levels = new List<SeasonLevelDto>()
                };

                for (int l = 1; l <= LEVELS_PER_SEASON; l++)
                {
                    var challenge = InverseChallengeGenerator.GenerateChallenge(s, l);
                    int[][] boardJagged = new int[8][];
                    for (int r = 0; r < 8; r++)
                    {
                        boardJagged[r] = new int[8];
                        for (int c = 0; c < 8; c++)
                        {
                            boardJagged[r][c] = challenge.InitialBoard[r, c];
                        }
                    }

                    sDto.Levels.Add(new SeasonLevelDto
                    {
                        LevelId = l,
                        TargetLines = challenge.TargetLines,
                        MaxMoves = challenge.MaxMoves,
                        InitialBoard = boardJagged,
                        BriefingQuote = challenge.BriefingQuote,
                        VictoryLore = challenge.VictoryLore
                    });
                }

                Seasons.Add(sDto);
            }
        }

        public static SeasonDto GetSeason(int seasonId)
        {
            if (seasonId < 1) seasonId = 1;
            if (seasonId > Seasons.Count) seasonId = Seasons.Count;
            return Seasons[seasonId - 1];
        }

        /// <summary>
        /// Chuyển đổi dữ liệu lưu trữ thành GameChallenge thực thi trong ván đấu
        /// </summary>
        public static GameChallenge GetChallenge(int seasonId, int levelId)
        {
            if (seasonId < 1) seasonId = 1;
            if (seasonId > TOTAL_SEASONS) seasonId = TOTAL_SEASONS;
            if (levelId < 1) levelId = 1;
            if (levelId > LEVELS_PER_SEASON) levelId = LEVELS_PER_SEASON;

            var sDto = GetSeason(seasonId);
            var lDto = sDto.Levels[levelId - 1];

            int[,] board2D = new int[8, 8];
            if (lDto.InitialBoard != null)
            {
                for (int r = 0; r < Math.Min(8, lDto.InitialBoard.Length); r++)
                {
                    if (lDto.InitialBoard[r] != null)
                    {
                        for (int c = 0; c < Math.Min(8, lDto.InitialBoard[r].Length); c++)
                        {
                            board2D[r, c] = lDto.InitialBoard[r][c];
                        }
                    }
                }
            }

            Color accent = ColorTranslator.FromHtml(sDto.AccentColorHex);

            return new GameChallenge
            {
                SeasonId = seasonId,
                LevelId = levelId,
                SeasonTitle = sDto.Title,
                LevelTitle = $"Màn {levelId}: {sDto.ElementName} Khởi Thủy",
                ElementName = sDto.ElementName,
                Icon = sDto.Icon,
                Subtitle = sDto.Subtitle,
                StoryLore = sDto.LoreDescription,
                BriefingQuote = lDto.BriefingQuote,
                VictoryLore = lDto.VictoryLore,
                BlastFloatingText = $"{sDto.Icon} {sDto.ElementName.ToUpper()} THỨC TỈNH!",
                ThemeColorIndex = sDto.ThemeColorIndex,
                AccentColor = accent,
                TargetLines = lDto.TargetLines,
                LinesCleared = 0,
                MaxMoves = lDto.MaxMoves,
                MovesLeft = lDto.MaxMoves,
                InitialBoard = board2D
            };
        }

        private static string GetSeasonLore(int s)
        {
            switch (s)
            {
                case 1: return "Hàn gắn các phiến đá cổ đại và đánh thức dòng sinh khí lục địa ngầm bừng tỉnh sau giấc ngủ ngàn năm.";
                case 2: return "Phá vỡ lớp băng vĩnh cửu ngàn năm đang giam cầm dòng suối nguồn nguyên sơ của thế giới.";
                case 3: return "Thuần hóa dòng dung nham cuộn trào để kích hoạt lò rèn nguyên tố tối thượng.";
                case 4: return "Hội tụ năng lượng sét nguyên thủy trên đỉnh tháp cuồng phong để kích hoạt mạch điện từ trường thế giới.";
                case 5: return "Lặn sâu vào rạn san hô cổ đại dưới đáy vực thẳm ngàn trượng, đánh thức Thủy Tề Leviathan say ngủ.";
                case 6: return "Vượt qua những cơn bão cát cuồng phong sa mạc, khai phá bí mật lăng mộ vàng bị vùi lấp.";
                case 7: return "Đối mặt với các vết nứt không gian kỳ dị và lỗ đen thứ nguyên tại ranh giới thực tại.";
                case 8: return "Bước lên đỉnh cao ngai vàng vũ trụ hội tụ 8 đại nguyên tố, viết nên thời khắc Khải Huyền tối thượng!";
                default: return "Hành trình sử thi phiêu lưu qua các chiều không gian vô tận.";
            }
        }
    }
}
