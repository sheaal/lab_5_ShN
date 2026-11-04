using lab_4_ShN.Data;
using lab_4_ShN.Models;
using lab_4_ShN.Data;
using System.Text.Json.Serialization;

namespace lab_4_ShN.Models
{
    public abstract class CEnemyTemplate
    {
        [JsonInclude] public string Name { get; protected set; }
        [JsonInclude] public EnemyIcon Icon { get; protected set; }
        [JsonInclude] public int Level { get; protected set; }
        [JsonInclude] public BigNumber HP { get; protected set; }
        [JsonInclude] public BigNumber Damage { get; protected set; }
        [JsonInclude] public BigNumber Gold { get; protected set; }
        [JsonInclude] public double SpawnChance { get; protected set; }
        [JsonInclude] public double HealthModifier { get; protected set; }
        [JsonInclude] public double GoldModifier { get; protected set; }

        [JsonIgnore]
        public abstract string TypeName { get; }

        protected CEnemyTemplate(string name, EnemyIcon icon, int level, BigNumber hp, BigNumber damage, BigNumber gold, double spawnChance, double healthModifier, double goldModifier)
        {
            Name = name;
            Icon = icon;
            Level = level;
            HP = hp;
            Damage = damage;
            Gold = gold;
            SpawnChance = spawnChance;
            HealthModifier = healthModifier;
            GoldModifier = goldModifier;
        }

        public override string ToString() => Name;
    }
}