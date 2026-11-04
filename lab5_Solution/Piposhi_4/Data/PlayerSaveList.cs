using System.IO;
using System.Text.Json;
using lab_4_ShN.Models;

namespace lab_4_ShN.Data
{
    public class PlayerSaveList : ISaveList<Player>
    {
        public void Save(Player player, string path)
        {
            var data = player.GetSaveData();
            var options = new JsonSerializerOptions { WriteIndented = true };
            options.Converters.Add(new BigNumberConverter()); 
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(path, json);
        }

        public Player Load(string path)
        {
            if (!File.Exists(path)) return new Player();
            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions();
            options.Converters.Add(new BigNumberConverter()); 
            var data = JsonSerializer.Deserialize<PlayerSaveData>(json, options);
            var p = new Player();
            if (data != null) p.LoadFromSaveData(data);
            return p;
        }
    }
}