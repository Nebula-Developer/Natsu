using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Native;

public interface INativePlatform {
    string Title { get; set; }
    Vector2 Size { get; }
    CursorStyle Cursor { get; set; }
    CursorMode CursorMode { get; set; }
    bool KeyboardVisible { get; set; }
    string Clipboard { get; set; }
    bool VSync { get; set; }
    float UpdateFrequency { get; set; }
    void Exit();
    void SendSignal(string signal, object? data = null);
}
