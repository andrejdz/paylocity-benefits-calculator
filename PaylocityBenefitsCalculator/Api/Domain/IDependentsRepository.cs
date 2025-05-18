using Api.Domain.Entities;

namespace Api.Domain;

public interface IDependentsRepository
{
    /// <summary>
    /// Gets <see cref="Dependent"/> by its id.
    /// </summary>
    /// <param name="id">Id of a dependent.</param>
    /// <returns>The dependent or null if it is not found.</returns>
    Dependent? GetDependentById(int id);

    /// <summary>
    /// Gets collection of all dependents.
    /// </summary>
    /// <returns>Collection of all dependents.</returns>
    List<Dependent> GetDependents();
}