using Core.Exceptions;

namespace MovieTheater.Middleware
{
    public class HandleExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"Error 404 NotFound: {ex.Message}");
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Error 400 BadRequest: {ex.Message}");
            }
            catch (Exception ex) 
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Error 500 InternalServerError: {ex}");
            }
        }
    }
}
