using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Mapping;
using System;
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
        public async Task<IActionResult> Index(int id = 1, string search = "")
        {
            if (!string.IsNullOrEmpty(search))
            {
                var searchResult = await userService.GetEmployeesSearchResults<EmployeeDataViewModel>(search);

                if (searchResult.Any())
                {
                    return View(new EmployeesIndexViewModel
                    {
                        PagesCount = 1,
                        CurrentPage = 1,
                        Employees = searchResult.ToList()
                    });
                }
                ModelState.AddModelError("Found", "User not found!");
            }

            int pageCount = (int)Math.Ceiling((double)userService.CountAllEmployees() / usersOnPage);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }
            var employees = await userService.GetEmployeePageItems<EmployeeDataViewModel>(id, usersOnPage);
            EmployeesIndexViewModel viewModel = new()
            {
                PagesCount = pageCount,
                CurrentPage = id,
                Employees = employees.ToList(),
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Add()
        {
            var user = await userManager.GetUserAsync(User);
            if (user?.EmployeeData != null)
            {
                return RedirectToAction("Index", "Users");
            }
            return this.View();
        }

        [Authorize(Roles = "Employee, Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(EmployeeInputModel input)
        {
            if (userService.IsAlreadyAdded(input.UserEmail))
            {
                ModelState.AddModelError("Added", "User already added!");
            }

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            var appUser = new ApplicationUser
            {
                UserName = input.UserUserName,
                IsAdult = input.UserIsAdult,
                Email = input.UserEmail,
                FirstName = input.UserFirstName,
                LastName = input.UserLastName,
                PhoneNumber = input.UserPhoneNumber,
                SecurityStamp = DateTime.UtcNow.Ticks.ToString()
            };

            await userManager.CreateAsync(appUser, input.UserPassword);
            await userManager.AddToRoleAsync(appUser, "Employee");

            var employee = new EmployeeData
            {
                UserId = appUser.Id,
                UCN = input.UCN,
                SecondName = input.SecondName,
                IsActive = true,
                DateOfAppointment = DateTime.UtcNow,
                User = appUser
            };

            await userService.AddAsync(employee);

            return RedirectToAction("Index", "Users");
        }

        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Update(string id)
        {
            var employee = await userService.GetAsync<EmployeeInputModel>(id);
            var appUser = await userManager.FindByIdAsync(id);

            if (employee != null)
            {
                return this.View(employee);
            }

            return RedirectToAction("Index", "Users");
        }

        [Authorize(Roles = "Employee, Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(EmployeeInputModel input, string id)
        {
            var employee = await userService.GetAsync<EmployeeDataViewModel>(id);

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            var data = MappingConfig.Instance.Map<EmployeeData>(input);
            var appUserData = MappingConfig.Instance.Map<ApplicationUser>(input);
            data.UserId = employee.UserId;

            await userService.UpdateAsync(data);
            await userManager.UpdateAsync(appUserData);

            return RedirectToAction("Index", "Users");
        }

        [Authorize(Roles = "Employee, Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await userService.GetAsync<EmployeeDataViewModel>(id);

            if (employee != null)
            {
                var userInContext = await userManager.FindByIdAsync(id);
                await userManager.RemoveFromRoleAsync(userInContext, "Employee");
                await userManager.AddToRoleAsync(userInContext, "User");
                await userService.DeleteAsync(id);
            }

            return RedirectToAction("Index", "Users");
        }


        //Add userscounter
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> All(int id = 1, string search = "")
        {
            if (!string.IsNullOrEmpty(search))
            {
                var searchResult = await userService.GetUsersSearchResults<ApplicationUser>(search);

                if (searchResult.Any())
                {
                    return View(new UserIndexViewModel
                    {
                        PagesCount = 1,
                        CurrentPage = 1,
                        Users = searchResult.ToList()
                    });
                }
                ModelState.AddModelError("Found", "User not found!");
            }

            int pageCount = (int)Math.Ceiling((double)userService.CountAllEmployees() / usersOnPage);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }
            var users = await userService.GetUserPageItems<ApplicationUser>(id, usersOnPage);
            UserIndexViewModel viewModel = new()
            {
                PagesCount = pageCount,
                CurrentPage = id,
                Users = users.ToList(),
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Promote(string id = null)
        {
            //Todo

            return this.View();
        }
    }
}
