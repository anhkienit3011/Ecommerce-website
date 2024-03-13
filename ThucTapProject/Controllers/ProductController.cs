using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.EditModel.request;
using ThucTapProject.Page;
using ThucTapProject.Services;

namespace ThucTapProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase {
        private readonly ProductService _productService;
        private readonly AppDbContext _context;

        public ProductController() {
            _productService = new ProductService();
            _context = new AppDbContext();
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Create([FromForm] ProductEditModel NewProductEM) {
            if (ModelState.IsValid) {
                var res = await _productService.Create(NewProductEM);
                return Ok(res);
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("modify/{ProductId}")]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Modify(int ProductId, [FromForm] ProductEditModel NewProductEM) {
            if (ModelState.IsValid) {
                var res = await _productService.Modify(ProductId, NewProductEM);
                return Ok(res);
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("remove/{ProductId}")]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Remove(int ProductId) {
            if (ModelState.IsValid) {
                var res = await _productService.Remove(ProductId);
                return Ok(res);
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("get/{ProductId}")]
        public async Task<IActionResult> Get(int ProductId) {
            if (ModelState.IsValid) {
                var res = _productService.Get(ProductId);
                return Ok(res);
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("getall")]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> GetAll([FromForm] Pagination pagination) {
            if (ModelState.IsValid) {
                var res = await _productService.GetAll(pagination);
                return Ok(res);
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] Pagination pagination, SearhProductRequest model) {
            if (model.From.HasValue && model.To.HasValue && model.From > model.To) {
                ModelState.AddModelError("", "giá trị 'to' lớn hơn hoặc bằng 'from'");
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid) {
                var res = await _productService.Search(model.SearchKey, pagination, model.From, model.To, model.SortBy);
                return Ok(res);
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("FavoritProduct")]
        public async Task<IActionResult> GetFavoritProducts([FromQuery] Pagination pagination) {
            var res = await _productService.GetFavoritProducts(pagination);
            return Ok(res);
        }

        [HttpGet("relatedproduct{ProductId}")]
        public async Task<IActionResult> GetRelatedProducts(int ProductId, [FromQuery] Pagination pagination) {
            var res = await _productService.GetRelatedProducts(ProductId, pagination);
            return Ok(res);
        }
    }
}
