using Microsoft.AspNetCore.Http;
using MediatR;

public class UploadStudentImageCommand : IRequest<string>
{
    public Guid StudentId { get; set; }
    public IFormFile ImageFile { get; set; }
}