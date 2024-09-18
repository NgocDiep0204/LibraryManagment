using api_librarymanagment.Models.Library;
using api_librarymanagment.Models.SignUp;
using api_librarymanagment.Models.Login;


namespace api_librarymanagment.Services
{
    public interface IAuthenticationService
    {
        public Task<ApplicationUser> Resgister(RegisterUser registerUser);
        public Task<bool> ValidateUser(LoginUser loginUser);
    }
}
