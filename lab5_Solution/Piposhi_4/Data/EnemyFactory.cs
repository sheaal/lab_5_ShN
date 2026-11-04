using System;
using System.Linq;
using System.Reflection;
using lab_4_ShN.Models;

namespace lab_4_ShN.Data
{
    public static class EnemyFactory
    {
        public static IEnemy Create(CEnemyTemplate template, BigNumber health, BigNumber gold)
        {
            string typeName = $"{template.TypeName}Enemy";

            var type = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == typeName && typeof(IEnemy).IsAssignableFrom(t));

            if (type == null)
                throw new Exception($"Класс врага не найден: {typeName}");

            return (IEnemy)Activator.CreateInstance(
                type,
                template.Name,
                template.Icon.ImagePath,
                health,
                gold,
                template
            )!;
        }
    }
}