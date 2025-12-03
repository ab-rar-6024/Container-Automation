using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContainerAutomationApp.Filters
{
    public class AdminOnly : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        if (string.IsNullOrEmpty(session.GetString("AdminUsername")))
        {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
        }
    }
}

}
