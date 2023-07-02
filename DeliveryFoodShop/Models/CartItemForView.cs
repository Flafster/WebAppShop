using System;

namespace WebAppShop.Models
{
    public class CartItemForView
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime DataCreatedCartItem { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductCategory { get; set; }

    }
}
