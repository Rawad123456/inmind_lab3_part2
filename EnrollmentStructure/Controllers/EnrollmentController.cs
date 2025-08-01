using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnrollmentStructure.Data;
using EnrollmentStructure.Entities;
using EnrollmentStructure.DTOs;
using EnrollmentStructure.Extensions;
using EnrollmentStructure.Tenant;

namespace EnrollmentStructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITenantProvider _tenantProvider;

    public EnrollmentController(AppDbContext context, ITenantProvider tenantProvider)
    {
        _context = context;
        _tenantProvider = tenantProvider;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetAll()
    {
        var enrollments = await _context.Enrollments
            .Select(e => new EnrollmentDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                TenantId = e.TenantId
            })
            .ToListAsync();

        return Ok(enrollments);
    }


   
    [HttpGet("{id}")]
    public async Task<ActionResult<EnrollmentDto>> Get(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment == null)
            return NotFound();

        return Ok(new EnrollmentDto
        {
            Id = enrollment.Id,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId
        });
    }

  
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnrollmentCreateDto dto)
    {
        var tenantId = _tenantProvider.GetTenantId();


        if (tenantId is null)
            return BadRequest("TenantId header is missing.");

        // Validate course and student belong to the same tenant
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == dto.StudentId && s.TenantId == tenantId);
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == dto.CourseId && c.TenantId == tenantId);

        if (student is null || course is null)
            return BadRequest("Student and Course must belong to the same tenant as the header.");

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            TenantId = tenantId
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        var response = new EnrollmentDto
        {
            Id = enrollment.Id,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            TenantId = tenantId
        };

        return CreatedAtAction(nameof(Get), new { id = enrollment.Id }, response);
    }


   
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EnrollmentDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch.");

        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
            return NotFound();

        enrollment.StudentId = dto.StudentId;
        enrollment.CourseId = dto.CourseId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
            return NotFound();

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
