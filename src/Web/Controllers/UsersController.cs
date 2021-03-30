using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private const int usersOnPage = 10;
        public UsersController(IUserService userService,
                               UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }
        public IActionResult Index(int id = 1, string search = "")
        {
            if (!string.IsNullOrEmpty(search))
            {
                var searchResult = userService.GetSearchResults<EmployeeDataViewModel>(search).ToList();
                if (searchResult.Count != 0)
                {
                    return View(new EmployeesIndexViewModel
                    {
                        PagesCount = 1,
                        CurrentPage = 1,
                        Employees = searchResult
                    });
                }
                ModelState.AddModelError("Found", "User not found!");
            }

            int pageCount = (int)Math.Ceiling((double)userService.CountAllEmployees() / usersOnPage);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }
            var employees = userService.GetPageItems<EmployeeDataViewModel>(id, usersOnPage).ToList();
            EmployeesIndexViewModel viewModel = new EmployeesIndexViewModel
            {
                PagesCount = pageCount,
                CurrentPage = id,
                Employees = employees,
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Add()
        {
            var user = await userManager.GetUserAsync(User);
            if (user?.EmployeeData != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return this.View();
        }

        [Authorize(Roles = "Manager, Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(EmployeeInputModel input)
        {
            if (userService.IsAlreadyAdded(input.Email))
            {
                ModelState.AddModelError("Added", "Cinema already added!");
            }

            var appUser = new ApplicationUser
            {
                IsAdult = input.IsAdult,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
            };
            var passwordHasher = new PasswordHasher<ApplicationUser>(); 
            appUser.PasswordHash = passwordHasher.HashPassword(appUser, input.Password);

            await userManager.CreateAsync(appUser);

            var employee = new EmployeeData
            {
                UserId = appUser.Id,
                UCN = input.UCN,
                SecondName = input.SecondName,
                IsActive = true,
                DateOfAppointment = DateTime.Now
            };

            await userService.AddAsync(employee);

            return RedirectToAction(nameof(Index));
        }
    }
}
