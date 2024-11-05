using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WSSTest.InterfacesLib;
using WSSTest.Models;

namespace WSSTest.DataContext
{
    public class EFGroupRepository : IRepository<Models.Group>
    {
        private CompanyContext _dbContext;
        public EFGroupRepository(CompanyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Models.Group item)
        {
            await _dbContext.Groups.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(List<Models.Group> items)
        {
            await _dbContext.Groups.AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Models.Group item)
        {
            _dbContext.Groups.Remove(item);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<Models.Group> GetAsync(int id)
        {
           return await _dbContext.Groups.Where(group => group.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Models.Group>> GetList()
        {
            return await _dbContext.Groups.ToListAsync();
        }

        public async Task UpdateAsync(Models.Group item)
        {
            _dbContext.Groups.Entry(item).State = EntityState.Modified;
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
