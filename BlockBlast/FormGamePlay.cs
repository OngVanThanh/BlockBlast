using BlockBlast.Module;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockBlast
{
    public partial class fGamePlay : Form
    {
        // Smart Drag Offset: khối gạch được nâng lên trên con trỏ để không bị che tầm nhìn
        private const int LIFT_OFFSET_Y = 35;
        // Khoảng chừa quanh bitmap khối gạch đang bay (dành cho bóng đổ)
        private const int GHOST_PADDING = 12;

        private readonly GameManager gameManager = new GameManager();
        private readonly TutorialManager tutorialManager = new TutorialManager();
        private readonly Random fxRandom = new Random();

        private int[,] draggingShape = null;
        private int draggingColorIndex = 0;
        private Panel activeDockPanel = null;
        private int hoverRow = -1;
        private int hoverCol = -1;
        private int dragOffsetX = 0;
        private int dragOffsetY = 0;

        // ===== Trạng thái hoạt họa & khóa tương tác =====
        private bool isProcessingMove = false;
        private readonly HashSet<Point> flashingCells = new HashSet<Point>(); // Point.X = cột, Point.Y = hàng
        private float flashScale = 1f;
        private string floatingComboText = "";
        private PointF floatingTextPos = PointF.Empty;
        private float floatingTextAlpha = 0f;
        private readonly Timer floatingTextTimer;

        // Lớp phủ vẽ khối gạch đang bay theo chuột ở mọi vị trí
        private DragGhostWindow dragGhost;

        private Label lblTutorialGuide;
        private int sessionGameInitialHighScore = 0;
        private bool isGameOver = false;

        public fGamePlay()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // tránh nháy nền khi rung bàn cờ

            floatingTextTimer = new Timer { Interval = 16 };
            floatingTextTimer.Tick += FloatingTextTimer_Tick;
        }

        private static string GetHighScoreFilePath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder = System.IO.Path.Combine(appData, "BlockBlast");
            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }
            return System.IO.Path.Combine(folder, "highscore.dat");
        }

        private void LoadSavedHighScore()
        {
            try
            {
                string path = GetHighScoreFilePath();
                if (System.IO.File.Exists(path))
                {
                    string content = System.IO.File.ReadAllText(path);
                    if (int.TryParse(content.Trim(), out int saved) && saved > 0)
                    {
                        gameManager.HighScore = saved;
                    }
                }
            }
            catch { }
        }

        private void SaveHighScoreSilently()
        {
            try
            {
                string path = GetHighScoreFilePath();
                System.IO.File.WriteAllText(path, gameManager.HighScore.ToString());
            }
            catch { }
        }

        private static string GetSettingsFilePath()
        {
            string folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlockBlast");
            if (!System.IO.Directory.Exists(folder)) System.IO.Directory.CreateDirectory(folder);
            return System.IO.Path.Combine(folder, "settings.dat");
        }

        private void LoadAudioSettings()
        {
            try
            {
                string path = GetSettingsFilePath();
                if (System.IO.File.Exists(path))
                {
                    string txt = System.IO.File.ReadAllText(path).Trim();
                    SoundManager.IsMuted = (txt == "1" || txt.Equals("true", StringComparison.OrdinalIgnoreCase));
                }
            }
            catch { SoundManager.IsMuted = false; }
        }

        private void SaveAudioSettingsSilently()
        {
            try
            {
                System.IO.File.WriteAllText(GetSettingsFilePath(), SoundManager.IsMuted ? "1" : "0");
            }
            catch { }
        }

        private void fGamePlay_Load(object sender, EventArgs e)
        {
            LoadSavedHighScore();
            sessionGameInitialHighScore = gameManager.HighScore;
            LoadAudioSettings();
            btnSound.Text = SoundManager.IsMuted ? "LOA: TẮT" : "LOA: BẬT";

            if (btnReplay.Parent != pnlBoard)
            {
                pnlBoard.Controls.Add(btnReplay);
            }
            btnReplay.Visible = false;
            pnlGameOver.Visible = false;

            // 1. Khởi tạo Label hướng dẫn trước khi tính toán Layout
            lblTutorialGuide = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 196, 77),
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblTutorialGuide);

            // 2. Tính toán vị trí và bố cục hoàn hảo cho Bàn cờ, Dock và các control
            LayoutGameControls();
            this.Resize += (s, ev) =>
            {
                if (this.WindowState != FormWindowState.Minimized)
                {
                    LayoutGameControls();
                }
            };

            // 3. Đăng ký sự kiện Mouse & Paint
            // Paint/MouseDown/MouseMove của Dock, Paint của pnlBoard và MouseMove/MouseUp của Form
            // đã được nối trong Designer -> KHÔNG đăng ký lại (tránh handler bị gọi 2 lần).
            pnlBoard.MouseMove += fGamePlay_MouseMove;
            pnlBoard.MouseUp += fGamePlay_MouseUp;
            pnlDock.MouseMove += fGamePlay_MouseMove;
            pnlDock.MouseUp += fGamePlay_MouseUp;
            pnlHeader.MouseMove += fGamePlay_MouseMove;
            pnlHeader.MouseUp += fGamePlay_MouseUp;

            // Ô Dock giữ Mouse Capture suốt quá trình kéo nên MouseUp sẽ đến chính ô Dock đó
            pnlDockPiece1.MouseUp += fGamePlay_MouseUp;
            pnlDockPiece2.MouseUp += fGamePlay_MouseUp;
            pnlDockPiece3.MouseUp += fGamePlay_MouseUp;

            btnSound.Click += (s, ev) =>
            {
                SoundManager.IsMuted = !SoundManager.IsMuted;
                SaveAudioSettingsSilently();
                btnSound.Text = SoundManager.IsMuted ? "LOA: TẮT" : "LOA: BẬT";
            };

            btnReplay.Click += btnReplay_Click;
            btnPause.Click += btnPause_Click;
            btnExit.Click += btnExit_Click;

            // Mất focus (Alt+Tab...) giữa lúc kéo -> trả khối gạch về Dock
            this.Deactivate += (s, ev) => CancelDrag();
            this.FormClosed += fGamePlay_FormClosed;

            ApplyModernTheme();
            UpdateScoreLabels();

            // 4. Khởi tạo màn chơi
            if (tutorialManager.IsTutorialActive)
            {
                LoadPuzzleLevel();
            }
            else
            {
                lblTutorialGuide.Visible = false;
                GenerateNewPieces();
            }
        }

        private void LayoutGameControls()
        {
            if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0) return;

            // 1. Chuẩn hóa bàn cờ 8x8 hình vuông theo kích thước pixel chuẩn
            int cellSize = BoardRenderer.CalculateCellSize(new Size(500, 500));
            int boardPixelDim = BoardRenderer.GetBoardPixelSize(cellSize);
            pnlBoard.Size = new Size(boardPixelDim, boardPixelDim);

            // Căn giữa bàn cờ tuyệt đối giữa khoảng trống của Form
            int boardX = (this.ClientSize.Width - pnlBoard.Width) / 2;
            int availableHeight = this.ClientSize.Height - pnlHeader.Height - pnlDock.Height;
            int boardY = pnlHeader.Height + (availableHeight - pnlBoard.Height) / 2;
            pnlBoard.Location = new Point(boardX, boardY);

            // Căn giữa nhãn hướng dẫn theo bàn cờ
            if (lblTutorialGuide != null)
            {
                lblTutorialGuide.Size = new Size(pnlBoard.Width, 40);
                lblTutorialGuide.Location = new Point(boardX, Math.Max(pnlHeader.Height + 5, boardY - 45));
            }

            // 2. Căn giữa 3 ô Dock trên thanh pnlDock
            const int dockSlotSize = 140;
            const int dockSpacing = 30;
            const int totalDockWidth = dockSlotSize * 3 + dockSpacing * 2; // 480px
            int startX = (pnlDock.ClientSize.Width - totalDockWidth) / 2;
            int dockY = (pnlDock.ClientSize.Height - dockSlotSize) / 2;

            pnlDockPiece1.Size = new Size(dockSlotSize, dockSlotSize);
            pnlDockPiece2.Size = new Size(dockSlotSize, dockSlotSize);
            pnlDockPiece3.Size = new Size(dockSlotSize, dockSlotSize);

            pnlDockPiece1.Location = new Point(startX, dockY);
            pnlDockPiece2.Location = new Point(startX + dockSlotSize + dockSpacing, dockY);
            pnlDockPiece3.Location = new Point(startX + (dockSlotSize + dockSpacing) * 2, dockY);

            ApplyRoundedRegion(pnlDockPiece1, 16);
            ApplyRoundedRegion(pnlDockPiece2, 16);
            ApplyRoundedRegion(pnlDockPiece3, 16);

            // 3. Định vị btnReplay ở chính giữa phía dưới bàn cờ
            int btnW = 180;
            int btnH = 50;
            btnReplay.Size = new Size(btnW, btnH);
            btnReplay.Location = new Point((pnlBoard.Width - btnW) / 2, (int)(pnlBoard.Height * 0.60f));
            ApplyRoundedRegion(btnReplay, 14);

            pnlGameOver.Visible = false;
        }

        private void fGamePlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            floatingTextTimer.Stop();
            floatingTextTimer.Dispose();
            if (dragGhost != null && !dragGhost.IsDisposed) dragGhost.Dispose();
        }

        // ================================================================
        //  GIAO DIỆN DARK THEME
        // ================================================================

        private void ApplyModernTheme()
        {
            foreach (Button btn in new[] { btnSound, btnPause, btnExit, btnReplay })
            {
                btn.Cursor = Cursors.Hand;
                ApplyRoundedRegion(btn, 12);
            }

            foreach (Panel dock in new[] { pnlDockPiece1, pnlDockPiece2, pnlDockPiece3 })
            {
                dock.Cursor = Cursors.Hand;
                ApplyRoundedRegion(dock, 16);
            }

            pnlScoreGroup.Paint += pnlScoreGroup_Paint;
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            using (GraphicsPath path = BoardRenderer.CreateRoundedPath(new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                control.Region = new Region(path);
            }
        }

        // Khung điểm bo góc tối sang trọng (#1E2235), các Label bên trong có nền trong suốt
        private void pnlScoreGroup_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, pnlScoreGroup.Width - 1, pnlScoreGroup.Height - 1);
            using (GraphicsPath path = BoardRenderer.CreateRoundedPath(rect, 14))
            using (Brush bg = new SolidBrush(Color.FromArgb(30, 34, 53)))
            using (Pen border = new Pen(Color.FromArgb(48, 54, 80)))
            {
                g.FillPath(bg, path);
                g.DrawPath(border, path);
            }

            using (Pen separator = new Pen(Color.FromArgb(48, 54, 80)))
            {
                g.DrawLine(separator, 190, 16, 190, rect.Height - 16);
            }
        }

        private void UpdateScoreLabels()
        {
            lblScore.Text = gameManager.CurrentScore.ToString();
            lblBestScore.Text = gameManager.HighScore.ToString();
        }

        // ================================================================
        //  MÀN CHƠI
        // ================================================================

        private void LoadPuzzleLevel()
        {
            TutorialStep level = tutorialManager.GetCurrentLevel();
            if (level == null) return;

            if (lblTutorialGuide != null)
            {
                lblTutorialGuide.Text = $"{level.Title}: {level.Instruction}";
                lblTutorialGuide.Visible = true;
            }

            // Đồng bộ bàn chơi với GameManager
            gameManager.Board.SetGrid(level.BoardLayout);

            pnlDockPiece1.Tag = null;
            pnlDockPiece1.Visible = true;
            pnlDockPiece1.Invalidate();

            pnlDockPiece3.Tag = null;
            pnlDockPiece3.Visible = true;
            pnlDockPiece3.Invalidate();

            pnlDockPiece2.Tag = new BlockPiece(level.DockPieceShape, level.ColorIndex);
            pnlDockPiece2.Visible = true;
            pnlDockPiece2.Invalidate();

            pnlBoard.Invalidate();
        }

        private void GenerateNewPieces()
        {
            SetupDockPiece(pnlDockPiece1);
            SetupDockPiece(pnlDockPiece2);
            SetupDockPiece(pnlDockPiece3);
        }

        private void SetupDockPiece(Panel dockPanel)
        {
            dockPanel.Tag = gameManager.CreateRandomPiece();
            dockPanel.Visible = true;
            dockPanel.Invalidate();
        }

        // ================================================================
        //  PAINT
        // ================================================================

        private void pnlBoard_Paint(object sender, PaintEventArgs e)
        {
            BoardRenderer.RenderBoard(
                e.Graphics,
                pnlBoard.Size,
                gameManager.Board,
                draggingShape,
                draggingColorIndex,
                hoverRow,
                hoverCol,
                flashingCells,
                flashScale
            );

            if (floatingTextAlpha > 0f)
            {
                BoardRenderer.RenderFloatingText(e.Graphics, floatingComboText, floatingTextPos, floatingTextAlpha);
            }

            if (gameManager.IsPaused)
            {
                BoardRenderer.RenderPausedOverlay(e.Graphics, pnlBoard.Size);
            }

            if (isGameOver)
            {
                int current = gameManager.CurrentScore;
                bool isNewRecord = current > sessionGameInitialHighScore && current > 0;
                BoardRenderer.RenderGameOverOverlay(e.Graphics, pnlBoard.Size, current, gameManager.HighScore, isNewRecord);
            }
        }

        private void pnlDockPiece_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = sender as Panel;
            if (pnl == null || !pnl.Visible) return;

            // Ô Dock của khối đang kéo để trống - hiển thị viền ô trống mờ
            if (pnl == activeDockPanel && draggingShape != null)
            {
                BoardRenderer.RenderEmptyDockSlot(e.Graphics, pnl.ClientRectangle);
                return;
            }

            BoardRenderer.RenderDockPiece(e.Graphics, pnl);
        }

        // ================================================================
        //  KÉO - THẢ (SMART DRAG + MAGNETIC SNAPPING)
        // ================================================================

        private void pnlDockPiece_MouseDown(object sender, MouseEventArgs e)
        {
            if (isGameOver || isProcessingMove || gameManager.IsPaused) return;
            if (e.Button != MouseButtons.Left || draggingShape != null) return;

            Panel dock = sender as Panel;
            if (dock == null || dock.Tag == null) return;

            BlockPiece piece = (BlockPiece)dock.Tag;
            activeDockPanel = dock;
            draggingShape = piece.Shape;
            draggingColorIndex = piece.ColorIndex;
            SoundManager.PlayPickup();

            // Tâm khối gạch (kích thước thật trên bàn cờ, tính cả GAP) trùng với con trỏ
            int cellSize = BoardRenderer.CalculateCellSize(pnlBoard.Size);
            Size pieceSize = BoardRenderer.GetPieceSize(draggingShape, cellSize);
            dragOffsetX = pieceSize.Width / 2;
            dragOffsetY = pieceSize.Height / 2;

            // KHÔNG ẩn ô Dock (ẩn control đang giữ capture sẽ làm mất MouseMove/MouseUp);
            // chỉ ngừng vẽ khối trong ô Dock đó.
            dock.Capture = true;
            dock.Invalidate();

            ShowDragGhost(cellSize);
            UpdateDragPosition();
        }

        private void fGamePlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (isGameOver || isProcessingMove || gameManager.IsPaused) return;
            if (draggingShape == null) return;

            UpdateDragPosition();
        }

        // Cập nhật vị trí khối gạch đang bay + ô hút nam châm trên bàn cờ
        private void UpdateDragPosition()
        {
            Point boardPoint = pnlBoard.PointToClient(Cursor.Position);

            // Smart offset: Nâng khối gạch lên trên ngón tay/chuột 35px
            Point topLeft = new Point(boardPoint.X - dragOffsetX, boardPoint.Y - dragOffsetY - LIFT_OFFSET_Y);

            // Khối gạch LUÔN bám theo chuột - kể cả ngoài bàn cờ hoặc trên ô không hợp lệ
            MoveDragGhost(pnlBoard.PointToScreen(topLeft));

            int newRow = -1, newCol = -1;
            if (BoardRenderer.GetMagneticCellAt(topLeft, pnlBoard.Size, out int r, out int c) &&
                gameManager.Board.CanPlaceShape(draggingShape, r, c))
            {
                newRow = r;
                newCol = c;
            }

            // Chỉ vẽ lại bàn cờ khi ô hover thực sự thay đổi
            if (newRow != hoverRow || newCol != hoverCol)
            {
                hoverRow = newRow;
                hoverCol = newCol;
                pnlBoard.Invalidate();
            }
        }

        private async void fGamePlay_MouseUp(object sender, MouseEventArgs e)
        {
            if (draggingShape == null) return;
            if (isGameOver || isProcessingMove || gameManager.IsPaused)
            {
                CancelDrag();
                return;
            }
            if (e.Button != MouseButtons.Left) return;

            int[,] shapeToPlace = draggingShape;
            int colorToPlace = draggingColorIndex;
            Panel currentDock = activeDockPanel;
            int targetRow = hoverRow;
            int targetCol = hoverCol;

            EndDrag();

            if (targetRow == -1 || targetCol == -1 || !gameManager.Board.CanPlaceShape(shapeToPlace, targetRow, targetCol))
            {
                // Nếu thả không hợp lệ thì trả lại Dock
                if (currentDock != null) currentDock.Invalidate();
                pnlBoard.Invalidate();
                return;
            }

            // Khóa mọi tương tác chuột cho đến khi hoạt họa kết thúc
            isProcessingMove = true;
            try
            {
                // 1. Thả gạch vào GameManager.Board
                gameManager.Board.PlaceShape(shapeToPlace, colorToPlace, targetRow, targetCol);
                SoundManager.PlayDrop();

                if (currentDock != null)
                {
                    currentDock.Tag = null;
                    currentDock.Invalidate();
                }

                pnlBoard.Refresh();

                // 2. Nổ hàng/cột (chớp sáng, rung, chữ nổi) và cộng điểm
                if (!await AnimateLineClearAsync()) return;

                // 3. Chuyển màn Tutorial / Game Thường
                if (tutorialManager.IsTutorialActive)
                {
                    if (tutorialManager.NextLevel())
                    {
                        if (!await DelayAsync(300)) return;
                        LoadPuzzleLevel();
                    }
                    else
                    {
                        // HOÀN THÀNH TUTORIAL
                        if (lblTutorialGuide != null) lblTutorialGuide.Visible = false;

                        if (!await DelayAsync(300)) return;

                        // Reset sạch sẽ bàn chơi qua GameManager
                        gameManager.ResetGame();
                        UpdateScoreLabels();

                        GenerateNewPieces();

                        pnlBoard.Invalidate();
                    }
                }
                else
                {
                    if (pnlDockPiece1.Tag == null && pnlDockPiece2.Tag == null && pnlDockPiece3.Tag == null)
                    {
                        GenerateNewPieces();
                    }
                    CheckGameOver();
                }
            }
            finally
            {
                isProcessingMove = false;
            }
        }

        // Kết thúc kéo: ẩn khối đang bay, nhả capture, xóa trạng thái kéo
        private void EndDrag()
        {
            HideDragGhost();
            if (activeDockPanel != null && activeDockPanel.Capture) activeDockPanel.Capture = false;

            draggingShape = null;
            activeDockPanel = null;
            hoverRow = -1;
            hoverCol = -1;
        }

        // Hủy kéo và trả khối gạch về Dock
        private void CancelDrag()
        {
            if (draggingShape == null) return;

            Panel dock = activeDockPanel;
            EndDrag();
            if (dock != null) dock.Invalidate();
            pnlBoard.Invalidate();
        }

        private void ShowDragGhost(int cellSize)
        {
            if (dragGhost == null || dragGhost.IsDisposed)
            {
                dragGhost = new DragGhostWindow { Owner = this };
            }

            using (Bitmap bmp = BoardRenderer.CreateFloatingPieceBitmap(draggingShape, draggingColorIndex, cellSize, GHOST_PADDING))
            {
                Point boardPoint = pnlBoard.PointToClient(Cursor.Position);
                Point topLeft = pnlBoard.PointToScreen(new Point(boardPoint.X - dragOffsetX, boardPoint.Y - dragOffsetY - LIFT_OFFSET_Y));
                dragGhost.SetBitmap(bmp, new Point(topLeft.X - GHOST_PADDING, topLeft.Y - GHOST_PADDING));
            }

            if (!dragGhost.Visible) dragGhost.Show();
        }

        private void MoveDragGhost(Point pieceTopLeftOnScreen)
        {
            if (dragGhost == null || dragGhost.IsDisposed || !dragGhost.Visible) return;
            dragGhost.MoveTo(new Point(pieceTopLeftOnScreen.X - GHOST_PADDING, pieceTopLeftOnScreen.Y - GHOST_PADDING));
        }

        private void HideDragGhost()
        {
            if (dragGhost != null && !dragGhost.IsDisposed && dragGhost.Visible) dragGhost.Hide();
        }

        // ================================================================
        //  HOẠT HỌA NỔ DÒNG / RUNG MÀN HÌNH / CHỮ NỔI
        // ================================================================

        // Trả về false nếu form đã bị đóng giữa chừng hoạt họa
        private async Task<bool> AnimateLineClearAsync()
        {
            List<int> fullRows = new List<int>();
            List<int> fullCols = new List<int>();
            gameManager.Board.FindFullLines(fullRows, fullCols);

            int linesCleared = fullRows.Count + fullCols.Count;
            if (linesCleared == 0) return true;

            int scoreGained = GameBoard.CalculateLineScore(linesCleared);

            // a. Chớp sáng trắng
            foreach (int r in fullRows)
                for (int c = 0; c < GameBoard.BOARD_SIZE; c++) flashingCells.Add(new Point(c, r));
            foreach (int c in fullCols)
                for (int r = 0; r < GameBoard.BOARD_SIZE; r++) flashingCells.Add(new Point(c, r));

            SoundManager.PlayLineClear(linesCleared);
            flashScale = 1f;
            pnlBoard.Refresh();
            if (!await DelayAsync(120)) return false;

            // b. Rung màn hình khi ăn Combo (>= 2 dòng)
            if (linesCleared >= 2 && !await ShakeBoardAsync()) return false;

            // c. Thu nhỏ & tan biến
            for (int i = 1; i <= 5; i++)
            {
                flashScale = 1f - i / 5f;
                pnlBoard.Refresh();
                if (!await DelayAsync(18)) return false;
            }

            // d. Chữ nổi Combo trôi lên
            ShowFloatingText(BuildComboText(linesCleared, scoreGained), GetCellsCenter(flashingCells));

            // e. Xóa dòng khỏi GameBoard, cộng điểm, cập nhật giao diện
            gameManager.Board.ClearLines(fullRows, fullCols);
            flashingCells.Clear();
            flashScale = 1f;

            gameManager.AddScore(scoreGained);
            SaveHighScoreSilently();
            UpdateScoreLabels();
            pnlBoard.Refresh();
            return true;
        }

        private async Task<bool> ShakeBoardAsync()
        {
            Point origin = pnlBoard.Location;
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    int dx = fxRandom.Next(3, 6) * (fxRandom.Next(2) == 0 ? -1 : 1);
                    int dy = fxRandom.Next(3, 6) * (fxRandom.Next(2) == 0 ? -1 : 1);
                    pnlBoard.Location = new Point(origin.X + dx, origin.Y + dy);
                    if (!await DelayAsync(25)) return false;
                }
            }
            finally
            {
                if (!pnlBoard.IsDisposed) pnlBoard.Location = origin;
            }
            return true;
        }

        private static string BuildComboText(int linesCleared, int scoreGained)
        {
            if (linesCleared >= 4) return "MEGA BLAST!\n+" + scoreGained;
            if (scoreGained >= 200) return "COMBO x" + linesCleared + "!\n+" + scoreGained;
            return "+" + scoreGained;
        }

        private PointF GetCellsCenter(IEnumerable<Point> cells)
        {
            int cellSize = BoardRenderer.CalculateCellSize(pnlBoard.Size);
            float sumX = 0, sumY = 0;
            int count = 0;
            foreach (Point p in cells)
            {
                Rectangle rect = BoardRenderer.GetCellRect(p.Y, p.X, cellSize);
                sumX += rect.X + rect.Width / 2f;
                sumY += rect.Y + rect.Height / 2f;
                count++;
            }

            int boardDim = BoardRenderer.GetBoardPixelSize(cellSize);
            if (count == 0) return new PointF(boardDim / 2f, boardDim / 2f);

            // Giữ chữ nằm trọn trong bàn cờ
            float x = Math.Max(120f, Math.Min(boardDim - 120f, sumX / count));
            float y = Math.Max(110f, Math.Min(boardDim - 50f, sumY / count));
            return new PointF(x, y);
        }

        private void ShowFloatingText(string text, PointF center)
        {
            floatingComboText = text;
            floatingTextPos = center;
            floatingTextAlpha = 1.6f; // > 1: giữ nguyên độ đậm một nhịp rồi mới mờ dần
            floatingTextTimer.Start();
        }

        private void FloatingTextTimer_Tick(object sender, EventArgs e)
        {
            floatingTextPos = new PointF(floatingTextPos.X, floatingTextPos.Y - 1.2f);
            floatingTextAlpha -= 0.03f;

            if (floatingTextAlpha <= 0f)
            {
                floatingTextAlpha = 0f;
                floatingComboText = "";
                floatingTextTimer.Stop();
            }
            pnlBoard.Invalidate();
        }

        private async Task<bool> DelayAsync(int milliseconds)
        {
            await Task.Delay(milliseconds);
            return !IsDisposed && !Disposing;
        }

        // ================================================================
        //  GAME OVER & NÚT ĐIỀU KHIỂN
        // ================================================================

        private void CheckGameOver()
        {
            List<BlockPiece> remainingPieces = new List<BlockPiece>();

            if (pnlDockPiece1.Tag != null)
                remainingPieces.Add((BlockPiece)pnlDockPiece1.Tag);

            if (pnlDockPiece2.Tag != null)
                remainingPieces.Add((BlockPiece)pnlDockPiece2.Tag);

            if (pnlDockPiece3.Tag != null)
                remainingPieces.Add((BlockPiece)pnlDockPiece3.Tag);

            if (gameManager.CheckGameOver(remainingPieces))
            {
                ShowGameOverModal();
            }
        }

        private void ShowGameOverModal()
        {
            CancelDrag();
            isGameOver = true;
            int current = gameManager.CurrentScore;
            if (current > gameManager.HighScore) gameManager.HighScore = current;
            SaveHighScoreSilently();
            UpdateScoreLabels();
            SoundManager.PlayGameOver();
            pnlGameOver.Visible = false;
            btnReplay.Visible = true;
            btnReplay.BringToFront();
            pnlBoard.Invalidate();
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            if (isProcessingMove) return;
            CancelDrag();

            isGameOver = false;
            btnReplay.Visible = false;
            pnlGameOver.Visible = false;
            gameManager.ResetGame();
            gameManager.IsPaused = false;
            btnPause.Text = "Tam Dung";
            sessionGameInitialHighScore = gameManager.HighScore;

            floatingTextTimer.Stop();
            floatingTextAlpha = 0f;
            floatingComboText = "";

            lblScore.Text = "0";
            UpdateScoreLabels();
            GenerateNewPieces();
            pnlBoard.Invalidate();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            CancelDrag();
            gameManager.IsPaused = !gameManager.IsPaused;
            btnPause.Text = gameManager.IsPaused ? "Tiep Tuc" : "Tam Dung";
            pnlBoard.Invalidate();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CancelDrag();
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát game không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}