using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThucTapProject.DAO;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.IServices;
using ThucTapProject.Page;
using ThucTapProject.ViewModel;

namespace ThucTapProject.Services
{
    public class DecentralizationService : IDecentralizationService
    {
        private readonly AppDbContext _context;
        private Mapper _mapper;

        public DecentralizationService()
        {
            _context = new AppDbContext();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }

        public async Task<string> decentralizeUserAsync(int AccountId, int decenId)
        {
            Accountt accToBeDecen =  _context.Accountt
                .Include(c => c.Decentralization)
                .FirstOrDefault(c => c.AccountId == AccountId);
            if(accToBeDecen == null)
            {
                return "khong ton tai tai khoan";
            }
            if(accToBeDecen.Decentralization.AuthorityName.ToLower() == "admin")
            {
                return "admin khong the cap quyen cho admin";
            }
            if (_context.Decentralization.FirstOrDefaultAsync(c => c.DecentralizationId == decenId)!=null)
            {
                accToBeDecen.DecentralizationId = decenId;
                _context.Update(accToBeDecen);
                _context.SaveChanges();
                return "Phan quyen thanh cong";
            }
            return "Cấp bậc không tồn tại";
        }
        public async Task<string> Add(string NewAuthorityName)
        {
            try
            {
                await _context.Decentralization.AddAsync(new Decentralization()
                {
                    AuthorityName = NewAuthorityName,
                    CreatedAt = DateTime.Now
                });
                await _context.SaveChangesAsync();
                return "ok";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> Edit(int IdDecen, string NewAuthorityName)
        {
            try
            {
                Decentralization? DecenToBeEdit = await _context.Decentralization
                .FirstOrDefaultAsync(c => c.DecentralizationId == IdDecen);
                DecenToBeEdit.AuthorityName = NewAuthorityName;
                DecenToBeEdit.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return "ok";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<pageResult<DecentralizationView>> GetAll(Pagination pagination)
        {
            IEnumerable<DecentralizationView> data = _context.Decentralization.Select(c => new DecentralizationView()
            {
                DecentralizationId = c.DecentralizationId,
                AuthorityName = c.AuthorityName
            }).AsEnumerable();

            IEnumerable<DecentralizationView> paginationData = pageResult<DecentralizationView>.ToPageResult(pagination, data);

            return new pageResult<DecentralizationView>(pagination, paginationData);
        }

        public async Task<DecentralizationView> Get(int Id)
        {
            try
            {
                Decentralization decen = await _context.Decentralization.FirstOrDefaultAsync(c => c.DecentralizationId == Id);
                return _mapper.Map<Decentralization, DecentralizationView>(decen);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Remove(int IdDecen)
        {
            try
            {
                await _context.Decentralization
                    .Where(c => c.DecentralizationId == IdDecen)
                    .ExecuteDeleteAsync();
                return "ok";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
