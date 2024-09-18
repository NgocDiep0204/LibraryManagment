using System.ComponentModel.DataAnnotations;

namespace api_librarymanagment.Models.Library
{
    public class PenaltyDetail
    {
        [Key]
        public string PenaltyId { get; set; } // Primary Key
        public string BookId { get; set; } // Foreign Key
        public string? Detail { get; set; }
        public int? LevelPenalty { get; set; }
        public double? Amount { get; set; }
        public int? ViolationTimes { get; set; }
        public virtual Penalty? PenaltyIdNavigation { get; set; }
        public virtual Book? BookIdNavigation { get; set; }

       
    }
}
