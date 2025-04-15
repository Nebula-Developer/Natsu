using Natsu.Core.InvalidationTemp;
using Natsu.Mathematics;

namespace Natsu.Core;

public partial class Element {
    private readonly List<Element> _children = new();
    private Element? _parentField;

    private Element? _parent {
        get => _parentField;
        set {
            if (_parentField == value) return;
            _parentField = value;

            OnParentChange(_parentField);
            DoParentChange?.Invoke(_parentField);
        }
    }

    /// <summary>
    ///     Sets the parent of this element to be the <see cref="ContentContainer" /> of the specified element.
    /// </summary>
    public Element? ContentParent {
        set => Parent = value?.ContentContainer;
    }

    /// <summary>
    ///     The parent of this element.
    /// </summary>
    public Element? Parent {
        get => _parent;
        set => updateParent(value);
    }

    /// <summary>
    ///     The content container used for appending content, rather than direct children.
    ///     <br />
    ///     This is useful for elements that don't directly want children, and instead
    ///     want to pass them to a different element.
    /// </summary>
    public virtual Element ContentContainer => this;

    /// <summary>
    ///     The content of this element that belongs to the <see cref="ContentContainer" />.
    /// </summary>
    public virtual IReadOnlyList<Element> Content {
        get => ContentContainer._children.AsReadOnly();
        set {
            lock (_children) {
                ContentContainer.Clear();
                ContentContainer.Add([.. value]);
            }
        }
    }

    /// <summary>
    ///     The children of this element.
    /// </summary>
    public virtual IReadOnlyList<Element> Children {
        get => _children;
        set {
            lock (_children) {
                Clear();
                Add([.. value]);
            }
        }
    }

    private bool findCircle(Element? element, HashSet<Element>? visited = null) {
        visited ??= new();

        if (element == null || visited.Contains(element)) return false;

        visited.Add(element);

        if (element == this) return true;

        foreach (Element? child in element.Children)
            if (findCircle(child, visited))
                return true;

        return false;
    }

    private void updateParent(Element? value, bool parentCheck = true) {
        if (_parent != value && Parent?.HasChild(this) == true) Parent.Remove(this);

        if (parentCheck && findCircle(value)) throw new InvalidOperationException("Circular parent detected");

        if (parentCheck && value?.HasChild(this) == false) value.Add(this);

        _parent = value;

        if (Parent?._app != null && Parent._app != App) App = Parent.App;

        Invalidate(ElementInvalidation.All);
    }

    /// <summary>
    ///     Removes element(s) from the children of this element.
    /// </summary>
    /// <param name="elements">The element(s) to remove</param>
    public void Remove(params Element[] elements) {
        lock (_children) {
            foreach (Element? element in elements) {
                _children.Remove(element);
                if (element.Parent == this) element._parent = null;
            }

            ChildrenChanged();
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
            ChildrenChanged();
        }
    }

    /// <summary>
    ///     Adds new element(s) to the children of this element.
    /// </summary>
    /// <param name="elements">The element(s) to add</param>
    public void Add(params Element[] elements) {
        lock (_children) {
            foreach (Element? element in elements) {
                addChild(element);
                if (element.Parent != this) element.updateParent(this, false);

                if (!element.Loaded && Loaded) element.Load();
            }

            ChildrenChanged();
        }
    }

    /// <summary>
    ///     Adds new element(s) to the <see cref="ContentContainer" />.
    /// </summary>
    /// <param name="elements">The element(s) to add</param>
    public void AddContent(params Element[] elements) => ContentContainer.Add(elements);

    /// <summary>
    ///     Removes element(s) from the <see cref="ContentContainer" />.
    /// </summary>
    /// <param name="elements">The element(s) to remove</param>
    public void RemoveContent(params Element[] elements) => ContentContainer.Remove(elements);

    /// <summary>
    ///     Clears the children of this element.
    /// </summary>
    /// <param name="dispose">Whether to dispose the children</param>
    public void Clear(bool dispose = false) {
        lock (_children) {
            if (dispose) ForChildren(child => child.Dispose());

            _children.Clear();
            ChildrenChanged();
        }
    }

    /// <summary>
    ///     Clears the content of the <see cref="ContentContainer" />.
    /// </summary>
    /// <param name="dispose">Whether to dispose the content</param>
    public void ClearContent(bool dispose = false) => ContentContainer.Clear(dispose);

    /// <summary>
    ///     Iterates over the children of this element in a thread-safe manner.
    /// </summary>
    /// <param name="action">The action to perform on each child</param>
    public void ForChildren(Action<Element> action) {
        lock (_children) {
            for (int i = 0; i < _children.Count; i++)
                if (_children[i] != null)
                    action(_children[i]);
        }
    }

    /// <summary>
    ///     Sorts the specified element into its correct position among the children.
    /// </summary>
    /// <param name="element">The element to sort</param>
    public void SortChild(Element element) {
        lock (_children) {
            _children.Remove(element);
            addChild(element);
        }
    }

    /// <summary>
    ///     Checks if this element has the specified element as a child.
    /// </summary>
    /// <param name="element">The element to check for</param>
    /// <returns>Whether this element has the specified element as a child</returns>
    public bool HasChild(Element element) => _children.Contains(element);

    protected virtual void OnChildrenChange() { }

    protected virtual void OnParentChange(Element? oldParent) { }

    public event Action? DoChildrenChange;

    public event Action<Element?>? DoParentChange;

    /// <summary>
    ///     Called when the children of this element change.
    /// </summary>
    public void ChildrenChanged() {
        if (ChildRelativeSizeAxes != Axes.None) Invalidate(ElementInvalidation.Geometry);

        OnChildrenChange();
        DoChildrenChange?.Invoke();
    }
}
