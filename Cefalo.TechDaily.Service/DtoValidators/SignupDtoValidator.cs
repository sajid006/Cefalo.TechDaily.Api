using Cefalo.TechDaily.Service.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.DtoValidators
{
    public class SignupDtoValidator : BaseDtoValidator<SignupDto>
    {
        public SignupDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(8).WithMessage("Password length must be at least 8");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid Email address");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
        }
    }
}
