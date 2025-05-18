using System;
using System.Collections.Generic;
using Api.Domain;
using Api.Domain.Entities;
using Api.UseCases;
using Api.UseCases.Employees;
using Api.UseCases.Mappers;
using Shouldly;
using Xunit;

namespace ApiTests.UnitTests.UseCases.Mappers;

public class EmployeeToEmployeeResponseMapperTests
{
    [Fact]
    public void ToEmployeeResponse_ShouldMapEmployeeToEmployeeResponse()
    {
        // arrange
        Employee employee = new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Salary = 50000m,
            DateOfBirth = new DateTime(1990, 1, 1),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Relationship = Relationship.Spouse,
                    DateOfBirth = new DateTime(1992, 2, 2),
                },
            },
        };

        // act
        EmployeeResponse actualResult = employee.ToEmployeeResponse();

        // assert
        EmployeeResponse expectedResult = new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Salary = 50000m,
            DateOfBirth = new DateTime(1990, 1, 1),
            Dependents = new List<DependentResponse>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Relationship = Relationship.Spouse,
                    DateOfBirth = new DateTime(1992, 2, 2),
                },
            },
        };
        actualResult.ShouldBeEquivalentTo(expectedResult);
    }
}