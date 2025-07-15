using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using lab3_inmind_part2.Exceptions; 

namespace lab3_inmind_part2.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var response = new ErrorResponse();

            _logger.LogError(exception, $" Exception: {exception.Message}");

            
            switch (exception)
            {
                case NotFoundException nfEx:
                    response.StatusCode = 404;
                    response.Message = nfEx.Message;
                    break;

                case ValidationException valEx:
                    response.StatusCode = 400;
                    response.Message = valEx.Message;
                    break;

                case DbUpdateException dbEx:
                    response.StatusCode = 500;
                    response.Message = "A database error occurred.";
                    break;

                case Exception ex when ex.Message.Contains("timeout"):
                    response.StatusCode = 408;
                    response.Message = "Request timed out.";
                    break;

                default:
                    response.StatusCode = 500;
                    response.Message = "An unexpected error occurred.";
                    break;
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };

            context.ExceptionHandled = true;
        }

        private class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }
    }
}
