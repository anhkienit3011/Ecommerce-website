using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThucTapProject.ViewModel
{
    public class ProductTypeView
    {
        public int ProductTypeId { get; set; }
        public string NameProductType { get; set; }
        public string? ImageTypeProduct { get; set; }
    }
}
