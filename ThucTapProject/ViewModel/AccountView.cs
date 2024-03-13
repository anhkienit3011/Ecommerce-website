using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Entities;

namespace ThucTapProject.ViewModel
{
    public class AccountView
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string? Avatar { get; set; }
        public int Status { get; set; }
    }
}
