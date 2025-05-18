using Api.Domain;
using Api.Domain.Entities;
using Api.UseCases.Mappers;
using MediatR;

namespace Api.UseCases.Dependents.Queries.GetDependents;

public class GetDependentsHandler : IRequestHandler<GetDependentsQuery, List<DependentResponse>>
{
    private readonly IDependentsRepository _repository;

    public GetDependentsHandler(IDependentsRepository repository)
    {
        _repository = repository;
    }

    public Task<List<DependentResponse>> Handle(
        GetDependentsQuery request,
        CancellationToken cancellationToken)
    {
        List<Dependent> dependents = _repository.GetDependents();

        return Task.FromResult(
            dependents.Select(d => d.ToDependentResponse())
                .ToList());
    }
}