using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Natsu.Platforms.Desktop;

/// <summary>
///     The settings used for creating a <see cref="DesktopWindow" />.
/// </summary>
public class DesktopWindowSettings {
    public GameWindowSettings GameWindowSettings { get; set; } = new() { UpdateFrequency = 300 };

    public NativeWindowSettings NativeWindowSettings { get; set; } = new() { Vsync = VSyncMode.On, MinimumClientSize = new Vector2i(75, 75) };

    /// <summary>
    ///     The target update frequency of the window, in Hz.
    /// </summary>
    public double UpdateFrequency {
        get => GameWindowSettings.UpdateFrequency;
        set => GameWindowSettings.UpdateFrequency = value;
    }

    /// <summary>
    ///     Whether to suspend the window timer when the window is dragged.
    ///     <br />
    ///     Only applies to Windows.
    /// </summary>
    public bool Win32SuspendTimerOnDrag {
        get => GameWindowSettings.Win32SuspendTimerOnDrag;
        set => GameWindowSettings.Win32SuspendTimerOnDrag = value;
    }

    /// <summary>
    ///     The title of the window.
    /// </summary>
    public string Title {
        get => NativeWindowSettings.Title;
        set => NativeWindowSettings.Title = value;
    }

    /// <summary>
    ///     The size of the window upon creation.
    /// </summary>
    public Mathematics.Vector2i BaseSize {
        get => new(NativeWindowSettings.ClientSize.X, NativeWindowSettings.ClientSize.Y);
        set => NativeWindowSettings.ClientSize = new(value.X, value.Y);
    }
}
