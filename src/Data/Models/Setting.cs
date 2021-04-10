using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    /// <summary>
    /// Default settings scheme
    /// </summary>
    public class Setting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
