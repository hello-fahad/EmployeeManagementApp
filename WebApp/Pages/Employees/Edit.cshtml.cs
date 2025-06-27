using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Filters;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Employees
{
    [EnsureValidModelStatePageFilter]
    [EnsureEmployeeExistsPageFilter]
    public class EditModel : PageModel
    {
        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }

        public EditModel(IDepartmentRepository departmentRepository, IEmployeeRepository employeeRepository)
        {
            DepartmentRepository = departmentRepository;
            EmployeeRepository = employeeRepository;
        }

        [BindProperty]
        public EmployeeViewModel? EmployeeViewModel { get; set; }
       

        public void OnGet(int id)
        {
            this.EmployeeViewModel = new EmployeeViewModel();
            this.EmployeeViewModel.Employee = EmployeeRepository.GetEmployeeById(id);
            this.EmployeeViewModel.Departments = DepartmentRepository.GetDepartments();
        }


        public IActionResult OnPost()
        {
            
            if(EmployeeViewModel is not null && EmployeeViewModel.Employee != null)
            {
                EmployeeRepository.UpdateEmployee(EmployeeViewModel.Employee);
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPostDeleteEmployee(int id)
        {
            var employee = EmployeeRepository.GetEmployeeById(id);

            EmployeeRepository.DeleteEmployee(employee);

            return RedirectToPage("Index");
        }
    }
}
