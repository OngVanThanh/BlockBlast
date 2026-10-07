namespace BlockBlast
{
    partial class fGamePlay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlBoard = new BlockBlast.Module.BufferedPanel();
            this.pnlGameOver = new System.Windows.Forms.Panel();
            this.btnReplay = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblInputGuide = new System.Windows.Forms.Label();
            this.lblFinalScore = new System.Windows.Forms.Label();
            this.lblGameOverTitle = new System.Windows.Forms.Label();
            this.pnlDock = new System.Windows.Forms.Panel();
            this.pnlDockPiece3 = new System.Windows.Forms.Panel();
            this.pnlDockPiece2 = new System.Windows.Forms.Panel();
            this.pnlDockPiece1 = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnSound = new System.Windows.Forms.Button();
            this.pnlScoreGroup = new System.Windows.Forms.Panel();
            this.lblBestScore = new System.Windows.Forms.Label();
            this.lblBestTitle = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblScoreTitle = new System.Windows.Forms.Label();
            this.pnlBoard.SuspendLayout();
            this.pnlGameOver.SuspendLayout();
            this.pnlDock.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlScoreGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBoard
            // 
            this.pnlBoard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlBoard.BackColor = System.Drawing.Color.White;
            this.pnlBoard.Controls.Add(this.pnlGameOver);
            this.pnlBoard.Location = new System.Drawing.Point(715, 230);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(490, 568);
            this.pnlBoard.TabIndex = 0;
            this.pnlBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBoard_Paint);
            // 
            // pnlGameOver
            // 
            this.pnlGameOver.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlGameOver.Controls.Add(this.btnReplay);
            this.pnlGameOver.Controls.Add(this.btnSave);
            this.pnlGameOver.Controls.Add(this.txtName);
            this.pnlGameOver.Controls.Add(this.lblInputGuide);
            this.pnlGameOver.Controls.Add(this.lblFinalScore);
            this.pnlGameOver.Controls.Add(this.lblGameOverTitle);
            this.pnlGameOver.Location = new System.Drawing.Point(20, 137);
            this.pnlGameOver.Name = "pnlGameOver";
            this.pnlGameOver.Size = new System.Drawing.Size(450, 480);
            this.pnlGameOver.TabIndex = 1;
            this.pnlGameOver.Visible = false;
            // 
            // btnReplay
            // 
            this.btnReplay.Location = new System.Drawing.Point(239, 285);
            this.btnReplay.Name = "btnReplay";
            this.btnReplay.Size = new System.Drawing.Size(75, 23);
            this.btnReplay.TabIndex = 5;
            this.btnReplay.Text = "Replay";
            this.btnReplay.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(280, 200);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(145, 201);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 22);
            this.txtName.TabIndex = 3;
            // 
            // lblInputGuide
            // 
            this.lblInputGuide.AutoSize = true;
            this.lblInputGuide.Location = new System.Drawing.Point(50, 160);
            this.lblInputGuide.Name = "lblInputGuide";
            this.lblInputGuide.Size = new System.Drawing.Size(128, 16);
            this.lblInputGuide.TabIndex = 2;
            this.lblInputGuide.Text = "Nhap ky luc cua ban";
            // 
            // lblFinalScore
            // 
            this.lblFinalScore.AutoSize = true;
            this.lblFinalScore.Location = new System.Drawing.Point(50, 108);
            this.lblFinalScore.Name = "lblFinalScore";
            this.lblFinalScore.Size = new System.Drawing.Size(28, 16);
            this.lblFinalScore.TabIndex = 1;
            this.lblFinalScore.Text = "100";
            // 
            // lblGameOverTitle
            // 
            this.lblGameOverTitle.AutoSize = true;
            this.lblGameOverTitle.Location = new System.Drawing.Point(0, 30);
            this.lblGameOverTitle.Name = "lblGameOverTitle";
            this.lblGameOverTitle.Size = new System.Drawing.Size(87, 16);
            this.lblGameOverTitle.TabIndex = 0;
            this.lblGameOverTitle.Text = "GAME OVER";
            // 
            // pnlDock
            // 
            this.pnlDock.BackColor = System.Drawing.Color.Chocolate;
            this.pnlDock.Controls.Add(this.pnlDockPiece3);
            this.pnlDock.Controls.Add(this.pnlDockPiece2);
            this.pnlDock.Controls.Add(this.pnlDockPiece1);
            this.pnlDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDock.Location = new System.Drawing.Point(0, 853);
            this.pnlDock.Name = "pnlDock";
            this.pnlDock.Size = new System.Drawing.Size(1902, 180);
            this.pnlDock.TabIndex = 1;
            // 
            // pnlDockPiece3
            // 
            this.pnlDockPiece3.BackColor = System.Drawing.Color.Snow;
            this.pnlDockPiece3.Location = new System.Drawing.Point(1050, 15);
            this.pnlDockPiece3.Name = "pnlDockPiece3";
            this.pnlDockPiece3.Size = new System.Drawing.Size(150, 150);
            this.pnlDockPiece3.TabIndex = 2;
            this.pnlDockPiece3.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDockPiece_Paint);
            this.pnlDockPiece3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlDockPiece_MouseDown);
            this.pnlDockPiece3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fGamePlay_MouseMove);
            // 
            // pnlDockPiece2
            // 
            this.pnlDockPiece2.BackColor = System.Drawing.Color.Snow;
            this.pnlDockPiece2.Location = new System.Drawing.Point(880, 15);
            this.pnlDockPiece2.Name = "pnlDockPiece2";
            this.pnlDockPiece2.Size = new System.Drawing.Size(150, 150);
            this.pnlDockPiece2.TabIndex = 1;
            this.pnlDockPiece2.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDockPiece_Paint);
            this.pnlDockPiece2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlDockPiece_MouseDown);
            this.pnlDockPiece2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fGamePlay_MouseMove);
            // 
            // pnlDockPiece1
            // 
            this.pnlDockPiece1.BackColor = System.Drawing.Color.Snow;
            this.pnlDockPiece1.Location = new System.Drawing.Point(710, 15);
            this.pnlDockPiece1.Name = "pnlDockPiece1";
            this.pnlDockPiece1.Size = new System.Drawing.Size(150, 150);
            this.pnlDockPiece1.TabIndex = 0;
            this.pnlDockPiece1.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDockPiece_Paint);
            this.pnlDockPiece1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlDockPiece_MouseDown);
            this.pnlDockPiece1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fGamePlay_MouseMove);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.btnExit);
            this.pnlHeader.Controls.Add(this.btnPause);
            this.pnlHeader.Controls.Add(this.btnSound);
            this.pnlHeader.Controls.Add(this.pnlScoreGroup);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.White;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1902, 90);
            this.pnlHeader.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(1711, 16);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(152, 45);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseCompatibleTextRendering = true;
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.BackColor = System.Drawing.Color.White;
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.ForeColor = System.Drawing.Color.Black;
            this.btnPause.Location = new System.Drawing.Point(1553, 16);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(152, 45);
            this.btnPause.TabIndex = 2;
            this.btnPause.Text = "Tam Dung";
            this.btnPause.UseCompatibleTextRendering = true;
            this.btnPause.UseVisualStyleBackColor = false;
            // 
            // btnSound
            // 
            this.btnSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSound.BackColor = System.Drawing.Color.White;
            this.btnSound.FlatAppearance.BorderSize = 0;
            this.btnSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSound.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSound.ForeColor = System.Drawing.Color.Black;
            this.btnSound.Location = new System.Drawing.Point(1417, 16);
            this.btnSound.Name = "btnSound";
            this.btnSound.Size = new System.Drawing.Size(152, 45);
            this.btnSound.TabIndex = 1;
            this.btnSound.Text = "Am Thanh";
            this.btnSound.UseCompatibleTextRendering = true;
            this.btnSound.UseVisualStyleBackColor = false;
            // 
            // pnlScoreGroup
            // 
            this.pnlScoreGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(38)))), ((int)(((byte)(58)))));
            this.pnlScoreGroup.Controls.Add(this.lblBestScore);
            this.pnlScoreGroup.Controls.Add(this.lblBestTitle);
            this.pnlScoreGroup.Controls.Add(this.lblScore);
            this.pnlScoreGroup.Controls.Add(this.lblScoreTitle);
            this.pnlScoreGroup.ForeColor = System.Drawing.Color.White;
            this.pnlScoreGroup.Location = new System.Drawing.Point(86, 12);
            this.pnlScoreGroup.Name = "pnlScoreGroup";
            this.pnlScoreGroup.Size = new System.Drawing.Size(400, 70);
            this.pnlScoreGroup.TabIndex = 0;
            // 
            // lblBestScore
            // 
            this.lblBestScore.AutoSize = true;
            this.lblBestScore.Location = new System.Drawing.Point(312, 16);
            this.lblBestScore.Name = "lblBestScore";
            this.lblBestScore.Size = new System.Drawing.Size(40, 16);
            this.lblBestScore.TabIndex = 3;
            this.lblBestScore.Text = "Show";
            // 
            // lblBestTitle
            // 
            this.lblBestTitle.AutoSize = true;
            this.lblBestTitle.Location = new System.Drawing.Point(192, 16);
            this.lblBestTitle.Name = "lblBestTitle";
            this.lblBestTitle.Size = new System.Drawing.Size(43, 16);
            this.lblBestTitle.TabIndex = 2;
            this.lblBestTitle.Text = "BEST";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(110, 16);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(43, 16);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "SHow";
            // 
            // lblScoreTitle
            // 
            this.lblScoreTitle.AutoSize = true;
            this.lblScoreTitle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lblScoreTitle.Location = new System.Drawing.Point(15, 18);
            this.lblScoreTitle.Name = "lblScoreTitle";
            this.lblScoreTitle.Size = new System.Drawing.Size(64, 23);
            this.lblScoreTitle.TabIndex = 0;
            this.lblScoreTitle.Text = "SCORE";
            // 
            // fGamePlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlDock);
            this.Controls.Add(this.pnlBoard);
            this.Name = "fGamePlay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BlockBlast";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fGamePlay_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDockPiece_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fGamePlay_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fGamePlay_MouseUp);
            this.pnlBoard.ResumeLayout(false);
            this.pnlGameOver.ResumeLayout(false);
            this.pnlGameOver.PerformLayout();
            this.pnlDock.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlScoreGroup.ResumeLayout(false);
            this.pnlScoreGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BlockBlast.Module.BufferedPanel pnlBoard;
        private System.Windows.Forms.Panel pnlDock;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlScoreGroup;
        private System.Windows.Forms.Label lblBestScore;
        private System.Windows.Forms.Label lblBestTitle;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblScoreTitle;
        private System.Windows.Forms.Button btnSound;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlDockPiece1;
        private System.Windows.Forms.Panel pnlDockPiece3;
        private System.Windows.Forms.Panel pnlDockPiece2;
        private System.Windows.Forms.Panel pnlGameOver;
        private System.Windows.Forms.Label lblGameOverTitle;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblInputGuide;
        private System.Windows.Forms.Label lblFinalScore;
        private System.Windows.Forms.Button btnReplay;
        private System.Windows.Forms.Button btnSave;
    }
}

