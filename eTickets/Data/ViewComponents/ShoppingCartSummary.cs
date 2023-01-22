using eTickets.Data.Cart;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace eTickets.Data.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart _cart;
        public ShoppingCartSummary(ShoppingCart cart) => _cart = cart;
        public IViewComponentResult Invoke()
        {
            List<ShoppingCartItem> shoppingCartItems = _cart.GetAllShoppingCartItems();
            return View(shoppingCartItems.Count);
        }
    }
}
