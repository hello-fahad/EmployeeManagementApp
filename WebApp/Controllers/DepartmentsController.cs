using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DepartmentsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        //[Route("/Department-list/{filter?}")]
        //public IActionResult SearchDepartments(string? filter)
        //{
        //    var departments = DepartmentRepository.GetDepartments(filter);
        //    return PartialView("_DepartmentList", departments);
        //}


        [Route("/Department-list/{filter?}")]
        public IActionResult SearchDepartments(string? filter)
        {
            return ViewComponent("DepartmentList", new { filter });
        }


        [HttpGet]
        public IActionResult Details(int id)
        {

            var department = DepartmentRepository.GetDepartmentById(id);
            if(department == null)
            {
                return View("Error", new List<string>() { "Department not found."});
            }

            return View(department);

        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if(!ModelState.IsValid)
            {
                return View("Error", ModelStateHelper.GetErrors(ModelState));
            }

            DepartmentRepository.UpdateDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            

            return View(new Department());

        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(!ModelState.IsValid)
            {
                return View("Error", ModelStateHelper.GetErrors(ModelState));
            }

            DepartmentRepository.AddDepartment(department);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = DepartmentRepository.GetDepartmentById(id);
            if(department == null)
            {
                ModelState.AddModelError("id", "Department not found.");
                return View("Error", ModelStateHelper.GetErrors(ModelState));
            }

            DepartmentRepository.DeleteDepartment(department);

            return RedirectToAction(nameof(Index));

        }

    }
}
