using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using lab_4_ShN.Models;

namespace lab_4_ShN.Data
{
    public class CEnemyTemplateConverter : JsonConverter<CEnemyTemplate>
    {
        public override CEnemyTemplate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("$type", out JsonElement typeProp))
                throw new JsonException("В JSON шаблона отсутствует поле $type.");

            string type = typeProp.GetString()!;
            string rawText = root.GetRawText();

            return type switch
            {
                "CNormalEnemyTemplate" => JsonSerializer.Deserialize<CNormalEnemyTemplate>(rawText, options)!,
                "CArmoredEnemyTemplate" => JsonSerializer.Deserialize<CArmoredEnemyTemplate>(rawText, options)!,
                "CRegeneratingEnemyTemplate" => JsonSerializer.Deserialize<CRegeneratingEnemyTemplate>(rawText, options)!,
                _ => throw new NotSupportedException($"Неизвестный тип противника: {type}")
            };
        }

        public override void Write(Utf8JsonWriter writer, CEnemyTemplate value, JsonSerializerOptions options)
        {
            string typeName = value.GetType().Name;
            string json = JsonSerializer.Serialize(value, value.GetType(), options);

            using JsonDocument doc = JsonDocument.Parse(json);

            writer.WriteStartObject();
            writer.WriteString("$type", typeName);
            foreach (var property in doc.RootElement.EnumerateObject())
                property.WriteTo(writer);
            writer.WriteEndObject();
        }
    }
}