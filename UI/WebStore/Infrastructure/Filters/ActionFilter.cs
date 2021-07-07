using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebStore
{
    internal class ActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }

    //internal class ActionFilterAsync : Attribute, IAsyncActionFilter
    internal class ActionFilterAsync : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //обработка context перед началом дальнейших действий

            var next_task = next();

            //набор действий выполняемых парралельно

            await next_task;

            //обработка результата

            await next();
        }
    }
}
