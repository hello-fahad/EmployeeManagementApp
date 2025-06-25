using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Views.Shared.Components.DepartmentList
{
    [ViewComponent]
    public class DepartmentListViewComponent : ViewComponent
    {
        public IDepartmentRepository DepartmentRepository { get; }
        public DepartmentListViewComponent(IDepartmentRepository departmentRepository)
        {
            DepartmentRepository = departmentRepository;
        }

        public IViewComponentResult Invoke(string? filter)
        {
            var departments = DepartmentRepository.GetDepartments(filter);

            return View(departments);
        }
    }
}
