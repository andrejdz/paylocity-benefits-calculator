using MediatR;

namespace Api.UseCases.Dependents.Queries.GetDependentById;

public class GetDependentByIdQuery : IRequest<DependentResponse?>
{
    public int Id { get; init; }
}