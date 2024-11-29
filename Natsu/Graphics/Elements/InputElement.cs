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

    public virtual bool OnMouseDown(MouseButton button, Vector2 position) => GrabFallback;
    public virtual void OnMouseUp(MouseButton button, Vector2 position) { }
    public virtual void OnMousePress(MouseButton button, Vector2 position) { }
    public virtual void OnMousePressDodge(MouseButton button, Vector2 position) { }

    public virtual bool OnMouseEnter(Vector2 position) => GrabFallback;
    public virtual void OnMouseLeave(Vector2 position) { }

    public virtual void OnMouseMove(Vector2 position) { }
    public virtual void OnMouseWheel(Vector2 delta) { }

    public virtual bool OnKeyDown(Key key) => GrabFallback;
    public virtual bool OnKeyUp(Key key) => GrabFallback;

    public virtual void OnFocus() { }
    public virtual void OnBlur() { }

    public event Action<MouseButton, Vector2>? MouseDownEvent;
    public event Action<MouseButton, Vector2>? MouseUpEvent;
    public event Action<MouseButton, Vector2>? MousePressEvent;
    public event Action<MouseButton, Vector2>? MousePressDodgeEvent;

    public event Action<Vector2>? MouseEnterEvent;
    public event Action<Vector2>? MouseLeaveEvent;

    public event Action<Vector2>? MouseMoveEvent;
    public event Action<Vector2>? MouseWheelEvent;

    public event Action<Key>? KeyDownEvent;
    public event Action<Key>? KeyUpEvent;

    public event Action? Focused;
    public event Action? Blurred;

    public bool MouseDown(MouseButton button, Vector2 position) {
        MouseButtons[button] = true;
        MouseDownEvent?.Invoke(button, position);
        return OnMouseDown(button, position);
    }

    public void MouseUp(MouseButton button, Vector2 position) {
        MouseButtons[button] = false;
        MouseUpEvent?.Invoke(button, position);
        OnMouseUp(button, position);
    }

    public void MousePress(MouseButton button, Vector2 position) {
        MousePressEvent?.Invoke(button, position);
        OnMousePress(button, position);
    }

    public void MousePressDodge(MouseButton button, Vector2 position) {
        MousePressDodgeEvent?.Invoke(button, position);
        OnMousePressDodge(button, position);
    }

    public bool MouseEnter(Vector2 position) {
        IsMouseOver = true;
        MouseEnterEvent?.Invoke(position);
        return OnMouseEnter(position);
    }

    public void MouseLeave(Vector2 position) {
        IsMouseOver = false;
        MouseLeaveEvent?.Invoke(position);
        OnMouseLeave(position);
    }

    public void MouseMove(Vector2 position) {
        MouseMoveEvent?.Invoke(position);
        OnMouseMove(position);
    }

    public void MouseWheel(Vector2 delta) {
        MouseWheelEvent?.Invoke(delta);
        OnMouseWheel(delta);
    }

    public bool KeyDown(Key key) {
        Keys[key] = true;
        KeyDownEvent?.Invoke(key);
        return OnKeyDown(key);
    }

    public void KeyUp(Key key) {
        Keys[key] = false;
        KeyUpEvent?.Invoke(key);
        OnKeyUp(key);
    }

    public void Focus() {
        IsFocused = true;
        Focused?.Invoke();
        OnFocus();
    }

    public void Blur() {
        IsFocused = false;
        Blurred?.Invoke();
        OnBlur();
    }
}