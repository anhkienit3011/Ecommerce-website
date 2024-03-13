using ThucTapProject.EditModel;
using ThucTapProject.Page;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices
{
    public interface IProductService
    {
        public Task<ApiResponse> Create(ProductEditModel NewProductED);
        public Task<ApiResponse> Modify(int id, ProductEditModel NewProductED);
        public Task<ApiResponse> Remove(int productId);
        public ApiResponse Get(int ProductId);
        public Task<ApiResponse> GetAll(Pagination pagination);
        public Task<ApiResponse> Search(string SearchKey, Pagination pagination, double? from, double? to, string sortBy);
        public Task<ApiResponse> GetFavoritProducts(Pagination pagination);
        public Task<ApiResponse> GetRelatedProducts(int ProductId, Pagination pagination);
    }
}
