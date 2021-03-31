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
        public async Task< IActionResult> Index(int id = 1, string search = "")
        {
            if (!string.IsNullOrEmpty(search))
            {
                var searchResult = await userService.GetSearchResults<EmployeeDataViewModel>(search);

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
            var employees = await userService.GetPageItems<EmployeeDataViewModel>(id, usersOnPage);
            EmployeesIndexViewModel viewModel = new()
            {
                PagesCount = pageCount,
                CurrentPage = id,
                Employees = employees.ToList(),
            };

            return View(viewModel);
        }

       // [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Add()
        {
            var user = await userManager.GetUserAsync(User);
            if (user?.EmployeeData != null)
            {
                return RedirectToAction("Index", "Userr");
            }
            return this.View();
        }

       // [Authorize(Roles = "Manager, Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(EmployeeInputModel input)
        {
            //Doesn't add in .netusers 
            //TO DO
            if (userService.IsAlreadyAdded(input.Email))
            {
                ModelState.AddModelError("Added", "User already added!");
            }

            var appUser = new ApplicationUser
            {
                UserName= input.UserName,
                IsAdult = input.IsAdult,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                PhoneNumber = input.PhoneNumber
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
                DateOfAppointment = DateTime.UtcNow
            };

            await userService.AddAsync(employee);

            return RedirectToAction("Index", "Users");
        }

       // [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Update(string id)
        {
            var employee = await userService.GetAsync<EmployeeInputModel>(id);
            
            if (employee != null)
            {
                return this.View(employee);
            }

            return RedirectToAction("Index", "Users");
        }

      //  [Authorize(Roles = "Manager, Admin")]
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

        [Authorize(Roles = "Manager, Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await userService.GetAsync<EmployeeDataViewModel>(id);

            if (employee!=null)
            {
                await userService.DeleteAsync(id);
            }

            return RedirectToAction("Index", "Users");
        }
    }
}
