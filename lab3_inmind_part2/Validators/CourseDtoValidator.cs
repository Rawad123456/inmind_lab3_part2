using FluentValidation;
using lab3_inmind_part2.DTOs;

namespace lab3_inmind_part2.Validators;

public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Course title is required.")
            .MaximumLength(100).WithMessage("Course title too long.");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0).WithMessage("Teacher ID must be a positive number.");
    }
}