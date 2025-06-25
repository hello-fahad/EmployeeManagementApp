using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Employees
{

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
            if(!ModelState.IsValid)
            {
                var errors = ModelStateHelper.GetErrors(ModelState);
                return RedirectToPage("/Error", new { errors });
            }

            if(EmployeeViewModel is not null && EmployeeViewModel.Employee != null)
            {
                EmployeeRepository.UpdateEmployee(EmployeeViewModel.Employee);
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPostDeleteEmployee(int id)
        {
            var employee = EmployeeRepository.GetEmployeeById(id);
            if(employee == null)
            {
                ModelState.AddModelError("id", "Employee not found");

                var errors = ModelStateHelper.GetErrors(ModelState);
                return RedirectToPage("/Error", new { errors });
            }

            EmployeeRepository.DeleteEmployee(employee);

            return RedirectToPage("Index");
        }

        
    }
}
