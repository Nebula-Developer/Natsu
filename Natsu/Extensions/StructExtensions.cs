using Natsu.Utils;

namespace Natsu.Extensions;

public static class StructExtensions {
    /// <summary>
    ///     Clamps a value between a minimum and maximum value.
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="value">The value to clamp</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns>The clamped value</returns>
    public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> {
        if (value.CompareTo(min) < 0) return min;
        if (value.CompareTo(max) > 0) return max;
        return value;
    }

    /// <summary>
    ///     Linearly interpolates between two values based on a given parameter t, clamping the result between a and b.
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="a">The start value</param>
    /// <param name="b">The end value</param>
    /// <param name="t">The interpolation parameter (0 = a, 1 = b)</param>
    /// <returns>The interpolated value</returns>
    public static T Lerp<T>(this T a, T b, float t) where T : IComparable<T> {
        if (t <= 0) return a;
        if (t >= 1) return b;

        dynamic da = a;
        dynamic db = b;
        return da + (db - da) * t;
    }

    /// <summary>
    ///     Creates a bindable object with the given value and an optional callback for when the value changes.
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="value">The initial value</param>
    /// <param name="onValueChanged">An optional callback that is called when the value changes</param>
    /// <returns>A bindable object with the given value</returns>
    public static Bindable<T> Bindable<T>(this T value, Action<T>? onValueChanged = null) where T : IComparable<T> => new(value, onValueChanged);
}
