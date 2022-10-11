using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.Fixtures;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.UnitTests.Fixtures
{
    public class FakeUserData
    {
        public User fakeUser;
        public FakeUserData()
        {
            fakeUser = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser.Username = "sajid1";
            fakeUser.Name = "Sajid Hasan";
            fakeUser.Email = "sajid1@gmail.com";
            fakeUser.UpdatedAt = DateTime.Now;
            fakeUser.CreatedAt = DateTime.Now;
            fakeUser.PasswordModifiedAt = DateTime.Now;
        }
    }
}
