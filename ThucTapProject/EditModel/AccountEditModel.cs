using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ThucTapProject.Helper;

namespace ThucTapProject.EditModel
{
    public class AccountEditModel
    {
        [Required(ErrorMessage = "AccountName is required")]
        [MaxLength(50, ErrorMessage = "Chiều dài tên phải trong khoảng 3 đến 50 kí tự")]
        [MinLength(3, ErrorMessage = "Chiều dài tên phải trong khoảng 3 đến 50 kí tự")]
        public string AccountName { get; set; }

        [AllowNull]
        [ImageIFormFile(ErrorMessage = "Chọn hình ảnh có các đuôi sau: jpeg, png, gif")]
        public IFormFile? ImageFile { get; set; }
    }
}
