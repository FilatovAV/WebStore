using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebStore
{
    class AutorizeFilter : IAuthorizationFilter
    {
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }


}
