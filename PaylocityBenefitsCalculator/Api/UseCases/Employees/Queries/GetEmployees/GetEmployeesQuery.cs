using MediatR;

namespace Api.UseCases.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : IRequest<List<EmployeeResponse>>
{
}