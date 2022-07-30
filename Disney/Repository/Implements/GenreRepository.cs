using Disney.Data;
using Disney.Entities;
using Disney.Repository.Interfaces;

namespace Disney.Repository.Implements
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext op) : base(op)
        {

        }
    }
}
