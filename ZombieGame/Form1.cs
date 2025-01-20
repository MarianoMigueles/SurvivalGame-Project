using GunExtreme.Entities;
using SpatialStructures;
using System.Numerics;
using ZombieGame.GameController.EntityManager;
using ZombieGame.GameController;

namespace ZombieGame
{
    public partial class Game : Form
    {
        public new Point MousePosition { get; private set; }
        public Game()
        {
            this.InitializeComponent();

            GameManager.InitializeComponent(this);
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            this.KeyDown += new KeyEventHandler(GameManager.Game_KeyDown);
            this.KeyUp += new KeyEventHandler(GameManager.Game_KeyUp);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
        }

        private void Form1_MouseMove(object? sender, MouseEventArgs e)
        {
            MousePosition = e.Location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GameManager.DrawQuadtreeBounds(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (!GameManager.GameIsRunning) return;

            switch (this.WindowState)
            {
                case FormWindowState.Minimized:
                    GameManager.OpenMenu();
                    break;
                case FormWindowState.Normal:
                    this.Focus();
                    break;
                case FormWindowState.Maximized:
                    this.Focus();
                    break;
            }

            GameManager.ReSizeQuadtree(this.Width, this.Height);
        }

        public void HideMenuButtons()
        {
            this.Reset_btn.Visible = false;
            this.Reset_btn.Enabled = false;

            this.ResumeGame_btn.Visible = false;
            this.ResumeGame_btn.Enabled = false;

            this.Show_Hitbox_btn.Visible = false;
            this.Show_Hitbox_btn.Enabled = false;
        }


        public void ShowMenuButtons()
        {
            this.Reset_btn.Visible = true;
            this.Reset_btn.Enabled = true;

            this.ResumeGame_btn.Visible = true;
            this.ResumeGame_btn.Enabled = true;

            this.Show_Hitbox_btn.Visible = true;
            this.Show_Hitbox_btn.Enabled = true;
        }

        public void ShowGameOver()
        {
            Label_GameOver.Visible = true;
            this.ResumeGame_btn.Visible = false;
            this.ResumeGame_btn.Enabled = false;
        }

        public void WritePlayerHp(int amount)
        {
            this.Player_HP.Text = $"Hp: {amount}"; ;
        }

        public void SumPlayerKill(int amount)
        {
            this.Player_Kills.Text = $"Kills: {amount}";
        }

        private void ResumeGame_btn_Click(object sender, EventArgs e)
        {
            GameManager.ResumeGame();
            this.Focus();
        }

        private void Reset_btn_Click(object sender, EventArgs e)
        {
            GameManager.RestartGame();
            Label_GameOver.Visible = false;
        }

        private void Show_Hitbox_Click(object sender, EventArgs e)
        {
            GameManager.ShowHitboxes();
        }

    }
}
