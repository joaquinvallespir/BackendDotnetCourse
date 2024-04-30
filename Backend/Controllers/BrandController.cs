using Backend.DTOs;
using Backend.Services;
using Backend.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto> _brandService;
        private IValidator<BrandInsertDto> _brandInsertValidator;
        private IValidator<BrandUpdateDto> _brandUpdateValidator;
        public BrandController([FromKeyedServices("brandService")] ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto> brandService,
            IValidator<BrandInsertDto> brandInsertValidator,
            IValidator<BrandUpdateDto> brandUpdateValidator)
        {
            _brandService = brandService;
            _brandInsertValidator = brandInsertValidator;
            _brandUpdateValidator = brandUpdateValidator;
        }

        [HttpGet]
        public async Task<IEnumerable<BrandDto>> Get() =>
           await _brandService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetById(int id)
        {
            var brandDto = await _brandService.GetById(id);

            return brandDto == null ? NotFound() : Ok(brandDto);
        }
        [HttpPost]
        public async Task<ActionResult<BrandDto>> Add(BrandInsertDto brandInsertDto)
        {
            var validationResult = await _brandInsertValidator.ValidateAsync(brandInsertDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var validation = _brandService.Validate(brandInsertDto);

            if (!validation.IsValid)
            {
                if (validation.ErrorType == ValidationErrorType.NotFound)
                {
                    return NotFound(validation.ErrorMessage);
                }
                else if (validation.ErrorType == ValidationErrorType.InvalidQuantity)
                {
                    return BadRequest(validation.ErrorMessage);
                }
            }

            var brandDto = await _brandService.Add(brandInsertDto);

            return brandDto == null ? NotFound() : Ok(CreatedAtAction(nameof(GetById), new { id = brandDto.Id }, brandDto)); //Recibe la URL del GetById Para mostrarlo por si el frontend lo necesita, Busca el Id que creamos, Devuelve La nueva cerveza creada
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BrandDto>> Update(int id, BrandUpdateDto brandUpdateDto)
        {
            var validationResult = await _brandUpdateValidator.ValidateAsync(brandUpdateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var validation = _brandService.Validate(brandUpdateDto);

            if (!validation.IsValid)
            {
                if (validation.ErrorType == ValidationErrorType.NotFound)
                {
                    return NotFound(validation.ErrorMessage);
                }
                else if (validation.ErrorType == ValidationErrorType.InvalidQuantity)
                {
                    return BadRequest(validation.ErrorMessage);
                }
            }
            var brandDto = await _brandService.Update(id, brandUpdateDto);

            return brandDto == null ? NotFound() : Ok(brandDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<BrandDto>> Delete(int id)
        {
            var brandDto = await _brandService.Delete(id);
            return brandDto == null ? NotFound() : Ok(brandDto); //Puede ir un return NoContent(); - Devuelvo el BeerDto por si el FrontEnd lo necesita para mostrarlo o algo
        }
    }
}
