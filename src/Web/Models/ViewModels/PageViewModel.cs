namespace Web.Models.ViewModels
{
    public class PageViewModel
    {
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string[] Params { get; set; }
    }
}
