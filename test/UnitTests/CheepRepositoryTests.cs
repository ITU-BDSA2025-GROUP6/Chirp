using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class CheepRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly CheepRepository _repository;
    private readonly CheepDbContext _context;

    public CheepRepositoryTests()
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
        public async Task CreateCheep_ShouldAutoGenerateId()
        {
            var cheep = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = "CwC I am the first cheep.",
                Timestamp = DateTime.UtcNow
            };

            var resultId = await _repository.CreateCheep(cheep);
            Assert.True(resultId > 0);
        }
        
        [Fact]
        public async Task CreateCheep_ShouldGenerateUniqueIds()
        {
            var cheep1 = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = "UwU I am a test cheep.",
                Timestamp = DateTime.UtcNow
            };
            var cheep2 = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = "OwO I am also a test cheep.",
                Timestamp = DateTime.UtcNow
            };

            var id1 = await _repository.CreateCheep(cheep1);
            var id2 = await _repository.CreateCheep(cheep2);

            Assert.NotEqual(id1, id2);
            Assert.True(id1 > 0);
            Assert.True(id2 > 0);
        }
    }

    public class UpdateCheep : CheepRepositoryTests
    {
        [Fact]
        public async Task UpdateCheep_ShouldUpdateCheepText()
        {
            // Arrange 
            var id = 1;
            var originalCheep = new CheepDTO
            {
                CheepID = id,
                AuthorName = "Test Author",
                Text = "Hello, this is a test cheep!"
            };
            
            await _repository.CreateCheep(originalCheep);
            
            var updatedCheep = new CheepDTO
            {
                CheepID = id,
                AuthorName = "Test Author",
                Text = "Updated text"
            };
            
            // Act
            await _repository.UpdateCheep(updatedCheep);

            // Assert
            var result = await _context.Cheeps.FindAsync(id);
            Assert.Equal("Updated text", result?.Text);
        }
    }

    public class DeleteCheep : CheepRepositoryTests
    {
        [Fact]
        public async Task DeleteCheep_ShouldDeleteCheep()
        {
            // Arrange
            var cheepDto = new CheepDTO
            {
                AuthorName = "Test Author",
                Text = "Hello, this is a test cheep!",
            };
            await _repository.CreateCheep(cheepDto);

            var cheep = await _context.Cheeps
                .Include(c => c.Author)
                .FirstAsync();
            
            // Act
            var result = await _repository.DeleteCheep(cheep.CheepID, cheep.Author.UserName);
            
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCheep_ShouldThrowExceptionIfCheepNotFound()
        {
            // Arrange
            var id = 2; 
            var name = "Test";
            
            // Act
            var result = await _repository.DeleteCheep(id, name);
            
            // Assert
            Assert.False(result);
        }
    }
}