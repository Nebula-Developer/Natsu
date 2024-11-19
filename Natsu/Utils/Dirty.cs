namespace Natsu.Utils;

public class Dirty {
    public bool IsDirty { get; private set; }

    public void Invalidate() => IsDirty = true;

    public T Validate<T>(T t) {
        // For chaining
        IsDirty = false;
        return t;
    }

    public void Validate() => IsDirty = false;
}