using Disney.Entities;

namespace Disney.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetByEmail(string email);
        bool UserExists(string email);
    }
}
