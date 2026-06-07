using FluentValidation;
using MyOwnApi.DTOs;

public class UpdateNoteDtoValidator : AbstractValidator<UpdateNoteDto>
{
    public UpdateNoteDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Priority)
            .Must(p => p == "Low" || p == "Medium" || p == "High")
            .WithMessage("Priority must be Low, Medium, or High.");

        RuleFor(x => x.DueDate)
            .Must(date => date == null || date > DateTime.UtcNow)
            .WithMessage("DueDate cannot be in the past.");
    }
}
