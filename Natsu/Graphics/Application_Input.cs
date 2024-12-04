#nullable disable

using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Graphics;

public partial class Application {

    public enum MouseEnterCacheState {
        Over = 1 << 0,
        Blocking = 1 << 1,
        Blocked = 1 << 2,
        None = 1 << 3,
        OverBlock = Over | Blocking
    }

    private readonly Dictionary<MouseButton, HashSet<InputElement>> _mouseDownCache = new();

    private readonly Dictionary<InputElement, MouseEnterCacheState> _mouseEnterCache = new();

    private InputElement _rawFocusedElement;
    public Dictionary<Key, bool> Keys = new();

    public Dictionary<MouseButton, bool> MouseState = new();

    protected List<GlobalInputElement> NonPositionalInputList = new();
    protected List<InputElement> PositionalInputList = new();

    public Vector2 MousePosition { get; private set; }

    private InputElement _focusedElement {
        get => _rawFocusedElement;
        set {
            if (_rawFocusedElement != null && _rawFocusedElement != value)
                _rawFocusedElement.Blur();

            _rawFocusedElement = value;

            if (_rawFocusedElement != null)
                _rawFocusedElement.Focus();
        }
    }
    public List<InputElement> InteractingElements => _mouseEnterCache.Where(x => x.Value.HasFlag(MouseEnterCacheState.Over)).Select(x => x.Key).ToList();
    public List<InputElement> AllInteractingElements => InteractingElements.Concat(NonPositionalInputList).ToList();

    protected static List<Element> ConditionalElementTree(Element cur, Func<Element, bool> condition = null, bool ignoreInactive = false) {
        if (cur == null) return new List<Element>();

        List<Element> elements = new();
        if (cur.Children.Count > 0)
            lock (cur.Children)
                for (int i = cur.Children.Count - 1; i >= 0; i--) {
                    if (!cur.Children[i].Active || cur.Children[i].BlockPositionalInput)
                        continue;

                    elements.AddRange(ConditionalElementTree(cur.Children[i], condition));
                }

        if (condition == null || condition(cur))
            elements.Add(cur);

        return elements;
    }

    public void ConstructInputLists() {
        PositionalInputList = ConditionalElementTree(Root, x => x is not GlobalInputElement && x is InputElement i && i.AcceptInput).Cast<InputElement>().ToList();
        NonPositionalInputList = ConditionalElementTree(Root, x => x is GlobalInputElement i && i.AcceptInput).Cast<GlobalInputElement>().ToList();
    }

    public void RemoveInputCandidate(Element elm) {
        if (elm is InputElement btn)
            PositionalInputList.Remove(btn);
        else if (elm is GlobalInputElement input)
            NonPositionalInputList.Remove(input);
    }

    public List<InputElement> GetInputCandidates(Vector2 position) => PositionalInputList.Where(x => x.PointInside(position)).ToList();

    protected virtual void OnMouseDown(MouseButton button, Vector2 position) { }
    protected virtual void OnMouseUp(MouseButton button, Vector2 position) { }
    protected virtual void OnMouseMove(Vector2 position) { }
    protected virtual void OnMouseWheel(Vector2 delta) { }
    protected virtual void OnKeyDown(Key key) { }
    protected virtual void OnKeyUp(Key key) { }

    public event Action<MouseButton, Vector2> DoMouseDown;
    public event Action<MouseButton, Vector2> DoMouseUp;
    public event Action<Vector2> DoMouseMove;
    public event Action<Vector2> DoMouseWheel;
    public event Action<Key> DoKeyDown;
    public event Action<Key> DoKeyUp;

