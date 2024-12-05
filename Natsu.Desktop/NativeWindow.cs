using Natsu.Input;
using Natsu.Mathematics;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;

using static OpenTK.Windowing.Common.Input.MouseCursor;

namespace Natsu.Platforms.Desktop;

public class NativeWindow(DesktopWindowSettings settings, DesktopWindow bridge) : GameWindow(settings.GameWindowSettings, settings.NativeWindowSettings), INativePlatform {
    public DesktopWindow Bridge { get; } = bridge;
    public MouseCursor TargetCursor { get; set; } = Hand;

    public void Exit() => Close();

    public new CursorStyle Cursor {
        get => base.Cursor.Shape switch {
            StandardShape.Arrow => CursorStyle.Pointer,
            StandardShape.Crosshair => CursorStyle.Crosshair,
            StandardShape.HResize => CursorStyle.ResizeHorizontal,
            StandardShape.VResize => CursorStyle.ResizeVertical,
            StandardShape.IBeam => CursorStyle.Text,
            _ => CursorStyle.Default
        };
        set => TargetCursor = value switch {
            CursorStyle.Pointer => Hand,
            CursorStyle.Crosshair => Crosshair,
            CursorStyle.ResizeHorizontal => HResize,
            CursorStyle.ResizeVertical => VResize,
            CursorStyle.Text => IBeam,
            _ => Default
        };
    }

    public CursorMode CursorMode {
        get => (CursorMode)CursorState;
        set => CursorState = (CursorState)value;
    }

    public new Vector2 Size => new(FramebufferSize.X, FramebufferSize.Y);

    public new bool VSync {
        get => base.VSync == VSyncMode.On;
        set => base.VSync = value ? VSyncMode.On : VSyncMode.Off;
    }

    public new float UpdateFrequency {
        get => (float)base.UpdateFrequency;
        set => base.UpdateFrequency = value;
    }

    public void SendSignal(string signal, object? data = null) {
        if (signal == "ping") Console.WriteLine("pong");
    }

    public bool KeyboardVisible {
        get => false;
        set { }
    }

    protected override void OnLoad() => Bridge.Load();
    protected override void OnResize(ResizeEventArgs e) => Bridge.Resize(e.Width, e.Height);

    protected override void OnUpdateFrame(FrameEventArgs e) {
        Bridge.Update();
        if (TargetCursor != base.Cursor) base.Cursor = TargetCursor;
    }

    protected override void OnRenderFrame(FrameEventArgs e) => Bridge.Render();
    protected override void OnUnload() => Bridge.Dispose();

    protected override void OnKeyDown(KeyboardKeyEventArgs e) => Bridge.KeyDown(e);
    protected override void OnKeyUp(KeyboardKeyEventArgs e) => Bridge.KeyUp(e);
    protected override void OnTextInput(TextInputEventArgs e) => Bridge.TextInput(e);

    protected override void OnMouseDown(MouseButtonEventArgs e) => Bridge.MouseDown(e);
    protected override void OnMouseUp(MouseButtonEventArgs e) => Bridge.MouseUp(e);
    protected override void OnMouseMove(MouseMoveEventArgs e) => Bridge.MouseMove(e);
    protected override void OnMouseWheel(MouseWheelEventArgs e) => Bridge.MouseWheel(e);
}
