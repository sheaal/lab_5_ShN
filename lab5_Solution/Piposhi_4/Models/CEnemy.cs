using lab_4_ShN.Data;
using System;

namespace lab_4_ShN.Models
{
    public abstract class CEnemy : IEnemy
    {
        public string Name { get; protected set; }
        public string ImagePath { get; protected set; }
        public BigNumber BaseLife { get; protected set; }
        public BigNumber CurrentLife { get; protected set; }
        public BigNumber Gold { get; protected set; }
        public bool IsDead => CurrentLife == BigNumber.Zero;

        protected CEnemyTemplate Template { get; set; }

        public event Action<CEnemy> Damaged;
        public event Action<CEnemy> Died;

        protected CEnemy(string name, string imagePath, BigNumber baseLife, BigNumber gold, CEnemyTemplate template)
        {
            Name = name;
            ImagePath = imagePath;
            BaseLife = baseLife;
            CurrentLife = baseLife;
            Gold = gold;
            Template = template;
        }

        public abstract bool TakeDamage(BigNumber damage);

        protected void InvokeDamaged()
        {
            Damaged?.Invoke(this);
            if (IsDead)
            {
                Died?.Invoke(this);
            }
        }
    }
}