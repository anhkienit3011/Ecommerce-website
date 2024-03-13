using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.IServices;
using ThucTapProject.ViewModel;
using ThucTapProject.Helper;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private IEnumerable<User> _userList;
        private Mapper Mapper;

        public UserService()
        {
            _context = new AppDbContext();
            _userList = _context.User
                                .Include(c => c.Accountt)
                                .Where(c => c.Accountt.Status != (int)Account_status.removed);
            Mapper = AutoMapperProfiles.InitializeAutoMapper();
        }
        public async Task<int> Add(User NewUser)
        {
            try
            {
                await _context.User.AddAsync(NewUser);
                await _context.SaveChangesAsync();
                return NewUser.UserId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task Edit(int IdUser, UserEditModel NewUserED)
        {
            string Name = CommonFunctions.NameFormat(NewUserED.UserName);   
            await _context.User
                .Where(c => c.UserId == IdUser)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(c => c.UserName, Name)
                    .SetProperty(c => c.Phone, NewUserED.Phone)
                    .SetProperty(c => c.Email, NewUserED.Email)
                    .SetProperty(c => c.Address, NewUserED.Address.Trim())
                    .SetProperty(c => c.UpdateAt, DateTime.Now));
        }

        public async Task Remove(int IdUser)
        {
            await _context.User.Where(c => c.UserId == IdUser).ExecuteDeleteAsync();
        }
        public async Task<UserViewModel> Get(int Id)
        {
            User user = _userList.Where(c => c.UserId==Id).FirstOrDefault();
            UserViewModel UserVM = Mapper.Map<UserViewModel>(user);
            return UserVM;
        }

        internal async Task<bool> IsExistedPhone(string PhoneNumber, string? OldPhone=" ")
        {
            return _context.User.Where(c => c.Phone != OldPhone).Any(c => c.Phone == PhoneNumber);
        }
        internal async Task<bool> IsExistedEmail(string NewEmail, string? OldEmail="")
        {
            return _userList.Where(c => c.Email != OldEmail).Any(c => c.Email == NewEmail);
        }
    }
}
