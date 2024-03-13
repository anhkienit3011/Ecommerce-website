using AutoMapper;
using System.Text;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.IServices;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Services
{
    public class RegistrationService
    {
        private readonly AccountService _accountService;
        private readonly UserService _userService;
        private readonly CartService _cartService;
        private Mapper _mapper;

        public RegistrationService()
        {
            _accountService = new AccountService();
            _userService = new UserService();
            _cartService = new CartService();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }

        public async Task<ApiResponse> Register(UserInformation infor)
        {
            try
            {
                // map dữ liệu từ object UserInformtion
                Accountt newAccount = _mapper.Map<UserInformation, Accountt>(infor);
                // kiểm tra file ảnh không rỗng
                if (infor.ImageFile != null)
                {
                    newAccount.Avatar = await CloudinaryService.uploadImage(infor.ImageFile);
                }
                // Thêm tài khoản mới, trả về kiểu tài khoản view
                AccountView NewAccount = await _accountService.Add(newAccount);

                User newUser = _mapper.Map<UserInformation, User>(infor);
                newUser.AccounttId = NewAccount.AccountId;
                // thêm user
                int UserId =  await _userService.Add(newUser);

                // Thêm giỏ hàng cho người dùng mới được tạo
                await _cartService.Create(UserId);
                
                return new ApiResponse { success = true, message = "Tạo tài khoản thành công"};
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
