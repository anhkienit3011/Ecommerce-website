using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Entities;

namespace ThucTapProject.ViewModel
{
    public class ProductView
    {
        private double _price;
        public int ProductId { get; set; }
        public string NameProduct { get; set; }
        public string? AvatarImageProduct { get; set; }
        public string? Title { get; set; }
        public int Discount { get; set; } = 0;
        public double Price {
            get {
                return _price;
            }
            set {
                _price = value;
                DiscountPrice = CalDiscountPrice();
            }
        }
        public string Status { get; set; }
        public int NumberOfViews { get; set; }
        public double DiscountPrice { get; set; }

        public double CalDiscountPrice() {
            return Price - (Price * Discount/ (double)100);
        }
    }

}
