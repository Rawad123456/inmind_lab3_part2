using FluentValidation;
using lab3_inmind_part2.DTOs;

namespace lab3_inmind_part2.Validators;

public class StudentDtoValidator : AbstractValidator<StudentDto>
{
    public StudentDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Student name is required.")
            .MaximumLength(50).WithMessage("Name can't be longer than 50 characters.");
    }
}