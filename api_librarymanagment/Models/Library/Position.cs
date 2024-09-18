using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace api_librarymanagment.Models.Library
{
    public class Position
    {
        [Key]
        public string PositionId { get; set; }
        public string? Compartment { get; set; }
        public string? Shelf { get; set; }
        public string? Area { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
        public string Pos => $"{Compartment ?? ""} {Shelf ?? ""} {Area ?? ""}".Trim();
    }
}
