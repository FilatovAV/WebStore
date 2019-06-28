using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebStore
{
    class ExceptionFilter : IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }


}
