using AutoMapper;
using ThucTapProject.DAO;
using ThucTapProject.IServices;
using ThucTapProject.ViewModel.response;
using ThucTapProject.Entities;
using ThucTapProject.EditModel;
using ThucTapProject.Helper;
using Microsoft.EntityFrameworkCore;
using ThucTapProject.ViewModel;

namespace ThucTapProject.Services {
    public class CartItemService : ICartItemService {
        private readonly AppDbContext _appContext;
        private Mapper _mapper;

        public CartItemService() {
            _appContext = new AppDbContext();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }

        public async Task<ApiResponse> Create(int UserId, CartItemEditModel NewCartItemEM) {
            // Lấy sản phẩm
            Product? product = _appContext.Product.FirstOrDefault(c => c.ProductId == NewCartItemEM.ProductId);
            if (product == null) {
                return new ApiResponse { success = false, message = "sản phẩm không tồn tại" };   
            }
            // kiểm tra trạng thái của sản phẩm 
            if (product.Status.Equals((int)Product_status.OutOfStock)) {
                return new ApiResponse { success = true, message = "Chọn hàng không thành công do sản phẩm đang hết hàng"};
            }
            // Kiểm tra sản phẩm đã tồn tại trong giỏ hàng, cộng thêm số lượng nếu sp đã tồn tại
            if (_appContext.CartItem.Any(c => c.ProductId == product.ProductId)) {
                CartItem cartitem = _appContext.CartItem.FirstOrDefault(c => c.ProductId == product.ProductId);
                cartitem.Quantity += NewCartItemEM.Quantity;
                _appContext.CartItem.Update(cartitem);
                _appContext.SaveChanges();
                return new ApiResponse { success = true, data = _mapper.Map<CartItemView>(cartitem) };
            }
            // Thêm sản phẩm vào giỏ hàng
            CartItem newCartItem = _mapper.Map<CartItem>(NewCartItemEM);
            newCartItem.CreateAt = DateTime.Now;
            newCartItem.CartId = _appContext.Carts.FirstOrDefault(c => c.UserId == UserId).CartId;
            _appContext.Add(newCartItem);
            _appContext.SaveChanges();
            // bao gồm thông tin sản phẩn vào chi tiết giỏ hàng
            newCartItem = _appContext.CartItem.Include(c => c.Product).FirstOrDefault(c => c.CartId == newCartItem.CartId);
            return new ApiResponse { success = true, data = _mapper.Map<CartItemView>(newCartItem) };
        }

        public async Task<ApiResponse> GetAll(int UserId) {
            await Console.Out.WriteLineAsync(UserId.ToString());
            Carts Cart = _appContext.Carts.FirstOrDefault(c => c.UserId == UserId);
            // kiểm tra giỏ hàng trống
            if (Cart == null) {
                return new ApiResponse { success = true, message  = "Giỏ hàng đang trống"};
            }
            var TotalCartItems = _appContext.CartItem
                .Where(c => c.CartId == Cart.CartId)
                .Include(c => c.Product)
                .Select(c => _mapper.Map<CartItemView>(c));
            return new ApiResponse { success = true, data = TotalCartItems };
        }

        public async Task<ApiResponse> Modify(int CartItemId, int Quantity) {
            CartItem? cartItem = _appContext.CartItem.FirstOrDefault(c => c.CartItemId == CartItemId);
            if (cartItem != null) {
                cartItem.Quantity = Quantity;
                _appContext.Update(cartItem);
                return new ApiResponse { success = true, message = "Cap nhat thanh cong" , data = cartItem};
            }
            return new ApiResponse { success = false, message = "cart item khong ton tai" };
        }
        
/*
        public Task<ApiResponse> Remove(int UserId, int CartItemId) {
            throw new NotImplementedException();
        }*/
    }
}
