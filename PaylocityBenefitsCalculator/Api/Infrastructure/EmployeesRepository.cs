using Api.Domain;
using Api.Domain.Entities;

namespace Api.Infrastructure;

/// <summary>
/// Concrete implementation of the <see cref="IEmployeesRepository"/> interface.
/// For now, it uses a read-only in-memory collection to store employees and their dependents.
/// In a real application, this would be replaced with a database or other persistent storage.
/// </summary>
public class EmployeesRepository : IEmployeesRepository
{
    /// <inheritdoc/>
    public Employee? GetEmployeeById(int id) => MockData.Employees.FirstOrDefault(e => e.Id == id);

    /// <inheritdoc/>
    public List<Employee> GetEmployees() => MockData.Employees.OrderBy(e => e.Id).ToList();
}