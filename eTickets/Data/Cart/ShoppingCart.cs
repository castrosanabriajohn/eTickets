using eTickets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Cart
{
    // Util class for adding, removing and manipulating data from a shopping cart
    public class ShoppingCart
    {
        private readonly AppDbContext _context;
        private const string Key = "ShoppingCartId";
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ShoppingCart(AppDbContext context) => _context = context; // Injected instance of DbContext for database communication
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            // Use services to check if there is a session with the same ShoppingCartId
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session; // Get session with Service Provider
            AppDbContext context = services.GetService<AppDbContext>();
            string shoppingCartId = session.GetString(key: Key) ?? Guid.NewGuid() // Otherwise generate a new id and set it to the new session
                                                                       .ToString();
            session.SetString(key: Key, value: shoppingCartId);
            return new ShoppingCart(context: context) { ShoppingCartId = shoppingCartId };
        }
        // Methods
        public void AddItemToCart(Movie movie)
        {
            switch (_context.ShoppingCartItems.FirstOrDefault(predicate: spi => spi.Movie.Id == movie.Id && spi.ShoppingCartId == ShoppingCartId))
            {
                case null:
                    _context.ShoppingCartItems.Add(entity: new()
                    {
                        ShoppingCartId = ShoppingCartId,
                        Movie = movie,
                        Amount = 1
                    });
                    break;
                default:
                    _context.ShoppingCartItems.FirstOrDefault(predicate: spi => spi.Movie.Id == movie.Id && spi.ShoppingCartId == ShoppingCartId).Amount++;
                    break;
            }
            _context.SaveChanges();
        }
        public void RemoveItemFromCart(Movie movie)
        {
            switch (_context.ShoppingCartItems.FirstOrDefault(predicate: spi => spi.Movie.Id == movie.Id && spi.ShoppingCartId == ShoppingCartId).Amount <= 1)
            {
                case true:
                    _context.ShoppingCartItems.Remove(entity: _context.ShoppingCartItems.FirstOrDefault(predicate: spi => spi.Movie.Id == movie.Id && spi.ShoppingCartId == ShoppingCartId));
                    break;
                default:
                    _context.ShoppingCartItems.FirstOrDefault(predicate: spi => spi.Movie.Id == movie.Id && spi.ShoppingCartId == ShoppingCartId).Amount--;
                    break;
            }
            _context.SaveChanges();
        }
        public List<ShoppingCartItem> GetAllShoppingCartItems() => ShoppingCartItems ??= _context.ShoppingCartItems.Where(predicate: spi => spi.ShoppingCartId == ShoppingCartId)
                                                                                                                   .Include(navigationPropertyPath: spi => spi.Movie)
                                                                                                                   .ToList();
        public double GetShoppingCartTotal() => _context.ShoppingCartItems.Where(predicate: spi => spi.ShoppingCartId == ShoppingCartId)
                                                                          .Select(selector: spi => spi.Movie.Price * spi.Amount)
                                                                          .Sum();
        public async Task ClearShoppingCartAsync()
        {
            List<ShoppingCartItem> shoppingCartItems = await _context.ShoppingCartItems.Where(predicate: spi => spi.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(shoppingCartItems);
            await _context.SaveChangesAsync();
            shoppingCartItems = new List<ShoppingCartItem>();
        }
    }
}
