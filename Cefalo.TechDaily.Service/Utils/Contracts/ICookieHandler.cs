using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Utils.Contracts
{
    public interface ICookieHandler
    {
        string Get(string key);
        void Set(string key, string Value, int? expireTime);
        void Remove(string key);
    }
}
