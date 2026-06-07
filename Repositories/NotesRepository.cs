using Microsoft.EntityFrameworkCore;
using MyOwnApi.Context;
using MyOwnApi.Models;

namespace MyOwnApi.Repositories;

public interface INotesRepository
{
    Task<List<Note>> GetAllAsync();
    Task<Note?> GetByIdAsync(int id);
    Task<Note?> GetByIdAndUserAsync(int id, string userId);
    Task AddAsync(Note note);
    Task UpdateAsync(Note note);
    Task SoftDeleteAsync(Note note); 
}

public class NotesRepository : INotesRepository
{
    private readonly ApplicationDbContext _context;

    public NotesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Note>> GetAllAsync()
    {
        return await _context.Notes
            .Where(n => !n.IsDeleted)                
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        return await _context.Notes
            .Where(n => !n.IsDeleted)                 
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<Note?> GetByIdAndUserAsync(int id, string userId)
    {
        return await _context.Notes
            .Where(n => !n.IsDeleted)                 
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
    }

    public async Task AddAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Note note)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Note note)
    {
        note.IsDeleted = true;                     
        await _context.SaveChangesAsync();
    }
}
