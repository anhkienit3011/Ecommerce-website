using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using System.Text;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.Services;
using ThucTapProject.ViewModel;

namespace ThucTapProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController()
        {
            _service = new UserService();
        }

        [HttpPut("EditUser")]
        [Authorize(Roles = "employee, clien")]
        public async Task<IActionResult> Edit(UserEditModel NewUserEM)
        {
            // lấy thông tin user hiện tại
            int currentUserId = GetCurrentUserId();
            UserViewModel? userVM = await _service.Get(currentUserId);
            // kiểm tra số điện thoại mới tồn tại
            if (await _service.IsExistedPhone(NewUserEM.Phone, userVM.Phone))
            {
                ModelState.AddModelError(string.Empty, Res.PhoneExisted);
            }
            // kiểm tra email mới đã tồn tại
            if (await _service.IsExistedEmail(NewUserEM.Email, userVM.Email))
            {
                ModelState.AddModelError(string.Empty, Res.EmailExisted);
            }
            // Thay đổi thành công
            if (ModelState.IsValid)
            {
                await _service.Edit(currentUserId, NewUserEM);
                return Ok(Res.EditSuccess);
            }
            // Thay đổi thất bại
            else
            {
                return Ok(ErrHelper.Log(ModelState));
            }
        }

        [HttpGet("getbyid{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(_service.Get(id));
        }
        private int GetCurrentUserId()
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            return currentUserId;
        }
    }
}
