using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UploadStudentImageCommandHandler : IRequestHandler<UploadStudentImageCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IBlobStorageService _blobStorageService;

    public UploadStudentImageCommandHandler(ApplicationDbContext context, IBlobStorageService blobStorageService)
    {
        _context = context;
        _blobStorageService = blobStorageService;
    }

    public async Task<string> Handle(UploadStudentImageCommand request, CancellationToken cancellationToken)
    {
     
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            throw new Exception("Student not found.");

        
        using var stream = request.ImageFile.OpenReadStream();
        var fileName = $"{student.Id}_{request.ImageFile.FileName}";
        var imageUrl = await _blobStorageService.UploadFileAsync(stream, fileName, "students");

        
        student.ProfileImageUrl = imageUrl;

        
        _context.Students.Update(student); 
        await _context.SaveChangesAsync(cancellationToken);

        
        return imageUrl;
    }
}