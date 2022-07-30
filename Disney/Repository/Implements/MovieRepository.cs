using Disney.Data;
using Disney.Entities;
using Disney.Repository.Interfaces;

namespace Disney.Repository.Implements
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext op) : base(op)
        {

        }
    }
}
