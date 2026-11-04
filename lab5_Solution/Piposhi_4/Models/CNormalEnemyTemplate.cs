using lab_4_ShN.Data;
using lab_4_ShN.Models;
using lab_4_ShN.Data;
using System.Text.Json.Serialization;

namespace lab_4_ShN.Models
{
    public class CNormalEnemyTemplate : CEnemyTemplate
    {
        public override string TypeName => "Normal";
        [JsonConstructor]
        public CNormalEnemyTemplate(string name, EnemyIcon icon, int level, BigNumber hp, BigNumber damage, BigNumber gold, double spawnChance, double healthModifier, double goldModifier)
            : base(name, icon, level, hp, damage, gold, spawnChance, healthModifier, goldModifier) { }
    }
}