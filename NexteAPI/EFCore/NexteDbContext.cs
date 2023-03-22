using Microsoft.EntityFrameworkCore;
using NexteAPI.EFCore;

namespace NexteServer.Efcore
{
    public sealed class NexteDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public NexteDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}