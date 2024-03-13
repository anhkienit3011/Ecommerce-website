using AutoMapper;
using Azure.Core;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.EditModel.request;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.IServices;
using ThucTapProject.Page;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Services
{
    public class AccountService : IAccountService {
        private readonly AppDbContext _context;
        private static Random rnd = new Random();
        private IEnumerable<Accountt> _accounts;
        private Mapper _mapper;
        public AccountService()
        {
            _context = new AppDbContext();
            _accounts = _context.Accountt
                .Include(c => c.Users)
                .Include(c => c.Decentralization)
                .Where(c => c.Status!=(int)Account_status.removed)
                .AsEnumerable();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }
        public async Task<string> SendAuthenToken(int IdAcc)
        {
            try
            {
                string NewToken = rnd.Next(30004224, 98923691).ToString();
                await UpdateToken(IdAcc, NewToken);
                User? UserToBeSend = await _context.User.Where(c => c.AccounttId == IdAcc).FirstOrDefaultAsync();
                Console.WriteLine(UserToBeSend.UserName);
                if(UserToBeSend == null)
                {
                    return "Not found account";
                }

                string bodyText = $"Hey {UserToBeSend.UserName}\n" +
                                $"From McDonald's\n" +
                                $"To validate you account please enter this code on your device: {NewToken}";

                await EmailService.SendEmail(UserToBeSend.Email, "Verify your account", bodyText); // send email

                return "ok";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateToken(int IdAcc, string token)
        {
            Accountt accountt = _accounts.Where(c => c.AccountId == IdAcc).First();

            accountt.ResetPasswordToken = token;
            accountt.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(24);
            _context.Accountt.Update(accountt);
            await _context.SaveChangesAsync();
        }

        public async Task<string> ChangePassword(int IdAcc, ChangePasswordRequest request)
        {
            Accountt? AccountToBeChanged = _accounts.Where(c => c.AccountId == IdAcc).FirstOrDefault();
            if (AccountToBeChanged == null)
            {
                return "Not found account";
            }
            string NewEncryptPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword, 13);
            if (BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, AccountToBeChanged.Password))
                {
                AccountToBeChanged.Password = NewEncryptPassword;
                AccountToBeChanged.UpdateAt = DateTime.UtcNow; 
                _context.Accountt.Update(AccountToBeChanged);
                await _context.SaveChangesAsync();
                return "Sửa mật khẩu thành công";
            }
            return "Mật Khẩu cũ không đúng";
        }

        public async Task<ApiResponse> CheckAuthenToken(int AccId, string token)
        {
            Accountt? AccountToCheck = _accounts.Where(c => c.AccountId==AccId).FirstOrDefault(); 
            if (AccountToCheck == null) {
                return new ApiResponse { success = false, message = "tai khoan khong ton tai" };
            }
            if(AccountToCheck.ResetPasswordToken == null)
            {
                return new ApiResponse { success = false, message= "Chưa có mã đổi mật khẩu" };
                
            }

            if(AccountToCheck.ResetPasswordTokenExpiry <= DateTime.Now)
            {
                return new ApiResponse { success = false, message = "Token out of date" };
            }
            if (AccountToCheck.ResetPasswordToken != token)
            {
                return new ApiResponse { success = false, message = "Token not match" };
            }

            AccountToCheck.Status = (int)Account_status.validated; // validated
            AccountToCheck.ResetPasswordToken = null;
            _context.Update(AccountToCheck);
            await _context.SaveChangesAsync();

            return new ApiResponse { success = true, data = AccountToCheck.AccountId};
        }

        public async Task<string> Edit(int IdAcc, AccountEditModel NewAccEd)
        {
            Accountt? AccToBeEdit = _accounts.Where(c => c.AccountId==IdAcc).FirstOrDefault();

            string? OldAvatarLink = AccToBeEdit.Avatar;

            bool IsAccNameExisted = _accounts
                .Where(c => c.AccountName != AccToBeEdit.AccountName)
                .Any(c => c.AccountName == NewAccEd.AccountName);

            if (IsAccNameExisted)
            {
                return "Tên tài khoản đã tồn tại";
            }
            try
            {
                string name = CommonFunctions.NameFormat(NewAccEd.AccountName);
                AccToBeEdit.UpdateAt = DateTime.Now;
                AccToBeEdit.AccountName = name;

                _context.Accountt.Update(AccToBeEdit);
                await _context.SaveChangesAsync();

                if (NewAccEd.ImageFile != null)
                {
                    AccToBeEdit.Avatar = await CloudinaryService.uploadImage(NewAccEd.ImageFile);
                    await _context.SaveChangesAsync();

                    if (OldAvatarLink != null)
                    {
                        await CloudinaryService.deleteImage(OldAvatarLink);
                    }
                }
                    
                return "Cập nhật tài khoản thành công";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<AccountView> Add(Accountt NewAcc)
        {
            try
            {
                await _context.Accountt.AddAsync(NewAcc);
                await _context.SaveChangesAsync();

                return _mapper.Map<Accountt, AccountView>(NewAcc);
            }
            catch (Exception ex)
            {
                await CloudinaryService.deleteImage(NewAcc.Avatar);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Accountt?> Get(int Id)
        {
            return await _context.Accountt
                .Where(c => c.AccountId == Id && c.Status!=(int)Account_status.removed)
                .Include(c => c.Users)
                .FirstOrDefaultAsync();
        }

        public async Task<ErrorMessage> Remove(int Id)
        {
            Accountt? accToBeDelete = await _context.Accountt.FindAsync(Id);
            if (accToBeDelete == null)
            {
                return ErrorMessage.NotFound;
            }

            accToBeDelete.Status = (int)Account_status.removed; // Remove sattus
            accToBeDelete.UpdateAt = DateTime.Now;
            _context.Accountt.Update(accToBeDelete);
            _context.SaveChanges();

            return ErrorMessage.Success;
        }

        public async Task PermanentRemove(int Id)
        {
            Accountt? accToBeDelete = await _context.Accountt.FindAsync(Id);
            if (accToBeDelete == null)
            {
                return;
            }

            User? UserToBeDelete = await _context.User.Where(c => c.AccounttId == Id).FirstOrDefaultAsync();
            if (UserToBeDelete != null)
            {
                _context.User.Remove(UserToBeDelete);
                await _context.SaveChangesAsync();
            }
            
            await CloudinaryService.deleteImage(accToBeDelete.Avatar);
            _context.Accountt.Remove(accToBeDelete);
            _context.SaveChanges();

            return;
        }

        public async Task<pageResult<AccountView>> Search(Pagination pagination, string? KeySearch=null)
        {
            IEnumerable<AccountView> paginData;
            if (KeySearch == null)
            {
                paginData = pageResult<AccountView>.ToPageResult(pagination, _accounts.Select(c => _mapper.Map<AccountView>(c)));
                return new pageResult<AccountView>(pagination, paginData);
            }
            else
            {
                KeySearch = KeySearch.ToLower();
            }

            IEnumerable<Accountt> data;
            KeySearch = KeySearch.Trim();

            if (KeySearch.Contains('@'))
            {
                data = _accounts
                    .Where(c => c.Users.First().Email==KeySearch)
                    .AsEnumerable();
            }
            else if (KeySearch.Length==KeySearch.Count(c => Char.IsNumber(c)))
            {
                data = _accounts
                    .Where(c => c.Users.FirstOrDefault().Phone == KeySearch)
                    .AsEnumerable();
            }
            else
            {
                data = _accounts
                    .Where(c => 
                            c.Users.First().UserName.ToLower().Contains(KeySearch) 
                            || c.AccountName.ToLower().Contains(KeySearch))
                    .AsEnumerable();
            }

            IEnumerable<AccountView> accountList = data.Select(c => _mapper.Map<AccountView>(c));

           /* IEnumerable<Accountt> paginData;*/
            if (accountList == null)
            {
                accountList = _accounts.Select(c => _mapper.Map<AccountView>(c));
            }

            paginData = pageResult<AccountView>.ToPageResult(pagination, accountList);
            return new pageResult<AccountView> ( pagination, paginData );
        }

        internal async Task<bool> IsAccountNameExisted(string AccName)
        {
            return _accounts.Any(c => c.AccountName == AccName);
        }
        internal async Task<Accountt?> Get(LogInCredential logIn)
        {
            Accountt? acc = _accounts
                .Where(c => c.AccountName == logIn.AccountName)
                .FirstOrDefault();
            if(acc == null) return null;
            bool MatchPassword = BCrypt.Net.BCrypt.EnhancedVerify(logIn.Password, acc.Password);
            if (MatchPassword)
            {
                return acc;
            }
            return null;
        }

        public async Task<ApiResponse> QuenMatKhau(string AccountName) {
            Accountt? acc = _accounts.FirstOrDefault(c => c.AccountName == AccountName);
            if (acc == null) {
                return new ApiResponse{ success = false, message= "Tài khoản không tồn tại"};
            }
            string resSenToken = await SendAuthenToken(acc.AccountId);
            if(resSenToken == "ok") {
                return new ApiResponse { data = acc.AccountId, success = true, message = "Hãy nhập mã OTP mà chúng tôi vừa gửi vào email của bạn" };
            } else {
                return new ApiResponse { success = false, message="Lỗi không xác định"};
            }
        }

        public async Task<ApiResponse> NewPassWord(int AccountId, NewPassWord newPassWordRequest) {
            if (newPassWordRequest.NewPassword != newPassWordRequest.ConfirmPassword) {
                return new ApiResponse {success = false, message="Mật khẩu mới và mật khẩu xác nhận không bằng nhau"};
            }
            Accountt? acc = _accounts.FirstOrDefault(c => c.AccountId == AccountId);
            if (acc == null) {
                return new ApiResponse { success = false, message = "Tài khoản không tồn tại" };
            }
            string NewEncryptPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassWordRequest.NewPassword, 13);
            acc.Password = NewEncryptPassword;
            acc.UpdateAt = DateTime.Now;
            _context.Update(acc);
            _context.SaveChanges();
            return new ApiResponse { success = true, message="Đổi mật khẩu mới thành công"};
        }
    }
}
