using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Cefalo.TechDaily.Api.CustomOutputFormatter.UserOutputFormatter
{
    public class PlainTextUserOutputFormatter: TextOutputFormatter
    {
        public PlainTextUserOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<UserDto>)
            {
                foreach (var user in context.Object as IEnumerable<UserDto>)
                {
                    FormatData(buffer, user);
                }
            }
            else
            {
                var user = context.Object as UserDto;
                FormatData(buffer, user);
            }
            return response.WriteAsync(buffer.ToString());
        }

        private static void FormatData(StringBuilder buffer, UserDto user)
        {
            buffer.AppendLine($"Username: {user.Username}");
            buffer.AppendLine($"Name: {user.Name}");
            buffer.AppendLine($"Email: {user.Email}");
            buffer.AppendLine($"Created At: {user.CreatedAt}");
            buffer.AppendLine($"Updated At: {user.UpdatedAt}");
            buffer.AppendLine();
        }
        protected override bool CanWriteType(Type type)
        {
            if (typeof(UserDto).IsAssignableFrom(type)
                || typeof(IEnumerable<UserDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
    
    }
}
