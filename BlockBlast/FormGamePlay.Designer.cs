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
            this.lblBestRecord = new System.Windows.Forms.Label();
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
            this.pnlBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(25)))), ((int)(((byte)(37)))));
            this.pnlBoard.Controls.Add(this.pnlGameOver);
            this.pnlBoard.Location = new System.Drawing.Point(715, 230);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(500, 500);
            this.pnlBoard.TabIndex = 0;
            this.pnlBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBoard_Paint);
            // 
            // pnlGameOver
            // 
            this.pnlGameOver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(18)))), ((int)(((byte)(21)))), ((int)(((byte)(32)))));
            this.pnlGameOver.Controls.Add(this.btnReplay);
            this.pnlGameOver.Controls.Add(this.lblBestRecord);
            this.pnlGameOver.Controls.Add(this.lblFinalScore);
            this.pnlGameOver.Controls.Add(this.lblGameOverTitle);
            this.pnlGameOver.Location = new System.Drawing.Point(0, 0);
            this.pnlGameOver.Name = "pnlGameOver";
            this.pnlGameOver.Size = new System.Drawing.Size(500, 500);
            this.pnlGameOver.TabIndex = 1;
            this.pnlGameOver.Visible = false;
            // 
            // btnReplay
            // 
            this.btnReplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(138)))), ((int)(((byte)(61)))));
            this.btnReplay.FlatAppearance.BorderSize = 0;
            this.btnReplay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(110)))), ((int)(((byte)(30)))));
            this.btnReplay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(160)))), ((int)(((byte)(90)))));
            this.btnReplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplay.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReplay.ForeColor = System.Drawing.Color.White;
            this.btnReplay.Location = new System.Drawing.Point(160, 270);
            this.btnReplay.Name = "btnReplay";
            this.btnReplay.Size = new System.Drawing.Size(180, 50);
            this.btnReplay.TabIndex = 3;
            this.btnReplay.Text = "▶ CHƠI LẠI";
            this.btnReplay.UseVisualStyleBackColor = false;
            // 
            // lblBestRecord
            // 
            this.lblBestRecord.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBestRecord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(165)))), ((int)(((byte)(181)))));
            this.lblBestRecord.Location = new System.Drawing.Point(0, 200);
            this.lblBestRecord.Name = "lblBestRecord";
            this.lblBestRecord.Size = new System.Drawing.Size(500, 36);
            this.lblBestRecord.TabIndex = 2;
            this.lblBestRecord.Text = "KỶ LỤC: 0";
            this.lblBestRecord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFinalScore
            // 
            this.lblFinalScore.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalScore.ForeColor = System.Drawing.Color.White;
            this.lblFinalScore.Location = new System.Drawing.Point(0, 140);
            this.lblFinalScore.Name = "lblFinalScore";
            this.lblFinalScore.Size = new System.Drawing.Size(500, 44);
            this.lblFinalScore.TabIndex = 1;
            this.lblFinalScore.Text = "ĐIỂM: 0";
            this.lblFinalScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGameOverTitle
            // 
            this.lblGameOverTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameOverTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.lblGameOverTitle.Location = new System.Drawing.Point(0, 60);
            this.lblGameOverTitle.Name = "lblGameOverTitle";
            this.lblGameOverTitle.Size = new System.Drawing.Size(500, 60);
            this.lblGameOverTitle.TabIndex = 0;
            this.lblGameOverTitle.Text = "GAME OVER";
            this.lblGameOverTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlDock
            // 
            this.pnlDock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(25)))), ((int)(((byte)(37)))));
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
            this.pnlDockPiece3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(53)))));
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
            this.pnlDockPiece2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(53)))));
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
            this.pnlDockPiece1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(53)))));
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
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(45)))), ((int)(((byte)(66)))));
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(70)))), ((int)(((byte)(102)))));
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(59)))), ((int)(((byte)(87)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
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
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(45)))), ((int)(((byte)(66)))));
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(70)))), ((int)(((byte)(102)))));
            this.btnPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(59)))), ((int)(((byte)(87)))));
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.ForeColor = System.Drawing.Color.White;
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
            this.btnSound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(45)))), ((int)(((byte)(66)))));
            this.btnSound.FlatAppearance.BorderSize = 0;
            this.btnSound.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(70)))), ((int)(((byte)(102)))));
            this.btnSound.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(59)))), ((int)(((byte)(87)))));
            this.btnSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSound.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSound.ForeColor = System.Drawing.Color.White;
            this.btnSound.Location = new System.Drawing.Point(1395, 16);
            this.btnSound.Name = "btnSound";
            this.btnSound.Size = new System.Drawing.Size(152, 45);
            this.btnSound.TabIndex = 1;
            this.btnSound.Text = "Am Thanh";
            this.btnSound.UseCompatibleTextRendering = true;
            this.btnSound.UseVisualStyleBackColor = false;
            // 
            // pnlScoreGroup
            // 
            this.pnlScoreGroup.BackColor = System.Drawing.Color.Transparent;
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
            this.lblBestScore.BackColor = System.Drawing.Color.Transparent;
            this.lblBestScore.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBestScore.ForeColor = System.Drawing.Color.White;
            this.lblBestScore.Location = new System.Drawing.Point(272, 14);
            this.lblBestScore.Name = "lblBestScore";
            this.lblBestScore.Size = new System.Drawing.Size(31, 41);
            this.lblBestScore.TabIndex = 3;
            this.lblBestScore.Text = "0";
            // 
            // lblBestTitle
            // 
            this.lblBestTitle.AutoSize = true;
            this.lblBestTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblBestTitle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBestTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(156)))), ((int)(((byte)(178)))));
            this.lblBestTitle.Location = new System.Drawing.Point(205, 24);
            this.lblBestTitle.Name = "lblBestTitle";
            this.lblBestTitle.Size = new System.Drawing.Size(51, 23);
            this.lblBestTitle.TabIndex = 2;
            this.lblBestTitle.Text = "BEST";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.BackColor = System.Drawing.Color.Transparent;
            this.lblScore.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(77)))));
            this.lblScore.Location = new System.Drawing.Point(92, 14);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(31, 41);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "0";
            // 
            // lblScoreTitle
            // 
            this.lblScoreTitle.AutoSize = true;
            this.lblScoreTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblScoreTitle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(156)))), ((int)(((byte)(178)))));
            this.lblScoreTitle.Location = new System.Drawing.Point(18, 24);
            this.lblScoreTitle.Name = "lblScoreTitle";
            this.lblScoreTitle.Size = new System.Drawing.Size(64, 23);
            this.lblScoreTitle.TabIndex = 0;
            this.lblScoreTitle.Text = "SCORE";
            // 
            // fGamePlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(19)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlDock);
            this.Controls.Add(this.pnlBoard);
            this.Name = "fGamePlay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BlockBlast";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fGamePlay_Load);
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
        private System.Windows.Forms.Label lblFinalScore;
        private System.Windows.Forms.Label lblBestRecord;
        private System.Windows.Forms.Button btnReplay;
    }
}

