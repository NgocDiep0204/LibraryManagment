using System.ComponentModel.DataAnnotations;

namespace api_librarymanagment.Models.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Bạn phải nhập tên tài khoản!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập Email!")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu!")]
        public string Password { get; set; }
        //public string Role { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string PhoneNumber { get; set; }

    }
}
