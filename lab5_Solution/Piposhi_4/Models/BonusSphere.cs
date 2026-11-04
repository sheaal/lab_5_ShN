using lab_4_ShN.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace lab_4_ShN.Models
{
    public abstract class BonusSphere
    {
        public Image Sprite { get; private set; }
        public double Lifetime { get; private set; }

        protected BonusSphere(double x, double y, double size, double lifetime)
        {
            Lifetime = lifetime;

            Sprite = new Image
            {
                Width = size,
                Height = size,
                RenderTransform = new TranslateTransform(x, y),
                Source = new BitmapImage(new Uri("pack://application:,,,/img/heart.png"))
            };
        }

        public void UpdateLifetime(double dt)
        {
            Lifetime -= dt;
        }

        public abstract void ApplyEffect(Player player);

        public bool IsMouseOn(Point mousePos)
        {
            if (Sprite.RenderTransform is TranslateTransform t)
            {
                return mousePos.X >= t.X && mousePos.X <= t.X + Sprite.Width &&
                       mousePos.Y >= t.Y && mousePos.Y <= t.Y + Sprite.Height;
            }
            return false;
        }
    }

    public class GoldSphere : BonusSphere
    {
        public GoldSphere(double x, double y, double size, double lifetime)
            : base(x, y, size, lifetime)
        {
        }

        public override void ApplyEffect(Player player)
        {
            player.AddGold(new BigNumber(50));
        }
    }
}