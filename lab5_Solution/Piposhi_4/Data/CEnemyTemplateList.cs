using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using lab_4_ShN.Models;

namespace lab_4_ShN.Data
{
    public class JsonEnemySaver : ISaveList<List<CEnemyTemplate>>
    {
        private readonly JsonSerializerOptions _options;

        public JsonEnemySaver()
        {
            _options = new JsonSerializerOptions { WriteIndented = true };
            _options.Converters.Add(new CEnemyTemplateConverter());
            _options.Converters.Add(new BigNumberConverter()); 
        }

        public List<CEnemyTemplate> Load(string path)
        {
            if (!File.Exists(path)) return new List<CEnemyTemplate>();
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<CEnemyTemplate>>(json, _options) ?? new List<CEnemyTemplate>();
        }

        public void Save(List<CEnemyTemplate> data, string path)
        {
            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(path, json);
        }
    }

    public class CEnemyTemplateList
    {
        private List<CEnemyTemplate> enemies;
        private readonly ISaveList<List<CEnemyTemplate>> _serializer;

        public CEnemyTemplateList()
        {
            enemies = new List<CEnemyTemplate>();
            _serializer = new JsonEnemySaver();
        }

        public void AddEnemy(CEnemyTemplate enemy) { if (enemy != null) enemies.Add(enemy); }
        public List<CEnemyTemplate> Enemies => enemies;

        public CEnemyTemplate? GetEnemyByName(string name) => enemies.Find(e => e.Name == name);

        public void DeleteEnemyByName(string name)
        {
            var found = GetEnemyByName(name);
            if (found != null) enemies.Remove(found);
        }

        public void SaveAll(string path) => _serializer.Save(enemies, path);

        public void LoadAll(string path)
        {
            enemies.Clear();
            enemies.AddRange(_serializer.Load(path));
        }
    }
}