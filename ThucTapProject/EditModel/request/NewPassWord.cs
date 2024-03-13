using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.EditModel.request {
    public class NewPassWord {

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Chiều dài tối thiểu là 6 kí tự")]
        [MaxLength(15, ErrorMessage = "Chiều dài tối đa là 15 kí tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
        ErrorMessage = "Mật Khẩu bao gồm ít nhất một chữ cái hoa, một chữ cái thường, một số, một kí tự đặc biệt")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }
    }
}
