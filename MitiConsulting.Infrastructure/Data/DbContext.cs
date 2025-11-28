using Microsoft.EntityFrameworkCore;
using MitiConsulting.Domain.Models;
namespace MitiConsulting.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options){}
        public DbSet<Rapport> Rapports { get ; set ;}

        // protected override void onModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.onModelCreating(modelBuilder);
        // }
    }
    
}