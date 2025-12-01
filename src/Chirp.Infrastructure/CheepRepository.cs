using System.ComponentModel.DataAnnotations;
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
            .FirstOrDefaultAsync(a => a.UserName == cheep.AuthorName);
        // End ChatGPT     

        if (author == null)
        {
            throw new InvalidOperationException("No such author: " + cheep.AuthorName);
        }
        
        if (cheep.Text.Length > 160)
        {
            throw new ValidationException("Cheep text too long: " + cheep.Text);
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

    public async Task<bool> DeleteCheep(int cheepId, string authorName)
    {
        var cheep = await _dbContext.Cheeps
            .Include(a => a.Author)
            .FirstOrDefaultAsync(a => a.CheepID == cheepId && a.Author!.UserName == authorName);
        if (cheep == null)
        {
            return false;
        }
        _dbContext.Cheeps.Remove(cheep);
        await _dbContext.SaveChangesAsync();
        
        return true;
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
                    CheepID = c.CheepID,
                    Text = c.Text,
                    AuthorName = c.Author!.UserName ?? string.Empty,
                    Timestamp = c.Timestamp
                })
                .Skip((page - 1) * 32)    // TODO check if offset is correct 
                .Take(32);

            return await query.ToListAsync();
        }
    }
    public async Task<List<CheepDTO>> GetCheepsFromAuthor(string author, int page)
    {
        var query = _dbContext.Cheeps
            .Include(c => c.Author)
            .Where(c => c.Author!.UserName == author)
            .OrderByDescending(c => c.Timestamp)
            .Select(c => new CheepDTO   
            {
                Text = c.Text,
                AuthorName = c.Author!.UserName ?? string.Empty,
                Timestamp = c.Timestamp
            })
            .Skip((page - 1) * 32)                 
            .Take(32);

        return await query.ToListAsync();
    }
    
}