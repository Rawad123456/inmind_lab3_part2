using inmind_session5_DDD.API.Services;
using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace inmind_session5_DDD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestMapperController : ControllerBase
    {
        private readonly ObjectMapperService _mapper;

        public TestMapperController(ObjectMapperService mapper)
        {
            _mapper = mapper;
        }

        [HttpPost("map-student-to-dto")]
        public ActionResult<StudentDto> MapStudentToDto([FromBody] Student student)
        {
            var dto = _mapper.Map<Student, StudentDto>(student);
            return Ok(dto);
        }

        [HttpPost("map-teacher-to-dto")]
        public ActionResult<TeacherDto> MapTeacherToDto([FromBody] Teacher teacher)
        {
            var dto = _mapper.Map<Teacher, TeacherDto>(teacher);
            return Ok(dto);
        }

        [HttpPost("map-course-to-dto")]
        public ActionResult<CourseDto> MapCourseToDto([FromBody] Course course)
        {
            var dto = _mapper.Map<Course, CourseDto>(course);
            return Ok(dto);
        }
    }
}