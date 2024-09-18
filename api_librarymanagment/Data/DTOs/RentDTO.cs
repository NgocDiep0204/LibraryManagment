namespace api_librarymanagment.Data.DTOs
{
    public class RentDTO
    {
        public string RentId { get; set; }
        public string StudentId { get; set; }
        public string? UserId { get; set; }
        public DateTime? RentDate { get; set; }
        public DateTime? ReturnDate { get; set;  }
        public int? Status { get; set; }
        public int? TotalBook { get; set; }
    }
}
