using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Repository
{
    public class BrandRepository : IRepository<Brand>
    {
        private StoreContext _context;

        public BrandRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> Get() =>
            await _context.Brands.ToListAsync();
        public async Task<IEnumerable<Brand>> GetByName() =>
           await _context.Brands.ToListAsync();

        public async Task<Brand> GetById(int id) =>
            await _context.Brands.FindAsync(id);
        public async Task<Brand> GetById(long id) =>
            await _context.Brands.FindAsync(id);
        public async Task Add(Brand brand) =>
            await _context.Brands.AddAsync(brand);

        public void Update(Brand brand)
        {
            _context.Attach(brand); //adjunta la entidad cuando ya existe
            _context.Brands.Entry(brand).State = EntityState.Modified; //ya sabe el repositorio que se modifico la entidad (?
        }


        public void Delete(Brand brand) =>
            _context.Brands.Remove(brand);

        public async Task Save() =>
        await _context.SaveChangesAsync();

        public IEnumerable<Brand> Search(Func<Brand, bool> filter) =>
            _context.Brands.Where(filter).ToList();
    }
}
