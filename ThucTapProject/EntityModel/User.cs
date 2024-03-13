using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using ThucTapProject.EntityModel;

namespace ThucTapProject.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int AccounttId { get; set; }
        public virtual Accountt? Accountt { get; set; }
         public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<Order>? Orders { get; set; }
        public virtual IEnumerable<ProductReview>? ProductReviews { get; set; }
        public virtual IEnumerable<Carts>? Carts { get; set; }
        public virtual IEnumerable<RefreshToken>? RefreshTokens { get; set; }
    }
}
