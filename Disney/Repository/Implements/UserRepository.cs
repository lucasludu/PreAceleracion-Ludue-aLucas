using Disney.Data;
using Disney.Entities;
using Disney.Repository.Interfaces;

namespace Disney.Repository.Implements
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext op) : base(op)
        {

        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(a => a.Email == email);
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(a => a.Email == email);
        }
    }
}
