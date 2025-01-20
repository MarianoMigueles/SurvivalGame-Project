namespace ZombieGame
{
    partial class Game
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Player_HP = new Label();
            Player_Kills = new Label();
            ResumeGame_btn = new Button();
            Reset_btn = new Button();
            Show_Hitbox_btn = new Button();
            Label_GameOver = new Label();
            SuspendLayout();
            // 
            // Player_HP
            // 
            Player_HP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Player_HP.AutoSize = true;
            Player_HP.Location = new Point(718, 20);
            Player_HP.Name = "Player_HP";
            Player_HP.Size = new Size(26, 15);
            Player_HP.TabIndex = 0;
            Player_HP.Text = "HP:";
            // 
            // Player_Kills
            // 
            Player_Kills.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Player_Kills.AutoSize = true;
            Player_Kills.Location = new Point(718, 44);
            Player_Kills.Name = "Player_Kills";
            Player_Kills.Size = new Size(31, 15);
            Player_Kills.TabIndex = 1;
            Player_Kills.Text = "Kills:";
            // 
            // ResumeGame_btn
            // 
            ResumeGame_btn.Anchor = AnchorStyles.Top;
            ResumeGame_btn.Enabled = false;
            ResumeGame_btn.Location = new Point(335, 177);
            ResumeGame_btn.Name = "ResumeGame_btn";
            ResumeGame_btn.Size = new Size(132, 60);
            ResumeGame_btn.TabIndex = 2;
            ResumeGame_btn.Text = "Resume game";
            ResumeGame_btn.UseVisualStyleBackColor = true;
            ResumeGame_btn.Visible = false;
            ResumeGame_btn.Click += ResumeGame_btn_Click;
            // 
            // Reset_btn
            // 
            Reset_btn.Enabled = false;
            Reset_btn.Location = new Point(335, 111);
            Reset_btn.Name = "Reset_btn";
            Reset_btn.Size = new Size(132, 60);
            Reset_btn.TabIndex = 3;
            Reset_btn.Text = "Reset game";
            Reset_btn.UseVisualStyleBackColor = true;
            Reset_btn.Visible = false;
            Reset_btn.Click += Reset_btn_Click;
            // 
            // Show_Hitbox_btn
            // 
            Show_Hitbox_btn.Enabled = false;
            Show_Hitbox_btn.Location = new Point(12, 12);
            Show_Hitbox_btn.Name = "Show_Hitbox_btn";
            Show_Hitbox_btn.Size = new Size(105, 32);
            Show_Hitbox_btn.TabIndex = 4;
            Show_Hitbox_btn.Text = "Show hitboxes";
            Show_Hitbox_btn.UseVisualStyleBackColor = true;
            Show_Hitbox_btn.Visible = false;
            Show_Hitbox_btn.Click += Show_Hitbox_Click;
            // 
            // Label_GameOver
            // 
            Label_GameOver.AutoSize = true;
            Label_GameOver.Font = new Font("Showcard Gothic", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label_GameOver.ForeColor = Color.OrangeRed;
            Label_GameOver.Location = new Point(338, 61);
            Label_GameOver.Name = "Label_GameOver";
            Label_GameOver.Size = new Size(129, 27);
            Label_GameOver.TabIndex = 5;
            Label_GameOver.Text = "Game Over";
            Label_GameOver.Visible = false;
            // 
            // Game
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Label_GameOver);
            Controls.Add(Show_Hitbox_btn);
            Controls.Add(Reset_btn);
            Controls.Add(ResumeGame_btn);
            Controls.Add(Player_Kills);
            Controls.Add(Player_HP);
            Name = "Game";
            Text = "Game";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Player_HP;
        private Label Player_Kills;
        private Button ResumeGame_btn;
        private Button Reset_btn;
        private Button Show_Hitbox_btn;
        private Label Label_GameOver;
    }
}
