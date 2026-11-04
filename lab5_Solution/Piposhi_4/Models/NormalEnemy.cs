using lab_4_ShN.Data;

namespace lab_4_ShN.Models
{
    public class NormalEnemy : CEnemy
    {
        public NormalEnemy(string name, string imagePath, BigNumber health, BigNumber goldReward, CEnemyTemplate template)
            : base(name, imagePath, health, goldReward, template) { }

        public override bool TakeDamage(BigNumber damage)
        {
            if (IsDead) return true;

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