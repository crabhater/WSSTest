using Microsoft.EntityFrameworkCore;
using WSSTest.Models;

namespace WSSTest.DataContext
{
    public class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions<CompanyContext> opt) : base(opt) 
        {
            Database.EnsureCreated();
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Departament> Departaments { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
