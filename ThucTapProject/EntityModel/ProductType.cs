using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace ThucTapProject.Entities
{
    public class ProductType
    {
        [Key]
        public int ProductTypeId { get; set; }
        public string NameProductType { get; set; }
        public string ImageTypeProduct { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<Product>? Products { get; set; }
    }
}
