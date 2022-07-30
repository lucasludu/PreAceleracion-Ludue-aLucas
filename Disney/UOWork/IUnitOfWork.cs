using Disney.Repository.Interfaces;

namespace Disney.UOWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepo { get; }
        IGenreRepository GenreRepo { get; }
        IMovieRepository MovieRepo { get; }
        ICharacterRepository CharacterRepo { get; }
        void Save();
    }
}
