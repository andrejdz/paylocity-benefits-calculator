using Api.Domain;
using Api.Domain.Entities;
using System;
using Api.UseCases.Mappers;
using Shouldly;
using Xunit;
using Api.UseCases;

namespace ApiTests.UnitTests.UseCases.Mappers;

public class DependentToDependentResponseMapperTests
{
    [Fact]
    public void ToDependentResponse_ShouldMapDependentToDependentResponse()
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

        // act
        DependentResponse actualResult = dependent.ToDependentResponse();

        // assert
        DependentResponse expectedResult = new()
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1992, 2, 2),
        };
        actualResult.ShouldBeEquivalentTo(expectedResult);
    }
}