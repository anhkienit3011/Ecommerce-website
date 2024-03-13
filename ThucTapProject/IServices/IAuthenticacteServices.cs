using ThucTapProject.EditModel;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices
{
    public interface IAuthenticacteServices
    {
        public Task<ApiResponse> GenerateJWT(UserLoginModel UserInfor);
        public Task<UserLoginModel> AuthenticateUser(LogInCredential login);
    }
}
