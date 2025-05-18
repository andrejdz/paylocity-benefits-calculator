using Api.Domain;
using Api.Domain.Entities;
using Api.UseCases.Mappers;
using MediatR;

namespace Api.UseCases.Dependents.Queries.GetDependentById;

public class GetDependentByIdHandler : IRequestHandler<GetDependentByIdQuery, DependentResponse?>
{
    private readonly IDependentsRepository _repository;

    public GetDependentByIdHandler(IDependentsRepository repository)
    {
        _repository = repository;
    }

    public Task<DependentResponse?> Handle(
        GetDependentByIdQuery request,
        CancellationToken cancellationToken)
    {
        Dependent? dependent = _repository.GetDependentById(request.Id);
        if (dependent is null)
        {
            return Task.FromResult<DependentResponse?>(null);
        }

        return Task.FromResult<DependentResponse?>(
            dependent.ToDependentResponse());
    }
}