using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.ViewModel {
    public class OrderInforView {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public IEnumerable<OrderDetailsView> ListItems { get; set; }
    }
}
