using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IUserService userService,
                               UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(int id = 1, string search = "", int pageSize = 10)
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
                        Employees = searchResult.ToList(),
                        Controller = "Users",
                        Action = nameof(Index),
                    });
                }
                ModelState.AddModelError("Found", "User not found!");
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            int pageCount = (int)Math.Ceiling((double)userService.CountAllEmployees() / pageSize);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }
            var employees = await userService.GetEmployeePageItems<EmployeeDataViewModel>(id, pageSize);
            EmployeesIndexViewModel viewModel = new()
            {
                PagesCount = pageCount,
                CurrentPage = id,
                Employees = employees.ToList(),
                Controller = "Users",
                Action = nameof(Index),
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Add()
        {
            var user = await userManager.GetUserAsync(User);
            if (user?.EmployeeData != null)
            {
                return RedirectToAction("Index", "Users");
            }
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeInputModel input)
        {
            if (userService.IsAlreadyAdded(input.Email))
            {
                ModelState.AddModelError("Added", "User already added!");
            }

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            var appUser = new ApplicationUser
            {
                UserName = input.UserName,
                IsAdult = input.IsAdult,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                PhoneNumber = input.PhoneNumber,
                SecurityStamp = DateTime.UtcNow.Ticks.ToString()
            };

            var res = await userManager.CreateAsync(appUser, input.Password);
            if (res.Errors?.Any() ?? false)
            {
                ModelState.AddModelError("General", string.Join("; ", res.Errors.Select(x => x.Description)));
                return this.View(input);
            }
            res = await userManager.AddToRoleAsync(appUser, "Employee");
            if (res.Errors?.Any() ?? false)
            {
                ModelState.AddModelError(nameof(input.Password), string.Join("; ", res.Errors.Select(x => x.Description)));
                return this.View(input);
            }

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

        public async Task<IActionResult> Update(string id)
        {
            var employee = await userService.GetEmployeeAsync<EmployeeInputModel>(id);
            var userData = await userService.GetUserAsync<EmployeeInputModel>(id);

            if (userData != null)
            {
                userData.UCN = employee.UCN;
                userData.SecondName = employee.SecondName;
                return this.View(userData);
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeInputModel input, string id)
        {
            var userData = await userService.GetUserAsync<EmployeeInputModel>(id);

            if (userData == null)
            {
                return this.NotFound();
            }

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }
            var user = await userManager.FindByIdAsync(id);
            user.FirstName = input.FirstName;
            user.LastName = input.LastName;
            user.IsAdult = input.IsAdult;
            user.PhoneNumber = input.PhoneNumber;
            user.UserName = input.UserName;
            user.Email = input.Email;

            var data = await userService.GetEmployeeAsync<EmployeeDataViewModel>(id);
            var employee = new EmployeeData
            {
                IsActive = true,
                DateOfAppointment = data.DateOfAppointment,
                DateOfResignation = null,
                SecondName = data.SecondName,
                UCN = input.UCN,
                User = user,
                UserId = id

            };

            user.EmployeeData = employee;

            await userService.UpdateAsync(employee);
            await userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Users");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await userService.GetEmployeeAsync<EmployeeDataViewModel>(id);

            if (employee != null)
            {
                var userInContext = await userManager.FindByIdAsync(id);
                await userManager.RemoveFromRoleAsync(userInContext, "Employee");
                await userManager.AddToRoleAsync(userInContext, "User");
                await userService.DeleteAsync(id);
            }

            return RedirectToAction("Index", "Users");
        }

        public async Task<IActionResult> All(int id = 1, string search = "", int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var searchResult = await userService.GetUsersSearchResults<UserDataViewModel>(search);

                if (searchResult.Any())
                {
                    return View(new UserIndexViewModel
                    {
                        PagesCount = 1,
                        CurrentPage = 1,
                        Users = searchResult.ToList(),
                        Controller = "Users",
                        Action = nameof(All),
                    });
                }
                ModelState.AddModelError("Found", "User not found!");
            }

            int pageCount = (int)Math.Ceiling((double)userService.CountAllUsers() / pageSize);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }
            var users = await userService.GetUserPageItems<UserDataViewModel>(id, pageSize);
            UserIndexViewModel viewModel = new()
            {
                PagesCount = pageCount,
                CurrentPage = id,
                Users = users.ToList(),
                Controller = "Users",
                Action = nameof(Index),
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Promote(string id)
        {
            var user = await userService.GetUserAsync<EmployeeInputModel>(id);

            if (user != null)
            {
                return this.View(user);
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Promote(EmployeeInputModel input, string id)
        {
            var userData = await userService.GetUserAsync<EmployeeInputModel>(id);

            if (userData == null)
            {
                return this.NotFound();
            }

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            var appUser = await userManager.FindByIdAsync(id);
            await userManager.AddToRoleAsync(appUser, "Employee");

            var employee = new EmployeeData
            {
                UserId = id,
                UCN = input.UCN,
                SecondName = input.SecondName,
                IsActive = true,
                DateOfAppointment = DateTime.UtcNow,
                User = appUser,
                DateOfResignation = null,
            };

            await userService.UpdateAsync(employee);

            return RedirectToAction("Index", "Users");
        }
    }
}
