using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ThucTapProject.EditModel;
using ThucTapProject.Page;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices {
    public interface IProductReviewService {
        public Task<ApiResponse> GetAllProductReview(int ProductId, Pagination pagination);
        public Task<ApiResponse> CreateRated(ProductReviewEditModel productReviewED);
        public Task<ApiResponse> GetNumberOfView(int ProductId);

    }
}
