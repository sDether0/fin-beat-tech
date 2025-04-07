using System.Text.Json.Serialization;
using System.Text.Json;
using FinBeatTech.Dto;
using FinBeatTech.Models;

namespace FinBeatTech.Helpers
{
    public class CustomJsonConverter : JsonConverter<List<DataItemDto>>
    {
        public override List<DataItemDto> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new List<DataItemDto>();

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Ожидался массив");

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException("Ожидался объект");

                reader.Read(); 
                string key = reader.GetString();
                reader.Read(); 
                string value = reader.GetString();
                reader.Read(); 

                result.Add(new DataItemDto { Code = int.Parse(key), Value = value });
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, List<DataItemDto> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException("For serialization use ResponseDataItemDto");
        }
    }
}
