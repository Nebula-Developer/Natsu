using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class InputElement : Element {
    public bool AcceptInput { get; set; } = true;
    public bool GrabFallback { get; set; } = true;
    public bool HoverCursor { get; set; } = true;
    public CursorStyle Cursor { get; set; } = CursorStyle.Pointer;

    public Dictionary<MouseButton, bool> MouseButtons { get; } = new();
    public Dictionary<Key, bool> Keys { get; } = new();
    public bool IsMouseOver { get; private set; }
    public bool IsFocused { get; private set; }

    protected virtual bool OnMouseDown(MouseButton button, Vector2 position) => GrabFallback;
    protected virtual void OnMouseUp(MouseButton button, Vector2 position) { }
    protected virtual void OnMousePress(MouseButton button, Vector2 position) { }
    protected virtual void OnMousePressDodge(MouseButton button, Vector2 position) { }

    protected virtual bool OnMouseEnter(Vector2 position) => GrabFallback;
    protected virtual void OnMouseLeave(Vector2 position) { }

    protected virtual void OnMouseMove(Vector2 position) { }
    protected virtual void OnMouseWheel(Vector2 delta) { }

    protected virtual bool OnKeyDown(Key key) => GrabFallback;
    protected virtual bool OnKeyUp(Key key) => GrabFallback;

    protected virtual void OnFocus() { }
    protected virtual void OnBlur() { }

    public event Action<MouseButton, Vector2>? DoMouseDown;
    public event Action<MouseButton, Vector2>? DoMouseUp;
    public event Action<MouseButton, Vector2>? DoMousePress;
    public event Action<MouseButton, Vector2>? DoMousePressDodge;

    public event Action<Vector2>? DoMouseEnter;
    public event Action<Vector2>? DoMouseLeave;

    public event Action<Vector2>? DoMouseMove;
    public event Action<Vector2>? DoMouseWheel;

    public event Action<Key>? DoKeyDown;
    public event Action<Key>? DoKeyUp;

    public event Action? DoFocus;
    public event Action? DoBlur;

    public bool MouseDown(MouseButton button, Vector2 position) {
        MouseButtons[button] = true;
        DoMouseDown?.Invoke(button, position);
        return OnMouseDown(button, position);
    }

    public void MouseUp(MouseButton button, Vector2 position) {
        MouseButtons[button] = false;
        DoMouseUp?.Invoke(button, position);
        OnMouseUp(button, position);
    }

    public void MousePress(MouseButton button, Vector2 position) {
        DoMousePress?.Invoke(button, position);
        OnMousePress(button, position);
    }

    public void MousePressDodge(MouseButton button, Vector2 position) {
        DoMousePressDodge?.Invoke(button, position);
        OnMousePressDodge(button, position);
    }

    public bool MouseEnter(Vector2 position) {
        IsMouseOver = true;
        DoMouseEnter?.Invoke(position);
        return OnMouseEnter(position);
    }

    public void MouseLeave(Vector2 position) {
        IsMouseOver = false;
        DoMouseLeave?.Invoke(position);
        OnMouseLeave(position);
    }

    public void MouseMove(Vector2 position) {
        DoMouseMove?.Invoke(position);
        OnMouseMove(position);
    }

    public void MouseWheel(Vector2 delta) {
        DoMouseWheel?.Invoke(delta);
        OnMouseWheel(delta);
    }

    public bool KeyDown(Key key) {
        Keys[key] = true;
        DoKeyDown?.Invoke(key);
        return OnKeyDown(key);
    }

    public void KeyUp(Key key) {
        Keys[key] = false;
        DoKeyUp?.Invoke(key);
        OnKeyUp(key);
    }

    public void Focus() {
        IsFocused = true;
        DoFocus?.Invoke();
        OnFocus();
    }

    public void Blur() {
        IsFocused = false;
        DoBlur?.Invoke();
        OnBlur();
    }

    protected override void OnAppChange(Application? old) {
        old?.RemoveInputCandidate(this);
        App?.ConstructInputLists();
    }
}
