using Api.Domain;
using Api.UseCases.Employees.Queries.GetEmployees;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Xunit;
using Api.UseCases.Employees;
using Api.UseCases;

namespace ApiTests.UnitTests.UseCases.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandlerTests
{
    private readonly IEmployeesRepository _repository = Substitute.For<IEmployeesRepository>();

    [Fact]
    public async Task Handle_ShouldReturnCollectionOfEmployees()
    {
        // arrange
        List<Employee> employees = new()
        {
            new Employee
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
            },
            new Employee
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
            },
        };
        _repository.GetEmployees()
            .Returns(employees);

        GetEmployeesQueryHandler handler = new(_repository);
        GetEmployeesQuery query = new();

        // act
        List<EmployeeResponse> actualResult = await handler.Handle(
             query,
             CancellationToken.None);

        // assert
        List<EmployeeResponse> expectedResult = new()
        {
            new EmployeeResponse
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
            },
            new EmployeeResponse
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
            },
        };
        actualResult.ShouldBeEquivalentTo(expectedResult);

        _repository.ReceivedWithAnyArgs(Quantity.Exactly(1))
            .GetEmployees();
    }
}