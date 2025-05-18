using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain;
using Api.Domain.Entities;
using Api.UseCases;
using Api.UseCases.Employees;
using Api.UseCases.Employees.Queries.GetEmployeeById;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Shouldly;
using Xunit;

namespace ApiTests.UnitTests.UseCases.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandlerTests
{
    private readonly IEmployeesRepository _repository = Substitute.For<IEmployeesRepository>();

    [Fact]
    public async Task Handle_ShouldReturnEmployee()
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
        _repository.GetEmployeeById(Arg.Any<int>())
            .Returns(employee);

        GetEmployeeByIdQueryHandler handler = new(_repository);
        GetEmployeeByIdQuery query = new()
        {
            Id = employee.Id,
        };

        // act
        EmployeeResponse? actualResponse = await handler.Handle(
            query,
            CancellationToken.None);

        // assert
        EmployeeResponse expectedResponse = new()
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

        actualResponse.ShouldBeEquivalentTo(expectedResponse);

        _repository.Received(Quantity.Exactly(1))
            .GetEmployeeById(employee.Id);
    }

    [Fact]
    public async Task Handle_EmployeeDoesNotExist_ShouldReturnNull()
    {
        // arrange
        _repository.GetEmployeeById(Arg.Any<int>())
            .Returns(default(Employee?));

        GetEmployeeByIdQueryHandler handler = new(_repository);
        GetEmployeeByIdQuery query = new()
        {
            Id = 1,
        };

        // act
        EmployeeResponse? actualResponse = await handler.Handle(
            query,
            CancellationToken.None);

        // assert
        actualResponse.ShouldBeNull();

        _repository.Received(Quantity.Exactly(1))
            .GetEmployeeById(query.Id);
    }
}