using FluentValidation;
using MyOwnApi.DTOs;

public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
{
    public CreateNoteDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.")
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Title cannot contain only spaces.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MinimumLength(3).WithMessage("Content must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Content cannot exceed 100 characters.")
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Content cannot contain only spaces.");

        RuleFor(x => x.Priority)
            .Must(p => p == "Low" || p == "Medium" || p == "High")
            .WithMessage("Priority must be Low, Medium, or High.");

        RuleFor(x => x.DueDate)
            .Must(date => date == null || date > DateTime.UtcNow)
            .WithMessage("DueDate cannot be in the past.");
    }
}
