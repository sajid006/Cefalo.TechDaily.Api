using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Cefalo.TechDaily.Database.Models;

namespace Cefalo.TechDaily.Api.CustomOutputFormatter
{
    public class HtmlOutputFormatter : TextOutputFormatter
    {
        public HtmlOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/html"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<Story>)
            {
                foreach (var story in context.Object as IEnumerable<Story>)
                {
                    FormatData(buffer, story);
                }
            }
            else
            {
                var story = context.Object as Story;
                FormatData(buffer, story);
            }
            return response.WriteAsync(buffer.ToString());
        }

        private static void FormatData(StringBuilder buffer, Story story)
        {
            buffer.AppendLine($"<p><h4>Id: {story.Id}</h4></p>");
            buffer.AppendLine($"<p><h4>Title: {story.Title}</h4></p>");
            buffer.AppendLine($"<p><h2>Authorname: {story.AuthorName}</h2></p>");
            buffer.AppendLine($"<p>Description: {story.Description}</p>");
            buffer.AppendLine($"<p><small>Created At: {story.CreatedAt}</small></p>");
            buffer.AppendLine($"<p><small>Updated At: {story.UpdatedAt}</small></p>");
        }
        protected override bool CanWriteType(Type type)
        {
            if (typeof(Story).IsAssignableFrom(type)
                || typeof(IEnumerable<Story>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
    }
}
