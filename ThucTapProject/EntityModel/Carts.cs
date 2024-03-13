using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.Entities
{
    public class Carts
    {
        [Key]
        public int CartId { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<CartItem>? CartItems { get; set; }
    }
}
