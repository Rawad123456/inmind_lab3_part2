using Application.Courses.Commands;
using FluentValidation;


namespace inmind_session5_DDD.Application.Validators;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Course title is required.")
            .MaximumLength(100).WithMessage("Title can't exceed 100 characters.");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0).WithMessage("Valid TeacherId is required.");
    }
}