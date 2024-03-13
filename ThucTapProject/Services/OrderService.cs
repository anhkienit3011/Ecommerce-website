using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.IServices;
using ThucTapProject.Page;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.GetAllOrderModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Services {
    public class OrderService : IOrderService {
        private readonly AppDbContext _appContext;
        private Mapper _mapper;

        public OrderService() {
            _appContext = new AppDbContext();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }

        public async Task<ApiResponse> CreateOrder(int UserId, OrderEditModel NewOrder) {
            using (var trans = _appContext.Database.BeginTransaction()) {
                try {
                    // lấy id giỏ hàng
                    int CartId = _appContext.Carts.FirstOrDefaultAsync(c => c.UserId == UserId).Result.CartId;

                    // Lấy chi tiết sản phẩm trong giỏ hàng
                    IEnumerable<CartItem> cartItems = _appContext.CartItem.Where(c => c.CartId == CartId).Include(c => c.Product);

                    // kiểm tra giỏ hàng trống
                    if (cartItems.Count() <= 0) {
                        return new ApiResponse { success = false, message = "Tạo đơn hàng thất bại do giỏ hàng chống" };
                    }

                    // kiểm tra phương thức thanh toán hợp lệ
                    /*if (!_appContext.Payment.Any(c => c.PaymentId == NewOrder.PaymentId && c.Status.Equals((int)Payment_status.Valid))) {
                        return new ApiResponse { success = false, message = "Tạo đơn hàng thất bại do phương thức thanh toán không hợp lệ" };
                    }*/

                    // Them thong tin dat hang
                    int statusId = _appContext.OrderStatus.FirstOrDefault(c => c.StatusName.Equals(Order_status.Processing.ToString())).OrderStatusId;
                    Order _newOrder = _mapper.Map<Order>(NewOrder);
                    _newOrder.UserId = UserId;
                    _newOrder.CreateAt = DateTime.Now;
                    _newOrder.OrderStatusId = statusId;
                    _newOrder.OriginalPrice = cartItems.Sum(c => c.Quantity * c.Product.Price);
                    _newOrder.ActualPrice = cartItems.Sum(c => c.Quantity * c.Product.CalDiscountPrice());
                    await _appContext.Order.AddAsync(_newOrder);
                    _appContext.SaveChanges();

                    List<OrderDetailsView> orderDetailListView = new List<OrderDetailsView>();
                    // Them chi tiet don dat hang
                    foreach (var item in cartItems) {
                        OrderDetail orderDetail = _mapper.Map<OrderDetail>(item);
                        orderDetail.CreateAt = DateTime.Now;
                        orderDetail.UpdateAt = null;
                        orderDetail.OrderId = _newOrder.OrderId;
                        orderDetail.PriceTotal = item.Product.CalDiscountPrice() * item.Quantity;
                        _appContext.OrderDetail.Add(orderDetail);
                        _appContext.SaveChanges();

                        // Lấy thông tin từng chi tiết trong đơn hàng
                        OrderDetailsView orderDetailsView = _mapper.Map<OrderDetailsView>(orderDetail);
                        orderDetailListView.Add(orderDetailsView);
                    }

                    // xoa chi tiet trong gio hang
                    _appContext.CartItem.RemoveRange(cartItems);
                    _appContext.SaveChanges();
                    trans.Commit();

                    // response
                    return new ApiResponse { success = true, data = orderDetailListView };
                } catch (Exception ex) {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public async Task<ApiResponse> GetAnOrder(int UserId, int OrderId) {
            // lấy danh sách chi tiết trong đơn hàng
            var OrderDetailList = _appContext.OrderDetail.Where(c => c.OrderId == OrderId);

            // kiểm tra đơn hàng tồn tại
            Order? order = _appContext.Order.FirstOrDefault(c => c.OrderId == OrderId);
            if (order == null) {
                return new ApiResponse { success = false, message = "Đơn hàng không tồn tại" };
            }

            // Ánh xạ dữ liệu từ bảng Order sang bảng OrderInforView
            OrderInforView OrderView = _mapper.Map<OrderInforView>(order);

            // Ánh xạ dữ liệu từ bảng OrderDetails sang bảng OrderDetailsView
            var OrderDetailsListView = OrderDetailList.Include(c => c.Product).Select(c => _mapper.Map<OrderDetailsView>(c));
            OrderView.ListItems = OrderDetailsListView;

            return new ApiResponse { success = true, data = OrderView };
        }

        public async Task<ApiResponse> GetAllOrder(int UserId, Pagination pagination) {
            IEnumerable<OrderInforStatus> orders = _appContext.Order
                .Include(c => c.OrderStatus)
                .Where(c => c.UserId == UserId)
                // sap xep theo trạng thái đặt hàng theo thứ tự từ chuẩn bị đến giao hàng thành công
                .OrderBy(c => c.OrderStatusId)
                .Select(c => _mapper.Map<OrderInforStatus>(c));
            // phan trang
            var PaginateData = pageResult<OrderInforStatus>.ToPageResult(pagination, orders);
            return new ApiResponse { success = true, data = new pageResult<OrderInforStatus>(pagination, PaginateData) };
        }

        public async Task<ApiResponse> UpdateOrderStatus(int OrderId, int StatusId) {
            Order? order = _appContext.Order.FirstOrDefault(c => c.OrderId == OrderId);
            // kiểm trạng thái cập nhật chùng với trạng thái hiện tại
            if (order.OrderStatusId == StatusId) {
                return new ApiResponse { success = true };
            }
            // kiểm tra nếu đơn hàng đã hoàn thành
            await Console.Out.WriteLineAsync(StatusId.ToString());
            await Console.Out.WriteLineAsync(((int)Order_status.Delivered).ToString());
            if (order.OrderStatusId == (int)Order_status.Delivered) {
                return new ApiResponse { success = false, message = "Cập nhật không thành công do đơn hàng đã được giao" };
            } else {
            }
            if (StatusId == (int)Order_status.Processing) {
                return new ApiResponse { success = true };
            } else {
                // kiem tra trang thai hop lệ
                if (!_appContext.OrderStatus.Any(c => c.OrderStatusId == StatusId)) {
                    return new ApiResponse { success = false, message = "Trạng thái khong hợp lệ" };
                }
                // lấy dữ liệu của order
                if (order == null) return new ApiResponse { success = true };
                // cập nhật status
                order.OrderStatusId = StatusId;
                _appContext.SaveChanges();
                // gửi email thông báo trạng thái đơn hàng đến người dùng
                string StatusString = "";
                switch (StatusId) {
                    case (int)Order_status.Preparing:
                        StatusString = "đang được chuẩn bị"; break;
                    case (int)Order_status.Shipped:
                        StatusString = $"đang trên đường giao đến bạn hãy chuẩn bị số tiền {order.ActualPrice} vnđ"; break;
                    case (int)Order_status.Delivered:
                        StatusString = "đã được giao thành công, mời bạn đánh giá sản phẩm của chúng tôi"; break;
                }

                string ToEmail = order.Email;
                string Subject = "Trạng thái đơn đặt hàng FoodPro";
                string Body = "Bạn có một đơn hàng " + StatusString;
                await EmailService.SendEmail(ToEmail, Subject, Body);
                return new ApiResponse { success = true };
            }
        }

        public async Task<ApiResponse> GetAllDeliveredOrderDetail(int UserId, Pagination pagination) {
            var Orders = _appContext.OrderDetail
                .Include(c => c.Order)
                .Include(c => c.Product)
                .Where(c => c.Order.UserId == UserId && c.Order.OrderStatusId.Equals((int)Order_status.Delivered))
                .OrderByDescending(c => c.Order.CreateAt)
                .Select(c => _mapper.Map<OrderDetailsView>(c));

            var paginateData = pageResult<OrderDetailsView>.ToPageResult(pagination, Orders);
            return new ApiResponse { success = true, data = new pageResult<OrderDetailsView>(pagination, paginateData) };
        }
    }
}
