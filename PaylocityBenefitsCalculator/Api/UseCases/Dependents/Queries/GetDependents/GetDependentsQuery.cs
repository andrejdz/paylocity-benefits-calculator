using MediatR;

namespace Api.UseCases.Dependents.Queries.GetDependents;

public class GetDependentsQuery : IRequest<List<DependentResponse>>
{
}