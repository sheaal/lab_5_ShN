using System;
using lab_4_ShN.Data;

namespace lab_4_ShN.Models
{
    public class RegeneratingEnemy : CEnemy
    {
        private int HealPercent { get; set; }
        private double HealChance { get; set; }
        private readonly Random _random = new Random();

        public RegeneratingEnemy(string name, string imagePath, BigNumber health, BigNumber goldReward, CEnemyTemplate template)
            : base(name, imagePath, health, goldReward, template)
        {
            if (template is CRegeneratingEnemyTemplate r)
            {
                HealPercent = r.HealPercent;
                HealChance = 0.3;
            }
            else
            {
                HealPercent = 0;
            }
        }

        public override bool TakeDamage(BigNumber damage)
        {
            if (IsDead) return true;

            if (HealPercent > 0 && _random.NextDouble() < HealChance)
            {
                BigNumber heal = BigNumber.MultiplyPercent(BaseLife, HealPercent);
                CurrentLife = CurrentLife + heal;
                if (CurrentLife > BaseLife) CurrentLife = BaseLife;

                InvokeDamaged();
                return false;
            }

            if (CurrentLife <= damage)
            {
                CurrentLife = BigNumber.Zero;
                InvokeDamaged();
                return true;
            }

            CurrentLife = CurrentLife - damage;
            InvokeDamaged();
            return false;
        }
    }
}