using Application.Teachers.Commands;
using FluentValidation;
using inmind_session5_DDD.Application.Teachers.Commands;

namespace inmind_session5_DDD.Application.Validators;

public class CreateTeacherCommandValidator : AbstractValidator<CreateTeacherCommand>
{
    public CreateTeacherCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Teacher name is required.")
            .MaximumLength(100).WithMessage("Name can't exceed 100 characters.");
    }
}