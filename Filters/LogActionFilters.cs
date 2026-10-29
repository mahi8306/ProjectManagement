using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductManagement.Models;

namespace ProductManagement.Filters
{
    public class ValidateProductFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("product", out var obj) && obj is Product p)
            {
                if (string.IsNullOrWhiteSpace(p.Name) || p.Price <= 0)
                    context.Result = new BadRequestObjectResult("Name required aur Price > 0 hona chahiye");
            }
        }
    }
}
