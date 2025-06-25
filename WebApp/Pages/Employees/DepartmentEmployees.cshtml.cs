using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.Employees
{
    public class DepartmentEmployeesModel : PageModel
    {
        public IDepartmentRepository DepartmentRepository { get; }

        public DepartmentEmployeesModel(IDepartmentRepository departmentRepository)
        {
            DepartmentRepository = departmentRepository;
        }

        public string? DepartmentName { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? DepartmentId { get; set; }

        public void OnGet()
        {
            if(DepartmentId.HasValue)
            {
                var department = DepartmentRepository.GetDepartmentById(DepartmentId.Value);
                DepartmentName = department?.Name;
            }
        }
    }
}
