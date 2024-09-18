using api_librarymanagment.Models.ServiceEmail;
namespace api_librarymanagment.Services
{
    public interface IEmailService
    {
        void SendEmail(Messages message);
    }
}
