using System;
using System.Windows;
using System.Windows.Media;

namespace Clicker
{
    public class CPointGiver : CCollectable
    {
        private double pointsValue;

        public CPointGiver(Point position, double size, double lifetime)
            : base(position, size, lifetime)
        {
            this.sprite.Fill = Brushes.BlueViolet;
            this.pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (this.isMouseOnObject(mousePosition) == false)
            {
                return false;
            }

            controller.pointsIncrease(this.pointsValue);
            return true;
        }
    }
}