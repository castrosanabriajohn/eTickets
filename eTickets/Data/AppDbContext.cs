using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data
{
    public class AppDbContext : DbContext
    {
        // Receives an options parameter; pass the options parameter to the base class using base keyword 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration for Actor_Movie; Define compound key
            modelBuilder.Entity<Actor_Movie>().HasKey(am => new
            {
                am.ActorId,
                am.MovieId
            });
            // Define as the joining model
            modelBuilder.Entity<Actor_Movie>()
                .HasOne(am => am.Movie)
                .WithMany(m => m.Actors_Movies)
                .HasForeignKey(am => am.MovieId);
            modelBuilder.Entity<Actor_Movie>()
                .HasOne(am => am.Actor)
                .WithMany(a => a.Actors_Movies)
                .HasForeignKey(am => am.ActorId);
            base.OnModelCreating(modelBuilder); // Pass the model builder to the base class
        }
        // Define table names for each model
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Actor_Movie> Actors_Movies { get; set; }
        // Order related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
