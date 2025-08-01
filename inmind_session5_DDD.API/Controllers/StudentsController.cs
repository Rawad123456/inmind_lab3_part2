using inmind_session5_DDD.Application.Students.Commands;
using inmind_session5_DDD.Application.Students.Queries;
using Microsoft.AspNetCore.Authorization;

namespace inmind_session5_DDD.API.Controllers;


using MediatR;
using Microsoft.AspNetCore.Mvc;


[ApiController]
//[Authorize]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var result = await _mediator.Send(new GetAllStudentsQuery());
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var query = new GetStudentByIdQuery(id);
        var student = await _mediator.Send(query);
        return Ok(student);
    }
    
    [HttpPost("{id}/upload-image")]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile file)
    {
        var command = new UploadStudentImageCommand
        {
            StudentId = id,
            ImageFile = file
        };

        var result = await _mediator.Send(command);
        return Ok(new { imageUrl = result });
    }

    
    [HttpGet("{id}/download-image")]
    public async Task<IActionResult> DownloadImage(Guid id)
    {
        var query = new DownloadStudentImageQuery(id);
        var result = await _mediator.Send(query);
        return result;
    }




    
}
