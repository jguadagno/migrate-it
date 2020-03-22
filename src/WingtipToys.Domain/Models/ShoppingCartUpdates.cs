namespace WingtipToys.Domain.Models
{
    public class ShoppingCartUpdates
    {
        public int ProductId { get; set; }
        public int PurchaseQuantity { get; set; }
        public bool RemoveItem { get; set; }
    }
}
