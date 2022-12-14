using Cefalo.TechDaily.Database.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Cefalo.TechDaily.Api.CustomOutputFormatter.StoryOutputFormatter
{
    public class PlainTextStoryOutputFormatter : TextOutputFormatter
    {
        public PlainTextStoryOutputFormatter()
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
            buffer.AppendLine($"Id: {story.Id}");
            buffer.AppendLine($"Title: {story.Title}");
            buffer.AppendLine($"Authorname: {story.AuthorName}");
            buffer.AppendLine($"Description: {story.Description}");
            buffer.AppendLine($"Created At: {story.CreatedAt}");
            buffer.AppendLine($"Updated At: {story.UpdatedAt}");
            buffer.AppendLine();
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
