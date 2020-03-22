using System.Collections.Generic;
using WingtipToys.Domain.Models;

namespace WingtipToys.Domain
{
    public interface ICartManager
    {
        bool AddOrder(Order order);
        void AddToCart(string cartId, int productId);
        void EmptyCart(string cartId);
        List<CartItem> GetCartItems(string cartId);
        decimal GetCartTotal(string cartId);
        int GetCountOfItemsInCart(string cartId);
        bool MigrateCart(string cartId, string userName);
        void RemoveItem(string removeCartID, int removeProductID);
        bool SaveOrderDetail(OrderDetail orderDetail);
        void UpdateItem(string updateCartID, int updateProductID, int quantity);
        bool UpdateOrderPaymentTransactionId(int orderId, string paymentTransactionId);
        void UpdateShoppingCartDatabase(string cartId, ShoppingCartUpdates[] CartItemUpdates);
    }
}