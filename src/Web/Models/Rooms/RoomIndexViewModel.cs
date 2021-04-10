using Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models.Rooms;

namespace Web.Models.ViewModels
{
    public class RoomIndexViewModel : PageViewModel
    {
        public IEnumerable<RoomViewModel> Rooms { get; set; }
        public double AllInclusivePrice { get; set; }
        public double BreakfastPrice { get; set; }
        public int MaxCapacity { get; set; }
        public bool AvailableOnly { get; set; }
        public RoomType[] Types { get; set; }
        public int MinCapacity { get; set; }

        public List<SelectListItem> GetCapacitySelectList()
        {
            return Enumerable.Range(1, MaxCapacity).Select(x =>
            new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString(),
                Selected = x == MinCapacity,
            }).ToList();
        }

        public List<SelectListItem> GetTypesSelectList()
        {
            var result = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value="",
                    Text="All",
                    Selected=false,
                }
            };

            var enumValues = Enum.GetValues(typeof(RoomType));
            foreach (var value in enumValues)
            {
                result.Add(
                    new SelectListItem
                    {
                        Value = ((int)value).ToString(),
                        Text = value.ToString(),
                        Selected = Types.Contains((RoomType)value),
                    }) ;
            }

            return result;
        }
    }
}
