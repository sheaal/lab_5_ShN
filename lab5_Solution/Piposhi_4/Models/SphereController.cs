using System;
using System.Collections.Generic;
using System.Windows;

namespace lab_4_ShN.Models
{
    public class SphereController
    {
        private List<BonusSphere> _spheres = new List<BonusSphere>();
        private Random _random = new Random();
        private double _spawnTimer = 0;
        private double _spawnRate = 2.0;
        private Size _sceneSize;

        public event Action<BonusSphere> SphereSpawned;
        public event Action<BonusSphere> SphereRemoved;

        public SphereController(Size sceneSize)
        {
            _sceneSize = sceneSize;
        }

        public void Update(double dt)
        {

            for (int i = _spheres.Count - 1; i >= 0; i--)
            {
                _spheres[i].UpdateLifetime(dt);
                if (_spheres[i].Lifetime <= 0)
                {
                    SphereRemoved?.Invoke(_spheres[i]);
                    _spheres.RemoveAt(i);
                }
            }

            _spawnTimer += dt;
            if (_spawnTimer >= _spawnRate)
            {
                _spawnTimer = 0;
                double x = _random.NextDouble() * (_sceneSize.Width - 50);
                double y = _random.NextDouble() * (_sceneSize.Height - 50);

                BonusSphere newSphere = new GoldSphere(x, y, 30, 5.0);
                _spheres.Add(newSphere);
                SphereSpawned?.Invoke(newSphere);
            }
        }

        public void HandleClick(Player player, Point mousePos)
        {
            for (int i = _spheres.Count - 1; i >= 0; i--)
            {
                if (_spheres[i].IsMouseOn(mousePos))
                {
                    _spheres[i].ApplyEffect(player);
                    SphereRemoved?.Invoke(_spheres[i]);
                    _spheres.RemoveAt(i);
                    break;
                }
            }
        }
    }
}