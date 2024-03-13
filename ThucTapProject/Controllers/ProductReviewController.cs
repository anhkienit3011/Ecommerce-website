using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Page;
using ThucTapProject.Services;

namespace ThucTapProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase {
        private readonly ProductReviewService _service;
        private readonly AppDbContext _context;

        public ProductReviewController() {
            _service = new ProductReviewService();
            _context = new AppDbContext();
        }

        [HttpPost("CreateRated")]
        [Authorize]
        public async Task<IActionResult> CreateRated(ProductReviewEditModel productReviewEM) {
            if (ModelState.IsValid) {
                int userId = await GetCurrentAccountId();
                productReviewEM.UserId = userId;
                var res = await _service.CreateRated(productReviewEM);
                return Ok(res);
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("get{productId}")]
        public async Task<IActionResult> GetAllProductReview(int productId, [FromQuery] Pagination pagination) {
            var res = await _service.GetAllProductReview(productId, pagination);
            return Ok(res);
        }

        private async Task<int> GetCurrentAccountId() {
            int currentUserId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            return currentUserId;
        }
    }
}
