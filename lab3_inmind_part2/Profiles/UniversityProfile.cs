using lab3_inmind_part2.DTOs;
using lab3_inmind_part2.Models;

namespace lab3_inmind_part2.Profiles;

using AutoMapper;




public class UniversityProfile : Profile
{
    public UniversityProfile()
    {
        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<Teacher, TeacherDto>().ReverseMap();
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
    }
}
