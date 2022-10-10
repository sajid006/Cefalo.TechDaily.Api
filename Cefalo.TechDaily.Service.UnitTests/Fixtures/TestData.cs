using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.UnitTests.Fixtures
{

    public class TestData
    {
        public Story fakeStory;
        public List<Story> fakeStoryList;
        public PostStoryDto fakePostStoryDto;
        public TestData()
        {
            fakeStory = A.Fake<Story>(x => x.WithArgumentsForConstructor(() => new Story()));
            fakeStory.Id = 0;
            fakeStory.Title = "Title";
            fakeStory.AuthorName = "sajid1";
            fakeStory.UpdatedAt = DateTime.Now;
            fakeStory.CreatedAt = DateTime.Now;
            fakeStory.Description = "habijabi";


            fakeStoryList = new List<Story> { fakeStory };
            fakeStoryList.Add(fakeStory);

            fakePostStoryDto = A.Fake<PostStoryDto>(x => x.WithArgumentsForConstructor(() => new PostStoryDto()));
            fakePostStoryDto.Title = "Title";
            fakePostStoryDto.AuthorName = "sajid1";
            fakePostStoryDto.Description = "Description";
        }
    }
}
