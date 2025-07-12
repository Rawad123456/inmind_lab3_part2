using lab3_inmind_part2.Data;
using lab3_inmind_part2.DTOs;
using lab3_inmind_part2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

namespace lab3_inmind_part2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly UniversityDbContext _context;
    private readonly IMapper _mapper;

    public TeachersController(UniversityDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    
    [HttpPost]
    public async Task<ActionResult<TeacherDto>> AddTeacher(TeacherDto dto)
    {
        var teacher = _mapper.Map<Teacher>(dto);
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, _mapper.Map<TeacherDto>(teacher));
    }

   
    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherDto>> GetTeacher(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();

        return _mapper.Map<TeacherDto>(teacher);
    }

  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAll()
    {
        var teachers = await _context.Teachers.ToListAsync();
        return Ok(_mapper.Map<List<TeacherDto>>(teachers));
    }
}