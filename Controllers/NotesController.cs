using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyOwnApi.DTOs;
using MyOwnApi.Services;

namespace MyOwnApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("notesLimiter")]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }

    // ---------------------------------------------------------
    // GET ALL NOTES (public for all users with API Key)
    // ---------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notes = await _notesService.GetAllAsync();
        return Ok(notes);
    }

    // ---------------------------------------------------------
    // GET NOTE BY ID (public for all users with API Key)
    // ---------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var note = await _notesService.GetByIdAsync(id);
        if (note == null)
            return NotFound("Note not found.");

        return Ok(note);
    }

    // ---------------------------------------------------------
    // CREATE NOTE (only with a valid API Key, asociated with user)
    // ---------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create(CreateNoteDto dto)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (userId == null)
            return Unauthorized("API Key invalid.");

        var note = await _notesService.CreateAsync(dto, userId);
        return Ok(note);
    }

    // ---------------------------------------------------------
    // UPDATE NOTE (only owners can update their note)
    // ---------------------------------------------------------
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateNoteDto dto)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (userId == null)
            return Unauthorized("API Key invalid.");

        try
        {
            var updated = await _notesService.UpdateAsync(id, dto, userId);
            return Ok(updated);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    // ---------------------------------------------------------
    // DELETE NOTE (only owners can delete their note)
    // ---------------------------------------------------------
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (userId == null)
            return Unauthorized("API Key invalid.");

        try
        {
            var deleted = await _notesService.DeleteAsync(id, userId);
            return Ok(new { message = "Note deleted successfully." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMyNotes()
    {
            var userId = HttpContext.Items["UserId"]?.ToString();
            if (userId == null) return Unauthorized();

            var notes = await _notesService.GetByUserIdAsync(userId);
            return Ok(notes);
        }

}
