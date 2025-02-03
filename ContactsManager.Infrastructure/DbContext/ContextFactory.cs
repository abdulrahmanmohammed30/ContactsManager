using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Entities.Context;

public class ContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var options = optionsBuilder.UseSqlServer(
            "Server=WARRIOR; Database=ContactsDb; Integrated Security=True; Trust Server Certificate=True"
        ).Options;
        return new AppDbContext(options);
    }
}
