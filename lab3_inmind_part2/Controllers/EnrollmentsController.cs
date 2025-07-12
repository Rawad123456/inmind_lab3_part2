using lab3_inmind_part2.Data;
using lab3_inmind_part2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab3_inmind_part2.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly UniversityDbContext _context;

    public EnrollmentsController(UniversityDbContext context)
    {
        _context = context;
    }

    
    [HttpPost]
    public async Task<IActionResult> Enroll(int studentId, int courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student == null || course == null)
            return NotFound("Student or Course not found.");

        var alreadyEnrolled = await _context.Enrollments
            .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        if (alreadyEnrolled)
            return BadRequest("Student already enrolled in this course.");

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok("Student enrolled successfully.");
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var enrollments = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ToListAsync();

        var result = enrollments.Select(e => new
        {
            e.Id,
            Student = e.Student.Name,
            Course = e.Course.Title
        });

        return Ok(result);
    }

   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEnrollment(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
            return NotFound();

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return Ok("Enrollment removed.");
    }
}