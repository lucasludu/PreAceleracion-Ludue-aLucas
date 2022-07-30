using System.ComponentModel.DataAnnotations;

namespace Disney.Auth.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password{ get; set; }
    }
}
