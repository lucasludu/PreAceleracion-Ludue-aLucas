using Disney.Data;
using Disney.Repository.Implements;
using Disney.Repository.Interfaces;

namespace Disney.UOWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository UserRepo { get; private set; }
        public IGenreRepository GenreRepo { get; private set; }
        public IMovieRepository MovieRepo { get; private set; }
        public ICharacterRepository CharacterRepo { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            UserRepo = new UserRepository(context);
            GenreRepo = new GenreRepository(context);
            MovieRepo = new MovieRepository(context);
            CharacterRepo = new CharacterRepository(context);
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
