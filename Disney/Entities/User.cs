using System.ComponentModel.DataAnnotations;

namespace Disney.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int Age { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt{ get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
