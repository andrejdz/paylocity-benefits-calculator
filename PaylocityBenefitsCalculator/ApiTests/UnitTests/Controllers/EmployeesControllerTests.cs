using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Domain;
using Api.Shared;
using Api.UseCases;
using Api.UseCases.Employees;
using Api.UseCases.Employees.Queries.GetEmployeeById;
using Api.UseCases.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace ApiTests.UnitTests.Controllers;

public class EmployeesControllerTests
{
    private readonly ISender _sender = Substitute.For<ISender>();

    [Fact]
    public async Task Get_ShouldReturn200HttpStatusCodeWithEmployeeData()
    {
        // arrange
        EmployeeResponse response = new()
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
        _sender.Send(
                Arg.Any<GetEmployeeByIdQuery>(),
                Arg.Any<CancellationToken>())
            .Returns(response);

        EmployeesController controller = new(_sender);
        const int employeeId = 1;

        // act
        ActionResult<ApiResponse<EmployeeResponse>> actualResult =
            await controller.Get(employeeId);

        // arrange
        ApiResponse<EmployeeResponse> actualApiResponse = actualResult.Value.ShouldNotBeNull();

        ApiResponse<EmployeeResponse> expectedApiResponse = new()
        {
            Data = response,
            Success = true,
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public async Task Get_EmployeeDoesNotExist_ShouldReturn404HttpStatusCode()
    {
        // arrange
        _sender.Send(
                Arg.Any<GetEmployeeByIdQuery>(),
                Arg.Any<CancellationToken>())
            .Returns(default(EmployeeResponse?));

        EmployeesController controller = new(_sender);
        const int employeeId = 1;

        // act
        ActionResult<ApiResponse<EmployeeResponse>> actualResult =
            await controller.Get(employeeId);

        // arrange
        NotFoundObjectResult notFoundObjectResult = actualResult.Result.ShouldBeOfType<NotFoundObjectResult>();
        ApiResponse<EmployeeResponse> actualApiResponse =
            notFoundObjectResult.Value.ShouldBeOfType<ApiResponse<EmployeeResponse>>();

        ApiResponse<EmployeeResponse> expectedApiResponse = new()
        {
            Success = false,
            Error = $"Employee with id {employeeId} was not found.",
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public async Task Get_ExceptionWasThrown_ShouldReturn500HttpStatusCode()
    {
        // arrange
        const string errorMessage = "Test error message.";
        _sender.Send(
                Arg.Any<GetEmployeeByIdQuery>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception(errorMessage));

        EmployeesController controller = new(_sender);
        const int employeeId = 1;

        // act
        ActionResult<ApiResponse<EmployeeResponse>> actualResult =
            await controller.Get(employeeId);

        // arrange
        ObjectResult objectResult = actualResult.Result.ShouldBeOfType<ObjectResult>();
        ApiResponse<EmployeeResponse> actualApiResponse =
            objectResult.Value.ShouldBeOfType<ApiResponse<EmployeeResponse>>();

        ApiResponse<EmployeeResponse> expectedApiResponse = new()
        {
            Success = false,
            Error = errorMessage,
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public async Task GetAll_ShouldReturn200HttpStatusCodeWithListOfEmployees()
    {
        // arrange
        List<EmployeeResponse> response = new()
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
        _sender.Send(
                Arg.Any<GetEmployeesQuery>(),
                Arg.Any<CancellationToken>())
            .Returns(response);

        EmployeesController controller = new(_sender);

        // act
        ActionResult<ApiResponse<List<EmployeeResponse>>> actualResult =
            await controller.GetAll();

        // arrange
        ApiResponse<List<EmployeeResponse>> actualApiResponse =
            actualResult.Value.ShouldNotBeNull();

        ApiResponse<List<EmployeeResponse>> expectedApiResponse = new()
        {
            Data = response,
            Success = true,
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public async Task GetAll_ExceptionWasThrown_ShouldReturn500HttpStatusCode()
    {
        // arrange
        const string errorMessage = "Test error message.";
        _sender.Send(
                Arg.Any<GetEmployeesQuery>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception(errorMessage));

        EmployeesController controller = new(_sender);

        // act
        ActionResult<ApiResponse<List<EmployeeResponse>>> actualResult =
            await controller.GetAll();

        // arrange
        ObjectResult objectResult = actualResult.Result.ShouldBeOfType<ObjectResult>();
        ApiResponse<List<EmployeeResponse>> actualApiResponse =
            objectResult.Value.ShouldBeOfType<ApiResponse<List<EmployeeResponse>>>();

        ApiResponse<List<EmployeeResponse>> expectedApiResponse = new()
        {
            Success = false,
            Error = errorMessage,
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }
}