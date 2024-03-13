using Microsoft.Identity.Client;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.Page;
using ThucTapProject.ViewModel;

namespace ThucTapProject.IServices
{
    public interface IDecentralizationService
    {
        public Task<string> Add(string NewAuthorityName);
        public Task<string> Edit(int Id, string NewAuthorityName);
        public Task<string> Remove(int Id);
        public Task<DecentralizationView> Get(int Id);
        public Task<pageResult<DecentralizationView>> GetAll(Pagination pagination);
        public Task<string> decentralizeUserAsync(int AccountId, int decenId);
    }
}
