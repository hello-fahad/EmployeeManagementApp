using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Filters
{
    public class EnsureDepartmentExistFilter : ActionFilterAttribute
    {



        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var departmentId = (int)context.ActionArguments["id"];

            var departmentRepository = context.HttpContext.RequestServices.GetService<IDepartmentRepository>();

            if(!departmentRepository.DepartmentExists(departmentId))
            {
                context.ModelState.AddModelError("id", "Department not found");

                var result = new ViewResult { ViewName = "Error" };

                var controller = context.Controller as Controller;
                result.ViewData = controller.ViewData;
                result.ViewData.Model = ModelStateHelper.GetErrors(context.ModelState);

                context.Result = result;
            }

        }
    }
}
