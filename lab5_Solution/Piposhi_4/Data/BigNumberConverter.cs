using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace lab_4_ShN.Data
{
    public class BigNumberConverter : JsonConverter<BigNumber>
    {
        public override BigNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
                return new BigNumber(reader.GetString()!);
            if (reader.TokenType == JsonTokenType.Number)
                return new BigNumber(reader.GetInt32());
            throw new JsonException($"Ожидалась строка или число для BigNumber, получено {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, BigNumber value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}