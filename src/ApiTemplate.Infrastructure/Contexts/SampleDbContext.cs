using ApiTemplate.Application.Interfaces;
using ApiTemplate.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplate.Infrastructure.Contexts;

public class SampleDbContext : DbContext, ISampleDbContext
{
    public DbSet<Sample> Samples { get; set; }

    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sample>(entity =>
        {
            entity.ToTable("Samples");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Name);
        });
    }

    
}

