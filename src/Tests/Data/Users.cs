using Data.Models;
using System;

namespace Tests.Data
{
    /// <summary>
    /// Users test data
    /// </summary>
    public static class Users
    {
        public static readonly ApplicationUser User1Employee = new()
        {
        };
        public static readonly ApplicationUser User2Employee = new()
        {
        };
        public static readonly ApplicationUser User3NotEmployee = new()
        {
        };
        public static readonly ApplicationUser User4NotEmployee = new ()
        {
        };
        public static readonly ClientData Client1User = new ()
        {
        };
        public static readonly ClientData Client2User = new ()
        {
        };
        public static readonly ClientData Client3User = new()
        {
        };
        public static readonly ClientData Client4User = new()
        {
        };
        public static readonly ClientData Client5User = new()
        {
        };
        public static readonly ClientData Client6User = new()
        {
        };

        public static readonly EmployeeData EmployeeUser1 = new ()
        {
            UserId = User1Employee.Id,
        };
        public static readonly EmployeeData EmployeeUser2 = new ()
        {
            UserId = User2Employee.Id,
        };

        public const string searchParam = "Test";
        public static readonly ApplicationUser UserForSearch = new()
        {
            Email = searchParam,
            FirstName=searchParam,
            LastName=searchParam,
            PhoneNumber=searchParam,
            UserName=searchParam,
        };
        public static readonly EmployeeData EmployeeForSearch = new()
        {
            UserId = UserForSearch.Id,
            SecondName = searchParam,
            UCN = searchParam,
            DateOfAppointment = DateTime.Today
        };

        public const string searchParam2 = "Test2";
        public static readonly ApplicationUser UserForSearch2 = new()
        {
            Email = searchParam2,
            FirstName = searchParam2,
            LastName = searchParam2,
            PhoneNumber = searchParam2,
            UserName = searchParam2,
        };
        public static readonly EmployeeData EmployeeForSearch2 = new()
        {
            UserId = UserForSearch2.Id,
            SecondName = searchParam2,
            UCN = searchParam2,
        };
        public static readonly ClientData ClientForSearch = new()
        {
            Email = searchParam,
            FullName = searchParam,
            IsAdult = false
        };
    }
}
