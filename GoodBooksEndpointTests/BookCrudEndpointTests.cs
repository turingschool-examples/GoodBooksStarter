using GoodBooksAPI.DataAccess;
using GoodBooksAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Net.Http.Json;
using System.Text;

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
        public async void GetBooks_ReturnsListOfBooks()
        {
            Book book1 = new Book { Title = "Dune", Description = "Sand Monsters" };
            Book book2 = new Book { Title = "Wool", Description = "Human Monsters" };
            List<Book> books = new() { book1, book2 };

            GoodBooksApiContext context = GetDbContext();
            HttpClient client = _factory.CreateClient();
            context.Books.AddRange(books);
            context.SaveChanges();

            HttpResponseMessage response = await client.GetAsync("/api/books");
            string content = await response.Content.ReadAsStringAsync();

            // The method ParseJson is defined below
            string expected = ParseJson(books);

            response.EnsureSuccessStatusCode();
            Assert.Equal(expected, content);

        }

        [Fact]
        public async void GetBook_ReturnsSingleBook()
        {
            Book book1 = new Book { Title = "Dune", Description = "Sand Monsters" };
            Book book2 = new Book { Title = "Wool", Description = "Human Monsters" };
            List<Book> books = new() { book1, book2 };

            GoodBooksApiContext context = GetDbContext();
            HttpClient client = _factory.CreateClient();
            context.Books.AddRange(books);
            context.SaveChanges();

            HttpResponseMessage response = await client.GetAsync("/api/books/2");
            string content = await response.Content.ReadAsStringAsync();

            // The method ParseJson is defined below
            string expected = ParseJson(book2);

            response.EnsureSuccessStatusCode();
            Assert.Equal(expected, content);
            Assert.DoesNotContain("Dune", content);

        }

        [Fact]
        public async void PostBook_CreatesBookInDb()
        {
            // Create fresh database
            GoodBooksApiContext context = GetDbContext();

            // Set up and send the request
            HttpClient client = _factory.CreateClient();
            var jsonString = "{\"Title\":\"Lamb's Wool\", \"Description\":\"Super Itchy\"}";
            var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/books", requestContent);

            // Get the first (and should be only) book from the db
            var newBook = context.Books.First();

            Assert.Equal("Created", response.StatusCode.ToString());
            Assert.Equal(201, (int)response.StatusCode);
            Assert.Equal("Lamb's Wool", newBook.Title);
        }

        [Fact]
        public async void PutBook_UpdatesDatabaseRecord()
        {
            Book book1 = new Book { Title = "Dune", Description = "Sand Monsters" };

            GoodBooksApiContext context = GetDbContext();
            context.Books.Add(book1);
            context.SaveChanges();

            HttpClient client = _factory.CreateClient();
            var jsonString = "{ \"Id\":\"1\", \"Title\":\"Beetlejuice\", \"Description\":\"Sand Monsters\" }";
            var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/books/1", requestContent);

            // Clear all previously tracked DB objects to get a new copy of the updated book
            context.ChangeTracker.Clear();

            Assert.Equal(204, (int)response.StatusCode);
            Assert.Equal("Beetlejuice", context.Books.Find(1).Title);
        }

        [Fact]
        public async void DeleteBook_RemovesDatabaseRecord()
        {
            Book book1 = new Book { Title = "Dune", Description = "Sand Monsters" };
            Book book2 = new Book { Title = "Wool", Description = "Human Monsters" };
            List<Book> books = new() { book1, book2 };

            GoodBooksApiContext context = GetDbContext();
            context.Books.AddRange(books);
            context.SaveChanges();

            HttpClient client = _factory.CreateClient();
            var response = await client.DeleteAsync("/api/books/1");

            Assert.Equal(204, (int)response.StatusCode);
            Assert.Equal(1, context.Books.Count());
        }

        // This method helps us create an expected value. We can use the Newtonsoft JSON serializer to build the string that we expect.  Without this helper method, we would need to manually create the expected JSON string.
        private string ParseJson(object obj)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });

            return json;
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