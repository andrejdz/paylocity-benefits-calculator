namespace Api.Shared.Options;

public class EmployeePaycheckCalculatorOptions
{
    public int PaychecksPerYear { get; init; }

    public decimal BaseCostPerMonth { get; init; }

    public decimal SalaryThresholdPerYear { get; init; }

    public int ExtraCostOverSalaryThresholdPercentsPerYear { get; init; }

    public decimal DependentCostPerMonth { get; init; }

    public decimal ExtraDependentCostPerMonth { get; init; }

    public int DependentAgeThreshold { get; init; }
}