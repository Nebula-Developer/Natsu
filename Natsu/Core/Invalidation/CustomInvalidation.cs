#nullable disable

namespace Natsu.Core.InvalidationTemp;

/// <summary>
///     A class that holds custom invalidation based on strings rather than enums.
///     <br />
///     This is useful for elements that need to invalidate based on custom logic.
/// </summary>
public class CustomInvalidation {
    private readonly HashSet<string> _invalidations = new();

    public void Invalidate(string invalidation) => _invalidations.Add(invalidation);

    public void Validate(string invalidation) => _invalidations.Remove(invalidation);

    public bool HasFlag(string invalidation) => _invalidations.Contains(invalidation);

    public void Clear() => _invalidations.Clear();

    public IReadOnlyCollection<string> GetInvalidations() => _invalidations.ToList().AsReadOnly();
}