    public bool MouseDown(MouseButton button, Vector2 position) {
        MouseMove(position);
        MouseState[button] = true;

        DoMouseDown?.Invoke(button, position);
        OnMouseDown(button, position);

        List<InputElement> elms = InteractingElements;
        if (!_mouseDownCache.ContainsKey(button))
            _mouseDownCache.Add(button, new HashSet<InputElement>());

        foreach (GlobalInputElement elm in NonPositionalInputList)
            if (elm.MouseDown(button, position))
                return true;

        foreach (InputElement elm in elms) {
            _mouseDownCache[button].Add(elm);

            if (elm.MouseDown(button, position)) {
                if (button == MouseButton.Left)
                    _focusedElement = elm;
                return true;
            }
        }

        if (button == MouseButton.Left)
            _focusedElement = null;
        return false;
    }

    public void MouseUp(MouseButton button, Vector2 position) {
        MouseMove(position);
        MouseState[button] = false;

        DoMouseUp?.Invoke(button, position);
        OnMouseUp(button, position);

        if (!_mouseDownCache.ContainsKey(button))
            return;

        foreach (GlobalInputElement elm in NonPositionalInputList)
            elm.MouseUp(button, position);

        foreach (InputElement elm in _mouseDownCache[button]) {
            elm.MouseUp(button, position);
            if (elm.PointInside(position)) elm.MousePress(button, position);
            else elm.MousePressDodge(button, position);
        }

        _mouseDownCache[button].Clear();
    }

    public void MouseMove(Vector2 position) {
        MousePosition = position;

        DoMouseMove?.Invoke(position);
        OnMouseMove(position);

        List<InputElement> elms = GetInputCandidates(position);

        foreach (GlobalInputElement elm in NonPositionalInputList)
            elm.MouseMove(position);

        foreach (InputElement elm in _mouseEnterCache.Keys.ToList())
            if (!elms.Contains(elm)) {
                if (_mouseEnterCache[elm].HasFlag(MouseEnterCacheState.Over)) elm.MouseLeave(position);

                _mouseEnterCache.Remove(elm);
            }

        bool blocked = false;
        foreach (InputElement elm in elms) {
            if (blocked) {
                if (_mouseEnterCache.ContainsKey(elm) && _mouseEnterCache[elm].HasFlag(MouseEnterCacheState.Over))
                    elm.MouseLeave(position);
                _mouseEnterCache[elm] = MouseEnterCacheState.Blocked;
                continue;
            }

            MouseEnterCacheState state = _mouseEnterCache.ContainsKey(elm) ? _mouseEnterCache[elm] : MouseEnterCacheState.None;

            if (!blocked && state.HasFlag(MouseEnterCacheState.Blocked))
                state = MouseEnterCacheState.None;

            if (state.HasFlag(MouseEnterCacheState.Blocking))
                blocked = true;

            if (state == MouseEnterCacheState.None) {
                if (elm.MouseEnter(position))
                    blocked = true;

                _mouseEnterCache[elm] = blocked ? MouseEnterCacheState.OverBlock : MouseEnterCacheState.Over;
            } else if (state.HasFlag(MouseEnterCacheState.Over)) elm.MouseMove(position);
        }

        if (_focusedElement != null /* && !elms.Contains(_focusedElement) */)
            _focusedElement.MouseMove(position);

        List<InputElement> interacting = InteractingElements;
        if (interacting.Count > 0) {
            InputElement top = interacting[0];
            if (top.HoverCursor)
                Platform.Cursor = top.Cursor;
        } else Platform.Cursor = CursorStyle.Default;
    }

    public void MouseWheel(Vector2 delta) {
        DoMouseWheel?.Invoke(delta);
        OnMouseWheel(delta);

        foreach (InputElement elm in AllInteractingElements)
            elm.MouseWheel(delta);
    }

    public void KeyDown(Key key) {
        Keys[key] = true;

        DoKeyDown?.Invoke(key);
        OnKeyDown(key);

        foreach (GlobalInputElement elm in NonPositionalInputList)
            elm.KeyDown(key);

        _focusedElement?.KeyDown(key);
    }

    public void KeyUp(Key key) {
        Keys[key] = false;

        DoKeyUp?.Invoke(key);
        OnKeyUp(key);

        foreach (GlobalInputElement elm in NonPositionalInputList)
            elm.KeyUp(key);

        _focusedElement?.KeyUp(key);
    }
}
