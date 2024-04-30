using Backend.DTOs;
using FluentValidation;

namespace Backend.Validators
{
    public class BrandInsertValidator : AbstractValidator<BrandInsertDto>
    {
        public BrandInsertValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre de la marca es obligatorio");
            RuleFor(x => x.Name).Length(1, 100).WithMessage("El nombre de la  marca debe de tener de 1 a 100 caracteres");
        }
    }
}
