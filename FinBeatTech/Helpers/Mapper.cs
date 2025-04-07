using FinBeatTech.Dto;
using FinBeatTech.Models;

namespace FinBeatTech.Helpers
{
    public static class Mapper
    {
        public static List<ResponseDataItemDto> ToDto(this List<DataItem> dataItem)
        {
            return dataItem.Select(x => new ResponseDataItemDto { Id = x.Id, Code = x.Code, Value = x.Value }).ToList();
        }

        public static List<DataItem> ToData(this List<DataItemDto> dataItem)
        {
            return dataItem.Select((x, i) => new DataItem { Id = i+1, Code = x.Code, Value = x.Value }).ToList();
        }
    }
}
