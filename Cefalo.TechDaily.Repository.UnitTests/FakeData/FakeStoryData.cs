using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Repository.UnitTests.FakeData
{
    public class FakeStoryData
    {
        public Story fakeStory,fakeStory2,fakeStory3,fakeStory4;
        public List<Story> fakeStoryList;
        public PostStoryDto fakePostStoryDto;
        public UpdateStoryDto fakeUpdateStoryDto;
        public FakeStoryData()
        {
            fakeStory = A.Fake<Story>(x => x.WithArgumentsForConstructor(() => new Story()));
            fakeStory.Id = 1;
            fakeStory.Title = "Title";
            fakeStory.AuthorName = "sajid1";
            fakeStory.UpdatedAt = DateTime.UtcNow;
            fakeStory.CreatedAt = DateTime.UtcNow;
            fakeStory.Description = "Description";

            fakeStory2 = A.Fake<Story>(x => x.WithArgumentsForConstructor(() => new Story()));
            fakeStory2.Id = 2;
            fakeStory2.Title = "Title";
            fakeStory2.AuthorName = "sajid2";
            fakeStory2.UpdatedAt = DateTime.UtcNow;
            fakeStory2.CreatedAt = DateTime.UtcNow;
            fakeStory2.Description = "Description";

            fakeStory3 = A.Fake<Story>(x => x.WithArgumentsForConstructor(() => new Story()));
            fakeStory3.Id = 3;
            fakeStory3.Title = "Title";
            fakeStory3.AuthorName = "sajid3";
            fakeStory3.UpdatedAt = DateTime.UtcNow;
            fakeStory3.CreatedAt = DateTime.UtcNow;
            fakeStory3.Description = "Description";

            fakeStory4 = A.Fake<Story>(x => x.WithArgumentsForConstructor(() => new Story()));
            fakeStory4.Id = 3;
            fakeStory4.Title = "Title";
            fakeStory4.AuthorName = "sajid3";
            fakeStory4.UpdatedAt = DateTime.UtcNow;
            fakeStory4.CreatedAt = DateTime.UtcNow;
            fakeStory4.Description = "Description";

            fakeStoryList = new List<Story>();
            fakeStoryList.Add(fakeStory);
            fakeStoryList.Add(fakeStory2);

            fakePostStoryDto = A.Fake<PostStoryDto>(x => x.WithArgumentsForConstructor(() => new PostStoryDto()));
            fakePostStoryDto.Title = "Title";
            fakePostStoryDto.AuthorName = "sajid1";
            fakePostStoryDto.Description = "Description";

            fakeUpdateStoryDto = A.Fake<UpdateStoryDto>(x => x.WithArgumentsForConstructor(() => new UpdateStoryDto()));
            fakeUpdateStoryDto.Title = "Title";
            fakeUpdateStoryDto.Description = "Description";
        }
        public static DateTime TrimMilliseconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0, dt.Kind);
        }
    }
}
