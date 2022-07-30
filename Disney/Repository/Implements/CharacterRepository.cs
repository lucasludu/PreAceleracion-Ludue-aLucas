using Disney.Data;
using Disney.Entities;
using Disney.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Disney.Repository.Implements
{
    public class CharacterRepository : GenericRepository<Character>, ICharacterRepository
    {
        public CharacterRepository(ApplicationDbContext op) : base(op)
        {

        }
        
    }
}
