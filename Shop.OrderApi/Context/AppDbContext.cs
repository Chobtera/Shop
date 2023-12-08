using Microsoft.EntityFrameworkCore;

namespace Shop.OrderApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    }
}
