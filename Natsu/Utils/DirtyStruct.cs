namespace Natsu.Utils;

public class DirtyStruct<T> where T : struct {
    public T Value;

    public bool IsDirty { get; private set; }

    public void Invalidate() => IsDirty = true;

    public T Validate(T t) {
        Value = t;
        IsDirty = false;
        return t;
    }
}
