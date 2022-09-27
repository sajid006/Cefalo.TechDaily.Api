using Cefalo.TechDaily.Service.Dto;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.DtoValidators
{
    public class UpdateUserDtoValidator: BaseDtoValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid Email address");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Password length must be at least 8")
                .When(x => !x.Password.IsNullOrEmpty());
        }
    }
}
