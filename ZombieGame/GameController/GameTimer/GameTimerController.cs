using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.GameController.GameTimer
{
    internal class GameTimerController
    {
        private System.Windows.Forms.Timer gameTimer;
        private Action updateAction;
        public int Interval { get; private set; }
        public bool IsRunning { get; private set; }

        public GameTimerController(int interval, Action updateAction)
        {
            this.Interval = interval;
            gameTimer = new()
            {
                Interval = Interval
            };
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            this.updateAction = updateAction;
        }

        public void Start()
        {
            gameTimer.Start();
            IsRunning = true;
        }
        
        public void Stop()
        {
            gameTimer.Stop();
            IsRunning = false;
        }

        public void SetInterval(int interval)
        {
            Interval = interval;
            gameTimer.Interval = Interval;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            updateAction?.Invoke();
        }
    }
}
