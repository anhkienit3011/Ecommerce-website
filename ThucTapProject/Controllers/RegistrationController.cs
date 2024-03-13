using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.Services;

namespace ThucTapProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly RegistrationService _service;
        private readonly AccountService _accService;
        private readonly UserService _userService;

        public RegistrationController()
        {
            _service = new RegistrationService();
            _accService = new AccountService();
            _userService = new UserService();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserInformation NewInfor)
        {
            // Kiểm tra số điện thoại đã tồn tại
            if (await _userService.IsExistedPhone(NewInfor.Phone))
            {
                ModelState.AddModelError(string.Empty, Res.PhoneExisted);
            }
            // Kiểm tra địa chỉ email đã tồn tại
            if (await _userService.IsExistedEmail(NewInfor.Email))
            {
                ModelState.AddModelError(string.Empty, Res.EmailExisted);
            }
            // Kiểm tra tên tài khoản đã tồn tại
            if(await _accService.IsAccountNameExisted(NewInfor.AccountName))
            {
                ModelState.AddModelError(string.Empty, Res.AccountNameExisted);
            }

            if (ModelState.IsValid)
            {
                var res = await _service.Register(NewInfor);
                return Ok(res);
            }
            else
            {
                // Trả về validation clien side
                return Ok(ModelState);
            }
        }
    }
}
