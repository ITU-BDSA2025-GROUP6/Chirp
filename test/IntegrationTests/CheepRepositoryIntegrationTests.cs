using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

public class CheepRepositoryIntegrationTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly CheepRepository _repository;
    private readonly CheepDbContext _context;
    
    public CheepRepositoryIntegrationTests() 
    {
    _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<CheepDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new CheepDbContext(options);
        _context.Database.EnsureCreated();

        var author = new Author
            { Id = "1", UserName = "Test Author", Email = "test@author.com", Cheeps = new List<Cheep>() };
        var author2 = new Author
            { Id = "2", UserName = "Test Author2", Email = "test2@email.com", Cheeps = new List<Cheep>() };

        //_context.Authors.Add(author);
        //_context.Authors.Add(author2);
        _context.Authors.AddRange(author, author2);
        _context.SaveChanges();

        _repository = new CheepRepository(_context);
    }
    
    public void Dispose()
    {
        // this method is called after all tests in the class have run
        // disposes of connection
        _connection.Close();
        _connection.Dispose();
        _context.Dispose();
    }
    
    public class CreateCheepTests : CheepRepositoryIntegrationTests
    {

        [Fact]
        public async Task CreateCheep_SavesCheepToDatabase()
        {
            //arrange
            var newCheep = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = "Test Cheep",
                Timestamp = DateTime.UtcNow
            };

            //act
            await _repository.CreateCheep(newCheep);

            //assert
            var cheepInDb = _context.Cheeps
                .Include(c => c.Author)
                .FirstOrDefault(c => c.Text == "Test Cheep" && c.Author.UserName == "Test Author");
            
            Assert.NotNull(cheepInDb);
            Assert.Equal("Test Cheep", cheepInDb.Text);
            Assert.Equal("Test Author", cheepInDb.Author.UserName);
        }


        [Fact]
        public async Task GetCheeps_ShouldReturnExpectedCheepsInOrder()
        {
            //arrange
            for (int i = 0; i < 65; i++)
            {
                var newCheep = new CheepDTO
                {
                    AuthorName = "Test Author",
                    Text = i.ToString(),
                    Timestamp = DateTime.UtcNow
                };
                
                await _repository.CreateCheep(newCheep);
            }
            
            //act
            var page1 = await _repository.GetCheeps(1);
            var page2 = await _repository.GetCheeps(2);

            //assert
            Assert.NotNull(page1);
            Assert.NotNull(page2);
            Assert.Equal(32, page1.Count);
            Assert.Equal(32, page2.Count);
        }
    }

}