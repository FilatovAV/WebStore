using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebStore
{
    class ActionFilter : IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }


}
