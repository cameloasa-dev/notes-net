using FluentValidation;
using MyOwnApi.DTOs;
using MyOwnApi.Exceptions;
using MyOwnApi.Models;
using MyOwnApi.Repositories;
namespace MyOwnApi.Services;
public interface INotesService
{
    Task<List<NoteResponseDto>> GetAllAsync();
    Task<List<NoteResponseDto>> GetByUserIdAsync(string userId);
    Task<NoteResponseDto> GetByIdAsync(int id);
    Task<NoteResponseDto> CreateAsync(CreateNoteDto dto, string userId);
    Task<NoteResponseDto> UpdateAsync(int id, UpdateNoteDto dto, string userId);
    Task DeleteAsync(int id, string userId);
    Task<NoteResponseDto> ToggleDoneAsync(int id);
}
public class NotesService : INotesService
{
    private readonly INotesRepository _notesRepository;

    public NotesService(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    // =====================================================================
    // GET ALL NOTES
    // =====================================================================
    public async Task<List<NoteResponseDto>> GetAllAsync()
    {
        var notes = await _notesRepository.GetAllAsync();
        return notes.Select(MapToDto).ToList();
    }

    // =====================================================================
    // GET NOTES BY USER ID
    // =====================================================================
    public async Task<List<NoteResponseDto>> GetByUserIdAsync(string userId)
    {
        var notes = await _notesRepository.GetAllAsync();

        return notes
            .Where(n => n.UserId == userId)
            .Select(MapToDto)
            .ToList();
    }

    // =====================================================================
    // GET NOTE BY ID
    // =====================================================================
    public async Task<NoteResponseDto> GetByIdAsync(int id)
    {
        var note = await _notesRepository.GetByIdAsync(id);
        if (note == null)
            throw new NotFoundException("Note not found.");

        return MapToDto(note);
    }

    // =====================================================================
    // CREATE NOTE
    // =====================================================================
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

        return MapToDto(note);
    }

    // =====================================================================
    // UPDATE NOTE
    // =====================================================================
    public async Task<NoteResponseDto> UpdateAsync(int id, UpdateNoteDto dto, string userId)
    {
        var note = await _notesRepository.GetByIdAndUserAsync(id, userId);
        if (note == null)
            throw new ForbiddenException("You are not the owner of this note.");

        note.Title = dto.Title;
        note.Content = dto.Content;
        note.DueDate = dto.DueDate;
        note.Priority = dto.Priority;
        note.IsPinned = dto.IsPinned;
        note.IsDone = dto.IsDone;
        note.UpdatedAt = DateTime.UtcNow;

        await _notesRepository.UpdateAsync(note);

        return MapToDto(note);
    }

    // =====================================================================
    // DELETE NOTE
    // =====================================================================
    public async Task DeleteAsync(int id, string userId)
    {
        var note = await _notesRepository.GetByIdAndUserAsync(id, userId);
        if (note == null)
            throw new ForbiddenException("You are not the owner of this note.");

        await _notesRepository.SoftDeleteAsync(note);
    }

    // =====================================================================
    // TOGGLE DONE
    // =====================================================================
    public async Task<NoteResponseDto> ToggleDoneAsync(int id)
    {
        var note = await _notesRepository.GetByIdAsync(id);
        if (note == null)
            throw new NotFoundException("Note not found.");

        note.IsDone = !note.IsDone;
        note.UpdatedAt = DateTime.UtcNow;

        await _notesRepository.UpdateAsync(note);

        return MapToDto(note);
    }

    // =====================================================================
    // PRIVATE MAPPER
    // =====================================================================
    private static NoteResponseDto MapToDto(Note n)
    {
        return new NoteResponseDto
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            CreatedAt = n.CreatedAt,
            UpdatedAt = n.UpdatedAt,
            IsDone = n.IsDone,
            DueDate = n.DueDate,
            Priority = n.Priority,
            IsPinned = n.IsPinned,
            UserId = n.UserId
        };
    }
}