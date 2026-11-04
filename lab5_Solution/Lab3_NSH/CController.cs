using System;
using System.Collections.Generic;
using System.Windows;

namespace Clicker
{
    public class CController
    {
        private List<CCollectable> objects;
        private double spawnRate;
        private double spawnTimer;
        private Random rng;
        private double minLifetime;
        private double maxLifetime;
        private double minSpriteSize;
        private double maxSpriteSize;
        private Size sceneSize;
        private double totalTime;
        private double remainingTime;
        private double points;

        public delegate void SphereEventHandler(CCollectable sphere);
        public event SphereEventHandler SphereSpawned;
        public event SphereEventHandler SphereRemoved;
        public event Action<string> LogMessageAdded;
        public event Action ScoreUpdated;

        public CController(double spawnRate, double gameDuration, Size sceneSize)
        {
            this.spawnRate = spawnRate;
            this.spawnTimer = 0;
            this.rng = new Random();
            this.minLifetime = 1;
            this.maxLifetime = 5;
            this.minSpriteSize = 20;
            this.maxSpriteSize = 50;
            this.sceneSize = sceneSize;
            this.totalTime = gameDuration;
            this.remainingTime = gameDuration;
            this.points = 0;
            this.objects = new List<CCollectable>();
        }

        public void update(double deltaTime)
        {
            this.remainingTime = this.remainingTime - deltaTime;

            for (int i = this.objects.Count - 1; i >= 0; i--)
            {
                this.objects[i].updateLifetime(deltaTime);

                if (this.objects[i].isExpired() == true)
                {
                    SphereRemoved?.Invoke(this.objects[i]);
                    this.objects.RemoveAt(i);
                }
            }

            if (this.remainingTime > 0)
            {
                this.spawnTimer = this.spawnTimer + deltaTime;

                if (this.spawnTimer >= this.spawnRate)
                {
                    this.spawnTimer = 0;
                    this.spawnObject();
                }
            }
        }

        private void spawnObject()
        {
            double maxX = this.sceneSize.Width - this.maxSpriteSize;
            double maxY = this.sceneSize.Height - this.maxSpriteSize;

            double x = this.rng.NextDouble() * maxX;
            double y = this.rng.NextDouble() * maxY;

            double size = this.minSpriteSize + this.rng.NextDouble() * (this.maxSpriteSize - this.minSpriteSize);
            double lifetime = this.minLifetime + this.rng.NextDouble() * (this.maxLifetime - this.minLifetime);

            CCollectable newObject;
            int type = this.rng.Next(3);

            if (type == 0) newObject = new CPointGiver(new Point(x, y), size, lifetime);
            else if (type == 1) newObject = new CCooldownReducer(new Point(x, y), size, lifetime);
            else newObject = new CTimeExtender(new Point(x, y), size, lifetime);

            SphereSpawned?.Invoke(newObject);

            this.objects.Add(newObject);
        }

        public bool handleClick(CPlayer player, Point mousePosition)
        {
            for (int i = this.objects.Count - 1; i >= 0; i--)
            {
                bool hit = this.objects[i].onClick(player, this, mousePosition);

                if (hit == true)
                {
                    SphereRemoved?.Invoke(this.objects[i]);
                    this.objects.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void pointsIncrease(double value)
        {
            this.points = this.points + value;
            ScoreUpdated?.Invoke();
            LogMessageAdded?.Invoke($"Получено {value:0.0} очков!");
        }

        public void extendTime(double value) => this.remainingTime += value;
        public double getPoints() => this.points;
        public double getRemainingTime() => this.remainingTime;
        public bool isFinished() => this.remainingTime <= 0;
    }
}