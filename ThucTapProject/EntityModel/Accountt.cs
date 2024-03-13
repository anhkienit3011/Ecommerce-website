using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThucTapProject.Helper;

namespace ThucTapProject.Entities
{
    public class Accountt
    {
        [Key]
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string? Avatar { get; set; }
        public string Password { get; set; }
        public int Status { get; set; } = (int)Account_status.notValidated;// DEFAULT: not validated
        public int DecentralizationId { get; set; } = 3;// DEFAULT: clien(3)
        public virtual Decentralization? Decentralization { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<User>? Users { get; set; }
    }
}
