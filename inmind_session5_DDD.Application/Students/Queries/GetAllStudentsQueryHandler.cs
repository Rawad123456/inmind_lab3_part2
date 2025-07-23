using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Application.Students.Queries;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GetAllStudentsQueryHandler> _logger;

    public GetAllStudentsQueryHandler(
        ApplicationDbContext context,
        IMemoryCache cache,
        ILogger<GetAllStudentsQueryHandler> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        const string cacheKey = "all_students";

        if (_cache.TryGetValue(cacheKey, out List<StudentDto>? cachedStudents))
        {
            _logger.LogInformation("Returned students from in-memory cache.");
            return cachedStudents!;
        }

        _logger.LogInformation("Cache miss: retrieving students from the database.");
        
        var students = await _context.Students.ToListAsync(cancellationToken);

        var studentDtos = students.Select(s => new StudentDto
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email
        }).ToList();

       
        _cache.Set(cacheKey, studentDtos, TimeSpan.FromMinutes(5));

        return studentDtos;
    }
}