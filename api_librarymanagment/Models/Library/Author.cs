using System.ComponentModel.DataAnnotations;

namespace api_librarymanagment.Models.Library
{
    public class Author
    {
        [Key]
        public string AuthorId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
