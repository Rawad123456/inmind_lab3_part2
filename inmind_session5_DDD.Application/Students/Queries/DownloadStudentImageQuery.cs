using MediatR;
using Microsoft.AspNetCore.Mvc;

public class DownloadStudentImageQuery : IRequest<FileStreamResult>
{
    public Guid StudentId { get; set; }

    public DownloadStudentImageQuery(Guid studentId)
    {
        StudentId = studentId;
    }
}