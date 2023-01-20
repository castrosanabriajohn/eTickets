using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
    }
}
