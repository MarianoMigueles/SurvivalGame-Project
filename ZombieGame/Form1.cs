using GunExtreme.Entities;
using System.Numerics;
using ZombieGame.GameController;
using ZombieGame.GameController.EntityManager;
using ZombieGame.Pools;

namespace ZombieGame
{
    public partial class Game : Form
    {
        private readonly KeyManager _keyManager;
        private readonly ManagerGameEntities _entityManager;
        public new Point MousePosition { get; private set; }
        public Game()
        {
            InitializeComponent();

            _entityManager = new(this);
            _entityManager.GenerateNewPlayer("Mariano");
            _keyManager = new(_entityManager.Player, this);

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
        }

        private void Form1_MouseMove(object? sender, MouseEventArgs e)
        {
            MousePosition = e.Location;
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            _keyManager.KeyDown(e.KeyCode);
        }

        private void Form1_KeyUp(object? sender, KeyEventArgs e)
        {
            _keyManager.KeyUp(e.KeyCode);
        }
    }
}
