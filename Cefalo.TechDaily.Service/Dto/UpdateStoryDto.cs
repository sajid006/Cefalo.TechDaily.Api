using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Dto
{
    public class UpdateStoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Description { get; set; }

    }
}
