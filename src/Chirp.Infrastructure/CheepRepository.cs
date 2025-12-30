using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;


public class CheepRepository : ICheepRepository
{
    private readonly CheepDbContext _dbContext;

    public CheepRepository(CheepDbContext dbContext)
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
            AuthorID = author.Id,
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
        const int pageSize = 32;

        return await _dbContext.Cheeps
            .Include(c => c.Author)
            .OrderByDescending(c => c.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CheepDTO
            {
                CheepID = c.CheepID,
                Text = c.Text,
                AuthorName = c.Author!.UserName ?? string.Empty,
                ProfilePicturePath = c.Author.ProfilePicturePath,
                Timestamp = c.Timestamp
            })
            .ToListAsync();
    }
    public async Task<List<CheepDTO>> GetCheeps(int page, string? currentUserId)
    {
        const int pageSize = 32;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return await _dbContext.Cheeps
                .Include(c => c.Author)
                .OrderByDescending(c => c.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CheepDTO
                {
                    CheepID = c.CheepID,
                    Text = c.Text,
                    AuthorName = c.Author!.UserName ?? string.Empty,
                    ProfilePicturePath = c.Author.ProfilePicturePath,
                    Timestamp = c.Timestamp,
                    IsRecheepedByCurrentUser = false
                })
                .ToListAsync();
        }

        var recheepedIdsQuery = _dbContext.Recheeps
            .Where(r => r.AuthorID == currentUserId)
            .Select(r => r.CheepID);

        return await _dbContext.Cheeps
            .Include(c => c.Author)
            .OrderByDescending(c => c.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CheepDTO
            {
                CheepID = c.CheepID,
                Text = c.Text,
                AuthorName = c.Author!.UserName ?? string.Empty,
                ProfilePicturePath = c.Author.ProfilePicturePath,
                Timestamp = c.Timestamp,
                IsRecheepedByCurrentUser = recheepedIdsQuery.Contains(c.CheepID)
            })
            .ToListAsync();
    }
    public async Task<List<CheepDTO>> GetCheepsFromAuthor(string author, int page)
    {
        const int pageSize = 32;

        var authorId = await _dbContext.Authors
            .Where(a => a.UserName == author)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (authorId == null)
            return new List<CheepDTO>();

        var authored = _dbContext.Cheeps
            .Where(c => c.AuthorID == authorId)
            .Select(c => new CheepDTO
            {
                Text = c.Text,
                AuthorName = c.Author!.UserName ?? string.Empty,
                Timestamp = c.Timestamp,
                ProfilePicturePath = c.Author.ProfilePicturePath
                                     ?? "/images/default.png"
            });

        var recheeped = _dbContext.Recheeps
            .Where(r => r.AuthorID == authorId)
            .Join(
                _dbContext.Cheeps,
                r => r.CheepID,
                c => c.CheepID,
                (r, c) => new CheepDTO
                {
                    Text = c.Text,
                    AuthorName = c.Author.UserName,
                    Timestamp = c.Timestamp,
                    ProfilePicturePath = c.Author.ProfilePicturePath
                                         ?? "/images/default.png"
                }
            );

        return await authored
            .Concat(recheeped)
            .OrderByDescending(c => c.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<CheepDTO>> GetCheepsFromFollowedAuthor(string userId, int page)
    {
        const int pageSize = 32;

        // IDs of users followed + the user itself
        var followedIds = await _dbContext.Follows
            .Where(f => f.FollowsId == userId)
            .Select(f => f.FollowedById)
            .ToListAsync();

        followedIds.Add(userId);

        var authoredCheeps = _dbContext.Cheeps
            .Where(c => followedIds.Contains(c.AuthorID))
            .Select(c => new CheepDTO
            {
                CheepID = c.CheepID,
                Text = c.Text,
                AuthorName = c.Author!.UserName ?? string.Empty,
                ProfilePicturePath = c.Author.ProfilePicturePath,
                Timestamp = c.Timestamp
            });

        var recheepedCheeps =
            from r in _dbContext.Recheeps
            where followedIds.Contains(r.AuthorID)
            join c in _dbContext.Cheeps on r.CheepID equals c.CheepID
            select new CheepDTO
            {
                CheepID = c.CheepID,
                Text = c.Text,
                AuthorName = c.Author!.UserName ?? string.Empty,
                ProfilePicturePath = c.Author.ProfilePicturePath,
                Timestamp = c.Timestamp
            };

        return await authoredCheeps
            .Concat(recheepedCheeps)
            .OrderByDescending(c => c.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<int> CreateRecheep(AuthorDTO? author, int cheepId)
    {
        if (author == null)
            throw new InvalidOperationException("No such author");

        var existing = await _dbContext.Recheeps
            .FirstOrDefaultAsync(r => r.AuthorID == author.Id && r.CheepID == cheepId);

        if (existing != null)
        {
            _dbContext.Recheeps.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return cheepId;
        }

        var newRecheep = new Recheep
        {
            AuthorID = author.Id,
            CheepID = cheepId
        };

        await _dbContext.Recheeps.AddAsync(newRecheep);
        await _dbContext.SaveChangesAsync();
        return cheepId;
    }
}