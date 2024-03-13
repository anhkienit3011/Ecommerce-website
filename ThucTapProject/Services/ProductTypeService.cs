using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.IServices;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly AppDbContext _appContext;
        private Mapper _mapper;

        public ProductTypeService()
        {
            _appContext = new AppDbContext();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }

        public async Task<ApiResponse> Create(ProductTypeEditModel NewProductTypeEM)
        {
            bool isNameExisted = _appContext.ProductType
                .Any(c => c.NameProductType == CommonFunctions.NameFormat(NewProductTypeEM.NameProductType));
            if (isNameExisted)
            {
                return new ApiResponse { success = false, message = "Tên loại sản phẩm đã tồn tại" };
            }
            ProductType newProductType = _mapper.Map<ProductType>(NewProductTypeEM);
            newProductType.CreateAt = DateTime.Now;
            newProductType.NameProductType = CommonFunctions.NameFormat(NewProductTypeEM.NameProductType);

            if (NewProductTypeEM.ImageTypeProduct is IFormFile ImageTypeProduct)
            {
                newProductType.ImageTypeProduct = await CloudinaryService.uploadImage(ImageTypeProduct);
            }

            _appContext.ProductType.Add(newProductType);
            _appContext.SaveChanges();

            return new ApiResponse { success = true, message = "Thêm loại sản phẩm thành công", data = _mapper.Map<ProductTypeView>(newProductType) };
        }

        public ApiResponse Get(int ProductTypeID)
        {
            ProductType? productType = _appContext.ProductType.FirstOrDefault(c => c.ProductTypeId == ProductTypeID);
            if (productType == null)
            {
                return new ApiResponse { message = "Loại sản phẩm không tồn tại", success = false };
            }
            return new ApiResponse { success = true, data = _mapper.Map<ProductTypeView>(productType) };
        }

        public ApiResponse GetAll()
        {
            var data = _appContext.ProductType.Select(productType => _mapper.Map<ProductTypeView>(productType));
            return new ApiResponse { success = true, data = data };
        }

        public async Task<ApiResponse> Modify(int productTypeId, ProductTypeEditModel NewProductTypeEM)
        {
            ProductType? ProductTypeToEdit = _appContext.ProductType.FirstOrDefault(c => c.ProductTypeId == productTypeId);
            if (ProductTypeToEdit == null)
            {
                return new ApiResponse { success = false, message = "Loại sản phẩm không tồn tại" };
            }

            bool isNameExisted = _appContext.ProductType
                .Where(c => c.NameProductType != ProductTypeToEdit.NameProductType)
                .Any(c => c.NameProductType == CommonFunctions.NameFormat(NewProductTypeEM.NameProductType));

            if (isNameExisted)
            {
                return new ApiResponse { success = false, message = " Tên loại sản phẩm đã tồn tại" };
            }

            if (NewProductTypeEM.ImageTypeProduct != null)
            {
                await CloudinaryService.deleteImage(ProductTypeToEdit.ImageTypeProduct);
                ProductTypeToEdit.ImageTypeProduct = await CloudinaryService.uploadImage(NewProductTypeEM.ImageTypeProduct);
            }
            ProductTypeToEdit.NameProductType = CommonFunctions.NameFormat(NewProductTypeEM.NameProductType);
            ProductTypeToEdit.UpdateAt = DateTime.Now;
            _appContext.Update(ProductTypeToEdit);
            _appContext.SaveChanges();

            return new ApiResponse { success = true, message = "Cập nhật thành công", data = _mapper.Map<ProductTypeView>(ProductTypeToEdit) };
        }

        public async Task<ApiResponse> Remove(int productTypeId)
        {
            ProductType? ProductType = _appContext.ProductType.FirstOrDefault(c => c.ProductTypeId == productTypeId);
            if(ProductType != null)
            {
                await CloudinaryService.deleteImage(ProductType.ImageTypeProduct);
                _appContext.Remove(ProductType);
                _appContext.SaveChanges();
                return new ApiResponse { success = true, message = "Xoa thanh cong" };
            }
            return new ApiResponse { success = false, message = "Khong ton tai loại sản phẩm" };
        }
    }
}
