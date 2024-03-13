using System.ComponentModel.DataAnnotations;
using ThucTapProject.DAO;
using ThucTapProject.Helper;

namespace ThucTapProject.EditModel.request {
    public class SearhProductRequest {
        public string SearchKey { get; set; } = "";
        public double? From { get; set; }
        public double? To { get; set; }
        public string SortBy { get; set; } = "price_asc";
    }

    public class LessThan : ValidationAttribute {
        private readonly AppDbContext _context;
        public LessThan() {
            _context = new AppDbContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value is int ID) {
                int idProductType = (int)value;
                if (!IsExisted(ID)) {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
        private bool IsExisted(int id) {
            return _context.ProductType.Any(c => c.ProductTypeId == id);
        }
    }
}
