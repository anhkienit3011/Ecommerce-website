using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using ThucTapProject.Helper;

namespace ThucTapProject.EditModel
{
    public class ProductTypeEditModel
    {
        [Required(ErrorMessage = "Tên Loại sản phẩm không để trống")]
        [MaxLength(60, ErrorMessage = "Tên loại sản phẩm giới hạn 60 kí tự")]
        public string NameProductType { get; set; }


        [Column(TypeName = ("VARCHAR"))]
        [AllowNull]
        [ImageIFormFile(ErrorMessage = "Chọn hình ảnh có các đuôi sau: jpeg, png, gif")]
        public IFormFile? ImageTypeProduct { get; set; }
    }
}
