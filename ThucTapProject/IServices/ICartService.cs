using ThucTapProject.EditModel;
using ThucTapProject.Page;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices {
    public interface ICartService {
        public Task<ApiResponse> Create(int UserId);
        public ApiResponse Remove(int UserId);
        public ApiResponse Get(int UserId);
        public ApiResponse GetAll();
    }
}
