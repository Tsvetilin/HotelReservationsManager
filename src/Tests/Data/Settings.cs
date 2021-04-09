using Data.Models;

namespace Tests.Data
{
    /// <summary>
    /// Settings test data
    /// </summary>
    public class Settings
    {
        public static readonly Setting AllInclusive = new()
        {
            Key = "AllInclusivePrice",
            Value = "100",
            Type = typeof(double).ToString(),
        };
        public static readonly Setting Breakfast = new()
        {
            Key = "BreakfastPrice",
            Value = "50",
            Type = typeof(double).ToString(),
        };
    }
}
