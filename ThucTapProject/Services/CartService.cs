using AutoMapper;
using ThucTapProject.DAO;
using ThucTapProject.IServices;
using ThucTapProject.ViewModel.response;
using ThucTapProject.Entities;
using CloudinaryDotNet;

namespace ThucTapProject.Services {
    public class CartService : ICartService {
        private readonly AppDbContext _appContext;
        private Mapper _mapper;

        public CartService() {
            _appContext = new AppDbContext();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }
        public async Task<ApiResponse> Create(int UserId) {
            try {
                await _appContext.Carts.AddAsync(new Carts { UserId = UserId, CreateAt = DateTime.Now });
                await _appContext.SaveChangesAsync();
                return new ApiResponse { success = true };
            } catch (Exception ex) {

                throw new AggregateException(ex.Message);
            }
        }

        public ApiResponse Get(int UserId) {
            try {
                return new ApiResponse { success =  true, data = _appContext.Carts.FirstOrDefault(c => c.UserId == UserId) };
            } catch (Exception ex) {

                throw new AggregateException(ex.Message);
            }
        }

        public ApiResponse GetAll() {
            return new ApiResponse { success = true, data = _appContext.Carts.AsEnumerable()};
        }

        public ApiResponse Remove(int UserId) {
            using (var trans = _appContext.Database.BeginTransaction()) {
                try {
                    Carts? cart = _appContext.Carts.FirstOrDefault(c => c.UserId == UserId);
                    _appContext.Carts.Remove(cart??new Carts { });
                    _appContext.SaveChanges();
                    trans.Commit();
                    return new ApiResponse { success = true };
                } catch (Exception ex) {
                    trans.Rollback();
                    throw new AggregateException(ex.Message);
                }
            }
        }
    }
}
