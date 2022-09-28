using Cefalo.TechDaily.Database.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Cefalo.TechDaily.Api.CustomOutputFormatter.StoryOutputFormatter
{
    public class CsvStoryOutputFormatter : TextOutputFormatter
    {
        public CsvStoryOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
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
            buffer.AppendLine($"{story.Id},{story.Title},{story.AuthorName},{story.Description},{story.CreatedAt},{story.UpdatedAt}");
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
