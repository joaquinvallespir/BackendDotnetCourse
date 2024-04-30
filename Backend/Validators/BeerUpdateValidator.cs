using Backend.DTOs;
using FluentValidation;

namespace Backend.Validators
{
    public class BeerUpdateValidator : AbstractValidator<BeerUpdateDto>
    {
        public BeerUpdateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es obligatorio"); //x Hace referencia a BeerInsertDto
            RuleFor(x => x.Name).Length(2, 20).WithMessage("El nombre debe tener de 2 a 20 caracteres");
            RuleFor(x => x.BrandId).NotNull().WithMessage("La marca es obligatoria");
            RuleFor(x => x.BrandId).GreaterThan(0).WithMessage("Hubo un error con el valor enviado de {PropertyName}");
        }
    }
}
