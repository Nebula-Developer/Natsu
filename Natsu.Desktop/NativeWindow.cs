using System.Runtime.InteropServices;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Native;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using TextCopy;
using static OpenTK.Windowing.Common.Input.MouseCursor;

namespace Natsu.Platforms.Desktop;

public class NativeWindow(DesktopWindowSettings settings, DesktopWindow bridge) : GameWindow(settings.GameWindowSettings, settings.NativeWindowSettings), INativePlatform {
    private RangeI _textCaret;
    public DesktopWindow Bridge { get; } = bridge;
    public MouseCursor TargetCursor { get; set; } = PointingHand;

    public void Exit() => Close();

    public new string TextInput { get; set; } = "";

    public RangeI TextCaret {
        get => _textCaret;
        set => _textCaret = new Range(Math.Clamp(value.Start, 0, TextInput.Length), Math.Clamp(value.End, 0, TextInput.Length));
    }

    public new CursorStyle Cursor {
        get =>
            base.Cursor.Shape switch {
                StandardShape.Arrow => CursorStyle.Pointer,
                StandardShape.Crosshair => CursorStyle.Crosshair,
                StandardShape.ResizeEW => CursorStyle.ResizeHorizontal,
                StandardShape.ResizeNS => CursorStyle.ResizeVertical,
                StandardShape.IBeam => CursorStyle.Text,
                _ => CursorStyle.Default
            };
        set =>
            TargetCursor = value switch {
                CursorStyle.Pointer => PointingHand,
                CursorStyle.Crosshair => Crosshair,
                CursorStyle.ResizeHorizontal => ResizeEW,
                CursorStyle.ResizeVertical => ResizeNS,
                CursorStyle.Text => IBeam,
                _ => Default
            };
    }

    public CursorMode CursorMode {
        get => (CursorMode)CursorState;
        set => CursorState = (CursorState)value;
    }

    public new Vector2 Size => new(FramebufferSize.X, FramebufferSize.Y);

    public Vector2 DisplayScale => TryGetCurrentMonitorScale(out float x, out float y) ? new(x, y) : Vector2.One;

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

    public string Clipboard {
        get => ClipboardService.GetText() ?? "";
        set => ClipboardService.SetText(value);
    }

    public PlatformType Type { get; } = PlatformType.Desktop;

    public PlatformFamily Family { get; } = Environment.OSVersion.Platform switch {
        PlatformID.Win32NT => PlatformFamily.Windows,
        PlatformID.Unix => PlatformFamily.Linux,
        PlatformID.MacOSX => PlatformFamily.MacOS,
        _ => PlatformFamily.Other
    };

    public PlatformArchitecture Architecture { get; } = Environment.Is64BitOperatingSystem ? PlatformArchitecture.x64 : PlatformArchitecture.x86;

    public string OSVersion { get; } = Environment.OSVersion.VersionString;
    public string OSName { get; } = RuntimeInformation.OSDescription;

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

    protected override void OnTextInput(TextInputEventArgs e) {
        int changed = Math.Abs(TextCaret.Length);
        if (changed > 0) {
            int start = Math.Min(TextCaret.Start, TextCaret.End);
            int end = Math.Max(TextCaret.Start, TextCaret.End);
            TextInput = TextInput.Remove(start, end - start);
            TextCaret = new(start, start);
        }

        TextInput = TextInput.Insert(TextCaret.Start, e.AsString);
        int pos = TextCaret.Start;
        TextCaret = new(TextCaret.Start + e.AsString.Length, TextCaret.Start + e.AsString.Length);

        Bridge.TextInput(e.AsString, pos, changed);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e) => Bridge.MouseDown(e);

    protected override void OnMouseUp(MouseButtonEventArgs e) => Bridge.MouseUp(e);

    protected override void OnMouseMove(MouseMoveEventArgs e) => Bridge.MouseMove(e);

    protected override void OnMouseWheel(MouseWheelEventArgs e) => Bridge.MouseWheel(e);
}
