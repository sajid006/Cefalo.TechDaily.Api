using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Contracts
{
    public interface IUriService
    {
        public Uri GetPageUri(int pageNumber,int pageSize, string route);
    }
}
