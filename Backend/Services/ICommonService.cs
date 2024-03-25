using Backend.DTOs;

namespace Backend.Services
{
    public interface ICommonService<T, TI, TU> //Recibe Dto, InsertDto y UpdateDto de cualquier fuente
    {
        public List<string> Errors { get; }
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task<T> GetById(long id);
        Task<IEnumerable<T>> GetByName(string name);
       Task<T> Add(TI tI);
        Task<T> Update(int id, TU tU);
        Task<T> Update(long id, TU tU);
        Task<T> Delete(int id);
        Task<T> Delete(long id);
        Validators.ValidationResult Validate(TI dto);
        Validators.ValidationResult Validate(TU dto);
    }
}
