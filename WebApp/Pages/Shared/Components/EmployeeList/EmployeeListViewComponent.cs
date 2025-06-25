using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Pages.Shared.Components.EmployeeList
{
    public class EmployeeListViewComponent : ViewComponent
    {

        public IEmployeeRepository EmployeeRepository { get; }

        public EmployeeListViewComponent(IEmployeeRepository employeeRepository)
        {
            EmployeeRepository = employeeRepository;
        }


        public IViewComponentResult Invoke(string? filter, int? departmentId)
        {
            return View(EmployeeRepository.GetEmployees(filter, departmentId));
        }
    }
}
