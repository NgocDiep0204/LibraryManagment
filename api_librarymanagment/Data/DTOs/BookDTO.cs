namespace api_librarymanagment.Data.DTOs
{
    public class BookDTO
    {
        public string? BookId { get; set; } 
        public string? CategoryId { get; set; }
        public string? AuthorId { get; set; }
        public string? PublicatorId { get; set; }
        public string? PositionId { get; set; }
        public string? Name { get; set; }
        public string? Language { get; set; }
        public int? Quantity { get; set; }
        public string? Image { get; set; }
      
    }
}
