namespace Natsu.Utils;

public static class BindableExtensions {
    public static IBindable<X> Cast<T, X>(this IBindable<T> bindable) {
        if (bindable is CastBindable<T, X> castBindable) return castBindable;

        return new CastBindable<T, X>(bindable);
    }
}

public interface IBindable<T> {
    T Value { get; set; }
    bool Settable { get; }

    bool IsBound { get; }
    bool IsBoundTo { get; }
    event Action<T>? DoValueChanged;

    void BindTo(IBindable<T> other);
    void Unbind();

    IBindable<X> Cast<X>(IBindable<T> bindable) => bindable.Cast<T, X>();
    CastBindable<T, string> Stringify() => new(this);
}

public class CastBindable<T, X> : Bindable<X> {
    private readonly IBindable<T>? _binded;
    private X _value = default!;

    public CastBindable(IBindable<T> other) {
        _binded = other;

        other.DoValueChanged += v => {
            _value = Cast<T, X>(v);
            RaiseChangeEvent(_value);
        };

        _value = Cast<T, X>(other.Value);
    }

    public override X Value {
        get => _value;
        set { }
    }

    public override bool Settable => false;
    public override bool IsBound => true;
    public override bool IsBoundTo => _binded != null && _binded.IsBound;

    public static B Cast<A, B>(A value) {
        if (value is B bValue) return bValue;
        if (value is IConvertible convertible) return (B)Convert.ChangeType(convertible, typeof(B));

        throw new InvalidCastException($"Cannot cast {typeof(A)} to {typeof(B)}");
    }

    public override void BindTo(IBindable<X> other) => throw new NotImplementedException("Cannot bind a CastBindable");
    public override void Unbind() => throw new NotImplementedException("Cannot unbind a CastBindable");
}

public class Bindable<T> : IBindable<T> {
    private Action<T>? _boundHandler;
    private IBindable<T>? _boundTo;
    private T _value = default!;

    public event Action<T>? DoValueChanged;

    public virtual bool Settable { get; set; }

    public virtual bool IsBound => _boundTo != null;
    public virtual bool IsBoundTo => _boundTo != null && _boundTo.IsBound;

    public virtual T Value {
        get => _value;
        set {
            if (Settable || EqualityComparer<T>.Default.Equals(_value, value)) return;

            _value = value;
            DoValueChanged?.Invoke(_value);

            if (_boundTo != null && !_boundTo.Settable) _boundTo.Value = value;
        }
    }

    public virtual void BindTo(IBindable<T> other) {
        _boundTo = other;
        Value = other.Value;

        _boundHandler = v => {
            if (!Settable) Value = v;
        };

        other.DoValueChanged += _boundHandler;
    }

    public virtual void Unbind() {
        if (_boundTo != null && _boundHandler != null) {
            _boundTo.DoValueChanged -= _boundHandler;
            _boundHandler = null;
        }

        _boundTo = null;
    }

    internal void RaiseChangeEvent(T value) => DoValueChanged?.Invoke(value);

    #region Constructors
    public Bindable() { }

    public Bindable(T value) => _value = value;

    public Bindable(T value, Action<T>? onValueChanged) : this(value) => DoValueChanged += onValueChanged;

    public Bindable(T value, Action<T>? onValueChanged, bool disabled) : this(value, onValueChanged) => Settable = disabled;

    public Bindable(Bindable<T> bindTo) => BindTo(bindTo);
    #endregion

    #region Operators
    public static implicit operator T(Bindable<T> bindable) => bindable.Value;

    public static implicit operator Bindable<T>(T value) => new(value);

    public static Bindable<T> operator +(Bindable<T> bindable, T value) {
        bindable.Value = (dynamic)bindable.Value! + value;
        return bindable;
    }

    public static Bindable<T> operator -(Bindable<T> bindable, T value) {
        bindable.Value = (dynamic)bindable.Value! - value;
        return bindable;
    }

    public static Bindable<T> operator *(Bindable<T> bindable, T value) {
        bindable.Value = (dynamic)bindable.Value! * value;
        return bindable;
    }

    public static Bindable<T> operator /(Bindable<T> bindable, T value) {
        bindable.Value = (dynamic)bindable.Value! / value;
        return bindable;
    }

    public static Bindable<T> operator %(Bindable<T> bindable, T value) {
        bindable.Value = (dynamic)bindable.Value! % value;
        return bindable;
    }

    public static Bindable<T> operator ++(Bindable<T> bindable) {
        bindable.Value = (dynamic)bindable.Value! + 1;
        return bindable;
    }

    public static Bindable<T> operator --(Bindable<T> bindable) {
        bindable.Value = (dynamic)bindable.Value! - 1;
        return bindable;
    }
    #endregion
}
