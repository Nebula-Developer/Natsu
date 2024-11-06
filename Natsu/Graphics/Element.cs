using System.Data;

using Natsu.Mathematics;

using SkiaSharp;

namespace Natsu.Graphics;

public partial class Element : IDisposable {
    public virtual string Name {
        get => _name ?? GetType().Name;
        set => _name = value;
    }
    private string? _name;

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
            _app = value;
            App.ConstructInputTree();
        }
    }
    private Application? _app;

    public IRenderer Renderer => App.Renderer;
    public ResourceLoader ResourceLoader => App.ResourceLoader;


    public Element? Parent {
        get => _parent;
        set {
            if (Parent?.HasChild(this) == true)
                Parent.Remove(this);
            _parent = value;
            if (Parent?.HasChild(this) == false)
                Parent.Add(this);
                
            if (Parent != null && Parent._app != null && Parent._app != App)
                App = Parent.App;
            else if (App != null)
                App.ConstructInputTree();

            UpdateMatrix();
        }
    }
    private Element? _parent;

    public int Index {
        get => _index;
        set {
            if (_index == value)
                return;
            _index = value;
            Parent?.SortChild(this);
        }
    }
    private int _index;

    private readonly List<Element> _children = new List<Element>();
    public IReadOnlyList<Element> Children => _children;
    public List<Element> RawChildren {
        get => _children;
        set {
            lock (_children) {
                Clear();
                Add(value.ToArray());
            }
        }
    }
        

    public void Remove(params Element[] elements) {
        lock (_children) {
            foreach (var element in elements) {
                _children.Remove(element);
                if (element.Parent == this)
                    element.Parent = null;
            }
        }
    }

    private void addChild(Element element) {
        lock (_children) {
            for (int i = 0; i < _children.Count; i++) {
                if (_children[i].Index > element.Index) {
                    _children.Insert(i, element);
                    return;
                }
            }
            _children.Add(element);
        }
    }

    public void Add(params Element[] elements) {
        lock (_children) {
            foreach (var element in elements) {
                addChild(element);
                if (element.Parent != this)
                    element.Parent = this;
            }
        }
    }

    public void SortChild(Element element) {
        lock (_children) {
            _children.Remove(element);
            addChild(element);
        }
    }

    public bool HasChild(Element element) => _children.Contains(element);

    public virtual void OnDispose() { }
    public virtual void OnUpdate() { }
    public virtual void OnUpdateChildren() => ForChildren(child => child.Update());
    public virtual void OnRender(ICanvas canvas) { }
    public virtual void OnRenderChildren(ICanvas canvas) => ForChildren(child => child.Render(canvas));

    public bool Clip { get; set; } = true;
    public float ClipRadius { get; set; } = 0;
    public bool ClipDifference { get; set; } = false;
    public bool ClipAntiAlias { get; set; } = true;

    public virtual void ClipCanvas(ICanvas canvas) {
        if (ClipRadius == 0) {
            canvas.ClipRect(new(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipDifference, ClipAntiAlias);
            return;
        }

        canvas.ClipRoundRect(new(0, 0, MathF.Round(DrawSize.X), MathF.Round(DrawSize.Y)), ClipRadius, ClipDifference, ClipAntiAlias);
    }

    public void Render(ICanvas canvas) {
        canvas.SetMatrix(Matrix);
        int save = canvas.Save();
        if (Clip)
            ClipCanvas(canvas);
        OnRender(canvas);
        // Any bound rendering can happen here
        // eg:
        // canvas.DrawRect(new(0, 0, MathF.Round(Size.X), MathF.Round(Size.Y)), new Paint() { Color = Colors.Red, IsStroke = true, StrokeWidth = 1 });
        OnRenderChildren(canvas);
        canvas.Restore(save);
    }

    public void Update() {
        OnUpdate();
        OnUpdateChildren();
    }

    public bool DisposeChildren { get; set; } = true;

    public void Dispose() {
        OnDispose();
        Parent?.Remove(this);
        if (DisposeChildren)
            Clear(true);
    }

    public void Clear(bool dispose = false) {
        lock (_children) {
            if (dispose)
                ForChildren(child => child.Dispose());
            _children.Clear();
        }
    }

    public void ForChildren(Action<Element> action) {
        lock (_children) {
            foreach (var child in _children)
                action(child);
        }
    }

    public virtual bool PointInside(Vector2 point) => Bounds.Contains(point);
    public bool HandlePositionalInput { get; set; } = false;
}
