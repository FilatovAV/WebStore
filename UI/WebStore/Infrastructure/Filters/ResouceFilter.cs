using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebStore
{
    class ResouceFilter : IResourceFilter
    {
        void IResourceFilter.OnResourceExecuted(ResourceExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IResourceFilter.OnResourceExecuting(ResourceExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }


}
