using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inmind_session5_DDD.Persistence;
using System.Net;

public class DownloadStudentImageQueryHandler : IRequestHandler<DownloadStudentImageQuery, FileStreamResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IBlobStorageService _blobStorageService;

    public DownloadStudentImageQueryHandler(ApplicationDbContext context, IBlobStorageService blobStorageService)
    {
        _context = context;
        _blobStorageService = blobStorageService;
    }

    public async Task<FileStreamResult> Handle(DownloadStudentImageQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null || string.IsNullOrEmpty(student.ProfileImageUrl))
            throw new Exception("Student or image not found.");

        
        var blobUri = new Uri(student.ProfileImageUrl);
        var blobName = Path.GetFileName(blobUri.LocalPath); 
        const string containerName = "students"; 

        var stream = await _blobStorageService.DownloadFileAsync(blobName, containerName);

        var contentType = GetContentType(blobName);

        return new FileStreamResult(stream, contentType)
        {
            FileDownloadName = blobName
        };
    }

    private string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}