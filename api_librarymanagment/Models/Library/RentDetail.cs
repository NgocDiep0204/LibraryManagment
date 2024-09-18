namespace api_librarymanagment.Models.Library
{
    public class RentDetail
    {
        public string RentId { get; set; }
        public string BookId { get; set; }
        public int? Quantity { get; set; }
        public virtual Book? BookIdNavigation { get; set; }
        public virtual Rent? RentIdNavigation { get; set; }

    }
}
