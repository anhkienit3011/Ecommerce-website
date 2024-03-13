using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Helper;

namespace ThucTapProject.Entities
{
    public class ProductReview
    {
        [Key]
        public int ProductReviewId { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public string? ContentRated { get; set; }
        public int PointEvaluation { get; set; }
        public string? ContentSeen { get; set; }
        public int Status { get; set; } = (int)Simple_status.Valid;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
