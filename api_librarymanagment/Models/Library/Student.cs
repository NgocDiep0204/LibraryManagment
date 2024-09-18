using System.ComponentModel.DataAnnotations;

namespace api_librarymanagment.Models.Library
{
    public class Student
    {
        [Key]
        public string StudentId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public virtual ICollection<Rent>? Rents { get; set; }
        public virtual ICollection<Penalty>? Penalties { get; set; }
    }
}
