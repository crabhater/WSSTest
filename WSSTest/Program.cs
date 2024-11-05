
using Microsoft.EntityFrameworkCore;
using WSSTest.DataContext;
using WSSTest.InterfacesLib;
using WSSTest.Models;

namespace WSSTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<CompanyContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("WSSTestConString")));
            builder.Services.AddScoped<IRepository<Company>, EFCompanyRepository>();
            builder.Services.AddScoped<IRepository<Departament>, EFDepartamentRepository>();
            builder.Services.AddScoped<IRepository<Group>, EFGroupRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
