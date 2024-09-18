namespace api_librarymanagment.Models.Library
{
    public class Book
    {
        public string BookId { get; set; }
        public string PublicatorId { get; set; }
        public string AuthorId { get; set; }
        public string PositionId { get; set; }
        public string CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Language { get; set; }    
        public int? Quantity { get; set; }
        public string? Image { get; set; }
        public virtual Category? CategoryIdNavigation { get; set; }
        public virtual Author? AuthorIdNavigation { get; set; }
        public virtual Publicator? PublicatorIdNavigation { get; set; }
        public virtual Position? PositionIdNavigation { get; set; }
        public virtual ICollection<RentDetail>? RentDetails { get; set; }
        public virtual ICollection<PenaltyDetail>? PenaltyDetails { get; set; }

       
        //public virtual ICollection<Author>? Authors { get; set; }
        //public virtual ICollection<Category>? Categories { get; set; }

    }
}
