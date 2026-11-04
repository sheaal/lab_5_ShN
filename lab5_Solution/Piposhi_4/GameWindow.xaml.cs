using lab_4_ShN.Models;
using lab_4_ShN.Data;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace lab_4_ShN
{
    public partial class GameWindow : Window
    {
        private Player _player;
        private IEnemy _currentEnemy;
        private EnemySpawner _spawner;
        private CEnemyTemplateList _templates;

        private CooldownTimer _attackCooldown;
        private PlayerSaveList _playerSave = new PlayerSaveList();
        private SphereController _sphereController;

        public GameWindow()
        {
            InitializeComponent();

            _player = new Player();
            _templates = new CEnemyTemplateList();

            _templates.LoadAll("enemies.json");

            if (_templates.Enemies.Count == 0)
            {
                MessageBox.Show("Нет шаблона противников! Загрузите редактор и создайте их.");
            }
            else
            {
                _spawner = new EnemySpawner(_templates.Enemies);
                _spawner.EnemySpawned += OnEnemySpawned;
                _spawner.SpawnEnemy(_player.UpgradeLevel);
            }

            _attackCooldown = new CooldownTimer(0.5);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
            timer.Start();
            _sphereController = new SphereController(new Size(800, 500));
            _sphereController.SphereSpawned += OnSphereSpawned;
            _sphereController.SphereRemoved += OnSphereRemoved;

            SphereCanvas.MouseLeftButtonDown += SphereCanvas_MouseLeftButtonDown;

            UpdateUI();
        }
        private void OnSphereSpawned(BonusSphere sphere)
        {
            SphereCanvas.Children.Add(sphere.Sprite);
        }

        private void OnSphereRemoved(BonusSphere sphere)
        {
            SphereCanvas.Children.Remove(sphere.Sprite);
        }

        private void SphereCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(SphereCanvas);
            _sphereController.HandleClick(_player, pos);
            UpdateUI();
        }

        private void OnEnemySpawned(IEnemy enemy)
        {
            _currentEnemy = enemy;

            if (_currentEnemy is CEnemy concreteEnemy)
            {
                concreteEnemy.Damaged += OnEnemyDamaged;
                concreteEnemy.Died += OnEnemyDied;
            }

            if (!string.IsNullOrEmpty(_currentEnemy.ImagePath) && File.Exists(_currentEnemy.ImagePath))
            {
                EnemyImage.Source = new BitmapImage(new Uri(_currentEnemy.ImagePath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                EnemyImage.Source = null;
            }

            UpdateUI();
        }

        private void OnEnemyDamaged(CEnemy enemy)
        {
            UpdateUI();
        }

        private void OnEnemyDied(CEnemy enemy)
        {
            _player.AddGold(enemy.Gold);
            UpdateUI();

            _spawner.SpawnEnemy(_player.UpgradeLevel);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            double dt = 0.1;
            _attackCooldown.update(dt);

            if (_sphereController != null)
            {
                _sphereController.Update(dt);
            }

            if (_attackCooldown.isReady())
                CooldownBar.Value = 1;
            else
            {
                double currentBase = _attackCooldown.getBaseCooldown();
                double remaining = _attackCooldown.getRemainingTime();
                CooldownBar.Value = 1 - (remaining / currentBase);
            }
        }

        private void UpdateUI()
        {
            if (_currentEnemy != null)
            {
                EnemyNameText.Text = _currentEnemy.Name;
                EnemyHPText.Text = $"HP: {_currentEnemy.CurrentLife} / {_currentEnemy.BaseLife}";
                EnemyRewardText.Text = $"Награда: {_currentEnemy.Gold}";
            }
            else
            {
                EnemyNameText.Text = "Нет врага";
                EnemyHPText.Text = "";
                EnemyRewardText.Text = "";
                EnemyImage.Source = null;
            }

            PlayerGoldText.Text = _player.Gold.ToString();
            PlayerDamageText.Text = _player.GetCurrentDamage().ToString();
            UpgradeCostText.Text = _player.UpgradeCost.ToString();
        }

        private void EnemyImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentEnemy == null || !_attackCooldown.isReady()) return;


            _currentEnemy.TakeDamage(_player.GetCurrentDamage());

            _attackCooldown.reset();
        }

        private void UpgradeCooldownButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player.TryUpgradeCooldown())
            {
                _attackCooldown.setBaseCooldown(_player.GetCooldownTime(0.5));
                UpdateUI();
            }
        }

        private void UpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player.TryUpgrade()) UpdateUI();
        }

        private void SaveGameButton_Click(object sender, RoutedEventArgs e)
        {
            string path = "player.json";
            _playerSave.Save(_player, path);
            MessageBox.Show("Прогресс успешно сохранен!", "Сохранение");
        }

        private void LoadGameButton_Click(object sender, RoutedEventArgs e)
        {
            _player = _playerSave.Load("player.json");
            UpdateUI();
            MessageBox.Show("Прогресс успешно загружен!", "Загрузка");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(SphereCanvas);
            _sphereController.HandleClick(_player, pos);
            UpdateUI();
        }
    }
}