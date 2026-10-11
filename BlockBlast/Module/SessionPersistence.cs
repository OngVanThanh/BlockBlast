using System;
using System.IO;
using System.Web.Script.Serialization;

namespace BlockBlast.Module
{
    public class DockPieceStateDto
    {
        public int[][] Shape { get; set; }
        public int ColorIndex { get; set; }
        public bool IsPlaced { get; set; }
    }

    public class GameSessionDto
    {
        public bool IsActive { get; set; } = false;
        public string GameMode { get; set; } = "Endless"; // "Endless" | "Challenge"
        public int Score { get; set; } = 0;
        public int ComboCount { get; set; } = 0;
        public int[][] BoardGrid { get; set; } // 8x8
        public DockPieceStateDto[] Pieces { get; set; } // 3 pieces
        public int SeasonId { get; set; } = 1;
        public int LevelId { get; set; } = 1;
        public int TargetLines { get; set; } = 0;
        public int LinesCleared { get; set; } = 0;
        public int MovesLeft { get; set; } = 0;
        public string SaveTime { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }

    /// <summary>
    /// Quản lý Lưu Trữ & Khôi Phục Trạng Thái Ván Đấu Dở Dang (Session Persistence).
    /// Ghi nhận trạng thái vào %AppData%/BlockBlast/game_session.json khi người chơi
    /// tạm thoát về Menu hoặc tắt chương trình.
    /// </summary>
    public static class SessionPersistence
    {
        private static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlockBlast", "game_session.json");

        public static bool HasSavedSession()
        {
            try
            {
                if (!File.Exists(FilePath)) return false;
                var session = LoadSession();
                return session != null && session.IsActive && session.BoardGrid != null;
            }
            catch
            {
                return false;
            }
        }

        public static GameSessionDto LoadSession()
        {
            try
            {
                if (!File.Exists(FilePath)) return null;
                string json = File.ReadAllText(FilePath);
                var serializer = new JavaScriptSerializer();
                return serializer.Deserialize<GameSessionDto>(json);
            }
            catch
            {
                return null;
            }
        }

        public static void SaveSession(GameSessionDto session)
        {
            try
            {
                string dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(session);
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }

        public static void ClearSession()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
            catch { }
        }
    }
}
