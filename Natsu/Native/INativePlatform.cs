using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Native;

public interface INativePlatform {
    string Title { get; set; }
    Vector2 Size { get; }
    Vector2 DisplayScale { get; }
    CursorStyle Cursor { get; set; }
    CursorMode CursorMode { get; set; }
    bool KeyboardVisible { get; set; }

    string Clipboard { get; set; }
    string TextInput { get; set; }
    RangeI TextCaret { get; set; }

    bool VSync { get; set; }
    float UpdateFrequency { get; set; }
    void Exit();
    void SendSignal(string signal, object? data = null);

    #region Platform Details

    PlatformType Type { get; }
    PlatformFamily Family { get; }
    PlatformArchitecture Architecture { get; }

    string OSVersion { get; }
    string OSName { get; }

    #endregion

}
