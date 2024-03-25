using Backend.DTOs;
using FluentValidation;

namespace Backend.Validators
{
    public class SaleUpdateValidator : AbstractValidator<SaleUpdateDto>
    {
        public SaleUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El ID es obligatorio");
            RuleFor(x => x.ClientName).NotEmpty().WithMessage("El nombre del cliente es obligatorio");
            RuleFor(x => x.ClientName).Length(3, 100).WithMessage("El nombre del cliente debe de tener de 3 a 100 caracteres");
            RuleFor(x => x.Quantity).GreaterThan(1).WithMessage("La cantidad no puede ser menor a 1");
        }
    }
}
