using Cefalo.TechDaily.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Utils.Contracts
{
    public interface IJwtTokenHandler
    {
        string CreateToken(User user);
        string GetLoggedinUsername();
        string GetTokenCreationTime();
    }
}
