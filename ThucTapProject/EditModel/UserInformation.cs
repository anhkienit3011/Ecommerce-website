using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using ThucTapProject.Helper;
using System.Diagnostics;

namespace ThucTapProject.EditModel
{
    public class UserInformation
    {
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(50, ErrorMessage = "Chiều dài tên phải trong khoảng 3 đến 50 kí tự")]
        [MinLength(3, ErrorMessage = "Chiều dài tên phải trong khoảng 3 đến 50 kí tự")]
        [RegularExpression("[^1-9!-/:-@[-`{-~]+$", ErrorMessage = "Tên cá nhân không gồm số va kí tự đặc biệt")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [MinLength(10, ErrorMessage = "Chiều dài số điện thoại không đủ 10 kí tự")]
        [MaxLength(11, ErrorMessage = "Chiều dài số điện thoại vượt quá 11 kí tự")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Hãy nhập một địa chỉ email hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "chiều dài tối đa của địa chỉ là 200 kí tự")]
        public string Address { get; set; }
        [Required(ErrorMessage = "AccountName is required")]
        [MaxLength(50, ErrorMessage = "Chiều dài tên phải trong khoảng 3 đến 50 kí tự")]
        [MinLength(3, ErrorMessage = "Chiều dài tên phải trong khoảng 3 đến 50 kí tự")]
        //[RegularExpression("^[a-zA-Z1-9\\s]+$", ErrorMessage = "Tên tài khoản không gồm kí tự đặc biệt")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Chiều dài tối thiểu là 6 kí tự")]
        [MaxLength(15, ErrorMessage = "Chiều dài tối đa là 15 kí tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", 
        ErrorMessage = "Mật Khẩu bao gồm ít nhất một chữ cái hoa, một chữ cái thường, một số, một kí tự đặc biệt")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password confirm is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        public string PasswordConfirm { get; set; }
        [AllowNull]
        [ImageIFormFile(ErrorMessage = "Chọn hình ảnh có các đuôi sau: jpeg, png, gif")]
        public IFormFile? ImageFile { get; set; }
    }
}
