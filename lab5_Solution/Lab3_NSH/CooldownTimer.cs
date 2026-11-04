using System;

namespace Clicker
{
    public class CooldownTimer
    {
        private double baseCooldown;
        private double currentTime;

        public CooldownTimer(double baseCooldown)
        {
            this.baseCooldown = baseCooldown;
            this.currentTime = 0;
        }

        public void update(double deltaTime)
        {
            if (this.currentTime > 0)
            {
                this.currentTime = this.currentTime - deltaTime;
            }

            if (this.currentTime < 0)
            {
                this.currentTime = 0;
            }
        }

        public bool isReady()
        {
            return this.currentTime <= 0;
        }

        public void reset()
        {
            this.currentTime = this.baseCooldown;
        }

        public void setBaseCooldown(double value)
        {
            this.baseCooldown = value;
        }

        public double getBaseCooldown()
        {
            return this.baseCooldown;
        }

        public double getRemainingTime()
        {
            return this.currentTime;
        }
    }
}