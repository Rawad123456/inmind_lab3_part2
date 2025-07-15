using FluentValidation;
using lab3_inmind_part2.DTOs;

namespace lab3_inmind_part2.Validators;

public class EnrollmentDtoValidator : AbstractValidator<EnrollmentDto>
{
    public EnrollmentDtoValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("StudentId must be greater than zero.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("CourseId must be greater than zero.");
    }
}