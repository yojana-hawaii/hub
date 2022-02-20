using hub.dal.interfaces.directory;
using hub.domain.model.directory;
using hub.mvc.Models;
using hub.mvc.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace hub.mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployee _employee;

        public HomeController(ILogger<HomeController> logger, IEmployee employee)
        {
            _logger = logger;
            _employee = employee;
        }

        //Display all employee as default
        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employee.GetAllEmployees();
            var employeeViewModel = ConvertToEmployeeViewModel(employees);
            return View(employeeViewModel);
        }
        
        //Helper function to create view model
        private object ConvertToEmployeeViewModel(IEnumerable<Employee> employees)
        {
            var empViewModel = employees
                .Select(emp => new EmployeeViewModel
                {
                    EmployeeId = emp.EmployeeId,
                    Name = _employee.GetFullName(emp.EmployeeId),
                    Location = _employee.GetEmployeeLocation(emp.EmployeeId),
                    JobTitle = _employee.GetEmployeeJobTitle(emp.EmployeeId),
                    Department = _employee.GetEmployeeDepartment(emp.EmployeeId),
                    Phone = _employee.GetEmployeePhone(emp.EmployeeId),
                    Email = emp.Email,
                    Keyword = emp.Keyword,
                });


            return empViewModel;
        }


        //display user profile of logged in user
        public IActionResult UserProfile()
        {
            var adfs_username = User.Identity.Name;
            Employee loggedInUser = _employee.GetEmployeeByADUserName(adfs_username);
            UserProfileViewModel userProfileViewModel = ConvertToUserProfileViewModel(loggedInUser);
            return View(userProfileViewModel);
        }

        private UserProfileViewModel ConvertToUserProfileViewModel(Employee loggedInUser)
        {
            var _message = "Good";
            if (loggedInUser == null || string.IsNullOrEmpty(loggedInUser.Username))
            {
                _message = "Problem";
            }

            UserProfileViewModel userProfileVM = new UserProfileViewModel()
            {
                Title = "User Profile",
                Employee = loggedInUser,
                Message = _message,
            };
            return userProfileVM;
        }

        //Logout of ADFS and redirect to homepage. Call private method coz it did not logout properly. Stackoverflow
        public async Task Logout()
        {
            await CustomLogout("/");
        }
        private async Task CustomLogout(string redirectUri)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var prop = new AuthenticationProperties()
            {
                RedirectUri = redirectUri
            };

            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, prop);
        }

        //Testing user AD group access to apge
        [Authorize(Policy = "TestUser")]
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Policy = "TestAdmin")]
        public IActionResult AdfsClaims()
        {
            return View();
        }


        //default error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
