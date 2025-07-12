using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using lab3_inmind_part2.Data;
using lab3_inmind_part2.DTOs;
using lab3_inmind_part2.Models;

namespace lab3_inmind_part2.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly UniversityDbContext _context;
    private readonly IMapper _mapper;

    public CoursesController(UniversityDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    
    [HttpPost]
    public async Task<ActionResult<CourseDto>> AddCourse(CourseDto dto)
    {
        var course = _mapper.Map<Course>(dto);

        
        var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == dto.TeacherId);
        if (!teacherExists)
            return BadRequest("Teacher ID not found.");

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, _mapper.Map<CourseDto>(course));
    }

   
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        return _mapper.Map<CourseDto>(course);
    }

   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
    {
        var courses = await _context.Courses.ToListAsync();
        return Ok(_mapper.Map<List<CourseDto>>(courses));
    }
}