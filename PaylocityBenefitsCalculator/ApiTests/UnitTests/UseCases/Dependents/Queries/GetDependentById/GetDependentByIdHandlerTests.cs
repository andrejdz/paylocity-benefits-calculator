using Api.Domain;
using Api.Domain.Entities;
using Api.UseCases;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.UseCases.Dependents.Queries.GetDependentById;
using Xunit;

namespace ApiTests.UnitTests.UseCases.Dependents.Queries.GetDependentById;

public class GetDependentByIdHandlerTests
{
    private readonly IDependentsRepository _repository = Substitute.For<IDependentsRepository>();

    [Fact]
    public async Task Handle_ShouldReturnDependent()
    {
        // arrange
        Dependent dependent = new()
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1992, 2, 2),
        };
        _repository.GetDependentById(Arg.Any<int>())
            .Returns(dependent);

        GetDependentByIdHandler handler = new(_repository);
        GetDependentByIdQuery query = new()
        {
            Id = dependent.Id,
        };

        // act
        DependentResponse? actualResponse = await handler.Handle(
            query,
            CancellationToken.None);

        // assert
        DependentResponse expectedResponse = new()
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1992, 2, 2),
        };

        actualResponse.ShouldBeEquivalentTo(expectedResponse);

        _repository.Received(Quantity.Exactly(1))
            .GetDependentById(dependent.Id);
    }

    [Fact]
    public async Task Handle_DependentDoesNotExist_ShouldReturnNull()
    {
        // arrange
        _repository.GetDependentById(Arg.Any<int>())
            .Returns(default(Dependent?));

        GetDependentByIdHandler handler = new(_repository);
        GetDependentByIdQuery query = new()
        {
            Id = 1,
        };

        // act
        DependentResponse? actualResponse = await handler.Handle(
            query,
            CancellationToken.None);

        // assert
        actualResponse.ShouldBeNull();

        _repository.Received(Quantity.Exactly(1))
            .GetDependentById(query.Id);
    }
}