using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThucTapProject.EditModel;
using ThucTapProject.Page;
using ThucTapProject.Services;

namespace ThucTapProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly OrderService _service;

        public OrderController() {
            _service = new OrderService();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Order(OrderEditModel orderEditModel) {
            if (ModelState.IsValid) {
                int userId = await GetCurrentAccountId();
                var res = await _service.CreateOrder(userId, orderEditModel);
                return Ok(res);
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("GetAnOrder/{OrderId}")]
        [Authorize]
        public async Task<IActionResult> GetAnOrder(int OrderId) {
            int userId = await GetCurrentAccountId();
            var res = await _service.GetAnOrder(userId, OrderId);
            return Ok(res);
        }

        [HttpGet("GetAllOrder")]
        [Authorize]
        public async Task<IActionResult> GetAllOrder([FromForm] Pagination pagination) {
            int userId = await GetCurrentAccountId();
            var res = await _service.GetAllOrder(userId, pagination);
            return Ok(res);
        }

        [HttpPatch("UpdateOrderStatus/{OrderId}")]
        [Authorize] // admin
        public async Task<IActionResult> UpdateOrderStatus(int OrderId, int StatusId) {
            var res = await _service.UpdateOrderStatus(OrderId, StatusId);
            return Ok(res);
        }

        [HttpGet("GetAllDeliveredOrderDetail")]
        [Authorize]
        public async Task<IActionResult> GetAllDeliveredOrderDetail([FromQuery] Pagination pagination) {
            int userId = await GetCurrentAccountId();
            var res = await _service.GetAllDeliveredOrderDetail(userId, pagination);
            return Ok(res);
        }

        private async Task<int> GetCurrentAccountId() {
            int currentUserId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            return currentUserId;
        }
    }
}
