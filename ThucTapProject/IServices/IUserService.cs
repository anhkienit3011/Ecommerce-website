using CloudinaryDotNet;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.Page;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices
{
    public interface IUserService
    {
        public Task<int> Add(User NewUser);
        public Task Edit(int Id, UserEditModel NewUserEM); // api
        public Task Remove(int Id);
        public Task<UserViewModel> Get(int Id);
    }
}
