using GoodBooksAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodBooksAPI.DataAccess
{
    public class GoodBooksApiContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public GoodBooksApiContext(DbContextOptions<GoodBooksApiContext> options) : base(options)
        {
            
        }
    }
}
