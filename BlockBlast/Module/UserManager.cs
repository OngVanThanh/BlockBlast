using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace BlockBlast.Module
{
    public class UserProfile
    {
        public string Username { get; set; } = "Player 1";
        public int BestScore { get; set; } = 0;
        public int StoryChapterReached { get; set; } = 1;
        public string LastPlayedDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        // Nhật Ký Thành Tích Sự Nghiệp (Lifetime Career Stats)
        public long LifetimeScore { get; set; } = 0;
        public int TotalLinesCleared { get; set; } = 0;
        public int MaxComboRecord { get; set; } = 0;
        public int TotalChallengesWon { get; set; } = 0;
        public int HighestSeasonUnlocked { get; set; } = 1;
        public int HighestLevelUnlocked { get; set; } = 1;

        // Danh sách các màn và mùa đã hoàn thành
        public List<string> CompletedLevels { get; set; } = new List<string>();
        public List<int> CompletedSeasons { get; set; } = new List<int>();

        // Danh sách các huân chương danh giá đã đạt
        public List<string> UnlockedMedals { get; set; } = new List<string>();
    }

    public static class UserManager
    {
        private static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlockBlast", "users_data.json");

        public static List<UserProfile> Users { get; private set; } = new List<UserProfile>();
        public static UserProfile CurrentUser { get; set; }

        static UserManager()
        {
            LoadUsers();
        }

        public static void LoadUsers()
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
                    Users = serializer.Deserialize<List<UserProfile>>(json) ?? new List<UserProfile>();
                }
            }
            catch
            {
                Users = new List<UserProfile>();
            }

            if (Users == null || Users.Count == 0)
            {
                Users = new List<UserProfile>
                {
                    new UserProfile { Username = "Player 1", BestScore = 0 }
                };
            }

            // Bảo đảm các collection không bao giờ null
            foreach (var u in Users)
            {
                if (u.CompletedLevels == null) u.CompletedLevels = new List<string>();
                if (u.CompletedSeasons == null) u.CompletedSeasons = new List<int>();
                if (u.UnlockedMedals == null) u.UnlockedMedals = new List<string>();
                if (u.HighestSeasonUnlocked < 1) u.HighestSeasonUnlocked = 1;
                if (u.HighestLevelUnlocked < 1) u.HighestLevelUnlocked = 1;
            }

            CurrentUser = Users[0];
        }

        public static void SaveUsers()
        {
            try
            {
                string dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                string json = serializer.Serialize(Users);
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }

        public static void SwitchUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return;
            username = username.Trim();
            var user = Users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                user = new UserProfile { Username = username };
                Users.Add(user);
            }
            CurrentUser = user;
            SaveUsers();
        }

        public static void UpdateScore(int score)
        {
            if (CurrentUser == null) return;
            CurrentUser.LastPlayedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            if (score > CurrentUser.BestScore)
            {
                CurrentUser.BestScore = score;
            }
            CurrentUser.LifetimeScore += score;
            CheckAndUnlockMedals();
            SaveUsers();
        }

        public static void UpdateStoryChapter(int chapter)
        {
            if (CurrentUser == null) return;
            if (chapter > CurrentUser.StoryChapterReached)
            {
                CurrentUser.StoryChapterReached = chapter;
            }
            SaveUsers();
        }

        /// <summary>
        /// Cập nhật thống kê chi tiết của một ván đấu vừa kết thúc
        /// </summary>
        public static void RecordGameStats(int sessionScore, int linesCleared, int maxCombo)
        {
            if (CurrentUser == null) return;

            CurrentUser.LastPlayedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            if (sessionScore > CurrentUser.BestScore)
            {
                CurrentUser.BestScore = sessionScore;
            }
            CurrentUser.LifetimeScore += sessionScore;
            CurrentUser.TotalLinesCleared += linesCleared;
            if (maxCombo > CurrentUser.MaxComboRecord)
            {
                CurrentUser.MaxComboRecord = maxCombo;
            }

            CheckAndUnlockMedals();
            SaveUsers();
        }

        /// <summary>
        /// Ghi nhận chiến thắng một màn trong mùa thử thách
        /// </summary>
        public static void RecordChallengeVictory(int seasonId, int levelId)
        {
            if (CurrentUser == null) return;

            string key = $"s{seasonId}_l{levelId}";
            if (!CurrentUser.CompletedLevels.Contains(key))
            {
                CurrentUser.CompletedLevels.Add(key);
                CurrentUser.TotalChallengesWon++;
            }

            // Mở khóa màn tiếp theo
            if (seasonId == CurrentUser.HighestSeasonUnlocked)
            {
                if (levelId >= CurrentUser.HighestLevelUnlocked && levelId < SeasonManager.LEVELS_PER_SEASON)
                {
                    CurrentUser.HighestLevelUnlocked = levelId + 1;
                }
                else if (levelId == SeasonManager.LEVELS_PER_SEASON)
                {
                    // Hoàn thành cả mùa!
                    if (!CurrentUser.CompletedSeasons.Contains(seasonId))
                    {
                        CurrentUser.CompletedSeasons.Add(seasonId);
                    }

                    if (CurrentUser.HighestSeasonUnlocked < SeasonManager.TOTAL_SEASONS)
                    {
                        CurrentUser.HighestSeasonUnlocked++;
                        CurrentUser.HighestLevelUnlocked = 1;
                    }
                }
            }

            CheckAndUnlockMedals();
            SaveUsers();
        }

        public static bool IsLevelCompleted(int seasonId, int levelId)
        {
            if (CurrentUser == null) return false;
            return CurrentUser.CompletedLevels.Contains($"s{seasonId}_l{levelId}");
        }

        public static bool IsLevelUnlocked(int seasonId, int levelId)
        {
            if (CurrentUser == null) return seasonId == 1 && levelId == 1;

            // Mùa thấp hơn mùa cao nhất đã mở thì mọi màn đều mở
            if (seasonId < CurrentUser.HighestSeasonUnlocked) return true;

            // Mùa hiện tại: các màn <= HighestLevelUnlocked đều mở
            if (seasonId == CurrentUser.HighestSeasonUnlocked)
            {
                return levelId <= CurrentUser.HighestLevelUnlocked;
            }

            // Mùa cao hơn: bị khóa
            return false;
        }

        public static bool IsSeasonCompleted(int seasonId)
        {
            if (CurrentUser == null) return false;
            return CurrentUser.CompletedSeasons.Contains(seasonId);
        }

        public static int GetSeasonProgress(int seasonId)
        {
            if (CurrentUser == null) return 0;
            int count = 0;
            for (int l = 1; l <= SeasonManager.LEVELS_PER_SEASON; l++)
            {
                if (IsLevelCompleted(seasonId, l)) count++;
            }
            return count;
        }

        /// <summary>
        /// Tự động kiểm tra và mở khóa các huân chương danh giá
        /// </summary>
        public static void CheckAndUnlockMedals()
        {
            if (CurrentUser == null) return;

            // Huân chương 8 mùa
            for (int s = 1; s <= SeasonManager.TOTAL_SEASONS; s++)
            {
                string medalKey = $"season_{s}_medal";
                if (GetSeasonProgress(s) >= SeasonManager.LEVELS_PER_SEASON && !CurrentUser.UnlockedMedals.Contains(medalKey))
                {
                    CurrentUser.UnlockedMedals.Add(medalKey);
                }
            }

            // 4 Huân chương cao thủ
            if (CurrentUser.MaxComboRecord >= 4 && !CurrentUser.UnlockedMedals.Contains("medal_combo_4"))
            {
                CurrentUser.UnlockedMedals.Add("medal_combo_4");
            }
            if (CurrentUser.BestScore >= 1000 && !CurrentUser.UnlockedMedals.Contains("medal_score_1000"))
            {
                CurrentUser.UnlockedMedals.Add("medal_score_1000");
            }
            if (CurrentUser.TotalLinesCleared >= 100 && !CurrentUser.UnlockedMedals.Contains("medal_lines_100"))
            {
                CurrentUser.UnlockedMedals.Add("medal_lines_100");
            }
            if (CurrentUser.TotalChallengesWon >= 30 && !CurrentUser.UnlockedMedals.Contains("medal_architect_30"))
            {
                CurrentUser.UnlockedMedals.Add("medal_architect_30");
            }
        }
    }
}
