using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.Services;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly ProductTypeService _productTypeService;
        private readonly AppDbContext _context;

        public ProductTypeController()
        {
            _productTypeService = new ProductTypeService();
            _context = new AppDbContext();
        }

        [HttpPost("CreateProductType")]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> CreateProductType([FromForm] ProductTypeEditModel NewProductTypeEM)
        {
            if(ModelState.IsValid)
            {
                var res = await _productTypeService.Create(NewProductTypeEM);
                return Ok(res);
            }
            else
            {
                return Ok(ErrHelper.Log(ModelState));
            }
        }

        [HttpGet("getbyid{id}")]
        public IActionResult Get(int id)
        {
            var res = _productTypeService.Get(id);
            return Ok(res);
        }

        [HttpDelete("remove{id}")]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _productTypeService.Remove(id);
            return Ok(res);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var res = _productTypeService.GetAll();
            return Ok(res);
        }

        [HttpPut("modify{id}")]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Modify(int id, [FromForm] ProductTypeEditModel productTypeEM)
        {
            if (ModelState.IsValid)
            {
                var res = await _productTypeService.Modify(id, productTypeEM);
                return Ok(res);
            }
            else return Ok(ErrHelper.Log(ModelState));
        }
    }
}
