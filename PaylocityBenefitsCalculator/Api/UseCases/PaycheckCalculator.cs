using Api.Domain;
using Api.Domain.Entities;
using Api.Shared.Options;
using Api.Shared.Utilities;
using Microsoft.Extensions.Options;

namespace Api.UseCases;

/// <summary>
/// Calculator must be called every time when paycheck calculation is needed.
/// We cannot calculate paycheck for the whole year, because salary, number of dependents,
/// base parameters might change during the current year.
/// Paycheck history must be stored somewhere separately.
/// </summary>
public class PaycheckCalculator : IPaycheckCalculator
{
    private const int Months = 12;

    private readonly IClockService _clockService;

    private readonly EmployeePaycheckCalculatorOptions _options;

    public PaycheckCalculator(
        IOptions<EmployeePaycheckCalculatorOptions> options,
        IClockService clockService)
    {
        _clockService = clockService;
        _options = options.Value;
    }

    /// <inheritdoc />
    public decimal CalculateFor(Employee employee) =>
        (employee.Salary - CalculateCostPerYear(employee)) / _options.PaychecksPerYear;

    private decimal CalculateCostPerYear(Employee employee)
    {
        decimal costPerYear = (_options.BaseCostPerMonth
                + CalculateDependentsCostPerMonth(employee.Dependents))
            * Months;
        costPerYear += CalculateExtraCostOverSalaryThresholdPerYear(employee.Salary);

        return costPerYear;
    }

    private decimal CalculateDependentsCostPerMonth(ICollection<Dependent> dependents)
    {
        if (dependents.Count == 0)
        {
            return 0m;
        }

        int numberOfDependentsOver50 = GetNumberOfDependentsOver50(dependents);

        return dependents.Count * _options.DependentCostPerMonth
               + numberOfDependentsOver50 * _options.ExtraDependentCostPerMonth;
    }

    private int GetNumberOfDependentsOver50(ICollection<Dependent> dependents)
    {
        DateTime currentUtcDate = _clockService.GetCurrentUtcDate();
        return dependents.Count(
            d => currentUtcDate >= d.DateOfBirth.Date.AddYears(_options.DependentAgeThreshold));
    }

    private decimal CalculateExtraCostOverSalaryThresholdPerYear(decimal salary)
    {
        if (!IsSalaryOverThreshold(salary))
        {
            return 0m;
        }

        return salary * _options.ExtraCostOverSalaryThresholdPercentsPerYear / 100;
    }

    private bool IsSalaryOverThreshold(decimal salary)
        => salary >= _options.SalaryThresholdPerYear;
}