using lab_4_ShN.Data;

namespace lab_4_ShN.Models
{
    public class Player
    {
        public BigNumber Gold { get; private set; }
        public BigNumber Damage { get; private set; }
        public BigNumber UpgradeCost { get; private set; }
        public int UpgradeLevel { get; private set; }

        private BigNumber cooldownUpgradeCost = new BigNumber(100);
        private int cooldownUpgradeLevel = 1;

        public Player()
        {
            Gold = new BigNumber(0);
            Damage = new BigNumber(1);
            UpgradeCost = new BigNumber(100);
            UpgradeLevel = 1;
        }

        public void AddGold(BigNumber amount)
        {
            if (amount < BigNumber.Zero) return;
            Gold = Gold + amount;
        }

        public bool TryUpgrade()
        {
            if (Gold < UpgradeCost) return false;

            Gold = Gold - UpgradeCost;
            UpgradeLevel++;
            Damage = Damage * 2;
            UpgradeCost = UpgradeCost * 2;
            return true;
        }

        public bool TryUpgradeCooldown()
        {
            if (Gold < cooldownUpgradeCost) return false;

            Gold = Gold - cooldownUpgradeCost;
            cooldownUpgradeLevel++;
            cooldownUpgradeCost = cooldownUpgradeCost * 2;
            return true;
        }

        public BigNumber GetCurrentDamage() => Damage;

        public PlayerSaveData GetSaveData()
        {
            return new PlayerSaveData
            {
                Gold = Gold.ToString(),
                Damage = Damage.ToString(),
                UpgradeCost = UpgradeCost.ToString(),
                UpgradeLevel = UpgradeLevel,
                CooldownUpgradeCost = cooldownUpgradeCost.ToString(),
                CooldownUpgradeLevel = cooldownUpgradeLevel
            };
        }

        public void LoadFromSaveData(PlayerSaveData data)
        {
            Gold = new BigNumber(data.Gold);
            Damage = new BigNumber(data.Damage);
            UpgradeCost = new BigNumber(data.UpgradeCost);
            UpgradeLevel = data.UpgradeLevel;
            cooldownUpgradeCost = new BigNumber(data.CooldownUpgradeCost);
            cooldownUpgradeLevel = data.CooldownUpgradeLevel;
        }
		public double GetCooldownTime(double baseTime)
		{
			double result = baseTime - (cooldownUpgradeLevel - 1) * 0.1;
			return result < 0.1 ? 0.1 : result;
		}
	}
}