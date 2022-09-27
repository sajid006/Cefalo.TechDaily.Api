using Cefalo.TechDaily.Service.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.DtoValidators
{
    public class PostStoryDtoValidator : BaseDtoValidator<PostStoryDto>
    {
        public PostStoryDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.AuthorName).NotEmpty().WithMessage("Please provide author name");
        }

    }
}
