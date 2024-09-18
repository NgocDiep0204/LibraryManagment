namespace api_librarymanagment.Models.Library
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
