using Data.Models;
using System.Collections.Generic;

namespace Tests.Data
{
    public static class Users
    {
        public static readonly ApplicationUser User1Employee = new ApplicationUser
        {
        };
        public static readonly ApplicationUser User2Employee = new ApplicationUser
        {
        };
        public static readonly ApplicationUser User3NotEmployee = new ApplicationUser
        {
        };
        public static readonly ApplicationUser User4NotEmployee = new ApplicationUser
        {
        };
        public static readonly ClientData Client1User = new ClientData
        {
        };
        public static readonly ClientData Client2User = new ClientData
        {
        };

        public static readonly EmployeeData EmployeeUser1 = new EmployeeData
        {
            UserId = User1Employee.Id,
        };
        public static readonly EmployeeData EmployeeUser2 = new EmployeeData
        {
            UserId = User2Employee.Id,
        };

        public const string searchParam = "Test";
        public static readonly ApplicationUser UserForSearch = new ApplicationUser
        {
            Email = searchParam,
            FirstName=searchParam,
            LastName=searchParam,
            PhoneNumber=searchParam,
            UserName=searchParam,
        };
        public static readonly EmployeeData EmployeeForSearch = new EmployeeData
        {
            UserId=UserForSearch.Id,
            SecondName=searchParam,
            UCN=searchParam,
        };

        public const string searchParam2 = "Test2";
        public static readonly ApplicationUser UserForSearch2 = new ApplicationUser
        {
            Email = searchParam2,
            FirstName = searchParam2,
            LastName = searchParam2,
            PhoneNumber = searchParam2,
            UserName = searchParam2,
        };
        public static readonly EmployeeData EmployeeForSearch2 = new EmployeeData
        {
            UserId = UserForSearch2.Id,
            SecondName = searchParam2,
            UCN = searchParam2,
        };
        public static readonly ClientData ClientForSearch = new ClientData
        {
            Email = searchParam,
            FullName = searchParam,
            IsAdult = false
        };
    }
}
