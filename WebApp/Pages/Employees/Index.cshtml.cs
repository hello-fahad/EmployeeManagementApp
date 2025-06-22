using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.Employees
{
    public class IndexModel : PageModel
    {
        public List<Employee>? Employees { get; set; }
        public void OnGet()
        {
            //this.Employees = EmployeeRepository.GetEmployees();
        }

        public IActionResult OnGetSearchEmployeeResult(string? filter)
        {
            //@await Component.InvokeAsync("EmployeeList")
            return ViewComponent("EmployeeList", new { filter });
        }
    }
}
