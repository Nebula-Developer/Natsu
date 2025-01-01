using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Native;

namespace Natsu.Core;

public partial class Element : IDisposable {
    private Application? _app;
    private int _index;
    private string? _name;

    public bool DisposeChildren { get; set; } = true;

    public bool Active { get; set; } = true;
    public bool Visible { get; set; } = true;
    public virtual bool BlockPositionalInput { get; set; }
    public bool Loaded { get; set; }

    public virtual string Name {
        get => _name ?? GetType().Name;
        set => _name = value;
    }

    public IRenderer Renderer => App.Renderer;
    public ResourceLoader ResourceLoader => App.ResourceLoader;
    public INativePlatform Platform => App.Platform;

    public int Index {
        get => _index;
        set {
            if (_index == value) return;

            _index = value;
            ContentParent?.SortChild(this);
        }
    }

    public bool Clip { get; set; } = false;
    public Vector2 ClipRadius { get; set; } = 0;
    public bool ClipDifference { get; set; } = false;
    public bool ClipAntiAlias { get; set; } = true;

    public void Dispose() {
        OnDispose();
        DoDispose?.Invoke();
        ContentParent?.Remove(this);
        if (DisposeChildren) Clear(true);
    }

    public bool Load() {
        if (Loaded) return false;

        OnLoad();
        DoLoad?.Invoke();
        Loaded = true;

        ForChildren(child => child.Load());

        return true;
    }

    public virtual void ClipCanvas(ICanvas canvas) {
        if (ClipRadius == Vector2.Zero) {
            canvas.ClipRect(new(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipDifference, ClipAntiAlias);
            return;
        }

        canvas.ClipRoundRect(new(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipRadius, ClipDifference, ClipAntiAlias);
    }

    public void Render(ICanvas canvas) {
        if (!Visible) return;

        canvas.SetMatrix(Matrix);
        int save = canvas.Save();
        if (Clip) ClipCanvas(canvas);

        OnRender(canvas);
        // Any bound rendering can happen here
        // eg:
        // canvas.DrawRect(new(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), new Paint() { Color = Colors.Magenta, IsStroke = true, StrokeWidth = 2 });
        OnRenderChildren(canvas);
        DoRender?.Invoke();

        canvas.Restore(save);
    }

    public void Update() {
        if (!Active) return;

        UpdateTransformSequences();

        OnUpdate();
        DoUpdate?.Invoke();
        OnUpdateChildren();
    }

    protected virtual void OnLoad() { }

    protected virtual void OnDispose() { }

    protected virtual void OnUpdate() { }

    protected virtual void OnUpdateChildren() => ForChildren(child => child.Update());

    protected virtual void OnRender(ICanvas canvas) { }

    protected virtual void OnRenderChildren(ICanvas canvas) => ForChildren(child => child.Render(canvas));

    protected virtual void OnAppChange(Application? old) { }

    public event Action? DoLoad;
    public event Action? DoDispose;

    public event Action? DoUpdate;
    public event Action? DoRender;

    public event Action<Application>? DoAppChange;

    #region Invalidation
    public InvalidationState InvalidationState { get; } = new();
    public Invalidation Invalidated => InvalidationState.State;

    public virtual bool OnInvalidate(Invalidation invalidation, InvalidationPropagation propagation) {
        if (invalidation.HasFlag(Invalidation.DrawSize) && Parent?.ChildRelativeSizeAxes != Axes.None) InvalidateParent(Invalidation.DrawSize);

        return false;
    }

    public virtual bool OnValidate(Invalidation invalidation, InvalidationPropagation propagation) => false;

    public bool Invalidate(Invalidation invalidation, InvalidationPropagation propagation = InvalidationPropagation.None) {
        if (invalidation == Invalidation.None) return false;

        if (OnInvalidate(invalidation, propagation)) return true;

        InvalidationState.Invalidate(invalidation);

        if (propagation.HasFlag(InvalidationPropagation.Parent)) InvalidateParent(invalidation, true);

        if (propagation.HasFlag(InvalidationPropagation.Children)) InvalidateChildren(invalidation, true);

        return true;
    }

    public bool Validate(Invalidation invalidation, InvalidationPropagation propagation = InvalidationPropagation.None) {
        if (invalidation == Invalidation.None) return false;

        if (OnValidate(invalidation, propagation)) return true;

        InvalidationState.Validate(invalidation);

        if (propagation.HasFlag(InvalidationPropagation.Parent)) ValidateParent(invalidation, true);

        if (propagation.HasFlag(InvalidationPropagation.Children)) ValidateChildren(invalidation, true);

        return true;
    }

    public bool InvalidateParent(Invalidation invalidation, bool propagate = false) => Parent?.Invalidate(invalidation, propagate ? InvalidationPropagation.Parent : InvalidationPropagation.None) ?? false;

    public bool ValidateParent(Invalidation invalidation, bool propagate = false) => Parent?.Validate(invalidation, propagate ? InvalidationPropagation.Parent : InvalidationPropagation.None) ?? false;

    public bool InvalidateChildren(Invalidation invalidation, bool propagate = false) {
        bool result = false;
        ForChildren(child => result |= child.Invalidate(invalidation, propagate ? InvalidationPropagation.Children : InvalidationPropagation.None));
        return result;
    }

    public bool ValidateChildren(Invalidation invalidation, bool propagate = false) {
        bool result = false;
        ForChildren(child => result |= child.Validate(invalidation, propagate ? InvalidationPropagation.Children : InvalidationPropagation.None));
        return result;
    }
    #endregion

#nullable disable
    public Application App {
        get {
            if (_app != null) return _app;

            if (ContentParent != null) {
                App = ContentParent.App;
                return _app;
            }

            return null;
        }
        set => setApp(value);
    }

    private bool setApp(Application app, bool constructInputLists = false) {
        if (_app == app) return false;

        _app?.RemoveInputCandidate(this);

        Application old = _app;
        _app = app;

        ForChildren(child => child.setApp(app));

        DoAppChange?.Invoke(old);
        OnAppChange(old);

        return true;
    }

    public override string ToString() => $"{Name} ({GetType().Name})";
}
