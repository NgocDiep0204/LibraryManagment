using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_librarymanagment.Models.Library
{
    public class Penalty
    {
        [Key]
        public string PenaltyId { get; set; } // Primary Key
        public string? StudentId { get; set; } // Foreign Key
        public DateTime? EntryDate { get; set; }
        public double? TotalAmount { get; set; }
            
        public string? UserId { get; set; }
       
        public virtual ApplicationUser? UserIdNavigation { get; set; }

        // Navigation properties
        public virtual Student? StudentIdNavigation { get; set; }
        public virtual ICollection<PenaltyDetail>? PenaltyDetails { get; set; }
            
        

    }
}
