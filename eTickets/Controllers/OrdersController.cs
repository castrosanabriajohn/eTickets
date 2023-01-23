using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly ShoppingCart _cart;
        private readonly IOrdersService _ordersService;
        public OrdersController(IMoviesService moviesService, ShoppingCart cart, IOrdersService ordersService) => (_moviesService, _cart, _ordersService) = (moviesService, cart, ordersService);
        public async Task<IActionResult> Index()
        {
            string userId = "";
            List<Order> orders = await _ordersService.GetOrdersByUserIdAsync(userId: userId);
            return View(model: orders);
        }
        // Index action to retrieve and display list of all shopping cart items
        public IActionResult ShoppingCart()
        {
            _cart.ShoppingCartItems = _cart.GetAllShoppingCartItems();
            ShoppingCartVM model = new() { ShoppingCart = _cart, ShoppingCartTotal = _cart.GetShoppingCartTotal() };
            return View(model: model);
        }
        public async Task<RedirectToActionResult> AddToShoppingCart(int id)  // Return type can be IActionResult
        {
            Movie movie = await _moviesService.GetByIdAsync(id);
            if (movie != null)
            {
                _cart.AddItemToCart(movie);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<RedirectToActionResult> RemoveFromShoppingCart(int id) // Return type can be IActionResult
        {
            Movie movie = await _moviesService.GetByIdAsync(id);
            if (movie != null)
            {
                _cart.RemoveItemFromCart(movie);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<IActionResult> CompleteOrder()
        {
            List<ShoppingCartItem> shoppingCartItems = _cart.GetAllShoppingCartItems();
            string userId = "";
            string userEmailAddress = "";
            await _ordersService.SaveOrderAsync(shoppingCartItems, userId, userEmailAddress); // Store order to db
            await _cart.ClearShoppingCartAsync(); // Remove shopping cart items from db
            return View("OrderCompleted");
        }
    }
}
