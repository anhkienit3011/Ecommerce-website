using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.Entities
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int CartId { get; set; }
        public virtual Carts? Cart { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public double CalDiscountPrice() {
            return Product.Price - (Product.Price * Product.Discount / 100);
            //c.Product.Price - (c.Product.Price * c.Product.Discount / 100)
        }
    }
}
