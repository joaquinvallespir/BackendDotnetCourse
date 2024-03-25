using Backend.Models;

namespace Backend.Repository
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> Get();
        Task<IEnumerable<TEntity>> GetByName();
        Task<TEntity> GetById(int id);
        Task<TEntity> GetById(long id);
        Task Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task Save();
        public IEnumerable<TEntity> Search(Func<TEntity, bool> filter);
       // public IEnumerable<TEntity> Search(Func<Sale/Beer, bool> filter);
    }
}
