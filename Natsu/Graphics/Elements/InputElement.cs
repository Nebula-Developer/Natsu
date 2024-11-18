using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class InputElement : Element {
    public bool AcceptInput { get; set; } = true;

    public virtual bool OnMouseDown(MouseButton button, Vector2 position) => false;
    public virtual void OnMouseUp(MouseButton button, Vector2 position) { }

    public virtual bool OnMouseEnter(Vector2 position) => false;
    public virtual void OnMouseLeave(Vector2 position) { }

    public virtual void OnMouseMove(Vector2 position) { }
    public virtual void OnMouseWheel(Vector2 delta) { }

    public virtual bool OnKeyDown(Key key) => false;
    public virtual bool OnKeyUp(Key key) => false;

    public virtual void OnFocus() { }
    public virtual void OnBlur() { }

    public event Action<MouseButton, Vector2>? MouseDownEvent;
    public event Action<MouseButton, Vector2>? MouseUpEvent;

    public event Action<Vector2>? MouseEnterEvent;
    public event Action<Vector2>? MouseLeaveEvent;

    public event Action<Vector2>? MouseMoveEvent;
    public event Action<Vector2>? MouseWheelEvent;

    public event Action<Key>? KeyDownEvent;
    public event Action<Key>? KeyUpEvent;

    public event Action? Focused;
    public event Action? Blurred;

    public bool MouseDown(MouseButton button, Vector2 position) {
        MouseDownEvent?.Invoke(button, position);
        return OnMouseDown(button, position);
    }

    public void MouseUp(MouseButton button, Vector2 position) {
        MouseUpEvent?.Invoke(button, position);
        OnMouseUp(button, position);
    }

    public bool MouseEnter(Vector2 position) {
        MouseEnterEvent?.Invoke(position);
        return OnMouseEnter(position);
    }

    public void MouseLeave(Vector2 position) {
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
        KeyDownEvent?.Invoke(key);
        return OnKeyDown(key);
    }

    public void KeyUp(Key key) => KeyUpEvent?.Invoke(key);

    public void Focus() {
        Focused?.Invoke();
        OnFocus();
    }

    public void Blur() {
        Blurred?.Invoke();
        OnBlur();
    }
}