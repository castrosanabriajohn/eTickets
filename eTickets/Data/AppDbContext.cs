using Microsoft.EntityFrameworkCore;

namespace eTickets.Data
{
    public class AppDbContext : DbContext
    {
        // Receives an options parameter; pass the options parameter to the base class using base keyword 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
