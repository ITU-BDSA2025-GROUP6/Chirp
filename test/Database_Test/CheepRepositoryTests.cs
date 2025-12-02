using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace Database_Test;

public class CheepRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly CheepRepository _repository;
    private readonly CheepDBContext _context;

    public CheepRepositoryTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<CheepDBContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new CheepDBContext(options);
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

    public class CreateCheepTests : CheepRepositoryTests
    {
        [Fact]
        public async Task createCheep_ShouldAddCheepToDatabase()
        {
            //Arrange
            var newCheep = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = "Hello, this is a test cheep!",
                Timestamp = DateTime.UtcNow
            };

            //Act
            await _repository.CreateCheep(newCheep);
            var createdCheep = await _context.Cheeps.Include(c => c.Author).FirstOrDefaultAsync();
            //Assert
            Assert.NotNull(createdCheep);
            Assert.Equal(1, createdCheep.CheepID);
            Assert.Equal("Test Author", createdCheep.Author?.UserName);
            Assert.Equal("Hello, this is a test cheep!", createdCheep.Text);
        }

        [Fact]
        public async Task CreateCheep_ShouldThrowExecptionIfCheepTooLong()
        {
            //Arrange
            var invalidCheep = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = new string('*', 200),
                Timestamp = DateTime.UtcNow
            };

            var validCheep = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = new string('*', 160),
                Timestamp = DateTime.UtcNow
            };


            var resultId = await _repository.CreateCheep(validCheep);
            Assert.True(resultId > 0);

            await Assert.ThrowsAsync<ValidationException>(async () => await _repository.CreateCheep(invalidCheep));
        }

        [Fact]
        public async Task CreateCheep_ShouldThrowExceptionIfAuthorNotFound()
        {
            var cheep = new CheepDTO
            {
                AuthorName = "No Author",
                Text = "Hello, this is a test cheep!",
                Timestamp = DateTime.UtcNow
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _repository.CreateCheep(cheep));
        }

        [Fact]
        public async Task CreateCheep_ShouldSetTimestampIfNotProvided()
        {
            var cheep = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = "Hello, this is a test cheep!",
            };

            var resultId = await _repository.CreateCheep(cheep);
            var savedCheep = await _context.Cheeps.FindAsync(resultId);

            Assert.NotNull(savedCheep);
            Assert.True(savedCheep.Timestamp < DateTime.UtcNow);
            Assert.True(DateTime.UtcNow - savedCheep.Timestamp < TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task CreateCheep_ShouldHandleProvidedCheepId()
        {
            var cheep = new CheepDTO
            {
                CheepID = 100,
                AuthorName = "Test Author",
                Text = "Hello, this is a test cheep!",
                Timestamp = DateTime.UtcNow
            };
            var resultId = await _repository.CreateCheep(cheep);
            Assert.True(resultId > 0);
            Assert.NotEqual(0, resultId);
            Assert.Equal(100, resultId);
        }

        [Fact]
        public async Task CreateCheep_ShouldThrowExceptionIfCheepIdAlreadyExists()
        {
            // Arrange
            var cheep1 = new CheepDTO
            {
                CheepID = 1,
                AuthorName = "Test Author",
                Text = "Hello, this is a test cheep!",
            };
            var cheep2 = new CheepDTO
            {
                CheepID = 1,
                AuthorName = "Test Author",
                Text = "Hello, this is a test cheep!",
            };

            // Act
            await _repository.CreateCheep(cheep1);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _repository.CreateCheep(cheep2));
        }
    }
}