using Microsoft.AspNetCore.Mvc.Filters;
using SharedLib.ResponseWrapper;

namespace SharedLib.Filters;

public class AutoValidateModelState : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = ResponseFactory.BadRequest(context.ModelState);
        }
    }
}