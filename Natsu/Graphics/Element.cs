using Natsu.Mathematics;
using Natsu.Platforms;
using Natsu.System;

namespace Natsu.Graphics;

public partial class Element : IDisposable {
    private Application? _app;
    private int _index;
    private string? _name;
    private Element? _parent;

    public bool DisposeChildren { get; set; } = true;

    public bool Active { get; set; } = true;
    public bool Visible { get; set; } = true;
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
            if (_index == value)
                return;

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
        if (DisposeChildren)
            Clear(true);
    }

    public bool Load() {
        if (Loaded)
            return false;

        OnLoad();
        DoLoad?.Invoke();
        Loaded = true;

        ForChildren(child => child.Load());

        return true;
    }

    public virtual void ClipCanvas(ICanvas canvas) {
        if (ClipRadius == Vector2.Zero) {
            canvas.ClipRect(new Rect(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipDifference, ClipAntiAlias);
            return;
        }

        canvas.ClipRoundRect(new Rect(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipRadius, ClipDifference, ClipAntiAlias);
    }

    public void Render(ICanvas canvas) {
        if (!Visible)
            return;

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
        if (!Active)
            return;

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
#nullable disable
    public Application App {
        get {
            if (_app != null)
                return _app;

            if (ContentParent != null) {
                App = ContentParent.App;
                return _app;
            }

            return null;
        }
        set => setApp(value);
    }

    private bool setApp(Application app, bool constructInputLists = false) {
        if (_app == app)
            return false;

        _app?.RemoveInputCandidate(this);

        Application old = _app;
        _app = app;

        ForChildren(child => child.setApp(app));

        DoAppChange?.Invoke(old);
        OnAppChange(old);

        return true;
    }
}
