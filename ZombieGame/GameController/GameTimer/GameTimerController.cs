

namespace ZombieGame.GameController.GameTimer
{
    /// <summary>
    /// Manages a game timer to execute periodic updates using a specified interval.
    /// </summary>
    internal class GameTimerController
    {
        private readonly System.Windows.Forms.Timer gameTimer;
        private readonly Action updateAction;

        /// <summary>
        /// Gets the interval of the timer in milliseconds.
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        /// Indicates whether the timer is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTimerController"/> class with an interval in seconds.
        /// </summary>
        /// <param name="intervalSeconds">The interval in seconds for the timer.</param>
        /// <param name="updateAction">The action to execute on each timer tick.</param>
        public GameTimerController(double intervalSeconds, Action updateAction)
            : this((int)(intervalSeconds * 1000), updateAction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTimerController"/> class with an interval in milliseconds.
        /// </summary>
        /// <param name="intervalTicks">The interval in milliseconds for the timer.</param>
        /// <param name="updateAction">The action to execute on each timer tick.</param>
        public GameTimerController(int intervalTicks, Action updateAction)
        {
            Interval = intervalTicks;
            this.updateAction = updateAction ?? throw new ArgumentNullException(nameof(updateAction));

            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = Interval
            };
            gameTimer.Tick += GameTimer_Tick;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            if (!IsRunning)
            {
                gameTimer.Start();
                IsRunning = true;
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                gameTimer.Stop();
                IsRunning = false;
            }
        }

        /// <summary>
        /// Updates the timer interval in milliseconds.
        /// </summary>
        /// <param name="interval">The new interval in milliseconds.</param>
        public void SetInterval(int interval)
        {
            Interval = interval;
            gameTimer.Interval = Interval;
        }

        /// <summary>
        /// Updates the timer interval in seconds.
        /// </summary>
        /// <param name="intervalSeconds">The new interval in seconds.</param>
        public void SetIntervalInSeconds(double intervalSeconds)
        {
            Interval = (int)(intervalSeconds * 1000);
            gameTimer.Interval = Interval;
        }

        /// <summary>
        /// Handles the timer tick event and executes the update action.
        /// </summary>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            updateAction?.Invoke();
        }
    }
}
