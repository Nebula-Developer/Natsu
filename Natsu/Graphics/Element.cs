using Natsu.Mathematics;

namespace Natsu.Graphics;

public partial class Element : IDisposable {
    private Application? _app;
    private int _index;
    private string? _name;
    private Element? _parent;
    private bool _loaded;

    public bool DisposeChildren { get; set; } = true;

    public virtual string Name {
        get => _name ?? GetType().Name;
        set => _name = value;
    }

    public Application App {
        get {
            if (_app != null)
                return _app;

            if (Parent != null) {
                App = Parent.App;
                return _app!;
            }

            throw new InvalidOperationException("Element is not attached to an application");
        }
        set {
            ForChildren(child => child.App = value);
            if (_app == value)
                return;
            
            _app?.RemoveInputCandidate(this);

            _app = value;
            App.ConstructInputLists();
        }
    }

    public IRenderer Renderer => App.Renderer;
    public ResourceLoader ResourceLoader => App.ResourceLoader;

    public int Index {
        get => _index;
        set {
            if (_index == value)
                return;

            _index = value;
            Parent?.SortChild(this);
        }
    }

    public void Dispose() {
        OnDispose();
        Disposed?.Invoke();
        Parent?.Remove(this);
        if (DisposeChildren)
            Clear(true);
    }

    public void Load() {
        if (_loaded)
            return;

        OnLoad();
        Loaded?.Invoke();
        _loaded = true;
    }
    
    public bool Clip { get; set; } = false;
    public float ClipRadius { get; set; } = 0;
    public bool ClipDifference { get; set; } = false;
    public bool ClipAntiAlias { get; set; } = true;

    public virtual void ClipCanvas(ICanvas canvas) {
        if (ClipRadius == 0) {
            canvas.ClipRect(new Rect(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipDifference, ClipAntiAlias);
            return;
        }

        canvas.ClipRoundRect(new Rect(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipRadius, ClipDifference, ClipAntiAlias);
    }

    public void Render(ICanvas canvas) {
        canvas.SetMatrix(Matrix);
        int save = canvas.Save();
        if (Clip) {
            ClipCanvas(canvas);
        }

        OnRender(canvas);
        // Any bound rendering can happen here
        // eg:
        // canvas.DrawRect(new(0, 0, MathF.Round(Size.X), MathF.Round(Size.Y)), new Paint() { Color = Colors.Magenta, IsStroke = true, StrokeWidth = 2 });
        OnRenderChildren(canvas);
        Rendered?.Invoke();

        canvas.Restore(save);
    }

    public void Update() {
        OnUpdate();
        Updated?.Invoke();
        OnUpdateChildren();
    }
    
    public virtual void OnLoad() { }
    public virtual void OnDispose() { }

    public virtual void OnUpdate() { }
    public virtual void OnUpdateChildren() => ForChildren(child => child.Update());

    public virtual void OnRender(ICanvas canvas) { }
    public virtual void OnRenderChildren(ICanvas canvas) => ForChildren(child => child.Render(canvas));

    public event Action? Loaded;
    public event Action? Disposed;

    public event Action? Updated;
    public event Action? Rendered;
}
