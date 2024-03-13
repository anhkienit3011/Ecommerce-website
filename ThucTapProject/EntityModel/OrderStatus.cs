using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.Entities
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatusId { get; set; }
        [MaxLength(50), Column(TypeName = ("VARCHAR"))]
        public string StatusName { get; set; }
        public virtual IEnumerable<Order>? Orders { get; set; }
    }
}
