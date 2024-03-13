using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.EditModel {
    public class CartItemEditModel {
        [Required(ErrorMessage = "Khong de trong")]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng sản phẩm tối thiểu để thêm vào giỏ hàng là 1")]
        public int Quantity { get; set; }
    }
}
