using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace ThucTapProject.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ProductType? ProductType { get; set; }
        public string NameProduct { get; set; }
        public double Price { get; set; }
        public string? AvatarImageProduct { get; set; }
        public string? Title { get; set; }
        public int Discount { get; set; } = 0;
        public int Status { get; set; } // default?
        public int NumberOfViews { get; set; } = 0;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<ProductImage>? ProductImages { get; set; }
        public virtual IEnumerable<OrderDetail>? OrderDetails { get; set; }
        public virtual IEnumerable<ProductReview>? ProductReviews { get; set; }
        public virtual IEnumerable<CartItem>? CartItems { get; set; }

        [NotMapped]
        public double DiscountPrice { get; set; }

        public double CalDiscountPrice() {
            return Price - (Price * Discount / 100);
            //c.Product.Price - (c.Product.Price * c.Product.Discount / 100)
        }
    }
}
