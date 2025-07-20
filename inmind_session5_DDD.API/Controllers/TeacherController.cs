using Application.Teachers.Commands;
using Application.Teachers.Queries;
using inmind_session5_DDD.Application.DTOs;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeacherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<TeacherDto>> Create(CreateTeacherCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<TeacherDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllTeachersQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetTeacherByIdQuery { Id = id });
        return Ok(result);
    }
}