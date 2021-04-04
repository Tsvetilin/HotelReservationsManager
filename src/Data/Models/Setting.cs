using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Setting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
