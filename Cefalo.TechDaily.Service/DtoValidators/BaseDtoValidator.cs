using Cefalo.TechDaily.Service.CustomExceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.DtoValidators
{
    public class BaseDtoValidator<T>: AbstractValidator<T>
    {
        public void ValidateDTO(T DTO)
        {
            var result = this.Validate(DTO);
            string err = "";
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    err += $"{error.PropertyName}: {error.ErrorMessage}\n";
                }
                throw new BadRequestException(err);
            }
        }
    }
}
