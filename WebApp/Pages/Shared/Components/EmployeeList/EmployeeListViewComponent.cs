using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Pages.Shared.Components.EmployeeList
{
    public class EmployeeListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string? filter)
        {
            return View(EmployeeRepository.GetEmployees(filter));
        }
    }
}
