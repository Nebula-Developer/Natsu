#nullable disable

using Natsu.Core.Elements;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Utils;

namespace Natsu.Core;

public partial class Application {
    public enum MouseEnterCacheState {
        Over = 1 << 0,
        Blocking = 1 << 1,
        Blocked = 1 << 2,
        None = 1 << 3,
        OverBlock = Over | Blocking
    }

    private readonly FallbackDictionary<MouseButton, List<GlobalInputElement>> _globalPressedInputs = new(new());

    private readonly FallbackDictionary<int, List<GlobalInputElement>> _globalPressedTouchInputs = new(new());

    private readonly Dictionary<MouseButton, HashSet<InputElement>> _mouseDownCache = new();

    private readonly Dictionary<InputElement, MouseEnterCacheState> _mouseEnterCache = new();
    private readonly Dictionary<int, HashSet<InputElement>> _touchDownCache = new();

    private InputElement _rawFocusedElement;
    public FallbackDictionary<Key, bool> Keys = new(false);

    public FallbackDictionary<MouseButton, bool> MouseState = new(false);

    protected List<GlobalInputElement> NonPositionalInputList = new();
    protected List<InputElement> PositionalInputList = new();

    public FallbackDictionary<int, Vector2> TouchPositions = new(new(0));
    public FallbackDictionary<int, bool> TouchState = new(false);

    public Vector2 MousePosition { get; private set; }

    private InputElement _focusedElement {
        get => _rawFocusedElement;
        set {
            if (_rawFocusedElement != null && _rawFocusedElement != value) _rawFocusedElement.Blur();

            _rawFocusedElement = value;

            if (_rawFocusedElement != null) _rawFocusedElement.Focus();
        }
    }

    public List<InputElement> InteractingElements => _mouseEnterCache.Where(x => x.Value.HasFlag(MouseEnterCacheState.Over)).Select(x => x.Key).ToList();

    public List<InputElement> AllInteractingElements => InteractingElements.Concat(NonPositionalInputList).ToList();

    protected static List<Element> ConditionalElementTree(Element cur, Func<Element, bool> condition = null, bool ignoreInactive = false) {
        if (cur == null) return new();

        List<Element> elements = new();
        if (cur.Children.Count > 0)
            lock (cur.Children) {
                for (int i = cur.Children.Count - 1; i >= 0; i--) {
                    if (cur.Children[i].BlockPositionalInput) continue;

                    elements.AddRange(ConditionalElementTree(cur.Children[i], condition));
                }
            }

        if (condition == null || condition(cur)) elements.Add(cur);

        return elements;
    }

    public void ConstructInputLists() {
        PositionalInputList = ConditionalElementTree(Root, x => x is not GlobalInputElement && x is InputElement i && i.AcceptInput).Cast<InputElement>().ToList();
        NonPositionalInputList = ConditionalElementTree(Root, x => x is GlobalInputElement i && i.AcceptInput).Cast<GlobalInputElement>().ToList();
    }

    public void RemoveInputCandidate(Element elm) {
        if (elm is InputElement btn)
            PositionalInputList.Remove(btn);
        else if (elm is GlobalInputElement input) NonPositionalInputList.Remove(input);
    }

    public List<InputElement> GetInputCandidates(Vector2 position) => PositionalInputList.Where(x => x.PointInside(position)).ToList();

    protected virtual void OnMouseDown(MouseButton button, Vector2 position) { }

    protected virtual void OnMouseUp(MouseButton button, Vector2 position) { }

    protected virtual void OnMouseMove(Vector2 position) { }

    protected virtual void OnMouseWheel(Vector2 delta) { }

    protected virtual void OnKeyDown(Key key, KeyMods mods) { }

    protected virtual void OnKeyUp(Key key, KeyMods mods) { }

    protected virtual void OnTextInput(string text) { }

    protected virtual void OnFocusedElementChange(InputElement elm) { }

    public event Action<MouseButton, Vector2> DoMouseDown;
    public event Action<MouseButton, Vector2> DoMouseUp;
    public event Action<Vector2> DoMouseMove;
    public event Action<Vector2> DoMouseWheel;
    public event Action<Key, KeyMods> DoKeyDown;
    public event Action<Key, KeyMods> DoKeyUp;
    public event Action<string> DoTextInput;
    public event Action<InputElement> DoFocusedElementChange;

