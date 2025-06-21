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
            this.Employees = EmployeeRepository.GetEmployees();
        }
    }
}
