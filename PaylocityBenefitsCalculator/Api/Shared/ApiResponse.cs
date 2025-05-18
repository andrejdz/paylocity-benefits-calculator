namespace Api.Shared;

/// <summary>
/// This must be shipped as a nuget package,
/// because I assume that it is used across all components.
/// </summary>
public class ApiResponse<T>
{
    public T? Data { get; init; }

    public bool Success { get; init; } = true;

    public string? Message { get; init; }

    public string? Error { get; init; }
}
