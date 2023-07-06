using Microsoft.EntityFrameworkCore;

namespace GoodBooksAPI.DataAccess
{
    public class GoodBooksApiContext : DbContext
    {
        public GoodBooksApiContext(DbContextOptions<GoodBooksApiContext> options) : base(options)
        {
            
        }
    }
}
