using Natsu.Native;

namespace Natsu.Extensions;

public static class INativePlatformExtensions {
    public static void SetProperty<[DynamicProperty(DynamicProperties.Accessible)] T>(this T platform, string property, object value) where T : INativePlatform => platform.GetPlatformType().GetProperty(property)?.SetValue(platform, value);
    public static void CallMethod<[DynamicProperty(DynamicProperties.Accessible)] T>(this T platform, string method, object?[] args) where T : INativePlatform => platform.GetPlatformType().GetMethod(method)?.Invoke(platform, args);
}
