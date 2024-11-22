using PoolClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.GameController.EntityManager;
using ZombieGame.Log;

namespace GunExtreme.Entities
{
    internal abstract class GameObject( int velocity, PictureBox pictureBox, IManagerGameEntities entintyGenerator) : IIdentificable
    {
        public int Id { get; set; }
        public abstract int PositionX { get; set; }
        public abstract int PositionY { get; set; }

        public Logger Logger = new("D:/Buenas_practicas/C#_proyects/ZombieGame/ZombieGame/Log/LogFile");

        public float DirectionX;
        public float DirectionY;
        public int Velocity { get; set; } = velocity;
        public PictureBox PictureBox { get; set; } = pictureBox;

        public bool HasChanged { get; set; } = false;

        protected readonly IManagerGameEntities _entintyManager = entintyGenerator;

        public virtual void UpdatePosition()
        {
            int newX = PositionX + (int)(DirectionX * Velocity);
            int newY = PositionY + (int)(DirectionY * Velocity);

            //Logger.LogInfo($"{this.GetType().Name} Position Before Update: X={PositionX}, Y={PositionY}");
            //Logger.LogInfo($"{this.GetType().Name} Direction: dX={DirectionX}, dY={DirectionY}");
            //Logger.LogInfo($"{this.GetType().Name} New Position Calculated: X={newX}, Y={newY}");

            if (this.PictureBox.InvokeRequired)
            {
                this.PictureBox.Invoke(new Action(() => UpdatePictureBoxLocation(newX, newY)));
            }
            else
            {
                UpdatePictureBoxLocation(newX, newY);
            }

            this.PositionX = newX;
            this.PositionY = newY;
        }

        private void UpdatePictureBoxLocation(int x, int y)
        {
            this.PictureBox.Location = new Point(x, y);
        }

        public virtual void Disappear() //The "object" die or disappear
        {
            _entintyManager.ReleaseEntity(this);
        }

        private int count = 10;

        public virtual void Reset(GameObject obj)
        {
            obj.PositionX = -10;
            obj.PositionY = -10;

            //obj.Velocity = 0;

            //UpdatePictureBoxLocation(PositionX, PositionY);

            OnReset();
        }

        protected virtual void OnReset()
        {
            
        }

        public static int GetFormWidth()
        {
            if(Form.ActiveForm != null)
            {
                return Form.ActiveForm.Width;
            }
            return 800;
        }

        public static int GetFormHeight()
        {
            if (Form.ActiveForm != null)
            {
                return Form.ActiveForm.Height;
            }
            return 600;
        }
    }
}
