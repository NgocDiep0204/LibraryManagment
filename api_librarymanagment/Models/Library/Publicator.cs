namespace api_librarymanagment.Models.Library
{
    public class Publicator
    {
        public string PublicatorId { get; set; }  
        public string? Name { get; set; }   
        public string? PhoneNumber { get; set; }    
        public ICollection<Book>? Books { get; set; }   
    }
}