    private void _setFocusedElement(InputElement elm) {
        _focusedElement = elm;
        OnFocusedElementChange(elm);
        DoFocusedElementChange?.Invoke(elm);
    }

    public bool MouseDown(MouseButton button, Vector2 position) {
        MouseMove(position);
        MouseState[button] = true;

        DoMouseDown?.Invoke(button, position);
        OnMouseDown(button, position);

        List<InputElement> elms = InteractingElements;
        if (!_mouseDownCache.ContainsKey(button)) _mouseDownCache.Add(button, new());

        _globalPressedInputs[button].Clear();

        foreach (GlobalInputElement elm in NonPositionalInputList) {
            if (!elm.AcceptInput || !elm.Active) continue;
            _globalPressedInputs[button].Add(elm);
            if (elm.MouseDown(button, position)) return true;
        }

        foreach (InputElement elm in elms) {
            if (!elm.AcceptInput || !elm.Active) continue;
            _mouseDownCache[button].Add(elm);

            if (elm.MouseDown(button, position)) {
                if (button == MouseButton.Left) _setFocusedElement(elm);

                return true;
            }
        }

        if (button == MouseButton.Left) _setFocusedElement(null);

        return false;
    }

    public void MouseUp(MouseButton button, Vector2 position) {
        MouseMove(position);
        MouseState[button] = false;

        DoMouseUp?.Invoke(button, position);
        OnMouseUp(button, position);

        if (!_mouseDownCache.ContainsKey(button)) return;

        foreach (GlobalInputElement elm in _globalPressedInputs[button])
            if (elm.AcceptInput && elm.Active)
                elm.MouseUp(button, position);

        foreach (InputElement elm in _mouseDownCache[button]) {
            elm.MouseUp(button, position);
            if (elm.PointInside(position))
                elm.MousePress(button, position);
            else
                elm.MousePressDodge(button, position);
        }

        _mouseDownCache[button].Clear();
    }

    public bool TouchDown(int id, Vector2 position) {
        TouchState[id] = true;
        TouchPositions[id] = position;

        List<InputElement> elms = GetInputCandidates(position);

        foreach (GlobalInputElement elm in NonPositionalInputList) {
            if (!elm.AcceptInput || !elm.Active) continue;
            _globalPressedTouchInputs[id].Add(elm);
            if (elm.TouchDown(id, position)) return true;
        }

        if (!_touchDownCache.ContainsKey(id)) _touchDownCache.Add(id, new());

        foreach (InputElement elm in elms) {
            if (!elm.AcceptInput || !elm.Active) continue;
            _touchDownCache[id].Add(elm);
            if (elm.TouchDown(id, position)) {
                _setFocusedElement(elm);
                return true;
            }
        }

        _setFocusedElement(null);

        return false;
    }

    public void TouchUp(int id, Vector2 position) {
        TouchState[id] = false;
        TouchPositions[id] = position;

        foreach (GlobalInputElement elm in _globalPressedTouchInputs[id])
            if (elm.AcceptInput && elm.Active)
                elm.TouchUp(id, position);

        foreach (InputElement elm in _touchDownCache[id]) {
            if (!elm.AcceptInput || !elm.Active) continue;

            elm.TouchUp(id, position);
            if (elm.PointInside(position))
                elm.TouchPress(id, position);
            else
                elm.TouchPressDodge(id, position);
        }

        _touchDownCache[id].Clear();
    }

    public void TouchMove(int id, Vector2 position) {
        TouchPositions[id] = position;

        foreach (GlobalInputElement elm in _globalPressedTouchInputs[id])
            if (elm.AcceptInput && elm.Active)
                elm.TouchMove(id, position);

        if (!_touchDownCache.TryGetValue(id, out HashSet<InputElement> cache)) return;
        foreach (InputElement elm in cache)
            if (elm.AcceptInput && elm.Active)
                elm.TouchMove(id, position);
    }

    public virtual void HandleCursorChange(CursorStyle style) => Platform.Cursor = style;

