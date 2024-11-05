using System.Data;

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
                
            if (Parent != null && Parent._app != null)
                App = Parent.App;
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
    public virtual void OnUpdateChildren() => ForChildren(child => child.Update());
    public virtual void OnRender(ICanvas canvas) { }
    public virtual void OnRenderChildren(ICanvas canvas) => ForChildren(child => child.Render(canvas));

    public void Render(ICanvas canvas) {
        canvas.SetMatrix(Matrix);
        OnRender(canvas);
        OnRenderChildren(canvas);
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
}
