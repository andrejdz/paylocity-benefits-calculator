using Api.Controllers;
using Api.Domain;
using Api.Shared;
using Api.UseCases;
using Api.UseCases.Dependents.Queries.GetDependents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.UseCases.Dependents.Queries.GetDependentById;
using Xunit;

namespace ApiTests.UnitTests.Controllers;

public class DependentsControllerTests
{
    private readonly ISender _sender = Substitute.For<ISender>();

    [Fact]
    public async Task GetAll_ShouldReturn200HttpStatusCodeWithListOfDependents()
    {
        // arrange
        List<DependentResponse> response = new()
        {
            new DependentResponse
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1992, 2, 2),
            },
            new DependentResponse
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2),
            },
        };
        _sender.Send(
                Arg.Any<GetDependentsQuery>(),
                Arg.Any<CancellationToken>())
            .Returns(response);

        DependentsController controller = new(_sender);

        // act
        ActionResult<ApiResponse<List<DependentResponse>>> actualResult =
            await controller.GetAll();

        // arrange
        ApiResponse<List<DependentResponse>> actualApiResponse =
            actualResult.Value.ShouldNotBeNull();

        ApiResponse<List<DependentResponse>> expectedApiResponse = new()
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
                Arg.Any<GetDependentsQuery>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception(errorMessage));

        DependentsController controller = new(_sender);

        // act
        ActionResult<ApiResponse<List<DependentResponse>>> actualResult =
            await controller.GetAll();

        // arrange
        ObjectResult objectResult = actualResult.Result.ShouldBeOfType<ObjectResult>();
        ApiResponse<List<DependentResponse>> actualApiResponse =
            objectResult.Value.ShouldBeOfType<ApiResponse<List<DependentResponse>>>();

        ApiResponse<List<DependentResponse>> expectedApiResponse = new()
        {
            Success = false,
            Error = errorMessage,
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public async Task Get_ShouldReturn200HttpStatusCodeWithDependentData()
    {
        // arrange
        DependentResponse response = new()
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1992, 2, 2),
        };
        _sender.Send(
                Arg.Any<GetDependentByIdQuery>(),
                Arg.Any<CancellationToken>())
            .Returns(response);

        DependentsController controller = new(_sender);
        const int dependentId = 1;

        // act
        ActionResult<ApiResponse<DependentResponse>> actualResult =
            await controller.Get(dependentId);

        // arrange
        ApiResponse<DependentResponse> actualApiResponse = actualResult.Value.ShouldNotBeNull();

        ApiResponse<DependentResponse> expectedApiResponse = new()
        {
            Data = response,
            Success = true,
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public async Task Get_DependentDoesNotExist_ShouldReturn404HttpStatusCode()
    {
        // arrange
        _sender.Send(
                Arg.Any<GetDependentByIdQuery>(),
                Arg.Any<CancellationToken>())
            .Returns(default(DependentResponse?));

        DependentsController controller = new(_sender);
        const int dependentId = 1;

        // act
        ActionResult<ApiResponse<DependentResponse>> actualResult =
            await controller.Get(dependentId);

        // arrange
        NotFoundObjectResult notFoundObjectResult = actualResult.Result.ShouldBeOfType<NotFoundObjectResult>();
        ApiResponse<DependentResponse> actualApiResponse =
            notFoundObjectResult.Value.ShouldBeOfType<ApiResponse<DependentResponse>>();

        ApiResponse<DependentResponse> expectedApiResponse = new()
        {
            Success = false,
            Error = $"Dependent with id {dependentId} was not found.",
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public async Task Get_ExceptionWasThrown_ShouldReturn500HttpStatusCode()
    {
        // arrange
        const string errorMessage = "Test error message.";
        _sender.Send(
                Arg.Any<GetDependentByIdQuery>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception(errorMessage));

        DependentsController controller = new(_sender);
        const int dependentId = 1;

        // act
        ActionResult<ApiResponse<DependentResponse>> actualResult =
            await controller.Get(dependentId);

        // arrange
        ObjectResult objectResult = actualResult.Result.ShouldBeOfType<ObjectResult>();
        ApiResponse<DependentResponse> actualApiResponse =
            objectResult.Value.ShouldBeOfType<ApiResponse<DependentResponse>>();

        ApiResponse<DependentResponse> expectedApiResponse = new()
        {
            Success = false,
            Error = errorMessage,
        };
        actualApiResponse.ShouldBeEquivalentTo(expectedApiResponse);
    }
}