    public void MouseMove(Vector2? pos = null, bool noMoveEvent = false) {
        Vector2 position = pos ?? MousePosition;

        MousePosition = position;

        if (!noMoveEvent) {
            DoMouseMove?.Invoke(position);
            OnMouseMove(position);
            foreach (GlobalInputElement elm in NonPositionalInputList)
                if (elm.AcceptInput && elm.Active)
                    elm.MouseMove(position);
        }

        List<InputElement> elms = GetInputCandidates(position);

        foreach (InputElement elm in _mouseEnterCache.Keys.ToList()) {
            if (!elm.AcceptInput || !elm.Active) continue;
            if (!elms.Contains(elm)) {
                if (_mouseEnterCache[elm].HasFlag(MouseEnterCacheState.Over)) elm.MouseLeave(position);

                _mouseEnterCache.Remove(elm);
            }
        }

        bool blocked = false;
        foreach (InputElement elm in elms) {
            if (!elm.AcceptInput || !elm.Active) continue;

            if (blocked) {
                if (_mouseEnterCache.ContainsKey(elm) && _mouseEnterCache[elm].HasFlag(MouseEnterCacheState.Over)) elm.MouseLeave(position);

                _mouseEnterCache[elm] = MouseEnterCacheState.Blocked;
                continue;
            }

            MouseEnterCacheState state = _mouseEnterCache.ContainsKey(elm) ? _mouseEnterCache[elm] : MouseEnterCacheState.None;

            if (!blocked && state.HasFlag(MouseEnterCacheState.Blocked)) state = MouseEnterCacheState.None;

            if (state.HasFlag(MouseEnterCacheState.Blocking)) blocked = true;

            if (state == MouseEnterCacheState.None) {
                if (elm.MouseEnter(position)) blocked = true;

                _mouseEnterCache[elm] = blocked ? MouseEnterCacheState.OverBlock : MouseEnterCacheState.Over;
            } else if (state.HasFlag(MouseEnterCacheState.Over) && !noMoveEvent) {
                elm.MouseMove(position);
            }
        }

        if (_focusedElement != null && !noMoveEvent) _focusedElement.MouseMove(position);

        List<InputElement> interacting = InteractingElements;
        if (interacting.Count > 0) {
            InputElement top = interacting[0];
            if (top.HoverCursor) HandleCursorChange(top.Cursor);
        } else {
            HandleCursorChange(CursorStyle.Default);
        }
    }

    public void MouseWheel(Vector2 delta) {
        DoMouseWheel?.Invoke(delta);
        OnMouseWheel(delta);

        foreach (InputElement elm in AllInteractingElements)
            if (elm.AcceptInput && elm.Active)
                elm.MouseWheel(delta);
    }

    public void KeyDown(Key key, KeyMods mods) {
        Keys[key] = true;

        DoKeyDown?.Invoke(key, mods);
        OnKeyDown(key, mods);

        foreach (GlobalInputElement elm in NonPositionalInputList)
            if (elm.AcceptInput && elm.Active)
                elm.KeyDown(key, mods);

        _focusedElement?.KeyDown(key, mods);
    }

    public void KeyUp(Key key, KeyMods mods) {
        Keys[key] = false;

        DoKeyUp?.Invoke(key, mods);
        OnKeyUp(key, mods);

        foreach (GlobalInputElement elm in NonPositionalInputList)
            if (elm.AcceptInput && elm.Active)
                elm.KeyUp(key, mods);

        _focusedElement?.KeyUp(key, mods);
    }

    public void TextInput(string text, int location, int replaced) {
        DoTextInput?.Invoke(text);
        OnTextInput(text);

        foreach (GlobalInputElement elm in NonPositionalInputList)
            if (elm.AcceptInput && elm.Active)
                elm.TextInput(text, location, replaced);

        _focusedElement?.TextInput(text, location, replaced);
    }

    public void CaretMove(int start, int end) {
        foreach (GlobalInputElement elm in NonPositionalInputList)
            if (elm.AcceptInput && elm.Active)
                elm.TextCaretMove(start, end);

        _focusedElement?.TextCaretMove(start, end);
    }
}
