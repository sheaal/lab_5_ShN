using lab_4_ShN.Data;

namespace lab_4_ShN.Models
{
    public interface IEnemy
    {
        string Name { get; }
        string ImagePath { get; }
        BigNumber BaseLife { get; }
        BigNumber CurrentLife { get; }
        BigNumber Gold { get; }
        bool IsDead { get; }
        bool TakeDamage(BigNumber damage);
    }
}