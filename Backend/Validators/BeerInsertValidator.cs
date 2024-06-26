﻿using Backend.DTOs;
using FluentValidation;

namespace Backend.Validators
{
    public class BeerInsertValidator : AbstractValidator<BeerInsertDto>
    {
        public BeerInsertValidator()
        {
           RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es obligatorio"); //x Hace referencia a BeerInsertDto
           RuleFor(x => x.Name).Length(2, 20).WithMessage("El nombre debe tener de 2 a 20 caracteres");
           RuleFor(x => x.BrandId).NotNull().WithMessage("La marca es obligatoria");
           RuleFor(x => x.BrandId).GreaterThan(0).WithMessage("Hubo un error con el valor enviado de {PropertyName}");
           RuleFor(x => x.Alcohol).GreaterThanOrEqualTo(0).WithMessage("El {PropertyName} no puede ser negativo");
           RuleFor(x => x.Alcohol).LessThanOrEqualTo(100).WithMessage("El {PropertyName} no puede ser mayor a 100");
        }
    }
}
