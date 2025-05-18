using Api.Domain.Entities;
using Api.UseCases.Employees;

namespace Api.UseCases.Mappers;

public static class EmployeeToEmployeeResponseMapper
{
    public static EmployeeResponse ToEmployeeResponse(this Employee employee) =>
        new()
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary,
            DateOfBirth = employee.DateOfBirth,
            Dependents = employee.Dependents.Select(
                    d => d.ToDependentResponse())
                .ToList(),
        };
}