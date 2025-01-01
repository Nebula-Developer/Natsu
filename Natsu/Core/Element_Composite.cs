using Natsu.Mathematics;

namespace Natsu.Core;

public partial class Element {
    private readonly List<Element> _children = new();
    private Element? _parent;

    public Element? ContentParent {
        get => _parent;
        set => Parent = value?.ContentContainer;
    }

    public Element? Parent {
        get => _parent;
        set => updateParent(value);
    }

    public virtual Element ContentContainer => this;

    public virtual IReadOnlyList<Element> Content {
        get => ContentContainer._children.AsReadOnly();
        set {
            lock (_children) {
                ContentContainer.Clear();
                ContentContainer.Add([.. value]);
            }
        }
    }

    public virtual IReadOnlyList<Element> Children {
        get => _children;
        set {
            lock (_children) {
                Clear();
                Add([.. value]);
            }
        }
    }

    private void updateParent(Element? value, bool parentCheck = true) {
        if (_parent != value && Parent?.HasChild(this) == true) Parent.Remove(this);

        if (parentCheck && value?.HasChild(this) == false) value.Add(this);

        _parent = value;

        if (ContentParent?._app != null && ContentParent._app != App) App = ContentParent.App;

        Invalidate(Invalidation.All);
    }

    public void Remove(params Element[] elements) {
        lock (_children) {
            foreach (Element? element in elements) {
                _children.Remove(element);
                if (element.ContentParent == this) element._parent = null;
            }

            CildrenChanged();
        }
    }

    private void addChild(Element element) {
        if (_children.Contains(element) || element == this) return;

        lock (_children) {
            for (int i = 0; i < _children.Count; i++)
                if (_children[i].Index > element.Index) {
                    _children.Insert(i, element);
                    return;
                }

            _children.Add(element);
            CildrenChanged();
        }
    }

    public void Add(params Element[] elements) {
        lock (_children) {
            foreach (Element? element in elements) {
                addChild(element);
                if (element.ContentParent != this) element.updateParent(this, false);

                if (!element.Loaded && Loaded) element.Load();
            }

            CildrenChanged();
        }
    }

    public void AddContent(params Element[] elements) => ContentContainer.Add(elements);

    public void Clear(bool dispose = false) {
        lock (_children) {
            if (dispose) ForChildren(child => child.Dispose());

            _children.Clear();
            CildrenChanged();
        }
    }

    public void ForChildren(Action<Element> action) {
        lock (_children) {
            for (int i = 0; i < _children.Count; i++)
                if (_children[i] != null)
                    action(_children[i]);
        }
    }

    public void SortChild(Element element) {
        lock (_children) {
            _children.Remove(element);
            addChild(element);
        }
    }

    public bool HasChild(Element element) => _children.Contains(element);

    protected virtual void OnChildrenChange() { }

    public event Action? DoChildrenChange;

    public void CildrenChanged() {
        if (ChildRelativeSizeAxes != Axes.None) Invalidate(Invalidation.Geometry);

        OnChildrenChange();
        DoChildrenChange?.Invoke();
    }
}
