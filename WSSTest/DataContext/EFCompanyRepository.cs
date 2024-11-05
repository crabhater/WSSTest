using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using WSSTest.InterfacesLib;
using WSSTest.Models;

namespace WSSTest.DataContext
{
    public class EFCompanyRepository : IRepository<Company>
    {
        private CompanyContext _dbContext;
        public EFCompanyRepository(CompanyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Company item)
        {
            await _dbContext.Companies.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(List<Company> items)
        {
            await _dbContext.AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company item)
        {
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<Company> GetAsync(int id)
        {
            return await _dbContext.Companies.Where(e => e.Id == id)
                                             .Include(c=> c.Child)
                                             .ThenInclude(d => d.Child)
                                             .FirstOrDefaultAsync();
        }

        public async Task<List<Company>> GetList()
        {
            return await _dbContext.Companies.Include(c => c.Child)
                                             .ThenInclude(d => d.Child)
                                             .ToListAsync();
        }

        public async Task UpdateAsync(Company item)
        {
            _dbContext.Companies.Entry(item).State = EntityState.Modified;
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
    }
}
