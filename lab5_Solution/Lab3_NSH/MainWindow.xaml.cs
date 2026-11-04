using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Clicker
{
    public partial class MainWindow : Window
    {
        private CController controller;
        private CPlayer player;
        private CooldownTimer cooldown;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this.GameCanvas.Children.Clear();
            this.LogListBox.Items.Clear();

            this.player = new CPlayer();
            this.cooldown = new CooldownTimer(0.5);

            Size sceneSize = new Size(this.GameCanvas.ActualWidth, this.GameCanvas.ActualHeight);

            this.controller = new CController(0.5, 30, sceneSize);

            this.controller.SphereSpawned += OnSphereSpawned;
            this.controller.SphereRemoved += OnSphereRemoved;
            this.controller.LogMessageAdded += OnLogMessageAdded;
            this.controller.ScoreUpdated += OnScoreUpdated;

            this.timer.Start();
        }

        private void OnSphereSpawned(CCollectable sphere)
        {
            this.GameCanvas.Children.Add(sphere.getSprite());
        }

        private void OnSphereRemoved(CCollectable sphere)
        {
            this.GameCanvas.Children.Remove(sphere.getSprite());
        }

        private void OnLogMessageAdded(string message)
        {
            this.LogListBox.Items.Add(message);
            this.LogListBox.ScrollIntoView(message);
        }

        private void OnScoreUpdated()
        {
            this.ScoreText.Text = this.controller.getPoints().ToString("0");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.controller == null) return;

            this.controller.update(0.1);
            this.cooldown.update(0.1);
            this.TimeText.Text = this.controller.getRemainingTime().ToString("0.0");

            if (this.controller.isFinished())
            {
                this.timer.Stop();
                MessageBox.Show("Игра окончена! Очки: " + this.controller.getPoints().ToString("0"));
            }
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.controller == null || this.cooldown.isReady() == false) return;

            Point mousePosition = e.GetPosition(this.GameCanvas);
            bool hit = this.controller.handleClick(this.player, mousePosition);

            if (hit == true) this.cooldown.reset();
        }
    }
}