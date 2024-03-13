using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.EditModel
{
    public class UserEditModel
    {
        [MaxLength(50, ErrorMessage = "Chiều dài tên phải trong khoảng 3 đến 50 kí tự")]
        [MinLength(3, ErrorMessage = "Chiều dài tên phải trong khoảng 3 đến 50 kí tự")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Tên cá nhân không gồm số và kí tự đặc biệt")]
        public string UserName { get; set; }
        [MinLength(10, ErrorMessage = "Chiều dài số điện thoại không đủ 10 kí tự")]
        [MaxLength(11, ErrorMessage = "Chiều dài số điện thoại vượt quá 11 kí tự")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage = "Hãy nhập một địa chỉ email hợp lệ")]
        public string Email { get; set; }
        [MaxLength(200, ErrorMessage = "chiều dài tối đa của địa chỉ là 200 kí tự")]
        public string Address { get; set; }
    }
}
