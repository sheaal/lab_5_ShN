using System;
using System.Collections.Generic;
using System.Linq;
using lab_4_ShN.Data;
using lab_4_ShN.Models;

namespace lab_4_ShN.Models
{
    public class EnemySpawner
    {
        private List<CEnemyTemplate> _templates;
        private Random _random = new Random();
        private double[] _normalized;

        public event Action<IEnemy> EnemySpawned;

        public EnemySpawner(List<CEnemyTemplate> templates)
        {
            _templates = templates ?? new List<CEnemyTemplate>();
            _normalized = Array.Empty<double>();
            NormalizeChances();
        }

        private void NormalizeChances()
        {
            if (_templates.Count == 0) return;
            double total = _templates.Sum(t => t.SpawnChance);

            if (total <= 0)
            {
                _normalized = Enumerable.Repeat(1.0 / _templates.Count, _templates.Count).ToArray();
                return;
            }
            _normalized = _templates.Select(t => t.SpawnChance / total).ToArray();
        }

        public IEnemy SpawnEnemy(int playerLevel)
        {
            if (_templates.Count == 0)
                throw new InvalidOperationException("Список шаблонов врагов пуст.");

            CEnemyTemplate template = PickRandomTemplate();

            int healthPercent = (int)((1 + template.HealthModifier * playerLevel) * 100);
            int goldPercent = (int)((1 + template.GoldModifier * playerLevel) * 100);

            BigNumber health = BigNumber.MultiplyPercent(template.HP, healthPercent);
            BigNumber gold = BigNumber.MultiplyPercent(template.Gold, goldPercent);

            IEnemy newEnemy = EnemyFactory.Create(template, health, gold);

            EnemySpawned?.Invoke(newEnemy);

            return newEnemy;
        }

        private CEnemyTemplate PickRandomTemplate()
        {
            double roll = _random.NextDouble();
            double cumulative = 0;

            for (int i = 0; i < _templates.Count; i++)
            {
                cumulative += _normalized[i];
                if (roll <= cumulative)
                    return _templates[i];
            }
            return _templates.Last();
        }
    }
}