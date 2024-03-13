using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Entities;

namespace ThucTapProject.ViewModel
{
    public class ProductImageView
    {
        public int ProductImageId { get; set; }
        public string? Title { get; set; }
        public string ImageProduct { get; set; }
        public int ProductId { get; set; }
        public int Status { get; set; }
    }
}
