using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Entities;
using ThucTapProject.Helper;

namespace ThucTapProject.EditModel
{
    public class ProductImageEditModel
    {
        [Required(ErrorMessage = "Ảnh sản phẩm không để trống")]
        [ImageIFormFile(ErrorMessage = "Chọn hình ảnh có các đuôi sau: jpeg, png, gif")]
        public string ImageProduct { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        [ProductExisted(ErrorMessage = "Sản phẩm không tồn tại")]
        public int ProductId { get; set; }
        [Range(0,1, ErrorMessage = "")]
        public int Status { get; set; }
    }
}
