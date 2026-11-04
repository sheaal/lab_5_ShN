using System.Text.Json.Serialization;

namespace lab_4_ShN.Data
{
    public class PlayerSaveData
    {
        [JsonInclude] public string Gold { get; set; }
        [JsonInclude] public string Damage { get; set; }
        [JsonInclude] public string UpgradeCost { get; set; }
        [JsonInclude] public int UpgradeLevel { get; set; }
        [JsonInclude] public string CooldownUpgradeCost { get; set; }
        [JsonInclude] public int CooldownUpgradeLevel { get; set; }
        [JsonInclude] public double SphereCooldownBonus { get; set; }
    }
}