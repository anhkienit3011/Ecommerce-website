using ThucTapProject.EditModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices
{
    public interface IProductTypeService
    {
        public Task<ApiResponse> Create(ProductTypeEditModel NewProductTypeEM); 
        public Task<ApiResponse> Modify(int id, ProductTypeEditModel NewProductTypeEM);
        public Task<ApiResponse> Remove(int productTypeId);
        public ApiResponse Get(int ProductTypeID);
        public ApiResponse GetAll();
    }
}
