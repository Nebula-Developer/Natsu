using Natsu.Native;

namespace Natsu.Extensions;

public static class INativePlatformExtensions {
    public static void SetProperty(this INativePlatform platform, string property, object value) => platform.GetType().GetProperty(property)?.SetValue(platform, value);
    public static void CallMethod(this INativePlatform platform, string method, object?[] args) => platform.GetType().GetMethod(method)?.Invoke(platform, args);
}
