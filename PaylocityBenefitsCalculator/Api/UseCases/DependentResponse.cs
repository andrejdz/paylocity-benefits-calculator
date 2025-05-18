using Api.Domain;

namespace Api.UseCases;

public class DependentResponse
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public Relationship Relationship { get; set; }
}
