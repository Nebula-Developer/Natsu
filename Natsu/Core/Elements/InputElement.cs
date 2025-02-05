using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Utils;

namespace Natsu.Core.Elements;

/// <summary>
///     An element that can take in positional input.
/// </summary>
public class InputElement : Element {
    /// <summary>
    ///     Whether this element can accept input.
    /// </summary>
    public bool AcceptInput { get; set; } = true;

    /// <summary>
    ///     Whether this element will block input if its handler is not present.
    /// </summary>
    public bool GrabFallback { get; set; } = true;

    /// <summary>
    ///     Whether this element will change the cursor when hovered.
    /// </summary>
    public bool HoverCursor { get; set; } = true;

    /// <summary>
    ///     The cursor style to use when the cursor is over this element.
    /// </summary>
    public CursorStyle Cursor { get; set; } = CursorStyle.Pointer;

    /// <summary>
    ///     The state of the mouse buttons in this element.
    /// </summary>
    public FallbackDictionary<MouseButton, bool> MouseButtons { get; } = new(false);

    /// <summary>
    ///     The state of the keys in this element.
    /// </summary>
    public FallbackDictionary<Key, bool> Keys { get; } = new(false);

    /// <summary>
    ///     Whether the mouse is over this element.
    /// </summary>
    public bool IsMouseOver { get; private set; }

    /// <summary>
    ///     Whether this element is focused.
    /// </summary>
    public bool IsFocused { get; private set; }

    public bool UseLocalPositions { get; set; }

    protected virtual bool OnMouseDown(MouseButton button, Vector2 position) => GrabFallback;

    protected virtual void OnMouseUp(MouseButton button, Vector2 position) { }

    protected virtual void OnMousePress(MouseButton button, Vector2 position) { }

    protected virtual void OnMousePressDodge(MouseButton button, Vector2 position) { }

    protected virtual bool OnTouchDown(int id, Vector2 position) => GrabFallback;

    protected virtual void OnTouchUp(int id, Vector2 position) { }

    protected virtual void OnTouchMove(int id, Vector2 position) { }

    protected virtual void OnTouchPress(int id, Vector2 position) { }

    protected virtual void OnTouchPressDodge(int id, Vector2 position) { }

    protected virtual void OnPressDown(int index, Vector2 position) { }

    protected virtual void OnPressUp(int index, Vector2 position) { }

    protected virtual void OnPress(int index, Vector2 position) { }

    protected virtual void OnPressDodge(int index, Vector2 position) { }

    protected virtual void OnPressMove(int index, Vector2 position) { }

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

    public event Action<int, Vector2>? DoPressDown;
    public event Action<int, Vector2>? DoPressUp;
    public event Action<int, Vector2>? DoPress;
    public event Action<int, Vector2>? DoPressDodge;
    public event Action<int, Vector2>? DoPressMove;

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
        if (UseLocalPositions) position = ToLocalSpace(position);
        MouseButtons[button] = true;
        DoMouseDown?.Invoke(button, position);
        bool r = OnMouseDown(button, position);
        PressDown((int)button, position, false);

        return r;
    }

    public void MouseUp(MouseButton button, Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        MouseButtons[button] = false;
        DoMouseUp?.Invoke(button, position);
        OnMouseUp(button, position);
        PressUp((int)button, position, false);
    }

    public void MousePress(MouseButton button, Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        DoMousePress?.Invoke(button, position);
        OnMousePress(button, position);
        Press((int)button, position, false);
    }

    public void MousePressDodge(MouseButton button, Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        DoMousePressDodge?.Invoke(button, position);
        OnMousePressDodge(button, position);
        PressDodge((int)button, position, false);
    }

    public bool TouchDown(int id, Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        DoTouchDown?.Invoke(id, position);
        bool r = OnTouchDown(id, position);
        PressDown(id, position, false);

        return r;
    }

    public void TouchUp(int id, Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        DoTouchUp?.Invoke(id, position);
        OnTouchUp(id, position);
        PressUp(id, position, false);
    }

    public void TouchMove(int id, Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        DoTouchMove?.Invoke(id, position);
        OnTouchMove(id, position);
        PressMove(id, position, false);
    }

    public void TouchPress(int id, Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        DoTouchPress?.Invoke(id, position);
        OnTouchPress(id, position);
        Press(id, position, false);
    }

    public void TouchPressDodge(int id, Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        DoTouchPressDodge?.Invoke(id, position);
        OnTouchPressDodge(id, position);
        PressDodge(id, position, false);
    }

    public void PressDown(int index, Vector2 position, bool applyLocalTransformation = true) {
        if (applyLocalTransformation && UseLocalPositions) position = ToLocalSpace(position);
        DoPressDown?.Invoke(index, position);
        OnPressDown(index, position);
    }

    public void PressUp(int index, Vector2 position, bool applyLocalTransformation = true) {
        if (applyLocalTransformation && UseLocalPositions) position = ToLocalSpace(position);
        DoPressUp?.Invoke(index, position);
        OnPressUp(index, position);
    }

    public void Press(int index, Vector2 position, bool applyLocalTransformation = true) {
        if (applyLocalTransformation && UseLocalPositions) position = ToLocalSpace(position);
        DoPress?.Invoke(index, position);
        OnPress(index, position);
    }

    public void PressDodge(int index, Vector2 position, bool applyLocalTransformation = true) {
        if (applyLocalTransformation && UseLocalPositions) position = ToLocalSpace(position);
        DoPressDodge?.Invoke(index, position);
        OnPressDodge(index, position);
    }

    public void PressMove(int index, Vector2 position, bool applyLocalTransformation = true) {
        if (applyLocalTransformation && UseLocalPositions) position = ToLocalSpace(position);
        DoPressMove?.Invoke(index, position);
        OnPressMove(index, position);
    }

    public bool MouseEnter(Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        IsMouseOver = true;
        DoMouseEnter?.Invoke(position);
        return OnMouseEnter(position);
    }

    public void MouseLeave(Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        IsMouseOver = false;
        DoMouseLeave?.Invoke(position);
        OnMouseLeave(position);
    }

    public void MouseMove(Vector2 position) {
        if (UseLocalPositions) position = ToLocalSpace(position);
        DoMouseMove?.Invoke(position);
        OnMouseMove(position);

        foreach ((MouseButton button, bool down) in MouseButtons.Primary)
            if (down)
                PressMove((int)button, position, false);
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
