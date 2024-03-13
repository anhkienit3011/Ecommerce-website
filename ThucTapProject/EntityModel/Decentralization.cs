using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThucTapProject.Entities
{
    public class Decentralization
    {
        [Key]
        public int DecentralizationId { get; set; }
        public string AuthorityName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<Accountt>? Accountts { get; set; }

        /*
         * ,1: admin
         * ,2: employee
         * ,3: clien
         */ 
    }
}
