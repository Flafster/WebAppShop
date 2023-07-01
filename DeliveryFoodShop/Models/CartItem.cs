namespace WebAppShop.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public int Quantity { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int ProductId { get; set; }
    }
}
