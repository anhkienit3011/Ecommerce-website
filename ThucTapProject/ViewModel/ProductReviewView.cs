using System.ComponentModel.DataAnnotations.Schema;
using ThucTapProject.Entities;
using ThucTapProject.Helper;

namespace ThucTapProject.ViewModel {
    public class ProductReviewView {
        public int ProductReviewId { get; set; }
        public int ProductId { get; set; }
        public string AccountName { get; set; }
        public string? Avatar { get; set; }
        public string? ContentRated { get; set; }
        public int PointEvaluation { get; set; }
        public string? ContentSeen { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
