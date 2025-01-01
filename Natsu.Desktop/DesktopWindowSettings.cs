using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Natsu.Platforms.Desktop;

public class DesktopWindowSettings {
    public GameWindowSettings GameWindowSettings { get; set; } = new() { UpdateFrequency = 300 };

    public NativeWindowSettings NativeWindowSettings { get; set; } = new() { Vsync = VSyncMode.On, MinimumClientSize = new Vector2i(75, 75) };

    public double UpdateFrequency {
        get => GameWindowSettings.UpdateFrequency;
        set => GameWindowSettings.UpdateFrequency = value;
    }

    public bool Win32SuspendTimerOnDrag {
        get => GameWindowSettings.Win32SuspendTimerOnDrag;
        set => GameWindowSettings.Win32SuspendTimerOnDrag = value;
    }

    public string Title {
        get => NativeWindowSettings.Title;
        set => NativeWindowSettings.Title = value;
    }

    public Mathematics.Vector2i BaseSize {
        get => new(NativeWindowSettings.ClientSize.X, NativeWindowSettings.ClientSize.Y);
        set => NativeWindowSettings.ClientSize = new(value.X, value.Y);
    }
}
