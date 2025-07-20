using Application.Enrollments.Commands;
using Application.Enrollments.Queries;
using inmind_session5_DDD.Application.DTOs;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace lab3_inmind_part2.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnrollmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEnrollmentCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id });
    }

    [HttpGet]
    public async Task<ActionResult<List<EnrollmentDto>>> GetAll()
    {
        var enrollments = await _mediator.Send(new GetAllEnrollmentsQuery());
        return Ok(enrollments);
    }
}