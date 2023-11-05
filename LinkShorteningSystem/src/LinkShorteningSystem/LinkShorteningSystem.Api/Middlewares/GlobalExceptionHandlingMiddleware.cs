using LinkShorteningSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace LinkShorteningSystem.WebApi.Middlewares
{
    public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Invokes the custom middleware to handle incoming HTTP requests.
        /// </summary>
        /// <param name="context">The HTTP context representing the current request and response.</param>
        /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
        /// <returns>A task representing the asynchronous middleware operation.</returns>
        /// <exception cref="OriginalLinkNotFoundException">Thrown when the requested OriginalLink is not found.</exception>
        /// <exception cref="ShortenedLinkNotFoundException">Thrown when the requested ShortendLink is not found.</exception>
        /// <exception cref="InvalidValueException">Thrown when an invalid value is encountered.</exception>
        /// <exception cref="Exception">Thrown when an unhandled exception occurs.</exception>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (InvalidValueException ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Bad request",
                    Detail = "An invalid value has been encountered"
                };

                string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);

                context.Response.ContentType = "application/json";
            }
            catch (Exception ex) when (ex is OriginalLinkNotFoundException || ex is ShortenedLinkNotFoundException)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Not found error",
                    Detail = "A not found error has occured"
                };

                string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);

                context.Response.ContentType = "application/json";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Server error",
                    Detail = "An internal server error has occured"
                };

                string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);

                context.Response.ContentType = "application/json";
            }
        }
    }
}