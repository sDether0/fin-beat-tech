using System.ComponentModel.DataAnnotations;

namespace FinBeatTech.Models
{
    public class DataItem
    {
        [Key]
        public int Id { get; set; }
        public int Code { get; set; }
        public string Value { get; set; }
    }
}
