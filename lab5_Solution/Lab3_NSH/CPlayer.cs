using System;

namespace Clicker
{
    public class CPlayer
    {
        private double points;
        private double gold;
        private double damage;
        private double cooldownBonus;

        public CPlayer()
        {
            this.points = 0;
            this.gold = 0;
            this.damage = 1;
            this.cooldownBonus = 0;
        }

        public double getPoints()
        {
            return this.points;
        }

        public void addPoints(double value)
        {
            this.points = this.points + value;
        }

        public double getGold()
        {
            return this.gold;
        }

        public void addGold(double value)
        {
            this.gold = this.gold + value;
        }

        public double getDamage()
        {
            return this.damage;
        }

        public void reduceCooldown(double value)
        {
            this.cooldownBonus = this.cooldownBonus + value;
        }

        public double getCooldownBonus()
        {
            return this.cooldownBonus;
        }
    }
}