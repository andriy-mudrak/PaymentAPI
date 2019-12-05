using System;
using System.Net;
using System.Threading.Tasks;
using BLL.Helpers.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace PaymentAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                var statusCode = httpContext.Response?.StatusCode;
                Log.Information($"Status code: {statusCode}");
            }
            catch (NotValidModelException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception message)
        {
            Log.Error($"Exception message: {message} ");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(new 
            {
                Message = "We are currently experiencing some issues. Please try again later."
            }.ToString());
        }
    }
}