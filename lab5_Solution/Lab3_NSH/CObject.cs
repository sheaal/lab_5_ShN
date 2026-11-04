using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Clicker
{
    public class CObject
    {
        private Point position;
        private Size size;
        private double lifetime;
        private Ellipse sprite;
        private double pointsValue;

        public CObject(Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            this.sprite = new Ellipse();
            this.sprite.Fill = Brushes.BlueViolet;
            this.sprite.StrokeThickness = 2;
            this.sprite.Stroke = Brushes.Black;
            this.sprite.HorizontalAlignment = HorizontalAlignment.Center;
            this.sprite.VerticalAlignment = VerticalAlignment.Center;
            this.sprite.Width = this.size.Width;
            this.sprite.Height = this.size.Height;
            this.sprite.RenderTransform = new TranslateTransform(position.X, position.Y);

            this.pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        }

        public Ellipse getSprite()
        {
            return this.sprite;
        }

        public double getPointsValue()
        {
            return this.pointsValue;
        }

        public bool isMouseOnObject(Point mousePosition)
        {
            double left = this.position.X;
            double top = this.position.Y;
            double right = left + this.size.Width;
            double bottom = top + this.size.Height;

            bool insideX = mousePosition.X >= left && mousePosition.X <= right;
            bool insideY = mousePosition.Y >= top && mousePosition.Y <= bottom;

            return insideX && insideY;
        }

        public void updateLifetime(double deltaTime)
        {
            this.lifetime = this.lifetime - deltaTime;
        }

        public bool isExpired()
        {
            return this.lifetime <= 0;
        }
    }
}