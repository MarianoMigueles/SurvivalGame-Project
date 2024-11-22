using GunExtreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.GameController.GameTimer;
using ZombieGame.Log;
using ZombieGame.Pools;

namespace ZombieGame.GameController.EntityManager
{
    internal class ManagerGameEntities : IManagerGameEntities
    {
        public readonly ObjectPool<GameObject> _objectPool;
        private readonly Game _gameForm;
        private readonly GameTimerController _gameTimerController;
        public Player Player { get; private set; }
        public Logger Logger = new("D:/Buenas_practicas/C#_proyects/ZombieGame/ZombieGame/Log/LogFile");
        public ManagerGameEntities(Game gameForm)
        {
            _gameForm = gameForm;

            _objectPool = new();
            _gameTimerController = new GameTimerController(16, UpdateEntities);
            _gameTimerController.Start();
        }

        public Point GetMousePosition()
        {
            return _gameForm.MousePosition;
        }

        public void ReleaseEntity(GameObject obj)
        {
            _objectPool.ReleaseObject(obj, x => x.Reset(x));
            _gameForm.Controls.Remove(obj.PictureBox);
        }

        public void AddEntity(GameObject obj)
        {
            _gameForm.Controls.Add(obj.PictureBox);
        }

        private void UpdateEntities()
        {
            bool reDraw = false;

            foreach (var entity in _objectPool.InUseObjects.Values)
            {
                if (entity.HasChanged)
                {
                    entity.UpdatePosition();
                    reDraw = true;
                }
            }

            if (reDraw)
            {
                _gameForm.Invalidate();
            }
            
        }

        public void GenerateNewPlayer(string name)
        {
            this.Player = (Player)_objectPool.GetObject(
                () => new Player(
                    name,
                    10,
                    0,
                    0,
                    0,
                    10,
                    new PictureBox
                    {
                        Width = 50,
                        Height = 50,
                        BackColor = Color.Blue,
                        Location = new Point(0, 0)
                    },
                    this
                )
            );

            AddEntity(this.Player);
        }

        public void GenerateNewZombie(string type)
        {
            Zombie newZombie = (Zombie)_objectPool.GetObject(
                () => new Zombie(
                    type,
                    5,
                    10,
                    5,
                    new PictureBox
                    {
                        Width = 50,
                        Height = 50,
                        BackColor = Color.Green,
                        Location = new Point(10, 10)
                    },
                    this
                )
            );

            AddEntity(newZombie);
        }

        public Bullet GenerateNewBullet()
        {
            Bullet newBullet = (Bullet)_objectPool.GetObject(
                () => new Bullet(
                    4,
                    0,
                    5,
                    new PictureBox
                    {
                        Width = 50,
                        Height = 50,
                        BackColor = Color.Yellow
                    },
                    this
                )
            );

            return newBullet;
        }

    }
}
