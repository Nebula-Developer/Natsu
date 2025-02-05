using Natsu.Extensions;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Native;

namespace Natsu.Core;

/// <summary>
///     A <see cref="INativePlatform" /> that will batch all property changes and apply them to another
///     <see cref="INativePlatform" /> when <see cref="Apply" /> is called.
/// </summary>
public class BatchNativePlatform : INativePlatform {
    public Dictionary<string, object> Properties { get; } = new();
    public List<(string, object?[])> Calls { get; } = new();

    public void Exit() => Calls.Add(("Exit", Array.Empty<object>()));
    public object? SendSignal(string signal, object? data = null) => null;

    public PlatformType Type => TryGetProperty<PlatformType>("Type");
    public PlatformFamily Family => TryGetProperty<PlatformFamily>("Family");
    public PlatformArchitecture Architecture => TryGetProperty<PlatformArchitecture>("Architecture");

    public void Apply(INativePlatform platform) {
        foreach (KeyValuePair<string, object> property in Properties) platform.SetProperty(property.Key, property.Value);

        foreach ((string method, object?[] args) in Calls) platform.CallMethod(method, args);
    }

    public T? TryGetProperty<T>(string property) {
        if (Properties.TryGetValue(property, out object? value)) return (T)value;

        return default;
    }
#nullable disable
    public string Title {
        get => TryGetProperty<string>("Title");
        set => Properties["Title"] = value;
    }

    public Vector2 Size => TryGetProperty<Vector2>("Size");
    public Vector2 DisplayScale => TryGetProperty<Vector2>("DisplayScale");

    public CursorStyle Cursor {
        get => TryGetProperty<CursorStyle>("Cursor");
        set => Properties["Cursor"] = value;
    }

    public CursorMode CursorMode {
        get => TryGetProperty<CursorMode>("CursorMode");
        set => Properties["CursorMode"] = value;
    }

    public bool KeyboardVisible {
        get => TryGetProperty<bool>("KeyboardVisible");
        set => Properties["KeyboardVisible"] = value;
    }

    public string Clipboard {
        get => TryGetProperty<string>("Clipboard");
        set => Properties["Clipboard"] = value;
    }

    public string TextInput {
        get => TryGetProperty<string>("TextInput");
        set => Properties["TextInput"] = value;
    }

    public RangeI TextCaret {
        get => TryGetProperty<RangeI>("TextCaret");
        set => Properties["TextCaret"] = value;
    }

    public bool VSync {
        get => TryGetProperty<bool>("VSync");
        set => Properties["VSync"] = value;
    }

    public float UpdateFrequency {
        get => TryGetProperty<float>("UpdateFrequency");
        set => Properties["UpdateFrequency"] = value;
    }

    public string OSVersion => TryGetProperty<string>("OSVersion");
    public string OSName => TryGetProperty<string>("OSName");
}
