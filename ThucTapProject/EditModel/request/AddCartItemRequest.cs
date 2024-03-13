namespace ThucTapProject.EditModel.request {
    public class AddCartItemRequest {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
