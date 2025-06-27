using Microsoft.AspNetCore.Mvc;
using WebApp.Filters;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Controllers
{
    [WriteToConsoleResourceFilter(Description = "Departments Controller")]
    public class DepartmentsController : Controller
    {
        public IDepartmentRepository DepartmentRepository { get; }

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            DepartmentRepository = departmentRepository;
        }

        [HttpGet]
        [WriteToConsoleResourceFilter(Description = "Index Method", Order = -1)]
        public IActionResult Index()
        {

            return View();
        }


        [Route("/Department-list/{filter?}")]
        public IActionResult SearchDepartments(string? filter)
        {
            return ViewComponent("DepartmentList", new { filter });
        }


        [HttpGet]
        [EndpointExpiresFilter(ExpiryDate = "2025-12-20")]
        [EnsureDepartmentExistFilter]
        public IActionResult Details(int id)
        {

            var department = DepartmentRepository.GetDepartmentById(id);
            
            return View(department);

        }

        [HttpPost]
        [EnsureValidModelStateFilter]
        public IActionResult Edit(Department department)
        {
            
            DepartmentRepository.UpdateDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        
        public IActionResult Create()
        {
            

            return View(new Department());

        }

        [HttpPost]
        [EnsureValidModelStateFilter]
        public IActionResult Create(Department department)
        {
            
            DepartmentRepository.AddDepartment(department);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [EnsureDepartmentExistFilter]
        public IActionResult Delete(int id)
        {
            var department = DepartmentRepository.GetDepartmentById(id);

            DepartmentRepository.DeleteDepartment(department);

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        //[HandleExceptionFilter]
        public IActionResult GetDepartments()
        {
            throw new ApplicationException("Testing exception handling for web api endpoints.");
            var departments = DepartmentRepository.GetDepartments();
            return Json(departments);
        }

    }
}
