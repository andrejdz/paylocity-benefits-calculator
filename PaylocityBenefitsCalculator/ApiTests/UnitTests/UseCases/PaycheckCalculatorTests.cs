using Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using Api.Domain;
using Api.Shared.Options;
using Api.Shared.Utilities;
using Api.UseCases;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ApiTests.UnitTests.UseCases;

public class PaycheckCalculatorTests
{
    private readonly IOptions<EmployeePaycheckCalculatorOptions> _options =
        Substitute.For<IOptions<EmployeePaycheckCalculatorOptions>>();

    public PaycheckCalculatorTests()
    {
        EmployeePaycheckCalculatorOptions options = new()
        {
            PaychecksPerYear = 26,
            BaseCostPerMonth = 1000m,
            SalaryThresholdPerYear = 80_000m,
            ExtraCostOverSalaryThresholdPercentsPerYear = 2,
            DependentCostPerMonth = 600m,
            ExtraDependentCostPerMonth = 200m,
            DependentAgeThreshold = 50,
        };
        _options.Value.Returns(options);
    }

    [Fact]
    public void Calculate_EmployeeWithoutDependentsAndBelowThreshold_ShouldCalculatePaycheck()
    {
        // arrange
        IClockService clockService = Substitute.For<IClockService>();
        clockService.GetCurrentUtcDate()
            .Returns(DateTime.UtcNow.Date);

        PaycheckCalculator calculator = new(
            _options,
            clockService);

        Employee employee = CreateEmployee(salary: 79_999.999m);

        // act
        decimal actualResult = calculator.CalculateFor(employee);

        // assert
        const decimal expectedResult = 2615.384m;
        actualResult.ShouldBe(expectedResult, tolerance: 0.001m);
    }

    [Theory]
    [InlineData(80_000, 2553.846)]
    [InlineData(85_000, 2742.308)]
    public void Calculate_EmployeeWithoutDependentsAndOverThreshold_ShouldCalculatePaycheck(
        decimal salary,
        decimal expectedPaycheckValue)
    {
        // arrange
        IClockService clockService = Substitute.For<IClockService>();
        clockService.GetCurrentUtcDate()
            .Returns(DateTime.UtcNow.Date);

        PaycheckCalculator calculator = new(
            _options,
            clockService);

        Employee employee = CreateEmployee(salary: salary);

        // act
        decimal actualResult = calculator.CalculateFor(employee);

        // assert
        actualResult.ShouldBe(expectedPaycheckValue, tolerance: 0.001m);
    }

    [Fact]
    public void Calculate_EmployeeWithDependentsBelow50_ShouldCalculatePaycheck()
    {
        // arrange
        IClockService clockService = Substitute.For<IClockService>();
        clockService.GetCurrentUtcDate()
            .Returns(ParseToUtcDate("2025-04-01"));

        PaycheckCalculator calculator = new(
            _options,
            clockService);

        List<Dependent> dependents = new()
        {
            new Dependent
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = ParseToUtcDate("1975-04-02"),
            },
            new Dependent
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = ParseToUtcDate("2010-06-23"),
            },
        };
        Employee employee = CreateEmployee(dependents: dependents);

        // act
        decimal actualResult = calculator.CalculateFor(employee);

        // assert
        const decimal expectedResult = 1869.230m;
        actualResult.ShouldBe(expectedResult, tolerance: 0.001m);
    }

    [Theory]
    [InlineData("1975-04-01")]
    [InlineData("1975-03-31")]
    public void Calculate_EmployeeWithDependentsOver50_ShouldCalculatePaycheck(string spouseDateOfBirth)
    {
        // arrange
        IClockService clockService = Substitute.For<IClockService>();
        clockService.GetCurrentUtcDate()
            .Returns(ParseToUtcDate("2025-04-01"));

        PaycheckCalculator calculator = new(
            _options,
            clockService);

        List<Dependent> dependents = new()
        {
            new Dependent
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = ParseToUtcDate(spouseDateOfBirth),
            },
            new Dependent
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = ParseToUtcDate("2010-06-23"),
            },
        };
        Employee employee = CreateEmployee(dependents: dependents);

        // act
        decimal actualResult = calculator.CalculateFor(employee);

        // assert
        const decimal expectedResult = 1776.923m;
        actualResult.ShouldBe(expectedResult, tolerance: 0.001m);
    }

    private static Employee CreateEmployee(
        decimal salary = 75_000m,
        List<Dependent>? dependents = null)
    {
        Employee employee = new()
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = salary,
            DateOfBirth = ParseToUtcDate("1974-12-30"),
            Dependents = dependents ?? new List<Dependent>()
        };
        return employee;
    }

    private static DateTime ParseToUtcDate(string date)
        => DateTime.Parse(
                date,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal)
            .Date;
}