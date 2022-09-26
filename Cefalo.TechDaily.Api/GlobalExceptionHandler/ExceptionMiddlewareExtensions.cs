
using Cefalo.TechDaily.Service.CustomExceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Cefalo.TechDaily.Api.GlobalExceptionHandler
{
    public static class ExceptionMiddlewareExtensions
    {
        
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Type type = contextFeature.Error.GetType();
                        context.Response.StatusCode = GetStatusCode(type);
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "An unknown error occured",
                        }.ToString());
                    }
                });
            });
        }
        public static int GetStatusCode(Type type)
        {
            if (type == typeof(BadRequestException)) return (int)HttpStatusCode.BadRequest;
            else if (type == typeof(UnauthorizedException)) return (int)HttpStatusCode.Unauthorized;
            else if (type == typeof(NotFoundException)) return (int)HttpStatusCode.NotFound;
            else if (type == typeof(ForbiddenException)) return (int)HttpStatusCode.Forbidden;
            else return (int)HttpStatusCode.InternalServerError;
        }
        /*
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
        */
    }
}
