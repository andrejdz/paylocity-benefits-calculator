using Api.Domain;
using Api.Domain.Entities;
using Api.UseCases;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.UseCases.Dependents.Queries.GetDependents;
using Xunit;

namespace ApiTests.UnitTests.UseCases.Dependents.Queries.GetDependents;

public class GetDependentsHandlerTests
{
    private readonly IDependentsRepository _repository = Substitute.For<IDependentsRepository>();

    [Fact]
    public async Task Handle_ShouldReturnCollectionOfDependents()
    {
        // arrange
        List<Dependent> dependents = new()
        {
            new Dependent
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1992, 2, 2),
            },
            new Dependent
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2),
            },
        };
        _repository.GetDependents()
            .Returns(dependents);

        GetDependentsHandler handler = new(_repository);
        GetDependentsQuery query = new();

        // act
        List<DependentResponse> actualResult = await handler.Handle(
             query,
             CancellationToken.None);

        // assert
        List<DependentResponse> expectedResult = new()
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
        actualResult.ShouldBeEquivalentTo(expectedResult);

        _repository.ReceivedWithAnyArgs(Quantity.Exactly(1))
            .GetDependents();
    }
}