using inmind_session5_DDD.Application.DTOs;

namespace inmind_session5_DDD.Application.Students.Queries;


using MediatR;
using System.Collections.Generic;



public class GetAllStudentsQuery : IRequest<List<StudentDto>> { }
