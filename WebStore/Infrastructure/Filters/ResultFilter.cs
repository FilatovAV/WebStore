using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebStore
{
    class ResultFilter : IResultFilter
    {
        void IResultFilter.OnResultExecuted(ResultExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }


}
