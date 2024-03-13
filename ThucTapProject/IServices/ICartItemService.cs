using ThucTapProject.EditModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices {
    public interface ICartItemService {
        public Task<ApiResponse> Create(int UserId, CartItemEditModel NewCartItemEM);
        public Task<ApiResponse> Modify(int CartItemId, int Quantity);
        //public Task<ApiResponse> Remove(int UserId, int CartItemId);
        public Task<ApiResponse> GetAll(int UserId);
    }
}
