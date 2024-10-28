using Microsoft.VisualBasic;


namespace Natsu.Utils;

public class Validated<T> where T : class {
    public T? Value { get; set; } = null;
    public bool IsValid { get; set; } = false;

    public void Invalidate() {
        IsValid = false;
        if (Value is IDisposable disposable)
            disposable.Dispose();
    }

    public T Validate(T value) {
        Value = value;
        IsValid = true;
        return value;
    }
}