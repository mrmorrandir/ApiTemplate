using ApiTemplate.Domain;

namespace ApiTemplate.Application.Interfaces;


public interface ISampleDbContext
{
    DbSet<Sample> Samples { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
