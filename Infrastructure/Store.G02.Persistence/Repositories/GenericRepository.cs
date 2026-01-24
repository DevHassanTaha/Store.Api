using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities;
using Store.G02.Domain.Entities.Products;
using Store.G02.Persistence.Data.Contexts;
namespace Store.G02.Persistence.Repositories
{
    public class GenericRepository<TKey, TEntity>(StoreDbContext _context) : IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool changeTracker = false)
        {
            if(typeof (TEntity) == typeof(Product))
            {
                return changeTracker ?
                 await _context.Products.Include(P => P.Brand).Include(P => P.Type).ToListAsync() as IEnumerable<TEntity>
                 : await _context.Products.AsNoTracking().Include(P => P.Brand).Include(P => P.Type).ToListAsync() as IEnumerable<TEntity>;
            }

            return changeTracker
                ? await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(TKey key)
        {
            if(typeof (TEntity) == typeof(Product))
            {
                return await _context.Products
                    .Include(P => P.Brand)
                    .Include(P => P.Type)
                    .FirstOrDefaultAsync(e => e.Id == key as int?) as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(key);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
    }
}
