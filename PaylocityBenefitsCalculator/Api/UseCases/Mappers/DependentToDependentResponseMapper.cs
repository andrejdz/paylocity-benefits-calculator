using Api.Domain.Entities;

namespace Api.UseCases.Mappers;

public static class DependentToDependentResponseMapper
{
    public static DependentResponse ToDependentResponse(this Dependent dependent) =>
        new()
        {
            Id = dependent.Id,
            FirstName = dependent.FirstName,
            LastName = dependent.LastName,
            DateOfBirth = dependent.DateOfBirth,
            Relationship = dependent.Relationship,
        };
}