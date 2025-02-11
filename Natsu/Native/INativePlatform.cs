using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Native;

public interface INativePlatform {
    /// <summary>
    ///     The title of the window.
    /// </summary>
    string Title { get; set; }

    /// <summary>
    ///     The size of the window.
    /// </summary>
    Vector2 Size { get; }

    /// <summary>
    ///     The display scale of the window.
    ///     <br />
    ///     This is often used to unify the size of applications across different displays.
    /// </summary>
    Vector2 DisplayScale { get; }

    /// <summary>
    ///     The style of the cursor.
    /// </summary>
    CursorStyle Cursor { get; set; }

    /// <summary>
    ///     The desktop locking mode of the cursor.
    /// </summary>
    CursorMode CursorMode { get; set; }

    /// <summary>
    ///     Whether the keyboard is visible.
    ///     <br />
    ///     This is primarily used to open the keyboard on mobile devices.
    /// </summary>
    bool KeyboardVisible { get; set; }

    /// <summary>
    ///     The clipboard text.
    /// </summary>
    string Clipboard { get; set; }

    /// <summary>
    ///     The text input of the platform.
    /// </summary>
    string TextInput { get; set; }

    /// <summary>
    ///     The text selection of the platform.
    /// </summary>
    RangeI TextCaret { get; set; }

    /// <summary>
    ///     Whether to use vertical synchronization.
    /// </summary>
    bool VSync { get; set; }

    /// <summary>
    ///     The update frequency of the platform.
    ///     <br />
    ///     Some platforms may ignore this value.
    /// </summary>
    float UpdateFrequency { get; set; }

    /// <summary>
    ///     Exits the platform, closing the application.
    /// </summary>
    void Exit();

    /// <summary>
    ///     Sends a signal to the platform.
    ///     <br />
    ///     This can be used to send platform-specific signals to the platform.
    /// </summary>
    /// <param name="signal">The signal to send</param>
    /// <param name="data">The data to send with the signal</param>
    object? SendSignal(string signal, object? data = null);

    #region Platform Details
    /// <summary>
    ///     The type of the host's platform.
    /// </summary>
    PlatformType Type { get; }

    /// <summary>
    ///     The family of the host's operating system.
    /// </summary>
    PlatformFamily Family { get; }

    /// <summary>
    ///     The architecture of the host's platform.
    /// </summary>
    PlatformArchitecture Architecture { get; }

    /// <summary>
    ///     The capability flags of the host's platform.
    /// </summary>
    PlatformCapabilities Capabilities { get; }

    /// <summary>
    ///     The version of the host's operating system.
    /// </summary>
    string OSVersion { get; }

    /// <summary>
    ///     The name of the host's operating system.
    /// </summary>
    string OSName { get; }
    #endregion

    #region Platform-Specific capabilities
    /// <summary>
    ///     Sends a haptic feedback signal to the platform.
    /// </summary>
    /// <remarks>
    ///     Primarily targets Android and iOS.
    /// </remarks>
    /// <param name="duration">The duration of the haptic feedback</param>
    /// <param name="intensity">The intensity of the haptic feedback</param>
    void HapticFeedback(float duration, float intensity);

    /// <summary>
    ///     Sends a notification to the platform.
    /// </summary>
    /// <param name="title">The title of the notification</param>
    /// <param name="message">The message of the notification</param>
    void Notify(string title, string message);
    #endregion
}
