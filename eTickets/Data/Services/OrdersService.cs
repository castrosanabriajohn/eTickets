using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;
        public OrdersService(AppDbContext context) => _context = context;
        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            List<Order> orders = await _context.Orders.Include(navigationPropertyPath: o => o.OrderItems)
                                                         .ThenInclude(navigationPropertyPath: oi => oi.Movie)
                                                         .Include(navigationPropertyPath: o => o.User)
                                                         .ToListAsync();
            if (userRole != "Admin")
            {
                orders = orders.Where(predicate: o => o.UserId == userId).ToList();
            }
            return orders;
        }

        public async Task SaveOrderAsync(List<ShoppingCartItem> shoppingCartItems, string userId, string userEmailAddress)
        {
            Order order = new()
            {
                UserId = userId,
                Email = userEmailAddress
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            foreach (ShoppingCartItem shoppingCartItem in shoppingCartItems)
            {
                OrderItem orderItem = new()
                {
                    OrderId = order.Id,
                    MovieId = shoppingCartItem.Movie.Id,
                    Amount = shoppingCartItem.Amount,
                    Price = shoppingCartItem.Movie.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}
