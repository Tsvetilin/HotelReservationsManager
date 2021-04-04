namespace Web.Models.ViewModels
{
    public class UserDataViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdult { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? EmployeeDataIsActive { get; set; }
    }
}
