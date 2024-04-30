using Backend.DTOs;
using FluentValidation;

namespace Backend.Validators
{
    public class BrandUpdateValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El ID es obligatorio");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre de la marca es obligatorio");
            RuleFor(x => x.Name).Length(1, 100).WithMessage("El nombre de la marca debe de tener de 1 a 100 caracteres");
        }
    }
}
