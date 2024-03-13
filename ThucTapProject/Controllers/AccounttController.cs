using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.EditModel.request;
using ThucTapProject.Helper;
using ThucTapProject.Page;
using ThucTapProject.Services;

namespace ThucTapProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccounttController : ControllerBase
    {
        private readonly AccountService _service;
        private readonly UserService _userService;
        private readonly AppDbContext _appDbContext;

        public AccounttController()
        {
            _service = new AccountService();
            _userService = new UserService();
            _appDbContext = new AppDbContext();
        }

        [HttpPut("sendtoken")]
        [Authorize]
        public async Task<IActionResult> SendToken()
        {
            int CurrentAccountId = await GetCurrentAccountId();
            await Console.Out.WriteLineAsync(CurrentAccountId.ToString());
            var res = await _service.SendAuthenToken(CurrentAccountId);
            return Ok(res);
        }

        [HttpPut("checktoken")]//
        [Authorize]
        public async Task<IActionResult> CheckToken(string Token)
        {
            int AccId = await GetCurrentAccountId();
            var res = await _service.CheckAuthenToken(AccId, Token);
            return Ok(res);
        }

        [HttpPut("quenmatkhau/checktoken{AccId}")]//
        public async Task<IActionResult> CheckToken(int AccId, string Token) {
            var res = await _service.CheckAuthenToken(AccId, Token);
            return Ok(res);
        }

        [HttpPut("quenmatkhau{AccountName}")]//
        public async Task<IActionResult> QuenMatKhau(string AccountName) {
            var res = await _service.QuenMatKhau(AccountName);
            return Ok(res);
            // neu thanh cong se tra ve account Id
        }

        [HttpPut("quenmatkhau/MatKhauMoi{AccountId}")]//
        public async Task<IActionResult> NewPassWord(int AccountId, NewPassWord newPassWord) {
            var res = await _service.NewPassWord(AccountId, newPassWord);
            return Ok(res);
        }

        [HttpGet("searchaccount")]
        [Authorize]
        public async Task<IActionResult> Search(string? keySearch, [FromQuery] Pagination pagination)
        {
            var res = await _service.Search(pagination, keySearch);
            return Ok(res);
        }

        [HttpPatch("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            int currentAccountId = await GetCurrentAccountId();
            await Console.Out.WriteLineAsync(currentAccountId.ToString());
            var res = await _service.ChangePassword(currentAccountId, request);
            return Ok(res);
        }

        [HttpGet("getaccount{AccId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Get(int AccId)
        {
            var res = await _service.Get(AccId);
            return Ok(res);
        }

        [HttpPut("editaccount{AccId}")]
        [Authorize]
        public async Task<IActionResult> Edit([FromForm] AccountEditModel NewAccEM)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ErrHelper.Log(ModelState));
            }
            int currentAccountId = await GetCurrentAccountId();
            var res = await _service.Edit(currentAccountId, NewAccEM);
            return Ok(res);

        }

        [HttpDelete("removeaccount{AccId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Remove(int AccId)
        {
            var res = await _service.Remove(AccId);
            return Ok(res);
        }

        private async Task<int> GetCurrentAccountId()
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            return _appDbContext.User.SingleOrDefaultAsync(c => c.UserId.Equals(currentUserId)).Result.AccounttId;
        }
    }
}
