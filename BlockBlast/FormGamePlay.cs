using BlockBlast.Module;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBlast
{
    public partial class fGamePlay : Form
    {
        private readonly GameManager gameManager = new GameManager();
        private readonly TutorialManager tutorialManager = new TutorialManager();

        private int[,] draggingShape = null;
        private int draggingColorIndex = 0;
        private Panel activeDockPanel = null;
        private int hoverRow = -1;
        private int hoverCol = -1;
        private int dragOffsetX = 0;
        private int dragOffsetY = 0;

        private Label lblTutorialGuide;

        public fGamePlay()
        {
            InitializeComponent();
        }

        private void fGamePlay_Load(object sender, EventArgs e)
        {
            // 1. Căn giữa giao diện bàn chơi
            int boardX = (this.ClientSize.Width - pnlBoard.Width) / 2;
            int availableHeight = this.ClientSize.Height - pnlHeader.Height - pnlDock.Height;
            int boardY = pnlHeader.Height + (availableHeight - pnlBoard.Height) / 2;
            pnlBoard.Location = new Point(boardX, boardY);

            // 2. Khởi tạo Label hướng dẫn
            lblTutorialGuide = new Label
            {
                AutoSize = false,
                Size = new Size(pnlBoard.Width, 40),
                Location = new Point(boardX, boardY - 45),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkOrange,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblTutorialGuide);

            // 3. Đăng ký sự kiện Mouse & Paint
            this.MouseMove += fGamePlay_MouseMove;
            pnlBoard.MouseMove += fGamePlay_MouseMove;
            this.MouseUp += fGamePlay_MouseUp;
            pnlBoard.MouseUp += fGamePlay_MouseUp;
            pnlBoard.Paint += pnlBoard_Paint;

            pnlDockPiece1.Paint += pnlDockPiece_Paint;
            pnlDockPiece2.Paint += pnlDockPiece_Paint;
            pnlDockPiece3.Paint += pnlDockPiece_Paint;

            pnlDockPiece1.MouseDown += pnlDockPiece_MouseDown;
            pnlDockPiece2.MouseDown += pnlDockPiece_MouseDown;
            pnlDockPiece3.MouseDown += pnlDockPiece_MouseDown;

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

            pnlDockPiece1.Visible = false;
            pnlDockPiece3.Visible = false;

            pnlDockPiece2.Tag = new BlockPiece(level.DockPieceShape, level.ColorIndex);
            pnlDockPiece2.Visible = true;

            pnlBoard.Invalidate();
            pnlDockPiece2.Invalidate();
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

        private void pnlBoard_Paint(object sender, PaintEventArgs e)
        {
            // Bỏ dragOffsetX và dragOffsetY, chỉ truyền 7 tham số
            BoardRenderer.RenderBoard(
                e.Graphics,
                pnlBoard.Size,
                gameManager.Board,
                draggingShape,
                draggingColorIndex,
                hoverRow,
                hoverCol
            );
        }

        private void pnlDockPiece_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = sender as Panel;
            if (pnl != null && pnl.Visible)
            {
                BoardRenderer.RenderDockPiece(e.Graphics, pnl);
            }
        }

        private void pnlDockPiece_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                activeDockPanel = sender as Panel;
                if (activeDockPanel != null && activeDockPanel.Tag != null)
                {
                    BlockPiece piece = (BlockPiece)activeDockPanel.Tag;
                    draggingShape = piece.Shape;
                    draggingColorIndex = piece.ColorIndex;

                    int rows = draggingShape.GetLength(0);
                    int cols = draggingShape.GetLength(1);
                    int tileSize = pnlBoard.Width / GameBoard.BOARD_SIZE;

                    dragOffsetX = (cols * tileSize) / 2;
                    dragOffsetY = (rows * tileSize) / 2;

                    activeDockPanel.Visible = false;
                    pnlBoard.Invalidate();
                }
            }
        }

        private void fGamePlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingShape != null)
            {
                Point boardPoint = pnlBoard.PointToClient(Cursor.Position);
                int tileSize = pnlBoard.Width / GameBoard.BOARD_SIZE;

                int topLeftX = boardPoint.X - dragOffsetX;
                int topLeftY = boardPoint.Y - dragOffsetY;

                int c = (topLeftX + tileSize / 2) / tileSize;
                int r = (topLeftY + tileSize / 2) / tileSize;

                if (r >= 0 && r < GameBoard.BOARD_SIZE && c >= 0 && c < GameBoard.BOARD_SIZE &&
                    gameManager.Board.CanPlaceShape(draggingShape, r, c))
                {
                    hoverRow = r;
                    hoverCol = c;
                }
                else
                {
                    hoverRow = -1;
                    hoverCol = -1;
                }

                pnlBoard.Invalidate();
            }
        }

        private void fGamePlay_MouseUp(object sender, MouseEventArgs e)
        {
            if (draggingShape == null) return;

            int[,] shapeToPlace = draggingShape;
            int colorToPlace = draggingColorIndex;
            Panel currentDock = activeDockPanel;

            draggingShape = null;
            activeDockPanel = null;

            if (hoverRow != -1 && hoverCol != -1)
            {
                // 1. Thả gạch vào GameManager.Board
                gameManager.Board.PlaceShape(shapeToPlace, colorToPlace, hoverRow, hoverCol);

                if (currentDock != null)
                {
                    currentDock.Tag = null;
                    currentDock.Visible = false;
                }

                // 2. Nổ hàng/cột và cộng điểm
                int scoreGained = gameManager.Board.ClearAndScoreLines();
                if (scoreGained > 0)
                {
                    gameManager.AddScore(scoreGained);
                    lblScore.Text = gameManager.CurrentScore.ToString();
                }

                hoverRow = -1;
                hoverCol = -1;

                pnlBoard.Refresh();

                // 3. Chuyển màn Tutorial / Game Thường
                if (tutorialManager.IsTutorialActive)
                {
                    if (tutorialManager.NextLevel())
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(300);
                        LoadPuzzleLevel();
                    }
                    else
                    {
                        // HOÀN THÀNH TUTORIAL
                        if (lblTutorialGuide != null) lblTutorialGuide.Visible = false;

                        Application.DoEvents();
                        System.Threading.Thread.Sleep(300);

                        // Reset sạch sẽ bàn chơi qua GameManager
                        gameManager.ResetGame();
                        lblScore.Text = gameManager.CurrentScore.ToString();

                        GenerateNewPieces();

                        pnlBoard.Invalidate();
                        pnlBoard.Refresh();
                    }
                }
                else
                {
                    if (!pnlDockPiece1.Visible && !pnlDockPiece2.Visible && !pnlDockPiece3.Visible)
                    {
                        GenerateNewPieces();
                    }
                    CheckGameOver();
                }
            }
            else
            {
                // Nếu thả không hợp lệ thì trả lại Dock
                if (currentDock != null)
                {
                    currentDock.Visible = true;
                    currentDock.Invalidate();
                }
                pnlBoard.Invalidate();
            }
        }

        private void CheckGameOver()
        {
            List<BlockPiece> remainingPieces = new List<BlockPiece>();

            if (pnlDockPiece1.Visible && pnlDockPiece1.Tag != null)
                remainingPieces.Add((BlockPiece)pnlDockPiece1.Tag);

            if (pnlDockPiece2.Visible && pnlDockPiece2.Tag != null)
                remainingPieces.Add((BlockPiece)pnlDockPiece2.Tag);

            if (pnlDockPiece3.Visible && pnlDockPiece3.Tag != null)
                remainingPieces.Add((BlockPiece)pnlDockPiece3.Tag);

            if (gameManager.CheckGameOver(remainingPieces))
            {
                pnlGameOver.Visible = true;
                pnlGameOver.BringToFront();
                lblFinalScore.Text = $"Điểm của bạn: {gameManager.CurrentScore}";
            }
        }
    }
}