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

    // =====================================================================
    // PUBLIC ENDPOINTS (GET)
    // =====================================================================

    // GET ALL NOTES (public, but only non-deleted)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notes = await _notesService.GetAllAsync();
        return Ok(notes);
    }

    // GET NOTE BY ID (public, but only non-deleted)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var note = await _notesService.GetByIdAsync(id);
        if (note == null)
            return NotFound("Note not found.");

        return Ok(note);
    }

    // =====================================================================
    // AUTHENTICATED ENDPOINTS (API KEY REQUIRED)
    // =====================================================================

    // CREATE NOTE (requires API Key)
    [HttpPost]
    public async Task<IActionResult> Create(CreateNoteDto dto)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (userId == null)
            return Unauthorized("API Key invalid.");

        var note = await _notesService.CreateAsync(dto, userId);
        return Ok(note);
    }

    // UPDATE NOTE (only owner)
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

    // DELETE NOTE (soft delete, only owner)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (userId == null)
            return Unauthorized("API Key invalid.");

        try
        {
            await _notesService.DeleteAsync(id, userId);
            return Ok(new { message = "Note deleted successfully." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    // =====================================================================
    // EXTRA FEATURES
    // =====================================================================

    // TOGGLE DONE
    [HttpPatch("{id}/done")]
    public async Task<ActionResult<NoteResponseDto>> ToggleDone(int id)
    {
        var response = await _notesService.ToggleDoneAsync(id);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    // GET MY NOTES (requires API Key)
    [HttpGet("mine")]
    public async Task<IActionResult> GetMyNotes()
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (userId == null)
            return Unauthorized("API Key invalid.");

        var notes = await _notesService.GetByUserIdAsync(userId);
        return Ok(notes);
    }
}
