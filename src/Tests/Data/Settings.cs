using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data
{
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
