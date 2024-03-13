using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using ThucTapProject.DAO;

namespace ThucTapProject.Helper
{
    public class ImageIFormFile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile ImageFile)
            {
                // Check if the uploaded file is an image
                if (!IsImage(ImageFile))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            // If the value is null or it's not an IFormFile, it's considered valid.
            return ValidationResult.Success;
        }

        private bool IsImage(IFormFile file)
        {
            // Check if the file's content type indicates it's an image.
            string[] allowedContentTypes = { "image/jpeg", "image/png", "image/gif" }; // Add more as needed.
            return allowedContentTypes.Contains(file.ContentType);
        }
    }

    public class ProductTypeNameExisted : ValidationAttribute
    {
        private readonly AppDbContext _context;
        public ProductTypeNameExisted()
        {
            _context = new AppDbContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string ProductTypeName)
            {
                if (IsExisted(ProductTypeName))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }

        private bool IsExisted(string ProductType)
        {
            string name = CommonFunctions.NameFormat(ProductType);
            return _context.ProductType.Any(c => c.NameProductType.ToLower() == name);
        }
    }

    public class ProductExisted : ValidationAttribute
    {
        private readonly AppDbContext _context;
        public ProductExisted()
        {
            _context = new AppDbContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int ProductId)
            {
                if (!IsExisted(ProductId))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }

        private bool IsExisted(int ProductId)
        {
            return _context.ProductType.Any(c => c.ProductTypeId == ProductId);
        }
    }
}
