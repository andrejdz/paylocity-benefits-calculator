using MediatR;

namespace Api.UseCases.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQuery : IRequest<EmployeeResponse?>
{
    public int Id { get; init; }
}