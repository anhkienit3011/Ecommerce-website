using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.EditModel;
using ThucTapProject.EditModel.request;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.Page;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.IServices
{
    public interface IAccountService
    {
        public Task<AccountView> Add(Accountt NewAcc);
        public Task<string> SendAuthenToken(int IdAcc);
        public Task<ApiResponse> CheckAuthenToken(int AccId, string token);
        public Task<string> Edit(int IdAcc, AccountEditModel NewAccEd);
        public Task<ErrorMessage> Remove(int Id); // xoa mem
        public Task PermanentRemove(int Id); // xoa vinh vien
        public Task<Accountt> Get(int Id);
        public Task<string> ChangePassword(int IdAcc, ChangePasswordRequest request);
        public Task<ApiResponse> QuenMatKhau(string UserName);
        public Task<ApiResponse> NewPassWord(int AccountId, NewPassWord newPassWord);
        public Task<pageResult<AccountView>> Search(Pagination pagination, string? KeySearch = "");

    }
}
