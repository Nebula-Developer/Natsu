using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Utils;

namespace Natsu.Graphics.Elements;

public class InputElement : Element {
    public bool AcceptInput { get; set; } = true;
    public bool GrabFallback { get; set; } = true;
    public bool HoverCursor { get; set; } = true;
    public CursorStyle Cursor { get; set; } = CursorStyle.Pointer;

    public FallbackDictionary<MouseButton, bool> MouseButtons { get; } = new(false);
    public FallbackDictionary<Key, bool> Keys { get; } = new(false);

    public bool IsMouseOver { get; private set; }
    public bool IsFocused { get; private set; }

    protected virtual bool OnMouseDown(MouseButton button, Vector2 position) => GrabFallback;
    protected virtual void OnMouseUp(MouseButton button, Vector2 position) { }

    protected virtual void OnMousePress(MouseButton button, Vector2 position) { }
    protected virtual void OnMousePressDodge(MouseButton button, Vector2 position) { }

    protected virtual bool OnTouchDown(int id, Vector2 position) => GrabFallback;
    protected virtual void OnTouchUp(int id, Vector2 position) { }

    protected virtual void OnTouchMove(int id, Vector2 position) { }
    protected virtual void OnTouchPress(int id, Vector2 position) { }
    protected virtual void OnTouchPressDodge(int id, Vector2 position) { }

    protected virtual void OnPressDown(Vector2 position) { }
    protected virtual void OnPressUp(Vector2 position) { }
    protected virtual void OnPress(Vector2 position) { }
    protected virtual void OnPressDodge(Vector2 position) { }

    protected virtual bool OnMouseEnter(Vector2 position) => GrabFallback;
    protected virtual void OnMouseLeave(Vector2 position) { }

    protected virtual void OnMouseMove(Vector2 position) { }
    protected virtual void OnMouseWheel(Vector2 delta) { }

    protected virtual bool OnKeyDown(Key key, KeyMods mods) => GrabFallback;
    protected virtual bool OnKeyUp(Key key, KeyMods mods) => GrabFallback;

    protected virtual void OnTextInput(string text, int location, int replaced) { }
    protected virtual void OnTextCaretMove(int start, int end) { }

    protected virtual void OnFocus() { }
    protected virtual void OnBlur() { }

    public event Action<MouseButton, Vector2>? DoMouseDown;
    public event Action<MouseButton, Vector2>? DoMouseUp;

    public event Action<MouseButton, Vector2>? DoMousePress;
    public event Action<MouseButton, Vector2>? DoMousePressDodge;

    public event Action<int, Vector2>? DoTouchDown;
    public event Action<int, Vector2>? DoTouchUp;

    public event Action<int, Vector2>? DoTouchMove;
    public event Action<int, Vector2>? DoTouchPress;
    public event Action<int, Vector2>? DoTouchPressDodge;

    public event Action<Vector2>? DoPressDown;
    public event Action<Vector2>? DoPressUp;
    public event Action<Vector2>? DoPress;
    public event Action<Vector2>? DoPressDodge;

    public event Action<Vector2>? DoMouseEnter;
    public event Action<Vector2>? DoMouseLeave;

    public event Action<Vector2>? DoMouseMove;
    public event Action<Vector2>? DoMouseWheel;

    public event Action<Key, KeyMods>? DoKeyDown;
    public event Action<Key, KeyMods>? DoKeyUp;

    public event Action<string, int, int>? DoTextInput;
    public event Action<int, int>? DoTextCaretMove;

    public event Action? DoFocus;
    public event Action? DoBlur;

    public bool MouseDown(MouseButton button, Vector2 position) {
        MouseButtons[button] = true;
        DoMouseDown?.Invoke(button, position);
        bool r = OnMouseDown(button, position);
        if (button == MouseButton.Left) PressDown(position);
        return r;
    }

    public void MouseUp(MouseButton button, Vector2 position) {
        MouseButtons[button] = false;
        DoMouseUp?.Invoke(button, position);
        OnMouseUp(button, position);
        if (button == MouseButton.Left) PressUp(position);
    }

    public void MousePress(MouseButton button, Vector2 position) {
        DoMousePress?.Invoke(button, position);
        OnMousePress(button, position);
        if (button == MouseButton.Left) Press(position);
    }

    public void MousePressDodge(MouseButton button, Vector2 position) {
        DoMousePressDodge?.Invoke(button, position);
        OnMousePressDodge(button, position);
        if (button == MouseButton.Left) PressDodge(position);
    }

    public bool TouchDown(int id, Vector2 position) {
        DoTouchDown?.Invoke(id, position);
        bool r = OnTouchDown(id, position);
        if (id == 0) PressDown(position);
        return r;
    }

    public void TouchUp(int id, Vector2 position) {
        DoTouchUp?.Invoke(id, position);
        OnTouchUp(id, position);
        if (id == 0) PressUp(position);
    }

    public void TouchMove(int id, Vector2 position) {
        DoTouchMove?.Invoke(id, position);
        OnTouchMove(id, position);
        if (id == 0) Press(position);
    }

    public void TouchPress(int id, Vector2 position) {
        DoTouchPress?.Invoke(id, position);
        OnTouchPress(id, position);
        if (id == 0) Press(position);
    }

    public void TouchPressDodge(int id, Vector2 position) {
        DoTouchPressDodge?.Invoke(id, position);
        OnTouchPressDodge(id, position);
        if (id == 0) PressDodge(position);
    }

    public void PressDown(Vector2 position) {
        DoPressDown?.Invoke(position);
        OnPressDown(position);
    }

    public void PressUp(Vector2 position) {
        DoPressUp?.Invoke(position);
        OnPressUp(position);
    }

    public void Press(Vector2 position) {
        DoPress?.Invoke(position);
        OnPress(position);
    }

    public void PressDodge(Vector2 position) {
        DoPressDodge?.Invoke(position);
        OnPressDodge(position);
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

    public bool KeyDown(Key key, KeyMods mods) {
        Keys[key] = true;
        DoKeyDown?.Invoke(key, mods);
        return OnKeyDown(key, mods);
    }

    public void KeyUp(Key key, KeyMods mods) {
        Keys[key] = false;
        DoKeyUp?.Invoke(key, mods);
        OnKeyUp(key, mods);
    }

    public void TextInput(string text, int location, int replaced) {
        DoTextInput?.Invoke(text, location, replaced);
        OnTextInput(text, location, replaced);
    }

    public void TextCaretMove(int start, int end) {
        DoTextCaretMove?.Invoke(start, end);
        OnTextCaretMove(start, end);
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
