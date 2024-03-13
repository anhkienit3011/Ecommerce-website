using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.IServices;
using ThucTapProject.Page;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Services {
    public class ProductReviewService : IProductReviewService {
        private readonly AppDbContext _appContext;
        private Mapper _mapper;

        public ProductReviewService() {
            _appContext = new AppDbContext();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }
        public async Task<ApiResponse> CreateRated(ProductReviewEditModel productReviewED) {
            OrderDetail? orderDetail = _appContext.OrderDetail
                .FirstOrDefault(c => c.OrderDetailId == productReviewED.OrderDetailId);
            // kiểm tra order detail
            if (orderDetail == null) {
                return new ApiResponse{ success=false, message = "chi tiết đơn hàng không tồn tại"};
            }
            // kiểm tra đã đánh giá
            if (orderDetail.IsComment) {
                return new ApiResponse { success = false, message = "sản phẩm đã được đánh giá" };
            }
            Order? order = _appContext.Order.FirstOrDefault(c => c.OrderId == orderDetail.OrderId);
            // kiem tra đơn hàng đã giao
            if(order.OrderStatusId < (int)Order_status.Delivered) {
                return new ApiResponse { success = false, message = "Không thể đánh giá do đơn hàng chưa được giao" };
            }
            // kiểm tra userid
            if (order.UserId != productReviewED.UserId) {
                return new ApiResponse { success = false, message = "Cảnh báo xâm nhập trái phép" };
            }
            using (var trans = _appContext.Database.BeginTransaction()) {
                try {
                    orderDetail.IsComment = true;
                    _appContext.Update(orderDetail);
                    _appContext.SaveChanges();

                    ProductReview _productReview = _mapper.Map<ProductReview>(productReviewED);
                    _productReview.CreateAt = DateTime.Now;
                    _productReview.ProductId = orderDetail.ProductId;
                    _appContext.ProductReview.Add(_productReview);
                    _appContext.SaveChanges();
                    trans.Commit();
                    // chuyển đổi sang đối tượng trả về cho người dùng
                    _productReview = _appContext.ProductReview
                        .Include(c=> c.User.Accountt)
                        .FirstOrDefault(c => c.ProductReviewId == _productReview.ProductReviewId);

                    return new ApiResponse { success = true, data = _mapper.Map<ProductReviewView>(_productReview) };
                } catch (Exception ex) {
                    trans.Rollback();
                    throw ex;
                }
            }
            
        }

        public async Task<ApiResponse> GetAllProductReview(int ProductId, Pagination pagination) {
            var productReviewViews = _appContext.ProductReview
                .Include(c => c.User.Accountt)
                .Where(c => c.ProductId == ProductId && c.Status != (int)Simple_status.Invalid)
                .ToList()
                .OrderByDescending(c => c.CreateAt)
                .Select(c => _mapper.Map<ProductReviewView>(c))
                .AsEnumerable();
            var PaginatedData = pageResult<ProductReviewView>.ToPageResult(pagination, productReviewViews);
            return new ApiResponse { success = true, data = new pageResult<ProductReviewView>(pagination, PaginatedData)};
        }

        public Task<ApiResponse> GetNumberOfView(int ProductId) {
            throw new NotImplementedException();
        }
    }
}
