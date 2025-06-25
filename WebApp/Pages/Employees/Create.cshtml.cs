using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Employees
{
    public class CreateModel : PageModel
    {
        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }

        public CreateModel(
            IDepartmentRepository departmentRepository,
            IEmployeeRepository employeeRepository)
        {
            DepartmentRepository = departmentRepository;
            EmployeeRepository = employeeRepository;
        }

        [BindProperty]
        public EmployeeViewModel? EmployeeViewModel { get; set; }
        

        public void OnGet()
        {
            this.EmployeeViewModel = new EmployeeViewModel();
            this.EmployeeViewModel.Employee = new Employee();
            this.EmployeeViewModel.Departments = DepartmentRepository.GetDepartments();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                var errors = ModelStateHelper.GetErrors(ModelState);

                return RedirectToPage("/Error", new { errors});
            }

            if(this.EmployeeViewModel is not null && this.EmployeeViewModel.Employee is not null)
            {
                EmployeeRepository.AddEmployee(this.EmployeeViewModel.Employee);
            }

            return RedirectToPage("Index");
        }
    }
}
