using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OT.Models
{

    public class Post
    {
        // user ID from AspNetUser table.
        public string? OwnerID { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }

        [Display(Name = "Post Date")]
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }
        public string? content { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Display(Name = "Subscribers")]
        public int subscribers { get; set; }
        public ContactStatus Status { get; set; }
    }

    public enum ContactStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
