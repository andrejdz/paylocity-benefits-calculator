using Api.Domain;
using Api.Domain.Entities;
using Api.UseCases.Mappers;
using MediatR;

namespace Api.UseCases.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeResponse>>
{
    private readonly IEmployeesRepository _repository;

    public GetEmployeesQueryHandler(IEmployeesRepository employeeRepository)
    {
        _repository = employeeRepository;
    }

    public Task<List<EmployeeResponse>> Handle(
        GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        List<Employee> employees = _repository.GetEmployees();
        return Task.FromResult(
            employees.Select(e => e.ToEmployeeResponse())
                .ToList());
    }
}