using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class SaleRepository : IRepository<Sale>
    {
        private StoreContext _context;

        public SaleRepository(StoreContext context) 
        {
            _context = context;        
        }
        public async Task<IEnumerable<Sale>> Get() =>
            await _context.Sales.ToListAsync();
        public async Task<IEnumerable<Sale>> GetByName() =>
           await _context.Sales.ToListAsync();

        public async Task<Sale> GetById(long id) =>
            await _context.Sales.FindAsync(id);
        public async Task<Sale> GetById(int id) =>
            await _context.Sales.FindAsync(id);



        public async Task Add(Sale sale) =>
            await _context.Sales.AddAsync(sale);

        public void Update(Sale sale)
        {
            _context.Attach(sale); //adjunta la entidad cuando ya existe
            _context.Sales.Entry(sale).State = EntityState.Modified; //ya sabe el repositorio que se modifico la entidad (?
        }


        public void Delete(Sale sale) =>
            _context.Sales.Remove(sale);

        public async Task Save() =>
        await _context.SaveChangesAsync();

        public IEnumerable<Sale> Search(Func<Sale, bool> filter) =>
            _context.Sales.Where(filter).ToList();
    }

}
