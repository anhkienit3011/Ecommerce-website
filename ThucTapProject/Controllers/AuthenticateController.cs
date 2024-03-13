using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThucTapProject.EditModel;
using ThucTapProject.Services;
using ThucTapProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly AuthenticacteServices _authenServices;

        public AuthenticateController(IConfiguration config)
        {
            _authenServices = new AuthenticacteServices(config);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] LogInCredential loginCre)
        {
            UserLoginModel? AccountUser = await _authenServices.AuthenticateUser(loginCre);
            
            if(AccountUser != null) // authorized user
            {
                return Ok(await _authenServices.GenerateJWT(AccountUser));
            }
            return Ok(new ApiResponse()
            {
                success = false,
                message = "Xác thực thất bại",
                data = null
            });
        }

/*        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }*/

        [HttpPost("renewToken")]
        public async Task<IActionResult> RenewTken(TokenModel model)
        {
            var res = await _authenServices.RenewToken(model);
            return Ok(res);
        }

    }
}
