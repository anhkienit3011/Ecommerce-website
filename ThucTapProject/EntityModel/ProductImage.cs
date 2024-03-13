using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThucTapProject.Entities
{
    public class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }
        public string? Title { get; set; }
        public string? ImageProduct { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
