namespace Natsu.Native;

[Flags]
public enum PlatformCapabilities {
    None = 0,
    HapticFeedback = 1 << 0,
    Notifications = 1 << 1,
    Touch = 1 << 2,
    Keyboard = 1 << 3,
    Mouse = 1 << 4,
    Clipboard = 1 << 5,
    TextInput = 1 << 6,
    Cursor = 1 << 7
}
