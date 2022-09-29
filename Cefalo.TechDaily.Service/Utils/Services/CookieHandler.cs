using Cefalo.TechDaily.Service.Utils.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Cefalo.TechDaily.Service.Utils.Services
{
    
    public class CookieHandler : ICookieHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CookieHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Get(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
            
        }

        public void Remove(string key)
        {
            //Response.Cookies.Delete(key);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMinutes(10000);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }
    }
}
