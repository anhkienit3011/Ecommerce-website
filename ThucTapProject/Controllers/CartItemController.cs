using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Services;

namespace ThucTapProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase {
        private readonly CartItemService _service;
        private readonly AppDbContext _context;

        public CartItemController() {
            _service = new CartItemService();
            _context = new AppDbContext();
        }

        [HttpGet("getall")]
        [Authorize]
        public async Task<IActionResult> GetAll() {
            int UserId = await GetCurrentAccountId();
            var res = await _service.GetAll(UserId);
            return Ok(res);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(CartItemEditModel NewCartItemEM) {
            if (ModelState.IsValid) {
                int UserId = await GetCurrentAccountId();
                var res = await _service.Create(UserId, NewCartItemEM);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }

        private async Task<int> GetCurrentAccountId() {
            int currentUserId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            return currentUserId;
        }
    }
}
