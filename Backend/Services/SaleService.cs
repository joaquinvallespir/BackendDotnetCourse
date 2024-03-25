using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Validators;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Backend.Services
{
    public class SaleService : ICommonService<SaleDto, SaleInsertDto, SaleUpdateDto>
    {
        private IRepository<Sale> _saleRepository;
        private IRepository<Beer> _beerRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }
        public SaleService(IRepository<Sale> saleRepository, IRepository<Beer> beerRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _beerRepository = beerRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }
        public async Task<IEnumerable<SaleDto>> Get()
        {
            var sales = await _saleRepository.Get();
            return sales.Select(s => _mapper.Map<SaleDto>(s));
        }
        public async Task<IEnumerable<SaleDto>> GetByName(string name)
        {
            var sales = await _saleRepository.GetByName();
            return sales.Select(s => _mapper.Map<SaleDto>(s));
      

        }
    
        public async Task<SaleDto> GetById(long id)
        {
            var sale = await _saleRepository.GetById(id); //Busca por id y trae el objeto o null
            if (sale != null)
            {
                var saleDto = _mapper.Map<SaleDto>(sale);

                return saleDto;
            }
            return null;

        }

        public async Task<SaleDto> GetById(int id)
        {
           
            return null;

        }

        public async Task<SaleDto> Add(SaleInsertDto saleInsertDto)
        {
            var sale = _mapper.Map<Sale>(saleInsertDto);
            sale.TotalPrice = await GetBeerPrice(sale.Quantity, sale.BeerId);
            sale.BeerName = await GetBeerName(sale.BeerId);
            sale.BuyDatetime = DateTime.Now;
            await _saleRepository.Add(sale);
            await _saleRepository.Save(); //el savechangesasync genera el id


            var saleDto = _mapper.Map<SaleDto>(sale);
            return saleDto;
        }
        public async Task<SaleDto> Update(long id, SaleUpdateDto saleUpdateDto)
        {
            var sale = await _saleRepository.GetById(id); //Busca por id y trae el objeto o null
            if (sale != null)
            {
                sale = _mapper.Map<SaleUpdateDto, Sale>(saleUpdateDto, sale); //Al mandar los dos parámetros se edita el existente, no se crea uno nuevo
                //Se manda el origen de la información para que ignore los campos que no estan en el automapper
                //No se reasigna el BeerId
                sale.TotalPrice = await GetBeerPrice(sale.Quantity, sale.BeerId);
                sale.BeerName = await GetBeerName(sale.BeerId);
                sale.BuyDatetime = DateTime.Now;
                _saleRepository.Update(sale);
                await _saleRepository.Save();

                var saleDto = _mapper.Map<SaleDto>(sale);
                return saleDto;
            }

            return null;
        }

        public async Task<SaleDto> Update(int id, SaleUpdateDto saleUpdateDto)
        {
            return null;
        }
        public async Task<SaleDto> Delete(long id)
        {
            var sale = await _saleRepository.GetById(id); //Busca por id y trae el objeto o null
            if (sale != null)
            {
                var saleDto = _mapper.Map<SaleDto>(sale);
                _saleRepository.Delete(sale);
                await _saleRepository.Save();


                return saleDto;
            }
            return null;
        }
        public async Task<SaleDto> Delete(int id)
        {
            return null;
        }
        public Validators.ValidationResult Validate(SaleInsertDto saleInsertDto)
        {
            var beer = _beerRepository.GetById(saleInsertDto.BeerId).Result; // Obtenemos la cerveza por su Id

            if (beer == null)
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.NotFound,
                    ErrorMessage = "No se encontró una cerveza con ese ID"
                };
            }

            if (beer.StockQuantity < saleInsertDto.Quantity)
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.InvalidQuantity,
                    ErrorMessage = "La cantidad de la venta supera el stock disponible de la cerveza"
                };
            }

            return new Validators.ValidationResult
            {
                IsValid = true
            };
        }
        public Validators.ValidationResult Validate(SaleUpdateDto saleUpdateDto)
        {
            var sale = _beerRepository.GetById(saleUpdateDto.BeerId).Result; // Obtenemos la cerveza por su Id

            if (sale == null)
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.NotFound,
                    ErrorMessage = "No se encontró una cerveza con ese ID"
                };
            }

            if (sale.StockQuantity < saleUpdateDto.Quantity)
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.InvalidQuantity,
                    ErrorMessage = "La cantidad de la venta supera el stock disponible de la cerveza"
                };
            }
            return new Validators.ValidationResult 
            { 
                IsValid = true
            };
        }
        private async Task<decimal> GetBeerPrice(int quantity, int beerID)
        {
            var beer = await _beerRepository.GetById(beerID);
            
            return beer.Price * quantity;
        }

        private async Task<string> GetBeerName(int beerID)
        {
            var beer = await _beerRepository.GetById(beerID);
            return beer.Name;
        }

       
    }
}
