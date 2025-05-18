namespace Api.UseCases.Employees;

public class EmployeeResponse
{
    public int Id { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public decimal Salary { get; init; }

    public DateTime DateOfBirth { get; init; }

    public ICollection<DependentResponse> Dependents { get; init; } = new List<DependentResponse>();
}
