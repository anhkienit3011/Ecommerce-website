using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [MaxLength(200), Column(TypeName = ("VARCHAR"))]
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<Order>? Orders { get; set; }

    }
}
