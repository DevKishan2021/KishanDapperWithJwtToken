using DapperWithJwtBL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace KishanDapperWithJwtToken.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = exception.Message;

            switch (exception)
            {
                case DbUpdateException dbEx:
                    statusCode = HttpStatusCode.BadRequest;
                    if (dbEx.InnerException is SqlException sqlEx)
                    {
                        // Error number for unique constraint violation
                        if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                        {
                            message = "Duplicate key error: A record with the same key already exists.";
                        }
                        else
                        {
                            message = $"SQL Error: {sqlEx.Message}";
                        }
                    }
                    else
                    {
                        message = $"Database Update Error: {dbEx.InnerException?.Message}";
                    }
                    _logger.LogError(message);
                    break;
                default:
                    _logger.LogError($"Unhandled Exception: {exception.Message}");
                    break;
            }

            var errResponse = new ApiResponse(message, statusCode, false);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(errResponse.ToString());
        }
    }
}
