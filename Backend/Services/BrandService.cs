using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class BrandService : ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto>
    {
        private IRepository<Brand> _brandRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }
        public BrandService(IRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }
        public async Task<IEnumerable<BrandDto>> Get()
        {
            var brands = await _brandRepository.Get();
            return brands.Select(b => _mapper.Map<BrandDto>(b));
        }

        public async Task<BrandDto> GetById(int id)
        {
            var brand = await _brandRepository.GetById(id); //Busca por id y trae el objeto o null
            if (brand != null)
            {
                var brandDto = _mapper.Map<BrandDto>(brand);

                return brandDto;
            }
            return null;

        }
        public async Task<BrandDto> GetById(long id)
        {
         
            return null;

        }

        public async Task<BrandDto> Add(BrandInsertDto brandInsertDto)
        {
            var brand = _mapper.Map<Brand>(brandInsertDto);
            await _brandRepository.Add(brand); 
            await _brandRepository.Save(); //el savechangesasync genera el id


            var brandDto = _mapper.Map<BrandDto>(brand);
            return brandDto;
        }
        public async Task<BrandDto> Update(int id, BrandUpdateDto brandUpdateDto)
        {
            var brand = await _brandRepository.GetById(id); //Busca por id y trae el objeto o null
            if (brand!=null)
            {
                brand = _mapper.Map<BrandUpdateDto, Brand>(brandUpdateDto, brand); //Al mandar los dos parámetros se edita el existente, no se crea uno nuevo
                //Se manda el origen de la información para que ignore los campos que no estan en el automapper
                //No se reasigna el BeerId
                _brandRepository.Update(brand);
                await _brandRepository.Save();

                var brandDto = _mapper.Map<BrandDto>(brand);
                return brandDto;
            }

            return null;
        }

        public async Task<BrandDto> Update(long id, BrandUpdateDto brandUpdateDto)
        {
            return null;
        }
        public async Task<BrandDto> Delete(int id)
        {
            var brand = await _brandRepository.GetById(id); //Busca por id y trae el objeto o null
            if (brand != null)
            {
                var brandDto = _mapper.Map<BrandDto>(brand);
                _brandRepository.Delete(brand);
                await _brandRepository.Save();

               
                return brandDto;
            }
            return null;
        }

        public async Task<BrandDto> Delete(long id)
        {
            return null;
        }
        public Validators.ValidationResult Validate(BrandInsertDto brandInsertDto)
        {
            var existingBrand = _brandRepository.Search(b => b.Name == brandInsertDto.Name).FirstOrDefault();
            if (existingBrand != null) //Si existe una cerveza con ese nombre entonces estará duplicada
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = Validators.ValidationErrorType.Duplicate,
                    ErrorMessage = "No puede existir una marca con un nombre ya existente"
                };
            }
            return new Validators.ValidationResult
            {
                IsValid = true
            };
        }
        public Validators.ValidationResult Validate(BrandUpdateDto brandUpdateDto)
        {
            var existingBrand = _brandRepository.Search(b => b.Name == brandUpdateDto.Name).FirstOrDefault();
            if (existingBrand != null && existingBrand.BrandId != brandUpdateDto.Id) //Se evalua que los ID sean distintos para no agarrar el mismo objeto que se esta modificando 
            {
                return new Validators.ValidationResult
                {
                    IsValid = false,
                    ErrorType = Validators.ValidationErrorType.Duplicate,
                    ErrorMessage = "No puede existir una marca con un nombre ya existente"
                };
            }
            return new Validators.ValidationResult
            {
                IsValid = true
            };   
        }
        public async Task<IEnumerable<BrandDto>> GetByName(string name)
        {
            
            return null;

        }
        private async Task<string> GetBrandName(int brandId)
        {
            return null;
        }
    }
}
