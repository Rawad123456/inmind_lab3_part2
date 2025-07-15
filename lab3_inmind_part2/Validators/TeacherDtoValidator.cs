using FluentValidation;
using lab3_inmind_part2.DTOs;

namespace lab3_inmind_part2.Validators;

public class TeacherDtoValidator : AbstractValidator<TeacherDto>
{
    public TeacherDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Teacher name is required.")
            .MaximumLength(50).WithMessage("Name must be less than 50 characters.");
    }
}