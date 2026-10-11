using BlockBlast.Module;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace BlockBlast
{
    public partial class fGamePlay
    {
        // ================================================================
        //  OVERLAY CONTROLS FOR MENU & MODALS
        // ================================================================
        private Panel pnlMainMenu;
        private Panel pnlLeaderboardModal; // Achievement Journal (2 Tabs)
        private Panel pnlAboutModal;
        private Panel pnlStorySelectModal; // Season Carousel & Level Grid (8 Seasons x 12 Levels)
        private Panel pnlChangeUserModal;
        private Panel pnlStoryBriefingModal;
        private Panel pnlStoryVictoryModal;
        private Panel pnlStoryDefeatModal;

        private Button btnResumeGame;
        private Button btnHeaderSettings;
        private Panel pnlSettingsModal;
        private Label lblSettingsStatusTitle;
        private Label lblSettingsStatusDetail;
        private Button btnSettingsAudio;
        private Button btnPlayEndless;
        private Button btnPlayStory;
        private Button btnLeaderboard;
        private Button btnAbout;
        private Button btnExitMenu;
        private Label lblUserCardName;
        private Label lblUserCardStats;
        private TextBox txtNewUserName;
        private ComboBox cboExistingUsers;

        // --- Season Carousel & Level Grid State ---
        private int currentCarouselSeason = 1;
        private Label lblCarouselSeasonTitle;
        private Label lblCarouselSeasonSubtitle;
        private Label lblCarouselSeasonLore;
        private Label lblCarouselProgress;
        private FlowLayoutPanel flpLevelGrid;
        private Button btnCarouselPrev;
        private Button btnCarouselNext;

        // --- 2-Tab Achievement Journal Controls ---
        private Button btnTabMedals;
        private Button btnTabStats;
        private Panel pnlTabMedals;
        private Panel pnlTabStats;
        private FlowLayoutPanel flpMedalsList;
        private Panel pnlCareerStatsContent;

        // --- Briefing Modal Controls ---
        private Label lblBriefingTitle;
        private Label lblBriefingSubtitle;
        private RichTextBox rtbBriefingQuote;
        private Label lblBriefingTarget;
        private Label lblBriefingMoves;
        private Button btnBriefingStart;
        private int currentBriefingSeason = 1;
        private int currentBriefingLevel = 1;

        // --- Victory Modal Controls ---
        private Label lblVictoryTitle;
        private Label lblVictorySubtitle;
        private RichTextBox rtbVictoryLore;
        private Label lblVictoryStats;
        private Button btnVictoryNext;
        private int currentVictorySeason = 1;
        private int currentVictoryLevel = 1;

        // --- Defeat Modal Controls ---
        private Label lblDefeatTitle;
        private Label lblDefeatReason;
        private RichTextBox rtbDefeatAdvice;
        private Button btnDefeatRetry;
        private int currentDefeatSeason = 1;
        private int currentDefeatLevel = 1;

        public void InitializeMenuAndModals()
        {
            try
            {
                // 1. Thêm nút [⚙️ CÀI ĐẶT] duy nhất vào thanh pnlHeader
                btnHeaderSettings = new Button
                {
                    Text = "⚙️ CÀI ĐẶT",
                    Size = new Size(140, 46),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    BackColor = Color.FromArgb(38, 43, 64),
                    ForeColor = Color.FromArgb(255, 196, 77),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnHeaderSettings.FlatAppearance.BorderSize = 0;
                btnHeaderSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(52, 59, 88);
                btnHeaderSettings.Click += (s, e) =>
                {
                    CancelDrag();
                    ShowSettingsModal();
                };
                pnlHeader.Controls.Add(btnHeaderSettings);
                btnHeaderSettings.SizeChanged += (s, e) => ApplyRoundedRegion(btnHeaderSettings, 12);
                ApplyRoundedRegion(btnHeaderSettings, 12);

                // 2. Xây dựng Main Menu Overlay
                BuildMainMenuOverlay();

                // 2b. Xây dựng Modal Cài Đặt & Tạm Dừng trong game
                BuildSettingsModal();

                // 3. Xây dựng Modal Nhật Ký Thành Tích & Huân Chương 2 Tab
                BuildAchievementJournalModal();

                // 4. Xây dựng Modal Giới Thiệu & Hướng Dẫn
                BuildAboutModal();

                // 5. Xây dựng Modal Hub Băng Chuyền Mùa & Lưới Màn Chơi (Season Carousel & Level Grid)
                BuildSeasonHubModal();

                // 6. Xây dựng Modal Đổi Tên Người Chơi
                BuildChangeUserModal();

                // 7. Xây dựng Modal Briefing Giới Thiệu Thử Thách Điện Ảnh
                BuildStoryBriefingModal();

                // 8. Xây dựng Modal Vinh Danh Chiến Thắng
                BuildStoryVictoryModal();

                // 9. Xây dựng Modal Thất Bại & Thử Lại
                BuildStoryDefeatModal();

                // Định vị các modal theo kích thước hiện tại
                LayoutMenuModals();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi khởi tạo Menu & Modals: " + ex.Message);
            }
        }

        // ================================================================
        //  1. MAIN MENU OVERLAY
        // ================================================================
        private void BuildMainMenuOverlay()
        {
            pnlMainMenu = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 19, 28), // Dark Slate phủ kín 100% màn hình
                Visible = false
            };
            pnlMainMenu.MouseDown += (s, e) => { /* Chặn click xuyên thấu */ };

            Panel card = new Panel
            {
                Name = "pnlMenuCenterCard",
                Size = new Size(560, 690),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 22);

            // Tiêu đề game
            Label lblTitle = new Label
            {
                Text = "BLOCK BLAST",
                Font = new Font("Segoe UI", 32f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 196, 77),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(540, 50),
                Location = new Point(10, 16)
            };

            Label lblSubtitle = new Label
            {
                Text = "8 EPIC SEASONS & INVERSE SOLVABLE EDITION",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 130, 160),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(540, 24),
                Location = new Point(10, 68)
            };

            // Khung thông tin người chơi hiện tại (Profile Card)
            Panel pnlUserCard = new Panel
            {
                Size = new Size(480, 72),
                Location = new Point(40, 100),
                BackColor = Color.FromArgb(32, 37, 56)
            };
            ApplyRoundedRegion(pnlUserCard, 14);

            lblUserCardName = new Label
            {
                Text = "👤 Người chơi: Player 1",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(16, 12),
                Size = new Size(330, 24)
            };

            lblUserCardStats = new Label
            {
                Text = "🏆 Kỷ lục: 0  |  🌟 Mùa: 1/8  |  🎯 Màn: 0/96",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(170, 175, 195),
                Location = new Point(16, 38),
                Size = new Size(330, 20)
            };

            Button btnSwitchUser = new Button
            {
                Text = "ĐỔI TÊN",
                Size = new Size(100, 40),
                Location = new Point(365, 16),
                BackColor = Color.FromArgb(48, 54, 80),
                ForeColor = Color.FromArgb(0, 210, 211),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSwitchUser.FlatAppearance.BorderSize = 0;
            btnSwitchUser.Click += (s, e) => ShowChangeUserModal();
            ApplyRoundedRegion(btnSwitchUser, 10);

            pnlUserCard.Controls.Add(lblUserCardName);
            pnlUserCard.Controls.Add(lblUserCardStats);
            pnlUserCard.Controls.Add(btnSwitchUser);

            // Nút [▶ TIẾP TỤC VÁN CHƠI] (hiện nổi bật khi có ván đấu dở dang)
            btnResumeGame = CreateMenuButton("▶  TIẾP TỤC VÁN CHƠI DỞ DANG", Color.FromArgb(16, 172, 132), Color.White, new Point(40, 184), new Size(480, 50));
            btnResumeGame.Visible = false;
            btnResumeGame.Click += (s, e) => ResumeSavedGame();

            // 1. CHƠI TỰ DO (ENDLESS DDA): Cam neon #FF9F43
            btnPlayEndless = CreateMenuButton("⚡  CHƠI TỰ DO (ENDLESS DDA)", Color.FromArgb(255, 159, 67), Color.White, new Point(40, 240), new Size(480, 50));
            btnPlayEndless.Click += (s, e) => StartEndlessMode();

            // 2. 8 ĐẠI MÙA SỬ THI (96 MÀN CHƠI): Xanh dương #54A0FF
            btnPlayStory = CreateMenuButton("🌟  8 ĐẠI MÙA SỬ THI (96 THỬ THÁCH)", Color.FromArgb(84, 160, 255), Color.White, new Point(40, 296), new Size(480, 50));
            btnPlayStory.Click += (s, e) => ShowStorySelectModal();

            // 3. NHẬT KÝ THÀNH TÍCH & BẢO TÀNG HUÂN CHƯƠNG: Xám tím #383E58, chữ vàng #FFC44D
            btnLeaderboard = CreateMenuButton("🎖️  NHẬT KÝ THÀNH TÍCH (2 TAB)", Color.FromArgb(56, 62, 88), Color.FromArgb(255, 196, 77), new Point(40, 352), new Size(480, 50));
            btnLeaderboard.Click += (s, e) => ShowLeaderboardModal();

            // 4. HƯỚNG DẪN & NHÓM TÁC GIẢ: Xám tối #2B3046, chữ trắng
            btnAbout = CreateMenuButton("ℹ️  HƯỚNG DẪN & NHÓM TÁC GIẢ", Color.FromArgb(43, 48, 70), Color.White, new Point(40, 408), new Size(480, 50));
            btnAbout.Click += (s, e) => ShowAboutModal();

            // 5. THOÁT TRÒ CHƠI: Xám viền đỏ #24283A, chữ đỏ #EE5253
            btnExitMenu = CreateMenuButton("❌  THOÁT TRÒ CHƠI", Color.FromArgb(36, 40, 58), Color.FromArgb(238, 82, 83), new Point(40, 464), new Size(480, 50));
            btnExitMenu.Click += (s, e) => btnExit_Click(s, e);

            Label lblFooter = new Label
            {
                Text = "Đồ Án Học Phần MiniGame C# WinForms • Hệ Thống 8 Đại Mùa Sử Thi 100% Solvable",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(90, 95, 120),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(540, 20),
                Location = new Point(10, 642)
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblSubtitle);
            card.Controls.Add(pnlUserCard);
            card.Controls.Add(btnResumeGame);
            card.Controls.Add(btnPlayEndless);
            card.Controls.Add(btnPlayStory);
            card.Controls.Add(btnLeaderboard);
            card.Controls.Add(btnAbout);
            card.Controls.Add(btnExitMenu);
            card.Controls.Add(lblFooter);

            pnlMainMenu.Controls.Add(card);
            this.Controls.Add(pnlMainMenu);
        }

        private void UpdateMainMenuLayout()
        {
            bool hasSaved = SessionPersistence.HasSavedSession();
            if (btnResumeGame != null)
            {
                btnResumeGame.Visible = hasSaved;
                if (hasSaved)
                {
                    var saved = SessionPersistence.LoadSession();
                    string modeStr = (saved.GameMode == "Challenge") ? $"Mùa {saved.SeasonId}-Màn {saved.LevelId}" : "Endless";
                    btnResumeGame.Text = $"▶  TIẾP TỤC VÁN CHƠI ({modeStr} • {saved.Score:#,##0} ĐIỂM)";
                }
            }

            int startY = hasSaved ? 184 : 194;
            int spacing = hasSaved ? 56 : 64;

            if (hasSaved && btnResumeGame != null)
            {
                btnResumeGame.Location = new Point(40, startY);
                startY += spacing;
            }

            if (btnPlayEndless != null) btnPlayEndless.Location = new Point(40, startY);
            if (btnPlayStory != null) btnPlayStory.Location = new Point(40, startY + spacing);
            if (btnLeaderboard != null) btnLeaderboard.Location = new Point(40, startY + spacing * 2);
            if (btnAbout != null) btnAbout.Location = new Point(40, startY + spacing * 3);
            if (btnExitMenu != null) btnExitMenu.Location = new Point(40, startY + spacing * 4);
        }

        private Button CreateMenuButton(string text, Color bg, Color fg, Point loc, Size size)
        {
            Button btn = new Button
            {
                Text = text,
                Location = loc,
                Size = size,
                BackColor = bg,
                ForeColor = fg,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.SizeChanged += (s, e) => ApplyRoundedRegion(btn, 12);
            ApplyRoundedRegion(btn, 12);
            return btn;
        }

        // ================================================================
        //  1B. POPUP MODAL CÀI ĐẶT & TẠM DỪNG (SETTINGS & PAUSE MODAL)
        // ================================================================
        private void BuildSettingsModal()
        {
            pnlSettingsModal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(210, 16, 19, 28),
                Visible = false
            };
            pnlSettingsModal.MouseDown += (s, e) => { /* Chặn click xuyên thấu */ };

            Panel card = new Panel
            {
                Name = "pnlSettingsCard",
                Size = new Size(500, 560),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 20);

            Button btnCloseTop = CreateTopCloseButton(card, () => ResumeFromSettings());

            Label lblTitle = new Label
            {
                Text = "⚙️ CÀI ĐẶT & TẠM DỪNG",
                Font = new Font("Segoe UI", 19f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 196, 77),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(480, 36),
                Location = new Point(10, 18)
            };

            // Badge trạng thái ván đấu hiện tại
            Panel pnlStatusBadge = new Panel
            {
                Size = new Size(420, 54),
                Location = new Point(40, 68),
                BackColor = Color.FromArgb(32, 37, 56)
            };
            ApplyRoundedRegion(pnlStatusBadge, 12);

            lblSettingsStatusTitle = new Label
            {
                Text = "TRẠNG THÁI: ĐANG TẠM DỪNG",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 210, 211),
                Location = new Point(14, 8),
                Size = new Size(392, 20)
            };

            lblSettingsStatusDetail = new Label
            {
                Text = "Điểm số hiện tại: 0  |  Kỷ lục cá nhân: 0",
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(170, 175, 195),
                Location = new Point(14, 28),
                Size = new Size(392, 20)
            };

            pnlStatusBadge.Controls.Add(lblSettingsStatusTitle);
            pnlStatusBadge.Controls.Add(lblSettingsStatusDetail);

            int btnY = 138;
            int btnSpacing = 58;

            // 1. TIẾP TỤC CHƠI
            Button btnResume = CreateMenuButton("▶  TIẾP TỤC CHƠI", Color.FromArgb(46, 204, 113), Color.White, new Point(40, btnY), new Size(420, 48));
            btnResume.Click += (s, e) => ResumeFromSettings();

            // 2. BẬT/TẮT ÂM THANH
            btnSettingsAudio = CreateMenuButton(
                SoundManager.IsMuted ? "🔇  ÂM THANH: ĐANG TẮT" : "🔊  ÂM THANH: ĐANG BẬT",
                Color.FromArgb(48, 54, 80),
                Color.FromArgb(255, 196, 77),
                new Point(40, btnY + btnSpacing),
                new Size(420, 48)
            );
            btnSettingsAudio.Click += (s, e) =>
            {
                SoundManager.IsMuted = !SoundManager.IsMuted;
                SaveAudioSettingsSilently();
                btnSettingsAudio.Text = SoundManager.IsMuted ? "🔇  ÂM THANH: ĐANG TẮT" : "🔊  ÂM THANH: ĐANG BẬT";
                if (btnSound != null) btnSound.Text = SoundManager.IsMuted ? "LOA: TẮT" : "LOA: BẬT";
                if (!SoundManager.IsMuted) SoundManager.PlayPop();
            };

            // 3. CHƠI LẠI VÁN MỚI
            Button btnRestart = CreateMenuButton("🔄  CHƠI LẠI VÁN MỚI", Color.FromArgb(230, 126, 34), Color.White, new Point(40, btnY + btnSpacing * 2), new Size(420, 48));
            btnRestart.Click += (s, e) =>
            {
                pnlSettingsModal.Visible = false;
                gameManager.IsPaused = false;
                RestartGameFromSettings();
            };

            // 4. VỀ MENU CHÍNH
            Button btnToMenu = CreateMenuButton("🏠  VỀ MENU CHÍNH", Color.FromArgb(56, 62, 88), Color.White, new Point(40, btnY + btnSpacing * 3), new Size(420, 48));
            btnToMenu.Click += (s, e) =>
            {
                pnlSettingsModal.Visible = false;
                SaveCurrentSessionSilently();
                ShowMainMenu();
            };

            // 5. THOÁT GAME
            Button btnQuit = CreateMenuButton("❌  THOÁT TRÒ CHƠI", Color.FromArgb(36, 40, 58), Color.FromArgb(238, 82, 83), new Point(40, btnY + btnSpacing * 4), new Size(420, 48));
            btnQuit.Click += (s, e) => btnExit_Click(s, e);

            Label lblFooter = new Label
            {
                Text = "Tiến trình ván đấu luôn được tự động lưu trữ an toàn khi thoát hoặc trở về menu.",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(90, 95, 120),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(480, 24),
                Location = new Point(10, 520)
            };

            card.Controls.Add(btnCloseTop);
            card.Controls.Add(lblTitle);
            card.Controls.Add(pnlStatusBadge);
            card.Controls.Add(btnResume);
            card.Controls.Add(btnSettingsAudio);
            card.Controls.Add(btnRestart);
            card.Controls.Add(btnToMenu);
            card.Controls.Add(btnQuit);
            card.Controls.Add(lblFooter);

            pnlSettingsModal.Controls.Add(card);
            this.Controls.Add(pnlSettingsModal);
        }

        public void ShowSettingsModal()
        {
            CancelDrag();
            gameManager.IsPaused = true;

            if (currentGameMode == GameMode.Story && activeChallenge != null)
            {
                lblSettingsStatusTitle.Text = $"ĐANG TẠM DỪNG: MÙA {activeChallenge.SeasonId} - {activeChallenge.LevelTitle.ToUpper()}";
                lblSettingsStatusDetail.Text = $"Tiến độ: {activeChallenge.LinesCleared}/{activeChallenge.TargetLines} Dòng  |  Nước đi: {activeChallenge.MovesLeft}/{activeChallenge.MaxMoves}";
            }
            else
            {
                lblSettingsStatusTitle.Text = "ĐANG TẠM DỪNG: CHẾ ĐỘ CHƠI TỰ DO (ENDLESS)";
                lblSettingsStatusDetail.Text = $"Điểm hiện tại: {gameManager.CurrentScore:#,##0}  |  Kỷ lục cá nhân: {gameManager.HighScore:#,##0}";
            }

            if (btnSettingsAudio != null)
            {
                btnSettingsAudio.Text = SoundManager.IsMuted ? "🔇  ÂM THANH: ĐANG TẮT" : "🔊  ÂM THANH: ĐANG BẬT";
            }

            pnlSettingsModal.Visible = true;
            pnlSettingsModal.BringToFront();
            LayoutMenuModals();
            pnlBoard.Invalidate();
        }

        public void ResumeFromSettings()
        {
            if (pnlSettingsModal != null) pnlSettingsModal.Visible = false;
            gameManager.IsPaused = false;
            pnlBoard.Invalidate();
        }

        private void RestartGameFromSettings()
        {
            if (currentGameMode == GameMode.Story && activeChallenge != null)
            {
                StartChallenge(activeChallenge.SeasonId, activeChallenge.LevelId);
            }
            else
            {
                RestartEndlessGame();
            }
        }

        private Button CreateTopCloseButton(Control card, Action onClose)
        {
            Button btn = new Button
            {
                Text = "✕",
                Size = new Size(36, 36),
                Location = new Point(card.Width - 50, 14),
                BackColor = Color.FromArgb(40, 45, 66),
                ForeColor = Color.FromArgb(200, 205, 220),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => onClose?.Invoke();
            ApplyRoundedRegion(btn, 18);
            return btn;
        }

        private Button CreateBackButton(string text, Point loc, Action onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(240, 48),
                Location = loc,
                BackColor = Color.FromArgb(48, 54, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => onClick?.Invoke();
            ApplyRoundedRegion(btn, 12);
            return btn;
        }

        // ================================================================
        //  2. NHẬT KÝ THÀNH TÍCH 2 TAB (BẢO TÀNG HUÂN CHƯƠNG & THỐNG KÊ)
        // ================================================================
        private void BuildAchievementJournalModal()
        {
            pnlLeaderboardModal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 19, 28),
                Visible = false
            };
            pnlLeaderboardModal.MouseDown += (s, e) => { };

            Panel card = new Panel
            {
                Name = "pnlLeaderboardCard",
                Size = new Size(720, 680),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 22);

            Button btnCloseTop = CreateTopCloseButton(card, () => { pnlLeaderboardModal.Visible = false; ShowMainMenu(); });

            Label lblTitle = new Label
            {
                Text = "🎖️ NHẬT KÝ THÀNH TÍCH SỰ NGHIỆP",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 196, 77),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(700, 40),
                Location = new Point(10, 16)
            };

            // Thanh Tab Header (2 Tab)
            Panel pnlTabHeader = new Panel
            {
                Size = new Size(660, 46),
                Location = new Point(30, 64),
                BackColor = Color.FromArgb(32, 37, 56)
            };
            ApplyRoundedRegion(pnlTabHeader, 12);

            btnTabMedals = new Button
            {
                Text = "🎖️ BẢO TÀNG HUÂN CHƯƠNG (12)",
                Size = new Size(328, 42),
                Location = new Point(2, 2),
                BackColor = Color.FromArgb(255, 196, 77),
                ForeColor = Color.FromArgb(18, 21, 32),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTabMedals.FlatAppearance.BorderSize = 0;
            ApplyRoundedRegion(btnTabMedals, 10);

            btnTabStats = new Button
            {
                Text = "📊 THỐNG KÊ SỰ NGHIỆP",
                Size = new Size(324, 42),
                Location = new Point(334, 2),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(170, 175, 195),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTabStats.FlatAppearance.BorderSize = 0;
            ApplyRoundedRegion(btnTabStats, 10);

            btnTabMedals.Click += (s, e) => SwitchAchievementTab(1);
            btnTabStats.Click += (s, e) => SwitchAchievementTab(2);

            pnlTabHeader.Controls.Add(btnTabMedals);
            pnlTabHeader.Controls.Add(btnTabStats);

            // Tab 1 Container: Danh sách Huân Chương
            pnlTabMedals = new Panel
            {
                Location = new Point(30, 118),
                Size = new Size(660, 480),
                BackColor = Color.Transparent,
                Visible = true
            };

            flpMedalsList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = false,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.Transparent
            };
            pnlTabMedals.Controls.Add(flpMedalsList);

            // Tab 2 Container: Thống Kê Sự Nghiệp
            pnlTabStats = new Panel
            {
                Location = new Point(30, 118),
                Size = new Size(660, 480),
                BackColor = Color.Transparent,
                Visible = false
            };

            pnlCareerStatsContent = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = false,
                BackColor = Color.Transparent
            };
            pnlTabStats.Controls.Add(pnlCareerStatsContent);

            // Nút quay lại menu
            Button btnBack = CreateBackButton("← QUAY LẠI MENU", new Point((card.Width - 240) / 2, 612), () =>
            {
                pnlLeaderboardModal.Visible = false;
                ShowMainMenu();
            });

            card.Controls.Add(btnCloseTop);
            card.Controls.Add(lblTitle);
            card.Controls.Add(pnlTabHeader);
            card.Controls.Add(pnlTabMedals);
            card.Controls.Add(pnlTabStats);
            card.Controls.Add(btnBack);

            pnlLeaderboardModal.Controls.Add(card);
            this.Controls.Add(pnlLeaderboardModal);
        }

        private void SwitchAchievementTab(int tabIndex)
        {
            if (tabIndex == 1)
            {
                btnTabMedals.BackColor = Color.FromArgb(255, 196, 77);
                btnTabMedals.ForeColor = Color.FromArgb(18, 21, 32);
                btnTabStats.BackColor = Color.Transparent;
                btnTabStats.ForeColor = Color.FromArgb(170, 175, 195);
                pnlTabMedals.Visible = true;
                pnlTabStats.Visible = false;
                PopulateMedalsList();
            }
            else
            {
                btnTabStats.BackColor = Color.FromArgb(84, 160, 255);
                btnTabStats.ForeColor = Color.White;
                btnTabMedals.BackColor = Color.Transparent;
                btnTabMedals.ForeColor = Color.FromArgb(170, 175, 195);
                pnlTabMedals.Visible = false;
                pnlTabStats.Visible = true;
                PopulateCareerStats();
            }
        }

        private void PopulateMedalsList()
        {
            flpMedalsList.Controls.Clear();
            var user = UserManager.CurrentUser;
            if (user == null) return;

            // Danh sách cấu hình 12 Huân chương
            var medals = new[]
            {
                new { Key = "season_1_medal", Icon = "🌿", Title = "Huân Chương Đất Mẹ", Desc = "Hoàn thành 12 màn Mùa 1: Lục Địa Đất Mẹ", Color = Color.FromArgb(46, 204, 113) },
                new { Key = "season_2_medal", Icon = "❄️", Title = "Huân Chương Băng Hà", Desc = "Hoàn thành 12 màn Mùa 2: Băng Hà Vĩnh Cửu", Color = Color.FromArgb(0, 210, 211) },
                new { Key = "season_3_medal", Icon = "🔥", Title = "Huân Chương Lò Rèn", Desc = "Hoàn thành 12 màn Mùa 3: Hỏa Ngục Vulcan", Color = Color.FromArgb(255, 71, 87) },
                new { Key = "season_4_medal", Icon = "⚡", Title = "Huân Chương Thiên Lôi", Desc = "Hoàn thành 12 màn Mùa 4: Đỉnh Tháp Sấm Sét", Color = Color.FromArgb(255, 165, 2) },
                new { Key = "season_5_medal", Icon = "🌊", Title = "Huân Chương Biển Sâu", Desc = "Hoàn thành 12 màn Mùa 5: Vực Thẳm Leviathan", Color = Color.FromArgb(30, 144, 255) },
                new { Key = "season_6_medal", Icon = "🏜️", Title = "Huân Chương Sa Mạc", Desc = "Hoàn thành 12 màn Mùa 6: Bão Cát Kim Tự Tháp", Color = Color.FromArgb(230, 126, 34) },
                new { Key = "season_7_medal", Icon = "🌌", Title = "Huân Chương Hư Không", Desc = "Hoàn thành 12 màn Mùa 7: Hư Không Vũ Trụ", Color = Color.FromArgb(155, 89, 182) },
                new { Key = "season_8_medal", Icon = "👑", Title = "Huân Chương Khải Huyền", Desc = "Chinh phục Mùa 8: Thời Khắc Khải Huyền Tối Thượng", Color = Color.FromArgb(241, 242, 246) },
                new { Key = "medal_combo_4", Icon = "💥", Title = "Bậc Thầy Combo", Desc = "Kích hoạt chuỗi nổ liên hoàn Mega Combo x4+", Color = Color.FromArgb(255, 107, 107) },
                new { Key = "medal_score_1000", Icon = "🌟", Title = "Triệu Điểm Huyền Thoại", Desc = "Đạt điểm số kỷ lục từ 1,000 điểm trở lên", Color = Color.FromArgb(255, 196, 77) },
                new { Key = "medal_lines_100", Icon = "🛡️", Title = "Chiến Thần Bất Diệt", Desc = "Phá hủy tổng cộng từ 100 dòng nổ trở lên", Color = Color.FromArgb(72, 219, 251) },
                new { Key = "medal_architect_30", Icon = "🏛️", Title = "Đại Kiến Trúc Sư", Desc = "Vượt qua thành công 30 màn thử thách mùa", Color = Color.FromArgb(29, 209, 161) }
            };

            foreach (var m in medals)
            {
                bool isUnlocked = user.UnlockedMedals != null && user.UnlockedMedals.Contains(m.Key);

                Panel badgeCard = new Panel
                {
                    Size = new Size(322, 68),
                    BackColor = isUnlocked ? Color.FromArgb(32, 38, 58) : Color.FromArgb(22, 25, 38),
                    Margin = new Padding(0, 0, 8, 8)
                };
                badgeCard.SizeChanged += (s, e) => ApplyRoundedRegion(badgeCard, 12);
                ApplyRoundedRegion(badgeCard, 12);

                Label lblIcon = new Label
                {
                    Text = m.Icon,
                    Font = new Font("Segoe UI", 20f),
                    Size = new Size(42, 42),
                    Location = new Point(8, 13),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label lblName = new Label
                {
                    Text = m.Title,
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    ForeColor = isUnlocked ? m.Color : Color.FromArgb(180, 185, 205),
                    Location = new Point(54, 8),
                    Size = new Size(185, 22)
                };

                Label lblDesc = new Label
                {
                    Text = m.Desc,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                    ForeColor = isUnlocked ? Color.FromArgb(170, 175, 195) : Color.FromArgb(115, 122, 142),
                    Location = new Point(54, 30),
                    Size = new Size(185, 32)
                };

                Label lblStatus = new Label
                {
                    Text = isUnlocked ? "✓ ĐÃ ĐẠT" : "🔒 KHÓA",
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = isUnlocked ? Color.FromArgb(46, 204, 113) : Color.FromArgb(120, 125, 145),
                    TextAlign = ContentAlignment.MiddleRight,
                    Location = new Point(238, 12),
                    Size = new Size(76, 20)
                };

                badgeCard.Controls.Add(lblIcon);
                badgeCard.Controls.Add(lblName);
                badgeCard.Controls.Add(lblDesc);
                badgeCard.Controls.Add(lblStatus);

                flpMedalsList.Controls.Add(badgeCard);
            }
        }

        private void PopulateCareerStats()
        {
            pnlCareerStatsContent.Controls.Clear();
            var user = UserManager.CurrentUser;
            if (user == null) return;

            int completedSeasonsCount = user.CompletedSeasons != null ? user.CompletedSeasons.Count : 0;
            int totalLevelsWon = user.TotalChallengesWon;

            var stats = new[]
            {
                new { Label = "👤 TÊN CHIẾN BINH", Value = user.Username, Color = Color.White },
                new { Label = "🏆 ĐIỂM SỐ KỶ LỤC (BEST SCORE)", Value = user.BestScore.ToString("#,##0"), Color = Color.FromArgb(255, 196, 77) },
                new { Label = "🌟 TỔNG ĐIỂM TRỌN ĐỜI (LIFETIME)", Value = user.LifetimeScore.ToString("#,##0"), Color = Color.FromArgb(84, 160, 255) },
                new { Label = "💥 TỔNG DÒNG ĐÃ NỔ (LINES CLEARED)", Value = user.TotalLinesCleared.ToString("#,##0"), Color = Color.FromArgb(46, 204, 113) },
                new { Label = "🔥 CHUỖI COMBO KỶ LỤC (MAX COMBO)", Value = $"x{user.MaxComboRecord}", Color = Color.FromArgb(255, 107, 107) },
                new { Label = "🎯 MÀN THỬ THÁCH ĐÃ CHINH PHỤC", Value = $"{totalLevelsWon} / 96 Màn", Color = Color.FromArgb(0, 210, 211) },
                new { Label = "👑 MÙA CAO NHẤT ĐÃ MỞ KHÓA", Value = $"Mùa {user.HighestSeasonUnlocked} / 8", Color = Color.FromArgb(241, 242, 246) },
                new { Label = "🏅 ĐẠI MÙA ĐÃ HOÀN THÀNH TOÀN DIỆN", Value = $"{completedSeasonsCount} / 8 Đại Mùa", Color = Color.FromArgb(255, 165, 2) }
            };

            int y = 10;
            foreach (var s in stats)
            {
                Panel card = new Panel
                {
                    Size = new Size(640, 50),
                    Location = new Point(10, y),
                    BackColor = Color.FromArgb(30, 35, 54)
                };
                ApplyRoundedRegion(card, 12);

                Label lblL = new Label
                {
                    Text = s.Label,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(170, 175, 195),
                    Location = new Point(16, 14),
                    Size = new Size(360, 24)
                };

                Label lblV = new Label
                {
                    Text = s.Value,
                    Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                    ForeColor = s.Color,
                    TextAlign = ContentAlignment.MiddleRight,
                    Location = new Point(380, 12),
                    Size = new Size(244, 26)
                };

                card.Controls.Add(lblL);
                card.Controls.Add(lblV);
                pnlCareerStatsContent.Controls.Add(card);
                y += 58;
            }
        }

        // ================================================================
        //  3. HUB BĂNG CHUYỀN MÙA & LƯỚI MÀN CHƠI (SEASON CAROUSEL & LEVEL GRID)
        // ================================================================
        private void BuildSeasonHubModal()
        {
            pnlStorySelectModal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 19, 28),
                Visible = false
            };
            pnlStorySelectModal.MouseDown += (s, e) => { };

            Panel card = new Panel
            {
                Name = "pnlStorySelectCard",
                Size = new Size(740, 680),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 22);

            Button btnCloseTop = CreateTopCloseButton(card, () => { pnlStorySelectModal.Visible = false; ShowMainMenu(); });

            // Banner Băng Chuyền Mùa (Season Carousel Banner)
            Panel pnlCarouselBanner = new Panel
            {
                Size = new Size(680, 160),
                Location = new Point(30, 20),
                BackColor = Color.FromArgb(32, 37, 56)
            };
            ApplyRoundedRegion(pnlCarouselBanner, 16);

            btnCarouselPrev = new Button
            {
                Text = "◀",
                Size = new Size(48, 48),
                Location = new Point(12, 56),
                BackColor = Color.FromArgb(48, 54, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCarouselPrev.FlatAppearance.BorderSize = 0;
            btnCarouselPrev.Click += (s, e) => ChangeCarouselSeason(-1);
            ApplyRoundedRegion(btnCarouselPrev, 12);

            btnCarouselNext = new Button
            {
                Text = "▶",
                Size = new Size(48, 48),
                Location = new Point(620, 56),
                BackColor = Color.FromArgb(48, 54, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCarouselNext.FlatAppearance.BorderSize = 0;
            btnCarouselNext.Click += (s, e) => ChangeCarouselSeason(1);
            ApplyRoundedRegion(btnCarouselNext, 12);

            lblCarouselSeasonTitle = new Label
            {
                Text = "🌿 MÙA 1: LỤC ĐỊA ĐẤT MẸ",
                Font = new Font("Segoe UI", 17f, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(540, 32),
                Location = new Point(70, 14)
            };

            lblCarouselSeasonSubtitle = new Label
            {
                Text = "VÙNG THẠCH TRẬN CỔ ĐẠI (ANCIENT MONOLITH)",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(170, 175, 195),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(540, 22),
                Location = new Point(70, 48)
            };

            lblCarouselSeasonLore = new Label
            {
                Text = "Hàn gắn các phiến đá cổ đại và đánh thức dòng sinh khí lục địa ngầm bừng tỉnh.",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(210, 215, 230),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(540, 38),
                Location = new Point(70, 72)
            };

            lblCarouselProgress = new Label
            {
                Text = "Tiến độ: [■■■□□□□□□□□□] 3/12 Màn (25%)",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 196, 77),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(540, 24),
                Location = new Point(70, 116)
            };

            pnlCarouselBanner.Controls.Add(btnCarouselPrev);
            pnlCarouselBanner.Controls.Add(btnCarouselNext);
            pnlCarouselBanner.Controls.Add(lblCarouselSeasonTitle);
            pnlCarouselBanner.Controls.Add(lblCarouselSeasonSubtitle);
            pnlCarouselBanner.Controls.Add(lblCarouselSeasonLore);
            pnlCarouselBanner.Controls.Add(lblCarouselProgress);

            // Tiêu đề phân cách lưới màn chơi
            Label lblGridHeader = new Label
            {
                Text = "🎯 CHỌN MÀN CHƠI THỬ THÁCH (12 CẤP ĐỘ NGUYÊN TỐ):",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(32, 194),
                Size = new Size(680, 24)
            };

            // Lưới 12 Màn Chơi (FlowLayoutPanel 3 hàng x 4 cột)
            flpLevelGrid = new FlowLayoutPanel
            {
                Location = new Point(30, 224),
                Size = new Size(680, 370),
                AutoScroll = false,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.Transparent
            };

            // Nút quay lại menu
            Button btnBack = CreateBackButton("← QUAY LẠI MENU", new Point((card.Width - 240) / 2, 612), () =>
            {
                pnlStorySelectModal.Visible = false;
                ShowMainMenu();
            });

            card.Controls.Add(btnCloseTop);
            card.Controls.Add(pnlCarouselBanner);
            card.Controls.Add(lblGridHeader);
            card.Controls.Add(flpLevelGrid);
            card.Controls.Add(btnBack);

            pnlStorySelectModal.Controls.Add(card);
            this.Controls.Add(pnlStorySelectModal);
        }

        private void ChangeCarouselSeason(int delta)
        {
            currentCarouselSeason += delta;
            if (currentCarouselSeason < 1) currentCarouselSeason = SeasonManager.TOTAL_SEASONS;
            if (currentCarouselSeason > SeasonManager.TOTAL_SEASONS) currentCarouselSeason = 1;
            UpdateSeasonCarouselUI();
        }

        private void UpdateSeasonCarouselUI()
        {
            var sDto = SeasonManager.GetSeason(currentCarouselSeason);
            var theme = InverseChallengeGenerator.GetTheme(currentCarouselSeason);

            Color accent = theme.AccentColor;
            lblCarouselSeasonTitle.Text = $"{theme.Icon} {sDto.Title.ToUpper()}";
            lblCarouselSeasonTitle.ForeColor = accent;
            lblCarouselSeasonSubtitle.Text = sDto.Subtitle.ToUpper();
            lblCarouselSeasonLore.Text = sDto.LoreDescription;

            int progress = UserManager.GetSeasonProgress(currentCarouselSeason);
            int percent = (progress * 100) / SeasonManager.LEVELS_PER_SEASON;
            int totalBars = 12;
            string barStr = new string('■', progress) + new string('□', totalBars - progress);
            lblCarouselProgress.Text = $"Tiến độ: [{barStr}] {progress}/12 Màn ({percent}%)";
            lblCarouselProgress.ForeColor = progress >= 12 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(255, 196, 77);

            // Cập nhật 12 nút màn chơi
            flpLevelGrid.Controls.Clear();
            for (int l = 1; l <= SeasonManager.LEVELS_PER_SEASON; l++)
            {
                bool isCompleted = UserManager.IsLevelCompleted(currentCarouselSeason, l);
                bool isUnlocked = UserManager.IsLevelUnlocked(currentCarouselSeason, l);

                Button btnL = new Button
                {
                    Size = new Size(158, 80),
                    Margin = new Padding(0, 0, 12, 12),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = isUnlocked ? Cursors.Hand : Cursors.Default
                };
                btnL.FlatAppearance.BorderSize = 0;

                int sId = currentCarouselSeason;
                int lId = l;

                if (isCompleted)
                {
                    btnL.Text = $"✓ MÀN {l}\n(Đã Vượt)";
                    btnL.BackColor = Color.FromArgb(39, 174, 96);
                    btnL.ForeColor = Color.White;
                    btnL.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                    btnL.Click += (s, e) => ShowStoryBriefingModal(sId, lId);
                }
                else if (isUnlocked)
                {
                    btnL.Text = $"▶ MÀN {l}\n(Sẵn Sàng)";
                    btnL.BackColor = accent;
                    btnL.ForeColor = (currentCarouselSeason == 8) ? Color.FromArgb(18, 21, 32) : Color.White;
                    btnL.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                    btnL.Click += (s, e) => ShowStoryBriefingModal(sId, lId);
                }
                else
                {
                    btnL.Text = $"🔒 MÀN {l}\n(Khóa)";
                    btnL.BackColor = Color.FromArgb(28, 33, 48);
                    btnL.ForeColor = Color.FromArgb(150, 158, 180);
                    btnL.Font = new Font("Segoe UI", 10.5f, FontStyle.Regular);
                    btnL.Click += (s, e) =>
                    {
                        SoundManager.PlayPop();
                        MessageBox.Show($"Màn {lId} hiện đang bị phong ấn!\nHãy vượt qua các màn thử thách trước để mở khóa.", "Chưa Mở Khóa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    };
                }

                btnL.SizeChanged += (s, e) => ApplyRoundedRegion(btnL, 12);
                ApplyRoundedRegion(btnL, 12);

                flpLevelGrid.Controls.Add(btnL);
            }
        }

        // ================================================================
        //  4. MODAL ĐỔI TÊN NGƯỜI CHƠI
        // ================================================================
        private void BuildChangeUserModal()
        {
            pnlChangeUserModal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 19, 28),
                Visible = false
            };
            pnlChangeUserModal.MouseDown += (s, e) => { };

            Panel card = new Panel
            {
                Name = "pnlChangeUserCard",
                Size = new Size(480, 360),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 20);

            Button btnCloseTop = CreateTopCloseButton(card, () => { pnlChangeUserModal.Visible = false; ShowMainMenu(); });

            Label lblTitle = new Label
            {
                Text = "👤 ĐỔI TÊN NGƯỜI CHƠI",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 35),
                Location = new Point(10, 20)
            };

            Label lblExisting = new Label
            {
                Text = "Chọn người chơi đã có:",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(170, 175, 195),
                Location = new Point(40, 75),
                Size = new Size(400, 22)
            };

            cboExistingUsers = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11f),
                BackColor = Color.FromArgb(32, 37, 56),
                ForeColor = Color.White,
                Location = new Point(40, 100),
                Size = new Size(400, 30)
            };
            cboExistingUsers.SelectedIndexChanged += (s, e) =>
            {
                if (cboExistingUsers.SelectedItem != null)
                {
                    txtNewUserName.Text = cboExistingUsers.SelectedItem.ToString();
                }
            };

            Label lblNew = new Label
            {
                Text = "Hoặc tạo tên người chơi mới:",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(170, 175, 195),
                Location = new Point(40, 150),
                Size = new Size(400, 22)
            };

            txtNewUserName = new TextBox
            {
                Font = new Font("Segoe UI", 12f),
                BackColor = Color.FromArgb(32, 37, 56),
                ForeColor = Color.White,
                Location = new Point(40, 175),
                Size = new Size(400, 34)
            };

            Button btnConfirm = new Button
            {
                Text = "XÁC NHẬN",
                Size = new Size(180, 48),
                Location = new Point(40, 250),
                BackColor = Color.FromArgb(0, 210, 211),
                ForeColor = Color.FromArgb(18, 21, 32),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += (s, e) =>
            {
                string name = txtNewUserName.Text.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    UserManager.SwitchUser(name);
                    gameManager.HighScore = UserManager.CurrentUser.BestScore;
                    UpdateUserCardInfo();
                    UpdateScoreLabels();
                    pnlChangeUserModal.Visible = false;
                    ShowMainMenu();
                }
            };
            ApplyRoundedRegion(btnConfirm, 12);

            Button btnCancel = new Button
            {
                Text = "← HỦY BỎ",
                Size = new Size(180, 48),
                Location = new Point(260, 250),
                BackColor = Color.FromArgb(48, 54, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                pnlChangeUserModal.Visible = false;
                ShowMainMenu();
            };
            ApplyRoundedRegion(btnCancel, 12);

            card.Controls.Add(btnCloseTop);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblExisting);
            card.Controls.Add(cboExistingUsers);
            card.Controls.Add(lblNew);
            card.Controls.Add(txtNewUserName);
            card.Controls.Add(btnConfirm);
            card.Controls.Add(btnCancel);

            pnlChangeUserModal.Controls.Add(card);
            this.Controls.Add(pnlChangeUserModal);
        }

        // ================================================================
        //  5. BRIEFING MODAL GIỚI THIỆU THỬ THÁCH ĐIỆN ẢNH
        // ================================================================
        private void BuildStoryBriefingModal()
        {
            pnlStoryBriefingModal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 19, 28),
                Visible = false
            };
            pnlStoryBriefingModal.MouseDown += (s, e) => { };

            Panel card = new Panel
            {
                Name = "pnlStoryBriefingCard",
                Size = new Size(700, 640),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 22);

            Button btnCloseTop = CreateTopCloseButton(card, () => { pnlStoryBriefingModal.Visible = false; ShowStorySelectModal(); });

            lblBriefingTitle = new Label
            {
                Text = "🌿 MÙA 1: LỤC ĐỊA ĐẤT MẸ",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(680, 42),
                Location = new Point(10, 18)
            };

            lblBriefingSubtitle = new Label
            {
                Text = "MÀN 1: KHỞI NGUYÊN SINH KHÍ",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(140, 145, 170),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(680, 24),
                Location = new Point(10, 62)
            };

            Panel pnlQuoteBox = new Panel
            {
                Size = new Size(640, 190),
                Location = new Point(30, 96),
                BackColor = Color.FromArgb(18, 21, 32)
            };
            ApplyRoundedRegion(pnlQuoteBox, 14);

            Label lblQuoteHeader = new Label
            {
                Text = "📜 LỜI DẪN TỪ BẬC THẦY KIẾN TẠO CỔ ĐẠI:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 196, 77),
                Location = new Point(16, 12),
                Size = new Size(608, 22)
            };

            rtbBriefingQuote = new RichTextBox
            {
                ReadOnly = true,
                BackColor = Color.FromArgb(18, 21, 32),
                ForeColor = Color.FromArgb(220, 225, 235),
                Font = new Font("Segoe UI", 10.5f, FontStyle.Italic),
                Size = new Size(608, 138),
                Location = new Point(16, 38),
                BorderStyle = BorderStyle.None
            };
            pnlQuoteBox.Controls.Add(lblQuoteHeader);
            pnlQuoteBox.Controls.Add(rtbBriefingQuote);

            // Mission Objective Cards
            Panel pnlTargetBox = new Panel
            {
                Size = new Size(310, 96),
                Location = new Point(30, 300),
                BackColor = Color.FromArgb(30, 35, 54)
            };
            ApplyRoundedRegion(pnlTargetBox, 14);

            Label lblTTitle = new Label
            {
                Text = "🎯 MỤC TIÊU PHONG ẤN",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 210, 211),
                Location = new Point(16, 12),
                Size = new Size(278, 22)
            };
            lblBriefingTarget = new Label
            {
                Text = "Kích nổ: 4 Hàng / Cột",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(16, 40),
                Size = new Size(278, 36)
            };
            pnlTargetBox.Controls.Add(lblTTitle);
            pnlTargetBox.Controls.Add(lblBriefingTarget);

            Panel pnlMovesBox = new Panel
            {
                Size = new Size(310, 96),
                Location = new Point(360, 300),
                BackColor = Color.FromArgb(30, 35, 54)
            };
            ApplyRoundedRegion(pnlMovesBox, 14);

            Label lblMTitle = new Label
            {
                Text = "⏱ GIỚI HẠN NƯỚC ĐI",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 159, 67),
                Location = new Point(16, 12),
                Size = new Size(278, 22)
            };
            lblBriefingMoves = new Label
            {
                Text = "Tối đa: 15 Lượt đặt khối",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(16, 40),
                Size = new Size(278, 36)
            };
            pnlMovesBox.Controls.Add(lblMTitle);
            pnlMovesBox.Controls.Add(lblBriefingMoves);

            Label lblTip = new Label
            {
                Text = "💡 Mẹo: Bố cục đã được tính toán 100% có thể giải được! Hãy tận dụng các khe hở để nổ dòng ngay.",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(160, 165, 185),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(640, 28),
                Location = new Point(30, 412)
            };

            btnBriefingStart = new Button
            {
                Text = "⚡ TIẾN VÀO CHIẾN TRƯỜNG",
                Size = new Size(340, 54),
                Location = new Point((card.Width - 340) / 2, 456),
                BackColor = Color.FromArgb(84, 160, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBriefingStart.FlatAppearance.BorderSize = 0;
            btnBriefingStart.Click += (s, e) =>
            {
                StartChallenge(currentBriefingSeason, currentBriefingLevel);
            };
            ApplyRoundedRegion(btnBriefingStart, 16);

            Button btnBack = CreateBackButton("← CHỌN MÀN KHÁC", new Point((card.Width - 240) / 2, 530), () =>
            {
                pnlStoryBriefingModal.Visible = false;
                ShowStorySelectModal();
            });

            card.Controls.Add(btnCloseTop);
            card.Controls.Add(lblBriefingTitle);
            card.Controls.Add(lblBriefingSubtitle);
            card.Controls.Add(pnlQuoteBox);
            card.Controls.Add(pnlTargetBox);
            card.Controls.Add(pnlMovesBox);
            card.Controls.Add(lblTip);
            card.Controls.Add(btnBriefingStart);
            card.Controls.Add(btnBack);

            pnlStoryBriefingModal.Controls.Add(card);
            this.Controls.Add(pnlStoryBriefingModal);
        }

        // ================================================================
        //  6. MODAL CHIẾN THẮNG & THẤT BẠI
        // ================================================================
        private void BuildStoryVictoryModal()
        {
            pnlStoryVictoryModal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 19, 28),
                Visible = false
            };
            pnlStoryVictoryModal.MouseDown += (s, e) => { };

            Panel card = new Panel
            {
                Name = "pnlStoryVictoryCard",
                Size = new Size(680, 580),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 22);

            Button btnCloseTop = CreateTopCloseButton(card, () => { pnlStoryVictoryModal.Visible = false; ShowMainMenu(); });

            lblVictoryTitle = new Label
            {
                Text = "🎉 CHIẾN THẮNG NGUYÊN TỐ!",
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 196, 77),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(660, 45),
                Location = new Point(10, 20)
            };

            lblVictorySubtitle = new Label
            {
                Text = "MÀN CHƠI ĐÃ ĐƯỢC CHINH PHỤC HOÀN TOÀN",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 210, 211),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(660, 26),
                Location = new Point(10, 68)
            };

            Panel pnlLoreBox = new Panel
            {
                Size = new Size(620, 180),
                Location = new Point(30, 106),
                BackColor = Color.FromArgb(18, 21, 32)
            };
            ApplyRoundedRegion(pnlLoreBox, 14);

            Label lblLoreHdr = new Label
            {
                Text = "📖 DIỄN BIẾN SỬ THI TIẾP THEO:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113),
                Location = new Point(16, 12),
                Size = new Size(588, 22)
            };

            rtbVictoryLore = new RichTextBox
            {
                ReadOnly = true,
                BackColor = Color.FromArgb(18, 21, 32),
                ForeColor = Color.FromArgb(220, 225, 235),
                Font = new Font("Segoe UI", 11f, FontStyle.Italic),
                Size = new Size(588, 128),
                Location = new Point(16, 38),
                BorderStyle = BorderStyle.None
            };
            pnlLoreBox.Controls.Add(lblLoreHdr);
            pnlLoreBox.Controls.Add(rtbVictoryLore);

            lblVictoryStats = new Label
            {
                Text = "🎯 Đã hoàn thành: 4/4 Dòng  |  ⏱ Nước đi còn lại: 5",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(620, 30),
                Location = new Point(30, 304)
            };

            btnVictoryNext = new Button
            {
                Text = "⚡ TIẾP TỤC MÀN KẾ TIẾP",
                Size = new Size(340, 52),
                Location = new Point((card.Width - 340) / 2, 350),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnVictoryNext.FlatAppearance.BorderSize = 0;
            btnVictoryNext.Click += (s, e) =>
            {
                pnlStoryVictoryModal.Visible = false;
                if (currentVictoryLevel < SeasonManager.LEVELS_PER_SEASON)
                {
                    ShowStoryBriefingModal(currentVictorySeason, currentVictoryLevel + 1);
                }
                else if (currentVictorySeason < SeasonManager.TOTAL_SEASONS)
                {
                    ShowStoryBriefingModal(currentVictorySeason + 1, 1);
                }
                else
                {
                    ShowLeaderboardModal();
                }
            };
            ApplyRoundedRegion(btnVictoryNext, 14);

            Button btnReplayChapter = new Button
            {
                Text = "🔄 CHƠI LẠI MÀN NÀY",
                Size = new Size(240, 44),
                Location = new Point(90, 422),
                BackColor = Color.FromArgb(48, 54, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReplayChapter.FlatAppearance.BorderSize = 0;
            btnReplayChapter.Click += (s, e) =>
            {
                pnlStoryVictoryModal.Visible = false;
                StartChallenge(currentVictorySeason, currentVictoryLevel);
            };
            ApplyRoundedRegion(btnReplayChapter, 12);

            Button btnHome = new Button
            {
                Text = "🏠 VỀ MENU CHÍNH",
                Size = new Size(240, 44),
                Location = new Point(350, 422),
                BackColor = Color.FromArgb(36, 40, 58),
                ForeColor = Color.FromArgb(200, 205, 220),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.Click += (s, e) =>
            {
                pnlStoryVictoryModal.Visible = false;
                ShowMainMenu();
            };
            ApplyRoundedRegion(btnHome, 12);

            card.Controls.Add(btnCloseTop);
            card.Controls.Add(lblVictoryTitle);
            card.Controls.Add(lblVictorySubtitle);
            card.Controls.Add(pnlLoreBox);
            card.Controls.Add(lblVictoryStats);
            card.Controls.Add(btnVictoryNext);
            card.Controls.Add(btnReplayChapter);
            card.Controls.Add(btnHome);

            pnlStoryVictoryModal.Controls.Add(card);
            this.Controls.Add(pnlStoryVictoryModal);
        }

        private void BuildStoryDefeatModal()
        {
            pnlStoryDefeatModal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 19, 28),
                Visible = false
            };
            pnlStoryDefeatModal.MouseDown += (s, e) => { };

            Panel card = new Panel
            {
                Name = "pnlStoryDefeatCard",
                Size = new Size(640, 540),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 22);

            Button btnCloseTop = CreateTopCloseButton(card, () => { pnlStoryDefeatModal.Visible = false; ShowStorySelectModal(); });

            lblDefeatTitle = new Label
            {
                Text = "⚠️ PHONG ẤN THẤT BẠI!",
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 71, 87),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(620, 45),
                Location = new Point(10, 20)
            };

            lblDefeatReason = new Label
            {
                Text = "Lý do: Hết số nước đi cho phép",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 159, 67),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(620, 26),
                Location = new Point(10, 68)
            };

            Panel pnlAdviceBox = new Panel
            {
                Size = new Size(580, 160),
                Location = new Point(30, 110),
                BackColor = Color.FromArgb(18, 21, 32)
            };
            ApplyRoundedRegion(pnlAdviceBox, 14);

            Label lblAdvHdr = new Label
            {
                Text = "💡 LỜI KHUYÊN TỪ BẬC THẦY KIẾN TẠO:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 210, 211),
                Location = new Point(16, 12),
                Size = new Size(548, 22)
            };

            rtbDefeatAdvice = new RichTextBox
            {
                ReadOnly = true,
                BackColor = Color.FromArgb(18, 21, 32),
                ForeColor = Color.FromArgb(210, 215, 230),
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Size = new Size(548, 110),
                Location = new Point(16, 38),
                BorderStyle = BorderStyle.None,
                Text = "Đừng nản lòng! Thuật toán Inverse Solvable đảm bảo 100% luôn có lời giải chiến thắng.\n" +
                       "• Hãy quan sát các hàng/cột đã có sẵn 5-6 ô gạch để kích nổ ngay từ lượt đầu.\n" +
                       "• Tránh đặt các khối cồng kềnh vào giữa làm tắc nghẽn bàn cờ."
            };
            pnlAdviceBox.Controls.Add(lblAdvHdr);
            pnlAdviceBox.Controls.Add(rtbDefeatAdvice);

            btnDefeatRetry = new Button
            {
                Text = "🔄 THỬ LẠI MÀN NÀY",
                Size = new Size(320, 52),
                Location = new Point((card.Width - 320) / 2, 300),
                BackColor = Color.FromArgb(255, 107, 107),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDefeatRetry.FlatAppearance.BorderSize = 0;
            btnDefeatRetry.Click += (s, e) =>
            {
                pnlStoryDefeatModal.Visible = false;
                StartChallenge(currentDefeatSeason, currentDefeatLevel);
            };
            ApplyRoundedRegion(btnDefeatRetry, 14);

            Button btnSelectChapter = new Button
            {
                Text = "← CHỌN MÀN KHÁC",
                Size = new Size(240, 44),
                Location = new Point(70, 375),
                BackColor = Color.FromArgb(48, 54, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSelectChapter.FlatAppearance.BorderSize = 0;
            btnSelectChapter.Click += (s, e) =>
            {
                pnlStoryDefeatModal.Visible = false;
                ShowStorySelectModal();
            };
            ApplyRoundedRegion(btnSelectChapter, 12);

            Button btnDefeatHome = new Button
            {
                Text = "🏠 VỀ MENU CHÍNH",
                Size = new Size(240, 44),
                Location = new Point(330, 375),
                BackColor = Color.FromArgb(36, 40, 58),
                ForeColor = Color.FromArgb(200, 205, 220),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDefeatHome.FlatAppearance.BorderSize = 0;
            btnDefeatHome.Click += (s, e) =>
            {
                pnlStoryDefeatModal.Visible = false;
                ShowMainMenu();
            };
            ApplyRoundedRegion(btnDefeatHome, 12);

            card.Controls.Add(btnCloseTop);
            card.Controls.Add(lblDefeatTitle);
            card.Controls.Add(lblDefeatReason);
            card.Controls.Add(pnlAdviceBox);
            card.Controls.Add(btnDefeatRetry);
            card.Controls.Add(btnSelectChapter);
            card.Controls.Add(btnDefeatHome);

            pnlStoryDefeatModal.Controls.Add(card);
            this.Controls.Add(pnlStoryDefeatModal);
        }

        // ================================================================
        //  MODAL GIỚI THIỆU & HƯỚNG DẪN (ABOUT)
        // ================================================================
        private void BuildAboutModal()
        {
            pnlAboutModal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 19, 28),
                Visible = false
            };
            pnlAboutModal.MouseDown += (s, e) => { };

            Panel card = new Panel
            {
                Name = "pnlAboutCard",
                Size = new Size(720, 680),
                BackColor = Color.FromArgb(24, 28, 43)
            };
            ApplyRoundedRegion(card, 22);

            Button btnCloseTop = CreateTopCloseButton(card, () => { pnlAboutModal.Visible = false; ShowMainMenu(); });

            Label lblTitle = new Label
            {
                Text = "🎮 THÔNG TIN ĐỒ ÁN & HƯỚNG DẪN",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.FromArgb(84, 160, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(700, 42),
                Location = new Point(10, 18)
            };

            RichTextBox rtbInfo = new RichTextBox
            {
                ReadOnly = true,
                BackColor = Color.FromArgb(18, 21, 32),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Size = new Size(660, 510),
                Location = new Point(30, 72),
                BorderStyle = BorderStyle.None
            };

            rtbInfo.Text =
                "=========================================================\n" +
                "  ĐỒ ÁN HỌC PHẦN: PHÁT TRIỂN MINIGAME C# WINDOWS FORMS\n" +
                "  TÊN DỰ ÁN: BLOCK BLAST - 8 EPIC LORE SEASONS & DDA ENGINE\n" +
                "=========================================================\n\n" +
                "👨‍🏫 GIẢNG VIÊN HƯỚNG DẪN: ThS. Giảng Viên Phụ Trách Học Phần\n\n" +
                "👥 DANH SÁCH NHÓM PHÁT TRIỂN (3 THÀNH VIÊN):\n" +
                "  1. Nguyễn Văn A (Nhóm Trưởng) - MSSV: 22000001\n" +
                "     - Đóng góp: Kiến trúc GameBoard vs GameChallenge, Thuật toán Mô Phỏng Ngược Inverse Solvable, Lưu Session JSON.\n" +
                "  2. Trần Thị B - MSSV: 22000002\n" +
                "     - Đóng góp: Đồ họa Canvas GDI+ Double Buffering, Animation Sóng Năng Lượng Thức Tỉnh & SFX Audio Engine.\n" +
                "  3. Lê Văn C - MSSV: 22000003\n" +
                "     - Đóng góp: Kho Lưu Trữ 8 Đại Mùa Sử Thi (96 Màn Chơi), Nhật Ký Thành Tích 2 Tab & Bẫy Lỗi Toàn Diện 100%.\n\n" +
                "---------------------------------------------------------\n" +
                "📖 CƠ CHẾ NỔI BẬT & TÍNH NĂNG ĐỘC BẢN:\n" +
                "---------------------------------------------------------\n" +
                "1. KHO LƯU TRỮ 8 ĐẠI MÙA SỬ THI (96 THỬ THÁCH):\n" +
                "   - Cấu hình qua JSON độc lập tại %AppData%/BlockBlast/seasons_data.json.\n" +
                "   - 8 Chủ đề nguyên tố độc đáo: Đất Mẹ -> Băng Giá -> Hỏa Ngục -> Thiên Lôi -> Biển Sâu -> Sa Mạc -> Hư Không -> Khải Huyền.\n\n" +
                "2. THUẬT TOÁN INVERSE SOLVABLE GENERATOR:\n" +
                "   - Khởi tạo ngược từ các trạng thái hợp lệ, bảo đảm 100% mọi màn đều có lời giải toán học chiến thắng.\n\n" +
                "3. TỰ ĐỘNG LƯU TRẠNG THÁI VÁN ĐẤU (SESSION PERSISTENCE):\n" +
                "   - Lưu tự động ván đấu dở dang vào game_session.json khi tắt game; khôi phục nguyên vẹn 100% khi mở lại.\n\n" +
                "4. BẢO TÀNG HUÂN CHƯƠNG 2 TAB:\n" +
                "   - Trưng bày 8 Huân chương Đại Mùa + 4 Huy chương Cao thủ cùng Thống kê sự nghiệp trọn đời.";

            Button btnBack = CreateBackButton("← QUAY LẠI MENU", new Point((card.Width - 240) / 2, 600), () =>
            {
                pnlAboutModal.Visible = false;
                ShowMainMenu();
            });

            card.Controls.Add(btnCloseTop);
            card.Controls.Add(lblTitle);
            card.Controls.Add(rtbInfo);
            card.Controls.Add(btnBack);

            pnlAboutModal.Controls.Add(card);
            this.Controls.Add(pnlAboutModal);
        }

        // ================================================================
        //  ĐIỀU HƯỚNG HIỂN THỊ MODAL & MENU
        // ================================================================
        public void LayoutMenuModals()
        {
            if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0) return;

            CenterModalCard(pnlMainMenu, "pnlMenuCenterCard");
            CenterModalCard(pnlSettingsModal, "pnlSettingsCard");
            CenterModalCard(pnlLeaderboardModal, "pnlLeaderboardCard");
            CenterModalCard(pnlAboutModal, "pnlAboutCard");
            CenterModalCard(pnlStorySelectModal, "pnlStorySelectCard");
            CenterModalCard(pnlChangeUserModal, "pnlChangeUserCard");
            CenterModalCard(pnlStoryBriefingModal, "pnlStoryBriefingCard");
            CenterModalCard(pnlStoryVictoryModal, "pnlStoryVictoryCard");
            CenterModalCard(pnlStoryDefeatModal, "pnlStoryDefeatCard");
        }

        private static void CenterModalCard(Panel overlay, string cardName)
        {
            if (overlay == null) return;
            Control[] found = overlay.Controls.Find(cardName, false);
            if (found.Length > 0)
            {
                Control card = found[0];
                int x = Math.Max(0, (overlay.ClientSize.Width - card.Width) / 2);
                int y = Math.Max(0, (overlay.ClientSize.Height - card.Height) / 2);
                card.Location = new Point(x, y);
            }
        }

        public void HideAllOverlays()
        {
            if (pnlMainMenu != null) pnlMainMenu.Visible = false;
            if (pnlSettingsModal != null) pnlSettingsModal.Visible = false;
            if (pnlLeaderboardModal != null) pnlLeaderboardModal.Visible = false;
            if (pnlAboutModal != null) pnlAboutModal.Visible = false;
            if (pnlStorySelectModal != null) pnlStorySelectModal.Visible = false;
            if (pnlChangeUserModal != null) pnlChangeUserModal.Visible = false;
            if (pnlStoryBriefingModal != null) pnlStoryBriefingModal.Visible = false;
            if (pnlStoryVictoryModal != null) pnlStoryVictoryModal.Visible = false;
            if (pnlStoryDefeatModal != null) pnlStoryDefeatModal.Visible = false;
        }

        private void PrepareModalDisplay()
        {
            gameManager.IsPaused = true;
            CancelDrag();
            HideAllOverlays();
            pnlHeader.Visible = false;
            pnlDock.Visible = false;
            pnlBoard.Visible = false;
            if (lblTutorialGuide != null) lblTutorialGuide.Visible = false;
            if (btnSkipTutorial != null) btnSkipTutorial.Visible = false;
        }

        public void ResumeGameplayUI()
        {
            HideAllOverlays();
            pnlHeader.Visible = true;
            pnlDock.Visible = true;
            pnlBoard.Visible = true;
            if (lblTutorialGuide != null) lblTutorialGuide.Visible = true;
            pnlBoard.Invalidate();
        }

        public void ShowMainMenu()
        {
            PrepareModalDisplay();
            UpdateUserCardInfo();
            UpdateMainMenuLayout();

            pnlMainMenu.Visible = true;
            pnlMainMenu.BringToFront();
            LayoutMenuModals();
        }

        public void HideMainMenu()
        {
            ResumeGameplayUI();
        }

        private void UpdateUserCardInfo()
        {
            if (UserManager.CurrentUser != null)
            {
                lblUserCardName.Text = $"👤 Người chơi: {UserManager.CurrentUser.Username}";
                lblUserCardStats.Text = $"🏆 Kỷ lục: {UserManager.CurrentUser.BestScore:#,##0}  |  🌟 Mùa: {UserManager.CurrentUser.HighestSeasonUnlocked}/8  |  🎯 Màn: {UserManager.CurrentUser.TotalChallengesWon}/96";
            }
        }

        public void ShowLeaderboardModal()
        {
            PrepareModalDisplay();
            SwitchAchievementTab(1); // Mặc định mở Tab Huân Chương
            pnlLeaderboardModal.Visible = true;
            pnlLeaderboardModal.BringToFront();
            LayoutMenuModals();
        }

        public void ShowAboutModal()
        {
            PrepareModalDisplay();
            pnlAboutModal.Visible = true;
            pnlAboutModal.BringToFront();
            LayoutMenuModals();
        }

        public void ShowStorySelectModal()
        {
            PrepareModalDisplay();
            currentCarouselSeason = UserManager.CurrentUser != null ? Math.Min(8, UserManager.CurrentUser.HighestSeasonUnlocked) : 1;
            UpdateSeasonCarouselUI();
            pnlStorySelectModal.Visible = true;
            pnlStorySelectModal.BringToFront();
            LayoutMenuModals();
        }

        public void ShowChangeUserModal()
        {
            PrepareModalDisplay();

            cboExistingUsers.Items.Clear();
            foreach (var u in UserManager.Users)
            {
                cboExistingUsers.Items.Add(u.Username);
            }
            if (UserManager.CurrentUser != null)
            {
                cboExistingUsers.SelectedItem = UserManager.CurrentUser.Username;
                txtNewUserName.Text = UserManager.CurrentUser.Username;
            }

            pnlChangeUserModal.Visible = true;
            pnlChangeUserModal.BringToFront();
            LayoutMenuModals();
        }

        public void ShowStoryBriefingModal(int seasonId, int levelId)
        {
            PrepareModalDisplay();
            currentBriefingSeason = seasonId;
            currentBriefingLevel = levelId;

            var challenge = SeasonManager.GetChallenge(seasonId, levelId);

            lblBriefingTitle.Text = $"{challenge.Icon} {challenge.SeasonTitle.ToUpper()}";
            lblBriefingTitle.ForeColor = challenge.AccentColor;
            lblBriefingSubtitle.Text = challenge.LevelTitle.ToUpper();
            rtbBriefingQuote.Text = challenge.BriefingQuote;
            lblBriefingTarget.Text = $"Kích nổ: {challenge.TargetLines} Hàng / Cột";
            lblBriefingMoves.Text = $"Tối đa: {challenge.MaxMoves} Lượt đi";

            btnBriefingStart.BackColor = challenge.AccentColor;
            btnBriefingStart.ForeColor = (seasonId == 8) ? Color.FromArgb(18, 21, 32) : Color.White;

            pnlStoryBriefingModal.Visible = true;
            pnlStoryBriefingModal.BringToFront();
            LayoutMenuModals();
        }

        public void ShowStoryVictoryModal(GameChallenge challenge)
        {
            PrepareModalDisplay();
            currentVictorySeason = challenge.SeasonId;
            currentVictoryLevel = challenge.LevelId;

            lblVictoryTitle.Text = $"🎉 {challenge.Icon} CHIẾN THẮNG NGUYÊN TỐ!";
            lblVictorySubtitle.Text = $"{challenge.SeasonTitle.ToUpper()} - {challenge.LevelTitle.ToUpper()}";
            rtbVictoryLore.Text = challenge.VictoryLore;
            lblVictoryStats.Text = $"🎯 Nổ thành công: {challenge.LinesCleared}/{challenge.TargetLines} Dòng  |  ⏱ Nước đi còn lại: {challenge.MovesLeft}";

            if (challenge.LevelId < SeasonManager.LEVELS_PER_SEASON)
            {
                btnVictoryNext.Text = $"⚡ TIẾP TỤC MÀN {challenge.LevelId + 1}";
                btnVictoryNext.BackColor = Color.FromArgb(46, 204, 113);
            }
            else if (challenge.SeasonId < SeasonManager.TOTAL_SEASONS)
            {
                btnVictoryNext.Text = $"👑 TIẾN VÀO MÙA {challenge.SeasonId + 1}";
                btnVictoryNext.BackColor = Color.FromArgb(255, 196, 77);
            }
            else
            {
                btnVictoryNext.Text = "👑 KHẢI HOÀN (BẢO TÀNG HUÂN CHƯƠNG)";
                btnVictoryNext.BackColor = Color.FromArgb(241, 242, 246);
            }

            pnlStoryVictoryModal.Visible = true;
            pnlStoryVictoryModal.BringToFront();
            LayoutMenuModals();
        }

        public void ShowStoryDefeatModal(GameChallenge challenge, string reason)
        {
            PrepareModalDisplay();
            currentDefeatSeason = challenge.SeasonId;
            currentDefeatLevel = challenge.LevelId;

            lblDefeatTitle.Text = $"⚠️ {challenge.Icon} PHONG ẤN THẤT BẠI!";
            lblDefeatReason.Text = $"Lý do: {reason} ({challenge.LinesCleared}/{challenge.TargetLines} Dòng)";

            pnlStoryDefeatModal.Visible = true;
            pnlStoryDefeatModal.BringToFront();
            LayoutMenuModals();
        }
    }
}
