using Api.Domain;
using Api.Domain.Entities;

namespace Api.Infrastructure;

/// <summary>
/// Concrete implementation of the <see cref="IDependentsRepository"/> interface.
/// For now, it uses a read-only in-memory collection to store employees and their dependents.
/// In a real application, this would be replaced with a database or other persistent storage.
/// </summary>
public class DependentsRepository : IDependentsRepository
{
    /// <inheritdoc/>
    public Dependent? GetDependentById(int id) =>
        MockData.Employees.SelectMany(e => e.Dependents)
            .FirstOrDefault(d => d.Id == id);

    /// <inheritdoc/>
    public List<Dependent> GetDependents() => MockData.Employees.SelectMany(e => e.Dependents)
        .OrderBy(d => d.Id)
        .ToList();
}