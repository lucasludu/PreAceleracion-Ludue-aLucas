using System.ComponentModel.DataAnnotations;

namespace Disney.Auth.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Age is Required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

    }
}
