using System.ComponentModel.DataAnnotations;
using ThucTapProject.Helper;

namespace ThucTapProject.EditModel
{
    public class LogInCredential
    {
        [Required(ErrorMessage = "Tên tài khoản không để trống")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không để trống")]
        public string Password { get; set; }
    }
}
