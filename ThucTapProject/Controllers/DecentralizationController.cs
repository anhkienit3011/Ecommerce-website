using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using ThucTapProject.DAO;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.Page;
using ThucTapProject.Services;

namespace ThucTapProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class DecentralizationController : ControllerBase
    {
        private readonly DecentralizationService _service;
        private readonly AppDbContext _context;

        public DecentralizationController()
        {
            _service = new DecentralizationService();
            _context = new AppDbContext();
        }

        [HttpPatch("decentraUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> decentralizeUser(int AccountId, int decenId)
        {
            var res = await _service.decentralizeUserAsync(AccountId, decenId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddDecen(string NewAuthorityName)
        {
            if (_context.Decentralization.Any(c => c.AuthorityName.ToLower() == NewAuthorityName.ToLower()))
            {
                return Ok("Tên phân quyền đã tồn tại");
            }
            else
            {
                var res = await _service.Add(NewAuthorityName);
                return Ok($"Thêm thành công tên phân quyền mới: {NewAuthorityName}");
            }

        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllDecen([FromQuery] Pagination pagination)
        {
            var res = await _service.GetAll(pagination);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _service.Get(id);
            return Ok(res);
        }

        [HttpDelete("remove{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _service.Remove(id);
            if(res == "ok")
            {
                return Ok(res);
            }
            return BadRequest();
        }

        [HttpPut("edit{id}")]
        public async Task<IActionResult> Edit(int id, string NewAuthorityName)
        {
            Decentralization OlDecen = _context.Decentralization.FirstOrDefault(c => c.DecentralizationId==id);
            bool IsExistedName = _context.Decentralization
                .Where(c => c.AuthorityName.ToLower() != OlDecen.AuthorityName.ToLower())
                .Any(c => c.AuthorityName.ToLower() == NewAuthorityName.ToLower());
            if (IsExistedName)
            {
                return Ok("Tên phân quyền đã tồn tại");
            }
            else
            {
                var res = await _service.Edit(id, NewAuthorityName);
                return Ok(res);
            }
        }
    }
}
