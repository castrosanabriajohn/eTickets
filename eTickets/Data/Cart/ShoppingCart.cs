using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace eTickets.Data.Cart
{
    // Util class for adding, removing and manipulating data from a shopping cart
    public class ShoppingCart
    {
        public AppDbContext _context;
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ShoppingCart(AppDbContext context) => _context = context; // Injected instance of DbContext for database communication
        // Methods
        public List<ShoppingCartItem> GetAllShoppingCartItems() => ShoppingCartItems ??= _context.ShoppingCartItems.Where(predicate: spi => spi.ShoppingCartId == ShoppingCartId)
                                                                                                                   .Include(navigationPropertyPath: spi => spi.Movie)
                                                                                                                   .ToList();
        public double GetShoppingCartTotal() => _context.ShoppingCartItems.Where(predicate: spi => spi.ShoppingCartId == ShoppingCartId)
                                                                          .Select(selector: spi => spi.Movie.Price * spi.Amount)
                                                                          .Sum();
    }
}
