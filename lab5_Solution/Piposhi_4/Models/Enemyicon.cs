using System.Text.Json.Serialization;

namespace lab_4_ShN.Models
{
    public class EnemyIcon
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }

        [JsonConstructor]
        public EnemyIcon(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
        }

        public override string ToString() => Name;
    }
}