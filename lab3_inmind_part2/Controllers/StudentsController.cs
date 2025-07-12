using lab3_inmind_part2.Data;
using lab3_inmind_part2.DTOs;
using lab3_inmind_part2.Models;

namespace lab3_inmind_part2.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutoMapper;



[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly UniversityDbContext _context;
    private readonly IMapper _mapper;

    public StudentsController(UniversityDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    
    [HttpPost]
    public async Task<ActionResult<StudentDto>> AddStudent(StudentDto dto)
    {
        var student = _mapper.Map<Student>(dto);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, _mapper.Map<StudentDto>(student));
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();

        return _mapper.Map<StudentDto>(student);
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
    {
        var students = await _context.Students.ToListAsync();
        return Ok(_mapper.Map<List<StudentDto>>(students));
    }
}
