using System.Net;
using System.Text.Json;

namespace API.Middleware
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception");

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var errorResponse = new
				{
					StatusCode = context.Response.StatusCode,
					Message = "An unexpected error occurred.",
					Details = ex.Message
				};

				var json = JsonSerializer.Serialize(errorResponse);

				await context.Response.WriteAsync(json);
			}
		}
	}
}
