using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Pages.Employees;

namespace WebApp.Filters
{
    public class EnsureEmployeeExistsPageFilter : Attribute, IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {

            if(context.HandlerMethod is not null && context.HandlerMethod.MethodInfo.Name == "OnGet")
            {
                return;
            }


            var employeeRepository = context.HttpContext.RequestServices.GetService<IEmployeeRepository>();


            object? employeeId;

            if (context.HandlerArguments.ContainsKey("id"))
            {
                employeeId = context.HandlerArguments["id"];
            }
            else
            {
                employeeId = ((EditModel)context.HandlerInstance).EmployeeViewModel.Employee.Id;
            }

            if(!employeeRepository.EmployeeExists((int)employeeId))
            {

                context.ModelState.AddModelError("id", "Employee not found");

                var errors = ModelStateHelper.GetErrors(context.ModelState);

                context.Result = new RedirectToPageResult("/Error", new { errors });
            }
            
            
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            
        }
    }
}
