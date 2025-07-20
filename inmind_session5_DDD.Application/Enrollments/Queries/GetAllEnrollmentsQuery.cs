
using inmind_session5_DDD.Application.DTOs;
using MediatR;

namespace Application.Enrollments.Queries;

public class GetAllEnrollmentsQuery : IRequest<List<EnrollmentDto>> { }