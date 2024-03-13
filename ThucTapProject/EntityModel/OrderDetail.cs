using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.Entities
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public double? PriceTotal { get; set; }
        public int Quantity { get; set; } = 0;
        public bool IsComment { get; set; } = false;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
