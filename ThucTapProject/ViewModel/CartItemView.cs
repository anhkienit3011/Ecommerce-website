namespace ThucTapProject.ViewModel {
    public class CartItemView {
        public int ProductId { get; set; }
        public string NameProduct { get; set; }
        public string AvatarImageProduct { get; set; }
        public int Quantity { get; set; }
        public double TotalDiscountPrice { get; set; }
        public double price { get; set; }
        public double discount { get; set; }
    }
}
