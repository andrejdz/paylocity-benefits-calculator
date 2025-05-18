using Api.Domain;
using Api.Domain.Entities;
using Api.UseCases.Mappers;
using MediatR;

namespace Api.UseCases.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponse?>
{
    private readonly IEmployeesRepository _repository;

    public GetEmployeeByIdQueryHandler(IEmployeesRepository repository)
    {
        _repository = repository;
    }

    public Task<EmployeeResponse?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        Employee? employee = _repository.GetEmployeeById(request.Id);
        if (employee is null)
        {
            return Task.FromResult<EmployeeResponse?>(null);
        }

        return Task.FromResult<EmployeeResponse?>(
            employee.ToEmployeeResponse());
    }
}