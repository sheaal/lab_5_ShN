using System;
using System.Windows;
using System.Windows.Media;

namespace Clicker
{
    public class CCooldownReducer : CCollectable
    {
        private double reductionValue;

        public CCooldownReducer(Point position, double size, double lifetime)
            : base(position, size, lifetime)
        {
            this.sprite.Fill = Brushes.Orange;
            this.reductionValue = 0.2;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (this.isMouseOnObject(mousePosition) == false)
            {
                return false;
            }

            player.reduceCooldown(this.reductionValue);
            return true;
        }
    }
}