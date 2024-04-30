using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class BeerService : ICommonService<BeerDto, BeerInsertDto, BeerUpdateDto>
    {
        private IRepository<Beer> _beerRepository;
        private IRepository<Brand> _brandRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }
        public BeerService(IRepository<Beer> beerRepository, IRepository<Brand> brandRepository, IMapper mapper)
        {
            _beerRepository = beerRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }
        public async Task<IEnumerable<BeerDto>> Get()
        {
            var beers = await _beerRepository.Get();
            return beers.Select(b => _mapper.Map<BeerDto>(b));
        }

        public async Task<BeerDto> GetById(int id)
        {
            var beer = await _beerRepository.GetById(id); //Busca por id y trae el objeto o null
            if (beer != null)
            {
                var beerDto = _mapper.Map<BeerDto>(beer);

                return beerDto;
            }
            return null;

        }
        public async Task<BeerDto> GetById(long id)
        {
         
            return null;

        }

        public async Task<BeerDto> Add(BeerInsertDto beerInsertDto)
        {
            var beer = _mapper.Map<Beer>(beerInsertDto);
            beer.BrandName = await GetBrandName(beer.BeerId);
            await _beerRepository.Add(beer); 
            await _beerRepository.Save(); //el savechangesasync genera el id


            var beerDto = _mapper.Map<BeerDto>(beer);
            return beerDto;
        }
        public async Task<BeerDto> Update(int id, BeerUpdateDto beerUpdateDto)
        {
            var beer = await _beerRepository.GetById(id); //Busca por id y trae el objeto o null
            if (beer!=null)
            {
                beer = _mapper.Map<BeerUpdateDto, Beer>(beerUpdateDto, beer); //Al mandar los dos parámetros se edita el existente, no se crea uno nuevo
                //Se manda el origen de la información para que ignore los campos que no estan en el automapper
                //No se reasigna el BeerId
                Console.WriteLine(beer.BrandName);
                beer.BrandName = await GetBrandName(beer.BrandId);
                Console.WriteLine(beer.BrandName);
                _beerRepository.Update(beer);
                await _beerRepository.Save();

                var beerDto = _mapper.Map<BeerDto>(beer);
                return beerDto;
            }

            return null;
        }

        public async Task<BeerDto> Update(long id, BeerUpdateDto beerUpdateDto)
        {
            return null;
        }
        public async Task<BeerDto> Delete(int id)
        {
            var beer = await _beerRepository.GetById(id); //Busca por id y trae el objeto o null
            if (beer != null)
            {
                var beerDto = _mapper.Map<BeerDto>(beer);
                _beerRepository.Delete(beer);
                await _beerRepository.Save();

               
                return beerDto;
            }
            return null;
        }

        public async Task<BeerDto> Delete(long id)
        {
            return null;
        }
        public Validators.ValidationResult Validate(BeerInsertDto beerInsertDto)
        {
            var existingBeer = _beerRepository.Search(b => b.Name == beerInsertDto.Name).FirstOrDefault();
            if (existingBeer != null) //Si existe una cerveza con ese nombre entonces estará duplicada
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = Validators.ValidationErrorType.Duplicate,
                    ErrorMessage = "No puede existir una cerveza con un nombre ya existente"
                };
            }
            var existingBrand = _brandRepository.Search(b => b.BrandId == beerInsertDto.BrandId).FirstOrDefault();
            if (existingBrand == null) 
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = Validators.ValidationErrorType.NotFound,
                    ErrorMessage = "La marca ingresada no existe"
                };
            }
            return new Validators.ValidationResult
            {
                IsValid = true
            };
        }
        public Validators.ValidationResult Validate(BeerUpdateDto beerUpdateDto)
        {
            var existingBeer = _beerRepository.Search(b => b.Name == beerUpdateDto.Name).FirstOrDefault();
            if (existingBeer != null && existingBeer.BeerId != beerUpdateDto.Id) //Se evalua que los ID sean distintos para no agarrar el mismo objeto que se esta modificando 
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = Validators.ValidationErrorType.Duplicate,
                    ErrorMessage = "No puede existir una cerveza con un nombre ya existente"
                };
            }
            var existingBrand = _brandRepository.Search(b => b.BrandId == beerUpdateDto.BrandId).FirstOrDefault();
            if (existingBrand == null)
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = Validators.ValidationErrorType.NotFound,
                    ErrorMessage = "La marca ingresada no existe"
                };
            }
            return new Validators.ValidationResult
            {
                IsValid = true
            };
            /*if (_beerRepository.Search(b => b.Name == beerUpdateDto.Name //Si existen cervezas con el mismo nombre, count va a ser mayor a 0
            && beerUpdateDto.Id != b.BeerId).Count() > 0) //Se evalua que los ID sean distintos para no agarrar el mismo objeto que se esta modificando 
            {
                Errors.Add("No puede existir una cerveza con un nombre ya existente");
                return false;
            }
            return true;*/
        }
        public async Task<IEnumerable<BeerDto>> GetByName(string name)
        {
            
            return null;

        }
        private async Task<string> GetBrandName(int brandId)
        {
            var brand = await _brandRepository.GetById(brandId);
            return brand.Name;
        }
    }
}
