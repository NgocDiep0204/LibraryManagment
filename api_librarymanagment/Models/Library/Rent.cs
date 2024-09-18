using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_librarymanagment.Models.Library
{
    public class Rent
    {
        [Key]
        public string RentId { get; set; }
        public string StudentId { get; set; }
        public string? UserId { get; set; }   
        public DateTime? RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }   
        public int? Status { get; set; }
        public int? TotalBook { get; set; }
       
        public virtual ApplicationUser? UserIdNavigation { get; set; }
        public virtual Student? StudentIdNavigation { get; set; }
        public virtual ICollection<RentDetail>? RentDetails { get; set; }
    }
}
