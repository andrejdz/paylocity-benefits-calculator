using Api.Domain.Entities;

namespace Api.Domain;

public interface IEmployeesRepository
{
    /// <summary>
    /// Gets <see cref="Employee"/> by its id.
    /// </summary>
    /// <param name="id">Id of an employee.</param>
    /// <returns>The employee or null if it is not found.</returns>
    Employee? GetEmployeeById(int id);

    /// <summary>
    /// Gets collection of all employees.
    /// </summary>
    /// <returns>Collection of all employees.</returns>
    List<Employee> GetEmployees();
}