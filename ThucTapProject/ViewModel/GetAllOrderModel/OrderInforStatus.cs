using System.Reflection.Metadata.Ecma335;

namespace ThucTapProject.ViewModel.GetAllOrderModel {
    public class OrderInforStatus {
        public int OrderId { get; set; }
        public DateTime CreateAt { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }

    }
}
