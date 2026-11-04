using lab_4_ShN.Data;
using lab_4_ShN.Models;
using lab_4_ShN.Data;
using System.Text.Json.Serialization;

namespace lab_4_ShN.Models
{
    public class CRegeneratingEnemyTemplate : CEnemyTemplate
    {
        public override string TypeName => "Regenerating";

        [JsonInclude]
        public int HealPercent { get; private set; } //  % здоровья, которое восстанавливается раз в 2 секунды


        [JsonConstructor]
        public CRegeneratingEnemyTemplate(string name, EnemyIcon icon, int level, BigNumber hp, BigNumber damage, BigNumber gold, double spawnChance, double healthModifier, double goldModifier, int healPercent)
            : base(name, icon, level, hp, damage, gold, spawnChance, healthModifier, goldModifier)
        {
            HealPercent = healPercent;
        }
    }
}