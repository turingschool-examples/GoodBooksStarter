using GoodBooksAPI.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace GoodBooksEndpointTests
{
    public class BookCrudEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BookCrudEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void GetBooks_ReturnsListOfBooks()
        {

        }

        private GoodBooksApiContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GoodBooksApiContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new GoodBooksApiContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }
}