using System;
using System.Windows;
using System.Windows.Media;

namespace Clicker
{
    public class CTimeExtender : CCollectable
    {
        private double extraTime;

        public CTimeExtender(Point position, double size, double lifetime)
            : base(position, size, lifetime)
        {
            this.sprite.Fill = Brushes.Green;
            this.extraTime = 3.0;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (this.isMouseOnObject(mousePosition) == false)
            {
                return false;
            }

            controller.extendTime(this.extraTime);
            return true;
        }
    }
}