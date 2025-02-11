using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Native;

namespace Natsu.Platforms.Empty;

public class EmptyPlatform : INativePlatform {
    public string Title { get; set; } = "Natsu Application";
    public Vector2 Size => Vector2.One;
    public Vector2 DisplayScale => Vector2.One;

    public CursorStyle Cursor { get; set; } = CursorStyle.Default;
    public CursorMode CursorMode { get; set; } = CursorMode.Default;
    public bool KeyboardVisible { get; set; }

    public string Clipboard { get; set; } = string.Empty;
    public string TextInput { get; set; } = string.Empty;
    public RangeI TextCaret { get; set; } = new(0, 0);

    public bool VSync { get; set; }
    public float UpdateFrequency { get; set; } = 60;

    public void Exit() { }
    public object? SendSignal(string signal, object? data = null) => null;

    public PlatformType Type => PlatformType.Other;
    public PlatformFamily Family => PlatformFamily.Other;
    public PlatformArchitecture Architecture => PlatformArchitecture.Other;

    public string OSVersion => "0.0.0";
    public string OSName => "Empty";

    public PlatformCapabilities Capabilities => PlatformCapabilities.None;

    public void HapticFeedback(float intensity, float duration) { }
    public void Notify(string title, string message) { }
}
