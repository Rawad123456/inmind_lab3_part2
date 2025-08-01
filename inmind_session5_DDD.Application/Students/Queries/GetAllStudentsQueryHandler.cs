using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Application.Students.Queries;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Common.Extensions;

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GetAllStudentsQueryHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllStudentsQueryHandler(
        ApplicationDbContext context,
        IMemoryCache cache,
        ILogger<GetAllStudentsQueryHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _httpContextAccessor.HttpContext?.GetTenantId();
        var cacheKey = string.IsNullOrWhiteSpace(tenantId) ? "all_students" : $"students_{tenantId}";

        if (_cache.TryGetValue(cacheKey, out List<StudentDto>? cachedStudents))
        {
            _logger.LogInformation("Returned students from in-memory cache.");
            return cachedStudents!;
        }

        _logger.LogInformation("Cache miss: retrieving students from the database.");

        var query = _context.Students.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            query = query.Where(s => s.TenantId == tenantId);
        }

        var students = await query.ToListAsync(cancellationToken);

        var studentDtos = students.Select(s => new StudentDto
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            TenantId = s.TenantId,
            ProfileImageUrl = s.ProfileImageUrl
        }).ToList();

        _cache.Set(cacheKey, studentDtos, TimeSpan.FromMinutes(5));

        return studentDtos;
    }
}
