using Chirp.Core;
using Chirp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;


public class CheepRepository : ICheepRepository
{
    private readonly CheepDBContext _dbContext;

    public CheepRepository(CheepDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateCheep(CheepDTO cheep)
    {
        // ChatGPT
        var author = await _dbContext.Authors
            .FirstOrDefaultAsync(a => a.Name == cheep.AuthorName);
        // End ChatGPT     

        if (author == null)
        {
            throw new InvalidOperationException("No such author: " + cheep.AuthorName);
        }

        Cheep newCheep = new Cheep
        {
            CheepID = cheep.CheepID, // *** Check if ID is set correct when method is used. ***
            Text = cheep.Text,
            Author = author,
            Timestamp = cheep.Timestamp == default
                ? DateTime.UtcNow
                : cheep.Timestamp // if no time is found we set a current time
        };

        var queryResult = await _dbContext.Cheeps.AddAsync(newCheep); // does not write to the database!
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return queryResult.Entity.CheepID;
    }

    public async Task<int> CreateAuthor(AuthorDTO author)
    {
        var newAuthor = new Author()
        {
            Name = author.Name,
            Email = author.Email,
        };
        var queryResult = await _dbContext.Authors.AddAsync(newAuthor);
        await _dbContext.SaveChangesAsync();
        return queryResult.Entity.Id;
    }

    public async Task<AuthorDTO> GetAuthorByName(string name)
    {
        try
        {
            var author = await _dbContext.Authors
                .Include(a => a.Cheeps)
                .FirstAsync(a => a.Name == name);

            return new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                Email = author.Email,
                Cheeps = author.Cheeps
            };
        }
        catch (InvalidOperationException) 
        {
            throw new InvalidOperationException("No such author with name: " + name);
        }
    }
    
    public async Task<AuthorDTO> GetAuthorByEmail(string email)
    {
        try
        {
            var author = await _dbContext.Authors
                .Include(a => a.Cheeps)
                .FirstAsync(a => a.Email == email);

            return new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                Email = author.Email,
                Cheeps = author.Cheeps
            };
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("No such author with email: " + email);
        }
    }

    /*
    public async Task<int> InsertAuthor(string username, string email)
    {
        var newAuthor = new Author
        {
            Name = username,
            Email = email,
            Cheeps = new List<Cheep>(),
        };

        _dbContext.Authors.Add(newAuthor);
        await _dbContext.SaveChangesAsync();

        return newAuthor.AuthorID;
    }
    */

    // uses alter in UI to change the contect of a specefic message 
    public async Task<int> UpdateCheep(CheepDTO alteredMessage)
    {
        
        var existingCheep = await _dbContext.Cheeps.FindAsync(alteredMessage.CheepID);
        if (existingCheep != null)
        {
            existingCheep.Text = alteredMessage.Text;
            existingCheep.Timestamp = alteredMessage.Timestamp;
            
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            // Optionally handle not found case
            throw new Exception($"Message with ID {alteredMessage.CheepID} not found.");
        }
        return alteredMessage.CheepID;
    }

    /// <summary>
    /// Returns all available cheeps for a given page as a list of "Cheep" Data Transfer Objects
    /// </summary>
    /// <param name="page">The page to return cheeps for</param>
    /// <returns></returns>
    public async Task<List<CheepDTO>> GetCheeps(int page)
    {
        {
            var query = _dbContext.Cheeps
                .Include(c => c.Author)
                .OrderByDescending(c => c.Timestamp)
                .Select(c => new CheepDTO   
                {
                    Text = c.Text,
                    AuthorName = c.Author.Name,
                    Timestamp = c.Timestamp
                })
                .Skip((page - 1) * 32)    // TODO check if offset is correct 
                .Skip((page - 1) * 10)
                .Take(32);

            return await query.ToListAsync();
        }
    }
    public async Task<List<CheepDTO>> GetCheepsFromAuthor(string author, int page)
    {
        var query = _dbContext.Cheeps
            .Include(c => c.Author)
            .Where(c => c.Author.Name == author)
            .OrderByDescending(c => c.Timestamp)
            .Select(c => new CheepDTO   
            {
                Text = c.Text,
                AuthorName = c.Author.Name,
                Timestamp = c.Timestamp
            })
            .Skip((page - 1) * 32)    // kept old offset logic, TODO check if correct                      
            .Take(32);

        return await query.ToListAsync();
    }
    
}