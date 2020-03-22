using System;
using System.Collections.Generic;
using System.Linq;
using WingtipToys.Data;
using WingtipToys.Domain.Models;

namespace WingtipToys.Logic
{
    public class Cart
    {
        private ProductContext _db = new ProductContext();

        public void AddToCart(string cartId, int productId)
        {
            var cartItem = _db.CartItems.SingleOrDefault(
                c => c.CartId == cartId
                && c.ProductId == productId);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists.
                cartItem = new CartItem
                {
                    CartItemId = Guid.NewGuid().ToString(),
                    ProductId = productId,
                    CartId = cartId,
                    Product = _db.Products.SingleOrDefault(
                   p => p.ProductID == productId),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart,
                // then add one to the quantity.
                cartItem.Quantity++;
            }
            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        // DEMO: Move the GetCartId back to the Web App

        public List<CartItem> GetCartItems(string cartId)
        {
            return _db.CartItems.Where(c => c.CartId == cartId).ToList();
        }

        public decimal GetCartTotal(string cartId)
        {
            // Multiply product price by quantity of that product to get
            // the current price for each of those products in the cart.
            // Sum all product price totals to get the cart total.
            decimal? total = decimal.Zero;
            total = (decimal?)(from cartItems in _db.CartItems
                               where cartItems.CartId == cartId
                               select (int?)cartItems.Quantity *
                               cartItems.Product.UnitPrice).Sum();
            return total ?? decimal.Zero;
        }

        //public ShoppingCartActions GetCart(HttpContext context)
        //{
        //    using (var cart = new ShoppingCartActions())
        //    {
        //        cart.ShoppingCartId = cart.GetCartId();
        //        return cart;
        //    }
        //}

        public void UpdateShoppingCartDatabase(string cartId, ShoppingCartUpdates[] CartItemUpdates)
        {
            using (var db = new Data.ProductContext())
            {
                try
                {
                    int cartItemCount = CartItemUpdates.Count();
                    List<CartItem> myCart = GetCartItems(cartId);
                    foreach (var cartItem in myCart)
                    {
                        // Iterate through all rows within shopping cart list
                        for (int i = 0; i < cartItemCount; i++)
                        {
                            if (cartItem.Product.ProductID == CartItemUpdates[i].ProductId)
                            {
                                if (CartItemUpdates[i].PurchaseQuantity < 1 || CartItemUpdates[i].RemoveItem == true)
                                {
                                    RemoveItem(cartId, cartItem.ProductId);
                                }
                                else
                                {
                                    UpdateItem(cartId, cartItem.ProductId, CartItemUpdates[i].PurchaseQuantity);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception("ERROR: Unable to Update Cart Database - " + exception.Message.ToString(), exception);
                }
            }
        }

        public void RemoveItem(string removeCartID, int removeProductID)
        {
            using (var _db = new Data.ProductContext())
            {
                try
                {
                    var myItem = (from c in _db.CartItems where c.CartId == removeCartID && c.Product.ProductID == removeProductID select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        // Remove Item.
                        _db.CartItems.Remove(myItem);
                        _db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Remove Cart Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void UpdateItem(string updateCartID, int updateProductID, int quantity)
        {
            using (var _db = new Data.ProductContext())
            {
                try
                {
                    var myItem = (from c in _db.CartItems where c.CartId == updateCartID && c.Product.ProductID == updateProductID select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        myItem.Quantity = quantity;
                        _db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Update Cart Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void EmptyCart(string cartId)
        {
            var cartItems = _db.CartItems.Where(c => c.CartId == cartId);
            foreach (var cartItem in cartItems)
            {
                _db.CartItems.Remove(cartItem);
            }
            // Save changes.
            _db.SaveChanges();
        }

        public int GetCountOfItemsInCart(string cartId)
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _db.CartItems
                          where cartItems.CartId == cartId
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }


        public void MigrateCart(string cartId, string userName)
        {
            var shoppingCart = _db.CartItems.Where(c => c.CartId == cartId);
            foreach (CartItem item in shoppingCart)
            {
                item.CartId = userName;
            }
            _db.SaveChanges();
        }

        public struct ShoppingCartUpdates
        {
            public int ProductId;
            public int PurchaseQuantity;
            public bool RemoveItem;
        }
    }
}