using ThucTapProject.EditModel;
using ThucTapProject.Page;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices {
    public interface IOrderService {
        public Task<ApiResponse> CreateOrder(int UserId, OrderEditModel NewOrder);
        public Task<ApiResponse> UpdateOrderStatus(int OrderId, int StatusId);
        //public Task<ApiResponse> CancelOrder(int UserId, int OrderId);
        public Task<ApiResponse> GetAnOrder(int UserId, int OrderId);
        public Task<ApiResponse> GetAllOrder(int UserId, Pagination pagination);
        public Task<ApiResponse> GetAllDeliveredOrderDetail(int UserId, Pagination pagination);
    }
}
