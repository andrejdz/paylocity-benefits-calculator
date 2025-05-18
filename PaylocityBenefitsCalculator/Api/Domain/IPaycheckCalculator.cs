using Api.Domain.Entities;

namespace Api.Domain;

public interface IPaycheckCalculator
{
    /// <summary>
    /// Calculates paycheck for a specific employee,
    /// based on the information it holds.
    /// </summary>
    /// <param name="employee">Employee for which a paycheck will be calculated.</param>
    /// <returns>Calculated paycheck.</returns>
    decimal CalculateFor(Employee employee);
}