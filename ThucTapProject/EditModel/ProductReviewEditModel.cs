using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThucTapProject.Entities;

namespace ThucTapProject.EditModel {
    public class ProductReviewEditModel {
        public int UserId { get; set; }
        public int OrderDetailId { get; set; }
        public string ContentRated { get; set; }
        [Range(1,5, ErrorMessage = "Điểm đánh giá từ 1 đến 5 sao")]
        public int PointEvaluation { get; set; } 
    }
}
