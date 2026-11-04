using lab_4_ShN.Data;

namespace lab_4_ShN.Models
{
    public class ArmoredEnemy : CEnemy
    {
        private BigNumber Armor { get; set; }

        public ArmoredEnemy(string name, string imagePath, BigNumber health, BigNumber goldReward, CEnemyTemplate template)
            : base(name, imagePath, health, goldReward, template)
        {
            Armor = template is CArmoredEnemyTemplate a ? a.Armor : BigNumber.Zero;
        }

        public override bool TakeDamage(BigNumber damage)
        {
            if (IsDead) return true;

            BigNumber actualDamage = damage - Armor;
            if (actualDamage < BigNumber.One) actualDamage = BigNumber.One;

            if (CurrentLife <= actualDamage)
            {
                CurrentLife = BigNumber.Zero;
                InvokeDamaged();
                return true;
            }

            CurrentLife = CurrentLife - actualDamage;
            InvokeDamaged();
            return false;
        }
    }
}