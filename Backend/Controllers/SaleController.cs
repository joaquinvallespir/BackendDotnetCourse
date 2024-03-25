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
    public class SaleController : ControllerBase
    {
        private ICommonService<SaleDto, SaleInsertDto, SaleUpdateDto> _saleService;
        private IValidator<SaleInsertDto> _saleInsertValidator;
        private IValidator<SaleUpdateDto> _saleUpdateValidator;
        public SaleController([FromKeyedServices("saleService")]ICommonService<SaleDto, SaleInsertDto, SaleUpdateDto> saleService,
            IValidator<SaleInsertDto> saleInsertValidator,
            IValidator<SaleUpdateDto> saleUpdateValidator)
        {
            _saleService = saleService;
            _saleInsertValidator = saleInsertValidator;
            _saleUpdateValidator = saleUpdateValidator;
        }

        [HttpGet]
        public async Task<IEnumerable<SaleDto>> Get() =>
           await _saleService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetById(long id)
        {
            var saleDto = await _saleService.GetById(id);

            return saleDto == null ? NotFound() : Ok(saleDto);
        }
        [HttpPost]
        public async Task<ActionResult<SaleDto>> Add(SaleInsertDto saleInsertDto)
        {
            var validationResult = await _saleInsertValidator.ValidateAsync(saleInsertDto);

            if (!validationResult.IsValid)
                {
                  return BadRequest(validationResult.Errors);
                }
              
            var validation = _saleService.Validate(saleInsertDto);
              
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
            
            var saleDto = await _saleService.Add(saleInsertDto);
            
            return saleDto == null ? NotFound() : Ok(CreatedAtAction(nameof(GetById), new { id = saleDto.Id }, saleDto)); //Recibe la URL del GetById Para mostrarlo por si el frontend lo necesita, Busca el Id que creamos, Devuelve La nueva cerveza creada
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<SaleDto>> Update(long id, SaleUpdateDto saleUpdateDto)
        {
            var validationResult = await _saleUpdateValidator.ValidateAsync(saleUpdateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var validation = _saleService.Validate(saleUpdateDto);

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
            var saleDto = await _saleService.Update(id, saleUpdateDto);

            return saleDto == null ? NotFound() : Ok(saleDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<SaleDto>> Delete(long id)
        {
            var saleDto = await _saleService.Delete(id);
            return saleDto == null ? NotFound() : Ok(saleDto); //Puede ir un return NoContent(); - Devuelvo el BeerDto por si el FrontEnd lo necesita para mostrarlo o algo
        }
    }
}
