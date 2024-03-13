using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using AutoMapper.Configuration;
using ThucTapProject.DAO;

namespace ThucTapProject.EditModel {
    public class OrderEditModel {
        [Required(ErrorMessage = "không để trống")]
        public int PaymentId { get; set; }
        [MaxLength(50, ErrorMessage = "Họ và tên không quá 50 kí tự")]
        [Required(ErrorMessage = "Họ và tên không để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "không để trống")]
        [EmailAddress(ErrorMessage = "Nhập email hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "không để trống")]
        [Phone(ErrorMessage = "Nhập số điện thoại hợp lệ")]
        public string Phone { get; set; }
        [MaxLength(200, ErrorMessage = "Chiều dài tối thiểu 200 kí tự")]
        [Required(ErrorMessage = "không để trống")]
        public string Address { get; set; }
    }

}
