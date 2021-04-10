namespace Web.Models.ViewModels
{
    public class HomePageViewModel : RoomIndexViewModel
    {
        public int TotalReservationsMade { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
    }
}
