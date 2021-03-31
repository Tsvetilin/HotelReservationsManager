using AutoMapper;
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
        private readonly IMapper mapper;
        private const int usersOnPage = 10;
        public UsersController(IUserService userService,
                               UserManager<ApplicationUser> userManager,
                               IMapper mapper)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.mapper = mapper;
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

       // [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Add()
        {
            var user = await userManager.GetUserAsync(User);
            if (user?.EmployeeData != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return this.View();
        }

       // [Authorize(Roles = "Manager, Admin")]
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

        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Update(string id)
        {
            var employee = await userService.GetAsync(id);
            
            if (employee?.UserId != null)
            {
                var inputModel = mapper.Map<EmployeeData, EmployeeInputModel>(employee); ;
                return this.View(inputModel);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager, Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(EmployeeInputModel input, string id)
        {
            var employee = await userService.GetAsync(id);

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            var data = mapper.Map< EmployeeInputModel, EmployeeData>(input);
            data.UserId = employee.UserId;
            
            await userService.UpdateAsync(data);
            
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager, Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await userService.GetAsync(id);

            if (employee?.UserId!=null)
            {
                await userService.DeleteAsync(id);
            }

            return RedirectToAction("Index");
        }
    }
}
