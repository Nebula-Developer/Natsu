using SkiaSharp;

namespace Natsu.Graphics;

public partial class Element : IDisposable {
    public string Name {
        get => _name ?? GetType().Name;
        set => _name = value;
    }
    private string? _name;

    public Application App {
        get => _app ?? throw new InvalidOperationException("Element is not attached to an application");
        set {
            if (_app == value)
                return;
            _app = value;
            ForChildren(child => child.App = value);
        }
    }
    private Application? _app;

    public IRenderer Renderer => App.Renderer;
    public IResourceManager ResourceManager => App.ResourceManager;


    public Element? Parent {
        get => _parent;
        set {
            if (Parent?.HasChild(this) == true)
                Parent.Remove(this);
            _parent = value;
            if (Parent?.HasChild(this) == false)
                Parent.Add(this);
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
    public virtual void OnRender(ICanvas canvas) { }

    public void Render(ICanvas canvas) {
        canvas.SetMatrix(Matrix);
        OnRender(canvas);
        ForChildren(child => child.Render(canvas));
    }

    public void Update() {
        OnUpdate();
        ForChildren(child => child.Update());
    }

    public void Dispose() {
        OnDispose();
        Parent?.Remove(this);
    }

    public void ForChildren(Action<Element> action) {
        lock (_children) {
            foreach (var child in _children)
                action(child);
        }
    }
}
