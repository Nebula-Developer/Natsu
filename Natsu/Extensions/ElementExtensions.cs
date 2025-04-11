using Natsu.Core;

namespace Natsu.Extensions;

public static class ElementExtensions {
    /// <summary>
    ///     Performs an action on the element and returns it for chaining.
    /// </summary>
    /// <typeparam name="T">The type of the element</typeparam>
    /// <param name="element">The element to perform the action on</param>
    /// <param name="action">The action to perform</param>
    /// <returns>The element itself, for chaining</returns>
    public static T With<T>(this T element, Action<T> action) where T : Element {
        action(element);
        return element;
    }
}
