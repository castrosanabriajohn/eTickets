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
        private readonly IMoviesService _service;
        private readonly ShoppingCart _cart;
        public OrdersController(IMoviesService service, ShoppingCart cart)
        {
            _service = service;
            _cart = cart;
        }
        // Index action to retrieve and display list of all shopping cart items
        public IActionResult ShoppingCart()
        {
            _cart.ShoppingCartItems = _cart.GetAllShoppingCartItems();
            ShoppingCartVM model = new() { ShoppingCart = _cart, ShoppingCartTotal = _cart.GetShoppingCartTotal() };
            return View(model: model);
        }
        public async Task<RedirectToActionResult> AddToShoppingCart(int id)
        {
            Movie movie = await _service.GetByIdAsync(id);
            if (movie != null)
            {
                _cart.AddItemToCart(movie);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<RedirectToActionResult> RemoveFromShoppingCart(int id)
        {
            Movie movie = await _service.GetByIdAsync(id);
            if (movie != null)
            {
                _cart.RemoveItemFromCart(movie);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
    }
}
