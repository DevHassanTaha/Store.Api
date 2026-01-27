using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Store.G02.Domain.Exceptions.NotFound;
using Store.G02.Shard.ErrorModels;

namespace Store.G02.Web.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // logic

                // 1. Set Status Code Of Response
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                // 2. Set Content Type Of Response

                context.Response.ContentType = "application/json";

                // 3. Set Body Of Response
                var response = new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = ex.Message
                };

                //return response
                await context.Response.WriteAsJsonAsync(response);

            }
        }
    }
}
