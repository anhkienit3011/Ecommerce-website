using ThucTapProject.Entities;

namespace ThucTapProject.ViewModel {
    public class OrderDetailsView {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string NameProduct { get; set; }
        public string? AvatarImageProduct { get; set; }
        public int Quantity { get; set; }
        public double? PriceTotal { get; set; }// discount price
        public bool IsComment { get; set; }
    }
}
