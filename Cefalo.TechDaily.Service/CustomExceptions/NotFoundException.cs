using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.CustomExceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
    }
}
