using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PaymentAPI.Helpers.Constants;
using PaymentAPI.Models;
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
            catch (Exception ex)
            {
                Log.Error($"Exception message: {ex} ");
                await HandleExceptionAsync(httpContext);
            }
        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo
            {
                Message = MiddlewareConstants.STANDART_ERROR_MESSAGE,
            }));
        }
    }
}