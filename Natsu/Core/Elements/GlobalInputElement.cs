namespace Natsu.Core.Elements;

/// <summary>
///     An input element that can take in any input, regardless of whether it is focused or not.
/// </summary>
public class GlobalInputElement : InputElement {
    public GlobalInputElement() => GrabFallback = false;
}
