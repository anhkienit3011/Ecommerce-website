using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Services;

namespace ThucTapProject.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int PaymentId { get; set; }
        public virtual Payment? Payment { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public double? OriginalPrice { get; set; }//
        public double? ActualPrice { get; set; }//
        [MaxLength(50), Column(TypeName = ("VARCHAR"))]
        public string FullName { get; set; }
        [MaxLength(100), Column(TypeName = ("VARCHAR"))]
        public string Email { get; set; }
        [MaxLength(11), Column(TypeName = ("VARCHAR"))]
        public string Phone { get; set; }
        [MaxLength(200), Column(TypeName = "VARCHAR")]
        public string Address { get; set; }
        public int OrderStatusId { get; set; }
        public virtual OrderStatus? OrderStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<OrderDetail>? OrderDetails { get; set; }
    }
}
