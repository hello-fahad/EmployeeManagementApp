using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DepartmentsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var departments = DepartmentsRepository.GetDepartments();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var department = DepartmentsRepository.GetDepartmentById(id);
            if(department == null)
            {
                return Content("<h3 style='color: red'>Department not found.</h3>");
            }

            return View(department);

        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if(!ModelState.IsValid)
            {
                return Content(GetErrorHTML(), "text/html");
            }

            DepartmentsRepository.UpdateDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            var html = @"
                    <h1>Add Department</h1>
                    <form method='post' action='/departments/create'>
                        <label>Name: <input type='text' name='Name' /></label><br />
                        <label>Description: <input type='text' name='Description' /></label><br />
                        <br/>
                        <button type='submit'>Add</button>
                    </form>";

            return Content(html, "text/html");

        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(!ModelState.IsValid)
            {
                return Content(GetErrorHTML(), "text/html");
            }

            DepartmentsRepository.AddDepartment(department);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = DepartmentsRepository.GetDepartmentById(id);
            if(department == null)
            {
                ModelState.AddModelError("id", "Department not found.");
                return Content(GetErrorHTML(), "text/html");
            }

            DepartmentsRepository.DeleteDepartment(department);

            return RedirectToAction(nameof(Index));

        }


        private string GetErrorHTML()
        {
            List<string> errorMessages = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach(var error in value.Errors)
                {
                    errorMessages.Add(error.ErrorMessage);
                }
            }

            string html = string.Empty;
            if(errorMessages.Count > 0)
            {
                html = $@"
                     <ul>
                        {string.Join("", errorMessages.Select(error => $"<li style='color:red;'>{error}</li>"))}
                     </ul>";
            }

            return html;
        }
    }
}
