using System.Text.Json.Serialization;

using FinBeatTech.Helpers;

namespace FinBeatTech.Dto
{
    [JsonConverter(typeof(CustomJsonConverter))]
    public class DataItemDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Value { get; set; }
    }

    public class ResponseDataItemDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Value { get; set; }
    }

    public class Wrapper
    {
        [JsonConverter(typeof(CustomJsonConverter))]
        public List<DataItemDto> Items { get; set; }
    }
}
