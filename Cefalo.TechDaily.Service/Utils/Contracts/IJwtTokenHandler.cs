using Cefalo.TechDaily.Database.Models;
using Microsoft.Extensions.Primitives;
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
        string VerifyToken(string token);
        string GetLoggedinUsername();
        string GetTokenCreationTime();
        Boolean HttpContextExists();
        void DeleteToken();
    }
}
