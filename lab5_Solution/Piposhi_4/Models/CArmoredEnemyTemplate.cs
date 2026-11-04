using lab_4_ShN.Data;
using System.Text.Json.Serialization;

namespace lab_4_ShN.Models
{
    public class CArmoredEnemyTemplate : CEnemyTemplate
    {
        public override string TypeName => "Armored";

        [JsonInclude]
        public BigNumber Armor { get; private set; } // броня

        [JsonConstructor]
        public CArmoredEnemyTemplate(string name, EnemyIcon icon, int level, BigNumber hp, BigNumber damage, BigNumber gold, double spawnChance, double healthModifier, double goldModifier, BigNumber armor)
            : base(name, icon, level, hp, damage, gold, spawnChance, healthModifier, goldModifier)
        {
            Armor = armor;
        }
    }
}