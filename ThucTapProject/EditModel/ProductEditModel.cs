using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Entities;
using ThucTapProject.DAO;
using ThucTapProject.Helper;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace ThucTapProject.EditModel
{
    public class ProductEditModel
    {
        [Required]
        [ProductTypeExisted(ErrorMessage = "Loại sản phẩm không tồn tại")]
        public int ProductTypeId { get; set; }
        [Required(ErrorMessage = "Tên Loại sản phẩm không để trống")]
        [MaxLength(60, ErrorMessage = "Tên sản phẩm giới hạn 200 kí tự")]
        [MinLength(6, ErrorMessage = "Tên sản phẩm tối thiểu 6 kí tự")]
        public string NameProduct { get; set; }


        [Required(ErrorMessage = "Giá sản phẩm không để trống")]
        [Range(1000,double.MaxValue, ErrorMessage = "Giá trị Sản phẩm không dưới 1.000 vnd")]
        public double Price { get; set; }


        [Required(ErrorMessage = "Ảnh đại diện sản phẩm không để trống")]
        [ImageIFormFile(ErrorMessage = "Chọn hình ảnh có các đuôi sau: jpeg, png, gif")]
        public IFormFile? AvatarImageProductFile { get; set; }


        public string? Title { get; set; }
        [Range(0,100,ErrorMessage = "Phần trăm khuyến mại dưới 100")]
        public int Discount { get; set; } = 0;

        //public int Status { get; set; }
        //public int NumberOfViews { get; set; } = 0;
    }

    public class ProductTypeExisted : ValidationAttribute
    {
        private readonly AppDbContext _context;
        public ProductTypeExisted()
        {
            _context = new AppDbContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int ID)
            {
                int idProductType = (int)value;
                if (!IsExisted(ID))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
        private bool IsExisted(int id)
        {
            return _context.ProductType.Any(c => c.ProductTypeId == id);
        }
    }
}
