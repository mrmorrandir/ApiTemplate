using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApiTemplate.Infrastructure.Contexts;


// The CoffeeLifeDbContextDesignTimeFactory class is used to create an instance of the CoffeeLifeDbContext class at design time.
// This class is used by the EF Core tools to create migrations.
// The class uses a MySql database.
public class SampleDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SampleDbContext>
{
    private const string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Samples;Integrated Security=True;TrustServerCertificate=true;";

    public SampleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>().UseSqlServer(_connectionString);
        return new SampleDbContext(optionsBuilder.Options);
    }
}