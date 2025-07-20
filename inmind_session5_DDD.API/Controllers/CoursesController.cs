using Application.Courses.Commands;
using Application.Courses.Queries;
using inmind_session5_DDD.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateCourse(CreateCourseCommand command)
    {
        var courseId = await _mediator.Send(command);
        return Ok(courseId);
    }

    [HttpGet]
    public async Task<ActionResult<List<CourseDto>>> GetAllCourses()
    {
        var result = await _mediator.Send(new GetAllCoursesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(int id)
    {
        var result = await _mediator.Send(new GetCourseByIdQuery { Id = id });
        return Ok(result);
    }
}