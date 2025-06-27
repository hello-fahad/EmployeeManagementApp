using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;

namespace WebApp.Filters
{
    public class EnsureValidModelStateFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if(!context.ModelState.IsValid)
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
