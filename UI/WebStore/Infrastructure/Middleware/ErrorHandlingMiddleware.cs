using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ErrorHandlingMiddleware> _Logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this._Next = next;
            this._Logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                throw;
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            _Logger.LogError(error.Message, "В ходе обработки входящего запроса произошло не обработанное исключение");
            return Task.CompletedTask;
        }
    }
}
