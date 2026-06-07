using MyOwnApi.DTOs;
using MyOwnApi.Models;
using MyOwnApi.Repositories;

namespace MyOwnApi.Services;

public interface INotesService
{
    Task<List<NoteResponseDto>> GetAllAsync();
    Task<NoteResponseDto?> GetByIdAsync(int id);
    Task<NoteResponseDto> CreateAsync(CreateNoteDto dto, string userId);
    Task<NoteResponseDto> UpdateAsync(int id, UpdateNoteDto dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<List<NoteResponseDto>> GetByUserIdAsync(string userId);
    Task<Note?> ToggleDoneAsync(int id);
}

public class NotesService : INotesService
{
    private readonly INotesRepository _notesRepository;

    public NotesService(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    // ---------------------------------------------------------
    // GET ALL
    // ---------------------------------------------------------
    public async Task<List<NoteResponseDto>> GetAllAsync()
    {
        var notes = await _notesRepository.GetAllAsync();

        return notes.Select(n => new NoteResponseDto
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            CreatedAt = n.CreatedAt,
            UpdatedAt = n.UpdatedAt,
            UserId = n.UserId
        }).ToList();
    }

    // ---------------------------------------------------------
    // GET BY ID
    // ---------------------------------------------------------
    public async Task<NoteResponseDto?> GetByIdAsync(int id)
    {
        var note = await _notesRepository.GetByIdAsync(id);
        if (note == null) return null;

        return new NoteResponseDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt,
            UserId = note.UserId
        };
    }

    // ---------------------------------------------------------
    // CREATE
    // ---------------------------------------------------------
    public async Task<NoteResponseDto> CreateAsync(CreateNoteDto dto, string userId)
    {
        var note = new Note
        {
            Title = dto.Title,
            Content = dto.Content,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            IsPinned = dto.IsPinned
        };

        await _notesRepository.AddAsync(note);

        return new NoteResponseDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    // ---------------------------------------------------------
    // UPDATE (verify ownership)
    // ---------------------------------------------------------
    public async Task<NoteResponseDto> UpdateAsync(int id, UpdateNoteDto dto, string userId)
    {
        var note = await _notesRepository.GetByIdAndUserAsync(id, userId);
        if (note == null)
            throw new UnauthorizedAccessException("You are not the owner of this note, you cannot update it.");

        note.Title = dto.Title;
        note.Content = dto.Content;
        note.DueDate = dto.DueDate;
        note.Priority = dto.Priority;
        note.IsPinned = dto.IsPinned;
        note.IsDone = dto.IsDone;
        note.UpdatedAt = DateTime.UtcNow;

        await _notesRepository.UpdateAsync(note);

        return new NoteResponseDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    // ---------------------------------------------------------
    // DELETE (verify ownership)
    // ---------------------------------------------------------
    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var note = await _notesRepository.GetByIdAndUserAsync(id, userId);
        if (note == null)
            throw new UnauthorizedAccessException("You are not the owner of this note, you cannot delete it.");

        await _notesRepository.DeleteAsync(note);
        return true;
    }

    // ---------------------------------------------------------
    // GET BY USER ID
    // ---------------------------------------------------------
    public async Task<List<NoteResponseDto>> GetByUserIdAsync(string userId)
    {
        var notes = await _notesRepository.GetAllAsync();

        return notes
            .Where(n => n.UserId == userId)
            .Select(n => new NoteResponseDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt,
                UserId = n.UserId
            })
            .ToList();
    }

    // ---------------------------------------------------------
    // TOGGLE DONE 
    // ---------------------------------------------------------
    public async Task<Note?> ToggleDoneAsync(int id)
    {
        var note = await _notesRepository.GetByIdAsync(id);
        if (note == null) return null;

        note.IsDone = !note.IsDone;
        await _notesRepository.UpdateAsync(note);

        return note;
    }

}
