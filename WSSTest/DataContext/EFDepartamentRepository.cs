using Microsoft.EntityFrameworkCore;
using WSSTest.InterfacesLib;
using WSSTest.Models;

namespace WSSTest.DataContext
{
    public class EFDepartamentRepository : IRepository<Departament>
    {
        private CompanyContext _dbContext;
        public EFDepartamentRepository(CompanyContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(Departament item)
        {
            await _dbContext.Departaments.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(List<Departament> items)
        {
            await _dbContext.Departaments.AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Departament item)
        {
            _dbContext.Departaments.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<Departament>> GetList()
        {
            return await _dbContext.Departaments.ToListAsync();
        }

        public async Task UpdateAsync(Departament item)
        {
            _dbContext.Departaments.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<Departament> GetAsync(int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
