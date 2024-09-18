using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using api_librarymanagment.Models.Login;
namespace api_librarymanagment.Models.Library
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<Rent>? Rents { get; set; }
        public virtual ICollection<Penalty>? Penalties { get; set; }
    }
}
