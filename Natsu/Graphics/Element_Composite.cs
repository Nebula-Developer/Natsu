namespace Natsu.Graphics;

public partial class Element {
    private readonly List<Element> _children = new();

    public Element? Parent {
        get => _parent;
        set {
            if (Parent?.HasChild(this) == true) Parent.Remove(this);

            _parent = value;
            if (Parent?.HasChild(this) == false) Parent.Add(this);

            if (Parent != null && Parent._app != null && Parent._app != App)
                App = Parent.App;
            else if (_app != null) App.ConstructInputLists();

            UpdateMatrix();
        }
    }

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
        lock (_children)
            foreach (Element element in elements) {
                _children.Remove(element);
                if (element.Parent == this) element.Parent = null;
            }
    }

    private void addChild(Element element) {
        lock (_children) {
            for (int i = 0; i < _children.Count; i++)
                if (_children[i].Index > element.Index) {
                    _children.Insert(i, element);
                    return;
                }

            _children.Add(element);
        }
    }

    public void Add(params Element[] elements) {
        lock (_children)
            foreach (Element element in elements) {
                addChild(element);
                if (element.Parent != this) element.Parent = this;
            }
    }

    public void Clear(bool dispose = false) {
        lock (_children) {
            if (dispose) ForChildren(child => child.Dispose());

            _children.Clear();
        }
    }

    public void ForChildren(Action<Element> action) {
        lock (_children)
            foreach (Element child in _children)
                action(child);
    }

    public void SortChild(Element element) {
        lock (_children) {
            _children.Remove(element);
            addChild(element);
        }
    }

    public bool HasChild(Element element) => _children.Contains(element);
}