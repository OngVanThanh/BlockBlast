using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBlast
{
    public partial class fGamePlay : Form
    {
        private readonly GameManager gameManager = new GameManager();

        // Trạng thái Drag - Drop
        private int[,] draggingShape = null;
        private int draggingColorIndex = 0;
        private Panel activeDockPanel = null;
        private int hoverRow = -1;
        private int hoverCol = -1;

        public fGamePlay()
        {
            InitializeComponent();
        }

        private void fGamePlay_Load(object sender, EventArgs e)
        {
            // Căn giữa UI
            int boardX = (this.ClientSize.Width - pnlBoard.Width) / 2;
            int availableHeight = this.ClientSize.Height - pnlHeader.Height - pnlDock.Height;
            int boardY = pnlHeader.Height + (availableHeight - pnlBoard.Height) / 2;
            pnlBoard.Location = new Point(boardX, boardY);

            pnlGameOver.Left = (this.ClientSize.Width - pnlGameOver.Width) / 2;
            pnlGameOver.Top = (this.ClientSize.Height - pnlGameOver.Height) / 2;
            pnlGameOver.Visible = false;

            // Đăng ký sự kiện Mouse
            this.MouseMove += fGamePlay_MouseMove;
            pnlBoard.MouseMove += fGamePlay_MouseMove;
            this.MouseUp += fGamePlay_MouseUp;
            pnlBoard.MouseUp += fGamePlay_MouseUp;

            GenerateNewPieces();
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
            BoardRenderer.RenderBoard(e.Graphics, gameManager.Board, draggingShape, draggingColorIndex, hoverRow, hoverCol);
        }

        private void pnlDockPiece_Paint(object sender, PaintEventArgs e)
        {
            BoardRenderer.RenderDockPiece(e.Graphics, sender as Panel);
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
                    activeDockPanel.Visible = false;
                }
            }
        }

        private void fGamePlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingShape != null)
            {
                Point boardPoint = pnlBoard.PointToClient(Cursor.Position);
                int c = (boardPoint.X - BoardRenderer.MARGIN) / (BoardRenderer.CELL_SIZE + BoardRenderer.GAP);
                int r = (boardPoint.Y - BoardRenderer.MARGIN) / (BoardRenderer.CELL_SIZE + BoardRenderer.GAP);

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
            if (e.Button == MouseButtons.Left && draggingShape != null)
            {
                if (hoverRow != -1 && hoverCol != -1)
                {
                    // Dat gach vao ban chơi
                    gameManager.Board.PlaceShape(draggingShape, draggingColorIndex, hoverRow, hoverCol);

                    // Xoa gach tai Dock
                    activeDockPanel.Tag = null;
                    activeDockPanel.Visible = false;

                    // Cong diem dat gach
                    int placedBlocks = 0;
                    foreach (int cell in draggingShape) if (cell != 0) placedBlocks++;
                    gameManager.AddScore(placedBlocks * 10);

                    // Kiem tra an dong/cot
                    int lineScore = gameManager.Board.ClearAndScoreLines();
                    if (lineScore > 0) gameManager.AddScore(lineScore);

                    // Cap nhat UI diem so
                    lblScore.Text = gameManager.CurrentScore.ToString();
                    lblBestScore.Text = gameManager.HighScore.ToString();

                    // Tao 3 khoi moi neu da dung het
                    if (pnlDockPiece1.Tag == null && pnlDockPiece2.Tag == null && pnlDockPiece3.Tag == null)
                    {
                        GenerateNewPieces();
                    }

                    // Kiem tra GameOver
                    CheckGameOver();
                }
                else
                {
                    if (activeDockPanel != null) activeDockPanel.Visible = true;
                }

                ResetDragState();
                pnlBoard.Invalidate();
            }
        }

        private void ResetDragState()
        {
            draggingShape = null;
            draggingColorIndex = 0;
            activeDockPanel = null;
            hoverRow = -1;
            hoverCol = -1;
        }

        private void CheckGameOver()
        {
            List<BlockPiece> remaining = new List<BlockPiece>();
            if (pnlDockPiece1.Tag != null) remaining.Add((BlockPiece)pnlDockPiece1.Tag);
            if (pnlDockPiece2.Tag != null) remaining.Add((BlockPiece)pnlDockPiece2.Tag);
            if (pnlDockPiece3.Tag != null) remaining.Add((BlockPiece)pnlDockPiece3.Tag);

            if (gameManager.CheckGameOver(remaining))
            {
                pnlGameOver.Visible = true;
                pnlGameOver.BringToFront();
                lblFinalScore.Text = $"Điểm của bạn: {gameManager.CurrentScore}";
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            gameManager.IsPaused = !gameManager.IsPaused;
            btnPause.Text = gameManager.IsPaused ? "▶" : "⏸";
            pnlBoard.Enabled = !gameManager.IsPaused;
            pnlDock.Enabled = !gameManager.IsPaused;
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            gameManager.ResetGame();
            lblScore.Text = "0";
            pnlGameOver.Visible = false;
            GenerateNewPieces();
            pnlBoard.Invalidate();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn dừng chơi và thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}