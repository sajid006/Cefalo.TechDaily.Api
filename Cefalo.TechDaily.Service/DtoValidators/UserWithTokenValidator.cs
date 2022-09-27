using Cefalo.TechDaily.Service.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.DtoValidators
{
    public class UserWithTokenValidator : BaseDtoValidator<UserWithToken>
    {
        public UserWithTokenValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid Email address");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Creation time cannot be empty")
                .Must(BeAValidDate).WithMessage("Invalid Creation time");
            RuleFor(x => x.UpdatedAt)
                .NotEmpty().WithMessage("Update time cannot be empty")
                .Must(BeAValidDate).WithMessage("Invalid Update time");
            RuleFor(x => x.PasswordModifiedAt)
                .NotEmpty().WithMessage("Password Modification time cannot be empty")
                .Must(BeAValidDate).WithMessage("Invalid Password Modification time");
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be empty");
        }
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
