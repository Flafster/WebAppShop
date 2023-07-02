namespace WebAppShop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public int ProductId { get; set; }
    }
}
